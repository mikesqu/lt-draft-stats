using System.Data;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;

namespace draft_data
{
    [DisallowConcurrentExecution]
    public class DailyDataRefresher : IJob
    {
        private readonly ILogger<DailyDataRefresher> _logger;
        private readonly IHttpClientFactory _cFactory;
        private readonly IHostEnvironment _env;

        public DailyDataRefresher(ILogger<DailyDataRefresher> logger, IHttpClientFactory cFactory, IHostEnvironment environment)
        {
            _logger = logger;
            _cFactory = cFactory;
            _env = environment;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"starting {nameof(DailyDataRefresher)}.{nameof(Execute)} ");

            if (_env.IsDevelopment())
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "reg6.json");
                string jsonContent = File.ReadAllText(jsonFilePath);

                // You can now use the jsonContent string to work with the JSON data
                _logger.LogInformation($"JSON file content: {jsonContent.Substring(0, 50)}");

                DataSet newSet = new DataSet();
                newSet.CreatedOn = DateTime.Now;


                IList<TempDraftee> tempDraftees = new List<TempDraftee>();

                try
                {
                    tempDraftees = JsonSerializer.Deserialize<List<TempDraftee>>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                }
                catch (JsonException ex)
                {
                    _logger.LogInformation($"Error deserializing into TempDraftee: {ex.Message}");
                    newSet.Draftees = new List<Draftee>();
                }

                _logger.LogInformation($"tempDraftees.Count: {tempDraftees.Count}");

                DataSet lastDataSet = newSet;
                lastDataSet.Draftees = new List<Draftee>();


                foreach (var td in tempDraftees)
                {
                    lastDataSet.Draftees.Add(new Draftee()
                    {
                        Position = int.Parse(td.Pos),
                        Number = td.Number,
                        Name = td.Name,
                        LastName = td.Lastname,
                        BirthDateYear = int.Parse(td.Bdate),
                        Department = int.Parse(td.Department),
                        Info = td.Info,
                        Region = "6"
                    });
                }

                await UpdatePageFile(newSet, 315,
                    0,
                    reg1Acceptable: 315,
                    reg2Acceptable: 257,
                    reg3Acceptable: 124,
                    reg4Acceptable: 7,
                    reg5Acceptable: 364,
                    reg6Acceptable: 2
                );

            }
            else
            {

                // alytus region
                string serializedResult1 = await GetDataFromAPI(region: 1, range: "0-2399");

                // kaunas region
                string serializedResult2 = await GetDataFromAPI(region: 2, range: "0-5899");

                // klaipeda region
                string serializedResult3 = await GetDataFromAPI(region: 3, range: "0-4499");

                // panevezio region
                string serializedResult4 = await GetDataFromAPI(region: 4, range: "0-2499");

                // siauliu region
                string serializedResult5 = await GetDataFromAPI(region: 5, range: "0-3099");

                // vilnius region
                string serializedResult6 = await GetDataFromAPI(region: 6, range: "0-7699");

                string stringResponsePreview1 = serializedResult1.Substring(0, 20);
                string stringResponsePreview2 = serializedResult2.Substring(0, 20);
                string stringResponsePreview3 = serializedResult3.Substring(0, 20);
                string stringResponsePreview4 = serializedResult4.Substring(0, 20);
                string stringResponsePreview5 = serializedResult5.Substring(0, 20);
                string stringResponsePreview6 = serializedResult6.Substring(0, 20);

                _logger.LogInformation($"received response preview region 1: {stringResponsePreview1}");
                _logger.LogInformation($"received response preview region 2: {stringResponsePreview2}");
                _logger.LogInformation($"received response preview region 3:{stringResponsePreview3}");
                _logger.LogInformation($"received response preview region 4:{stringResponsePreview4}");
                _logger.LogInformation($"received response preview region 5: {stringResponsePreview5}");
                _logger.LogInformation($"received response preview region 6: {stringResponsePreview6}");


                //deserialize dataset
                DataSet newSet = new DataSet();
                newSet.Draftees = new List<Draftee>();
                newSet.CreatedOn = DateTime.Now;

                IList<Draftee> region1Draftees = DeserializeDataSet(serializedResult1, region: "1");
                IList<Draftee> region2Draftees = DeserializeDataSet(serializedResult2, region: "2");
                IList<Draftee> region3Draftees = DeserializeDataSet(serializedResult3, region: "3");
                IList<Draftee> region4Draftees = DeserializeDataSet(serializedResult4, region: "4");
                IList<Draftee> region5Draftees = DeserializeDataSet(serializedResult5, region: "5");
                IList<Draftee> region6Draftees = DeserializeDataSet(serializedResult6, region: "6");

                _logger.LogInformation("deserialized draftees count region 1 {drafteesCount}", region1Draftees.Count);
                _logger.LogInformation("deserialized draftees count region 2 {drafteesCount}", region2Draftees.Count);
                _logger.LogInformation("deserialized draftees count region 3 {drafteesCount}", region3Draftees.Count);
                _logger.LogInformation("deserialized draftees count region 4 {drafteesCount}", region4Draftees.Count);
                _logger.LogInformation("deserialized draftees count region 5 {drafteesCount}", region5Draftees.Count);
                _logger.LogInformation("deserialized draftees count region 6 {drafteesCount}", region6Draftees.Count);

                List<Draftee> tempList = newSet.Draftees.ToList();

                tempList.AddRange(region1Draftees);
                tempList.AddRange(region2Draftees);
                tempList.AddRange(region3Draftees);
                tempList.AddRange(region4Draftees);
                tempList.AddRange(region5Draftees);
                tempList.AddRange(region6Draftees);

                newSet.Draftees = tempList;


                _logger.LogInformation("deserialized dataset with {drafteesCount} with CreatedOn: {datetime}", newSet.Draftees.Count, newSet.CreatedOn);

                // store a dataset
                await StoreNewDataSet(newSet);

                _logger.LogInformation("stored new dataset");

                int totalAcceptable = 0;
                int totalStartedService = 0;

                int reg1Acceptable = 0;
                int reg2Acceptable = 0;
                int reg3Acceptable = 0;
                int reg4Acceptable = 0;
                int reg5Acceptable = 0;
                int reg6Acceptable = 0;

                try
                {
                    totalAcceptable = GetTotalAcceptableCount();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read totalAcceptable");
                }


                try
                {
                    totalStartedService = GetTotalStartedService();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read totalStartedService");
                }



                try
                {
                    reg1Acceptable = GetReg1Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg1Acceptable");
                }

                try
                {
                    reg2Acceptable = GetReg2Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg2Acceptable");
                }

                try
                {
                    reg3Acceptable = GetReg3Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg3Acceptable");
                }

                try
                {
                    reg4Acceptable = GetReg4Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg4Acceptable");
                }

                try
                {
                    reg5Acceptable = GetReg5Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg5Acceptable");
                }

                try
                {
                    reg6Acceptable = GetReg6Acceptable();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to read reg6Acceptable");
                }


                await UpdatePageFile(newSet, totalAcceptable,
                    totalStartedService,
                    reg1Acceptable: reg1Acceptable,
                    reg2Acceptable: reg2Acceptable,
                    reg3Acceptable: reg3Acceptable,
                    reg4Acceptable: reg4Acceptable,
                    reg5Acceptable: reg5Acceptable,
                    reg6Acceptable: reg6Acceptable
                );

                _logger.LogInformation("updated page file");
            }

            // ----------------------------------------
            _logger.LogInformation($"finishing {nameof(DailyDataRefresher)}.{nameof(Execute)} ...");
        }

        private int GetReg6Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Vilniaus regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Vilniaus regionas.");
            }
            return 0;
        }

        private int GetReg5Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Šiaulių regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Šiaulių regionas.");
            }
            return 0;
        }

        private int GetReg4Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Panevėžio regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Panevėžio regionas.");
            }
            return 0;
        }

        private int GetReg3Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Klaipėdos regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Klaipėdos regionas.");
            }
            return 0;
        }

        private int GetReg2Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Kauno regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Kauno regionas.");
            }
            return 0;
        }

        private int GetReg1Acceptable()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var htmlDoc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // XPath to find the Alytaus region row under the "PAGAL REGIONUS" table
            string xpath = "//table[caption/h3[text()='PAGAL REGIONUS']]//h3[text()='Alytaus regionas']/following::tr[2]/td[4]";

            // Locate the desired node
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            // Extract and display the value
            if (node != null)
            {
                string alytausRegionTinkami = node.InnerText.Trim();
                return int.Parse(alytausRegionTinkami);
            }
            else
            {
                _logger.LogWarning("Value not found for Alytaus regionas.");
            }
            return 0;
        }


        private int GetTotalStartedService()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var doc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // Find the table with the header "IŠ VISO"
            var table = doc.DocumentNode.SelectSingleNode("//table[caption/h3[text()='IŠ VISO']]");

            if (table != null)
            {
                // Find the index of the "IŠ VISO pripažinti tinkami" column
                var headerCells = table.SelectNodes(".//tr[1]/td");

                // Find the index of the "IŠ VISO pradėjo tarnybą" column
                int indexPradejoTarnyba = headerCells
                    .Select((cell, index) => new { Cell = cell, Index = index })
                    .Where(x => x.Cell.InnerText.Trim() == "IŠ VISO pradėjo tarnybą")
                    .Select(x => x.Index)
                    .FirstOrDefault();

                if (indexPradejoTarnyba >= 0)
                {
                    // Find the value in the second row for both columns
                    var rows = table.SelectNodes(".//tr[position() > 1]");
                    var valuePradejoTarnyba = rows[0].SelectNodes("td")[indexPradejoTarnyba]?.InnerText.Trim();

                    _logger.LogInformation($"IŠ VISO pradėjo tarnybą: {valuePradejoTarnyba}");

                    if (valuePradejoTarnyba == null || valuePradejoTarnyba == string.Empty || string.IsNullOrWhiteSpace(valuePradejoTarnyba))
                    {
                        return 0;
                    }
                    else
                    {
                        return int.Parse(valuePradejoTarnyba);

                    }
                }
                else
                {
                    _logger.LogWarning("One or both target columns not found.");
                }
            }
            else
            {
                _logger.LogWarning("Table with header 'IŠ VISO' not found.");
            }


            return 0;

        }

        private int GetTotalAcceptableCount()
        {
            // Load the HTML document
            var web = new HtmlWeb();
            var doc = web.Load("https://www.karys.lt/tarnybos-budai/nuolatine-privalomoji-pradine-karo-tarnyba/2025-metu-saukimo-eiga/493");

            // Find the table with the header "IŠ VISO"
            var table = doc.DocumentNode.SelectSingleNode("//table[caption/h3[text()='IŠ VISO']]");

            if (table != null)
            {
                // Find the index of the "IŠ VISO pripažinti tinkami" column
                var headerCells = table.SelectNodes(".//tr[1]/td");
                int indexPripazintiTinkami = headerCells
                    .Select((cell, index) => new { Cell = cell, Index = index })
                    .Where(x => x.Cell.InnerText.Trim() == "IŠ VISO pripažinti tinkami")
                    .Select(x => x.Index)
                    .FirstOrDefault();


                if (indexPripazintiTinkami >= 0)
                {
                    // Find the value in the second row for both columns
                    var rows = table.SelectNodes(".//tr[position() > 1]");
                    var valuePripazintiTinkami = rows[0].SelectNodes("td")[indexPripazintiTinkami]?.InnerText.Trim();

                    _logger.LogInformation($"IŠ VISO pripažinti tinkami: {valuePripazintiTinkami}");

                    if (valuePripazintiTinkami == null || valuePripazintiTinkami == string.Empty || string.IsNullOrWhiteSpace(valuePripazintiTinkami))
                    {
                        return 0;
                    }
                    else
                    {
                        return int.Parse(valuePripazintiTinkami);
                    }
                }
                else
                {
                    _logger.LogWarning("One or both target columns not found.");
                }
            }
            else
            {
                _logger.LogWarning("Table with header 'IŠ VISO' not found.");
            }

            return 0;

        }

        private async Task<string> GetDataFromAPI(int region, string range)
        {
            // Create HttpClient instance
            using var httpClient = _cFactory.CreateClient();

            // Set base address
            httpClient.BaseAddress = new Uri("https://sauktiniai.karys.lt");

            // Create HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, $"/list.php?region={region}");

            // Add headers
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79");
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd, identity");
            request.Headers.Add("Range-Unit", "items");
            request.Headers.TryAddWithoutValidation("Range", range);
            request.Headers.Add("Referer", "https://sauktiniai.karys.lt/");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");
            request.Headers.Add("Priority", "u=0");
            request.Headers.Add("TE", "trailers");

            // Send request and get response
            var response = await httpClient.SendAsync(request);

            // Ensure success status code
            response.EnsureSuccessStatusCode();


            // Read response content


            string jsonContent = string.Empty;

            // Check if the response is compressed
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                // Decompress GZIP content
                using (var compressedStream = await response.Content.ReadAsStreamAsync())
                using (var decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(decompressionStream, Encoding.UTF8))
                {
                    jsonContent = await reader.ReadToEndAsync();
                }
            }
            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
            {
                // Decompress Deflate content
                using (var compressedStream = await response.Content.ReadAsStreamAsync())
                using (var decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(decompressionStream, Encoding.UTF8))
                {
                    jsonContent = await reader.ReadToEndAsync();

                }
            }
            else
            {
                // If not compressed, read as a normal string
                jsonContent = await response.Content.ReadAsStringAsync();
            }


            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new ArgumentNullException();
            }

            return jsonContent;

        }

        private async Task UpdatePageFile(DataSet dataSet, int totalAcceptable, int totalStartedService,
         int reg1Acceptable,
          int reg2Acceptable,
           int reg3Acceptable,
           int reg4Acceptable,
            int reg5Acceptable,
             int reg6Acceptable)
        {

            Random r = new Random();
            int randomMinutes = (int)r.NextInt64(10, 90);


            using var db = new DraftContext();

            _logger.LogInformation("Querying for a last data set");

            var lastDataSet = await db.
            DataSets
                .OrderBy(d => d.CreatedOn)
                .Include(d => d.Draftees)
                .AsNoTracking()
                .LastAsync();


            int hasToProvideData = lastDataSet.Draftees.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count();
            int hasToProvideDataUntilExact = lastDataSet.Draftees
                .Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count();

            int draftProcedureInProgress = lastDataSet.Draftees.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArrive = lastDataSet.Draftees.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArrive = lastDataSet.Draftees.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreening = lastDataSet.Draftees.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreening = lastDataSet.Draftees.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreening = lastDataSet.Draftees.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();

            int inService = lastDataSet.Draftees.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponed = lastDataSet.Draftees.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraft = lastDataSet.Draftees.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count();


            int reg1Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "1" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();
            int reg2Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "2" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();
            int reg3Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "3" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();
            int reg4Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "4" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();
            int reg5Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "5" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();
            int reg6Potential = lastDataSet.Draftees
                .Where(
                    d => d.Region == "6" &&
                    d.Info.Contains("atlieka tarnybą") == false &&
                    d.Info.Contains("privalomoji karo tarnyba atidėta") == false &&
                    d.Info.Contains("išbrauktas iš karinės įskaitos") == false
                ).Count();




            int sumOfMetrics = hasToProvideData +
             hasToProvideDataUntilExact +
              draftProcedureInProgress +
               isAsignedAndNeedsToArrive +
                quicklyHasToContactAndArrive +
                hasToAttendMedicalScreening +
                 hasToAttendAdditionalMedScreening +
                  hasToProvideAddtionalInfoAfterAdditionalMedScreening +
                  draftHasBeenPostponed + inService + removedFromDraft;


            _logger.LogInformation("DataSet.Draftees.Count: {DrafteesCount}", lastDataSet.Draftees.Count);
            _logger.LogInformation("hasToProvideData: {HasToProvideData}", hasToProvideData);
            _logger.LogInformation("hasToProvideDataUntilExact: {HasToProvideDataUntilExact}", hasToProvideDataUntilExact);
            _logger.LogInformation("draftProcedureInProgress: {DraftProcedureInProgress}", draftProcedureInProgress);
            _logger.LogInformation("isAsignedAndNeedsToArrive: {IsAsignedAndNeedsToArrive}", isAsignedAndNeedsToArrive);
            _logger.LogInformation("quicklyHasToContactAndArrive: {QuicklyHasToContactAndArrive}", quicklyHasToContactAndArrive);
            _logger.LogInformation("hasToAttendMedicalScreening: {HasToAttendMedicalScreening}", hasToAttendMedicalScreening);
            _logger.LogInformation("hasToAttendAdditionalMedScreening: {HasToAttendAdditionalMedScreening}", hasToAttendAdditionalMedScreening);
            _logger.LogInformation("hasToProvideAddtionalInfoAfterAdditionalMedScreening: {HasToProvideAddtionalInfoAfterAdditionalMedScreening}", hasToProvideAddtionalInfoAfterAdditionalMedScreening);
            _logger.LogInformation("inService: {InService}", inService);
            _logger.LogInformation("draftHasBeenPostponed: {DraftHasBeenPostponed}", draftHasBeenPostponed);
            _logger.LogInformation("removedFromDraft: {removedFromDraft}", removedFromDraft);


            if (lastDataSet.Draftees.Count != sumOfMetrics)
            {
                _logger.LogError("sum of metrics does not match, total draftees: {lastDataSetDrafteesCount}, sum of metrics: {sumOfMetrics}", lastDataSet.Draftees.Count, sumOfMetrics);
            }
            else
            {
                _logger.LogInformation
                            ("sum of metrics: {sumOfMetrics}", sumOfMetrics);

            }

            IList<Draftee> vilniusDataSet = lastDataSet.Draftees.Where(d => d.Region == "6").ToList();

            IList<Draftee> sect1 = vilniusDataSet.Take(new Range(0, 1257)).ToList();
            IList<Draftee> sect2 = vilniusDataSet.Take(new Range(1257, 2514)).ToList();
            IList<Draftee> sect3 = vilniusDataSet.Take(new Range(2514, 3771)).ToList();
            IList<Draftee> sect4 = vilniusDataSet.Take(new Range(3771, 5028)).ToList();
            IList<Draftee> sect5 = vilniusDataSet.Take(new Range(5028, 6285)).ToList();
            IList<Draftee> sect6 = vilniusDataSet.Take(new Range(6285, 7546)).ToList();




            int hasToProvideDataReg1s1 = sect1.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s1 = sect1.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s1 = sect1.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s1 = sect1.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s1 = sect1.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s1 = sect1.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s1 = sect1.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1 = sect1.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s1 = sect1.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s1 = sect1.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s1 = sect1.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;



            int hasToProvideDataReg1s2 = sect2.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s2 = sect2.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s2 = sect2.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s2 = sect2.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s2 = sect2.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s2 = sect2.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s2 = sect2.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2 = sect2.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s2 = sect2.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s2 = sect2.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s2 = sect2.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;



            int hasToProvideDataReg1s3 = sect3.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s3 = sect3.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s3 = sect3.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s3 = sect3.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s3 = sect3.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s3 = sect3.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s3 = sect3.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3 = sect3.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s3 = sect3.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s3 = sect3.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s3 = sect3.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;



            int hasToProvideDataReg1s4 = sect4.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s4 = sect4.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s4 = sect4.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s4 = sect4.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s4 = sect4.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s4 = sect4.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s4 = sect4.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4 = sect4.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s4 = sect4.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s4 = sect4.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s4 = sect4.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;



            int hasToProvideDataReg1s5 = sect5.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s5 = sect5.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s5 = sect5.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s5 = sect5.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s5 = sect5.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s5 = sect5.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s5 = sect5.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5 = sect5.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s5 = sect5.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s5 = sect5.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s5 = sect5.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;




            int hasToProvideDataReg1s6 = sect6.Where(d => d.Info == "privalote susisiekti ir pateikti savo duomenis").Count(); ;
            int hasToProvideDataUntilExactReg1s6 = sect6.Where(d => d.Info.Contains("privalote susisiekti ir pateikti savo duomenis")
                    && d.Info.Contains("iki")
                    && d.Info.Contains("2025"))
                 .Count(); ;
            int draftProcedureInProgressReg1s6 = sect6.Where(d => d.Info.Contains("šaukimo procedūros vykdomos")).Count();
            int isAsignedAndNeedsToArriveReg1s6 = sect6.Where(d => d.Info.Contains("privalote atvykti į")).Count();
            int quicklyHasToContactAndArriveReg1s6 = sect6.Where(d => d.Info.Contains("privalote skubiai susisiekti arba atvykti")).Count();
            int hasToAttendMedicalScreeningReg1s6 = sect6.Where(d => d.Info.Contains("privalote atvykti pasitikrinti sveikatos")).Count();
            int hasToAttendAdditionalMedScreeningReg1s6 = sect6.Where(d => d.Info.Contains("privalote papildomai pasitikrinti sveikatą")).Count();
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6 = sect6.Where(d => d.Info.Contains("privalote pateikti reikiamus medicininius dokumentus po papildomo ištyrimo")).Count();
            int inServiceReg1s6 = sect6.Where(d => d.Info.Contains("atlieka tarnybą")).Count();
            int draftHasBeenPostponedReg1s6 = sect6.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();
            int removedFromDraftReg1s6 = sect6.Where(d => d.Info.Contains("išbrauktas iš karinės įskaitos")).Count(); ;



            string fileContent = DomainConstants.GetPage(
                totalAcceptable: totalAcceptable,
                totalStartedService: totalStartedService,
                hasToProvideData: hasToProvideData,
                hasToProvideDataUntilExact: hasToProvideDataUntilExact,
                draftProcedureInProgress: draftProcedureInProgress,
                isAsignedAndNeedsToArrive: isAsignedAndNeedsToArrive,
                quicklyHasToContactAndArrive: quicklyHasToContactAndArrive,
                hasToAttendMedicalScreening: hasToAttendMedicalScreening,
                hasToAttendAdditionalMedScreening: hasToAttendAdditionalMedScreening,
                hasToProvideAddtionalInfoAfterAdditionalMedScreening: hasToProvideAddtionalInfoAfterAdditionalMedScreening,
                inService: inService,
                draftHasBeenPostponed: draftHasBeenPostponed,
                updatedOn: DateTime.Now - TimeSpan.FromMinutes(randomMinutes),
                removedFromDraft: removedFromDraft,
                reg1Acceptable: reg1Acceptable,
                reg2Acceptable: reg2Acceptable,
                reg3Acceptable: reg3Acceptable,
                reg4Acceptable: reg4Acceptable,
                reg5Acceptable: reg5Acceptable,
                reg6Acceptable: reg6Acceptable,
                reg1Potential: reg1Potential,
                reg2Potential: reg2Potential,
                reg3Potential: reg3Potential,
                reg4Potential: reg4Potential,
                reg5Potential: reg5Potential,
                reg6Potential: reg6Potential,
                hasToProvideDataReg1s1: hasToProvideDataReg1s1,
                hasToProvideDataUntilExactReg1s1: hasToProvideDataUntilExactReg1s1,
                draftProcedureInProgressReg1s1: draftProcedureInProgressReg1s1,
                isAsignedAndNeedsToArriveReg1s1: isAsignedAndNeedsToArriveReg1s1,
                quicklyHasToContactAndArriveReg1s1: quicklyHasToContactAndArriveReg1s1,
                hasToAttendMedicalScreeningReg1s1: hasToAttendMedicalScreeningReg1s1,
                hasToAttendAdditionalMedScreeningReg1s1: hasToAttendAdditionalMedScreeningReg1s1,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1,
                inServiceReg1s1: inServiceReg1s1,
                draftHasBeenPostponedReg1s1: draftHasBeenPostponedReg1s1,
                removedFromDraftReg1s1: removedFromDraftReg1s1,
                hasToProvideDataReg1s2: hasToProvideDataReg1s2,
                hasToProvideDataUntilExactReg1s2: hasToProvideDataUntilExactReg1s2,
                draftProcedureInProgressReg1s2: draftProcedureInProgressReg1s2,
                isAsignedAndNeedsToArriveReg1s2: isAsignedAndNeedsToArriveReg1s2,
                quicklyHasToContactAndArriveReg1s2: quicklyHasToContactAndArriveReg1s2,
                hasToAttendMedicalScreeningReg1s2: hasToAttendMedicalScreeningReg1s2,
                hasToAttendAdditionalMedScreeningReg1s2: hasToAttendAdditionalMedScreeningReg1s2,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2,
                inServiceReg1s2: inServiceReg1s2,
                draftHasBeenPostponedReg1s2: draftHasBeenPostponedReg1s2,
                removedFromDraftReg1s2: removedFromDraftReg1s2,
                hasToProvideDataReg1s3: hasToProvideDataReg1s3,
                hasToProvideDataUntilExactReg1s3: hasToProvideDataUntilExactReg1s3,
                draftProcedureInProgressReg1s3: draftProcedureInProgressReg1s3,
                isAsignedAndNeedsToArriveReg1s3: isAsignedAndNeedsToArriveReg1s3,
                quicklyHasToContactAndArriveReg1s3: quicklyHasToContactAndArriveReg1s3,
                hasToAttendMedicalScreeningReg1s3: hasToAttendMedicalScreeningReg1s3,
                hasToAttendAdditionalMedScreeningReg1s3: hasToAttendAdditionalMedScreeningReg1s3,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3,
                inServiceReg1s3: inServiceReg1s3,
                draftHasBeenPostponedReg1s3: draftHasBeenPostponedReg1s3,
                removedFromDraftReg1s3: removedFromDraftReg1s3,
                hasToProvideDataReg1s4: hasToProvideDataReg1s4,
                hasToProvideDataUntilExactReg1s4: hasToProvideDataUntilExactReg1s4,
                draftProcedureInProgressReg1s4: draftProcedureInProgressReg1s4,
                isAsignedAndNeedsToArriveReg1s4: isAsignedAndNeedsToArriveReg1s4,
                quicklyHasToContactAndArriveReg1s4: quicklyHasToContactAndArriveReg1s4,
                hasToAttendMedicalScreeningReg1s4: hasToAttendMedicalScreeningReg1s4,
                hasToAttendAdditionalMedScreeningReg1s4: hasToAttendAdditionalMedScreeningReg1s4,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4,
                inServiceReg1s4: inServiceReg1s4,
                draftHasBeenPostponedReg1s4: draftHasBeenPostponedReg1s4,
                removedFromDraftReg1s4: removedFromDraftReg1s4,
                hasToProvideDataReg1s5: hasToProvideDataReg1s5,
                hasToProvideDataUntilExactReg1s5: hasToProvideDataUntilExactReg1s5,
                draftProcedureInProgressReg1s5: draftProcedureInProgressReg1s5,
                isAsignedAndNeedsToArriveReg1s5: isAsignedAndNeedsToArriveReg1s5,
                quicklyHasToContactAndArriveReg1s5: quicklyHasToContactAndArriveReg1s5,
                hasToAttendMedicalScreeningReg1s5: hasToAttendMedicalScreeningReg1s5,
                hasToAttendAdditionalMedScreeningReg1s5: hasToAttendAdditionalMedScreeningReg1s5,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5,
                inServiceReg1s5: inServiceReg1s5,
                draftHasBeenPostponedReg1s5: draftHasBeenPostponedReg1s5,
                removedFromDraftReg1s5: removedFromDraftReg1s5,
                hasToProvideDataReg1s6: hasToProvideDataReg1s6,
                hasToProvideDataUntilExactReg1s6: hasToProvideDataUntilExactReg1s6,
                draftProcedureInProgressReg1s6: draftProcedureInProgressReg1s6,
                isAsignedAndNeedsToArriveReg1s6: isAsignedAndNeedsToArriveReg1s6,
                quicklyHasToContactAndArriveReg1s6: quicklyHasToContactAndArriveReg1s6,
                hasToAttendMedicalScreeningReg1s6: hasToAttendMedicalScreeningReg1s6,
                hasToAttendAdditionalMedScreeningReg1s6: hasToAttendAdditionalMedScreeningReg1s6,
                hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6: hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6,
                inServiceReg1s6: inServiceReg1s6,
                draftHasBeenPostponedReg1s6: draftHasBeenPostponedReg1s6,
                removedFromDraftReg1s6: removedFromDraftReg1s6
                );

            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataDir, "draft-content-root/draft-page.html");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(fileContent);
            }

            _logger.LogInformation("File created/updated at: {filePath}", filePath);

        }

        private IList<Draftee> DeserializeDataSet(string serializedData, string region)
        {
            IList<Draftee> draftees = new List<Draftee>();

            IList<TempDraftee> tempDraftees = new List<TempDraftee>();

            try
            {
                tempDraftees = JsonSerializer.Deserialize<List<TempDraftee>>(serializedData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing into TempDraftee: {ex.Message}");
                throw;
            }


            foreach (var td in tempDraftees)
            {
                draftees.Add(new Draftee()
                {
                    Position = int.Parse(td.Pos),
                    Number = td.Number,
                    Name = td.Name,
                    LastName = td.Lastname,
                    BirthDateYear = int.Parse(td.Bdate),
                    Department = int.Parse(td.Department),
                    Info = td.Info,
                    Region = region
                });
            }


            return draftees;
        }

        private async Task<DraftContext> StoreNewDataSet(DataSet newSet)
        {
            using var db = new DraftContext();

            // Note: This sample requires the database to be created before running.
            _logger.LogInformation($"Database path: {db.DbPath}.");

            // Create
            _logger.LogInformation("Inserting a new dataset");

            db.DataSets.Add(newSet);

            await db.SaveChangesAsync();

            return db;
        }
    }
}