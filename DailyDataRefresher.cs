using System.Data;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace draft_data
{
    [DisallowConcurrentExecution]
    public class DailyDataRefresher : IJob
    {
        private readonly ILogger<DailyDataRefresher> _logger;
        private readonly IHttpClientFactory _cFactory;

        public DailyDataRefresher(ILogger<DailyDataRefresher> logger, IHttpClientFactory cFactory)
        {
            _logger = logger;
            _cFactory = cFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            // Random r = new Random();
            // int randomMinutes = (int)r.NextInt64(10, 90);
            // TimeSpan delay = TimeSpan.FromMinutes(randomMinutes);


            // await Task.Delay(delay);

            // _logger.LogInformation($"starting {nameof(DailyDataRefresher)}.{nameof(Execute)} ... after delay {delay.TotalMinutes} minutes");


            _logger.LogInformation($"starting {nameof(DailyDataRefresher)}.{nameof(Execute)} ");

            // ---------------------------------------


            string serializedResult = await GetDataFromAPI();

            string stringResponsePreview = serializedResult.Take(100).ToString();

            _logger.LogInformation($"received response preview: {stringResponsePreview}");


            //deserialize dataset
            DataSet newSet = DeserializeDataSet(serializedResult);

            _logger.LogInformation("deserialized dataset");

            // store a dataset
            await StoreNewDataSet(newSet);

            _logger.LogInformation("stored new dataset");


            await UpdatePageFile(newSet);

            _logger.LogInformation("updated page file");


            // ----------------------------------------
            _logger.LogInformation($"finishing {nameof(DailyDataRefresher)}.{nameof(Execute)} ...");
        }

        private async Task<string> GetDataFromAPI()
        {
            // Create HttpClient instance
            using var httpClient = _cFactory.CreateClient();

            // Set base address
            httpClient.BaseAddress = new Uri("https://sauktiniai.karys.lt");

            // Create HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, "/list.php?region=6");

            // Add headers
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79");
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd, identity");
            request.Headers.Add("Range-Unit", "items");
            request.Headers.TryAddWithoutValidation("Range", "0-7999");
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
                    Console.WriteLine("Decompressed Response Content:");
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
                    Console.WriteLine("Decompressed Response Content (Deflate):");

                }
            }
            else
            {
                // If not compressed, read as a normal string
                jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Raw Response Content (Uncompressed):");
            }

            Console.WriteLine($"jsonContent preview: {jsonContent}");

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new ArgumentNullException();
            }

            return jsonContent;

        }

        private async Task UpdatePageFile(DataSet dataSet)
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
            int draftHasBeenPostponed = lastDataSet.Draftees.Where(d => d.Info.Contains("privalomoji karo tarnyba atidėta")).Count();


            string fileContent = DomainConstants.GetPage(
                hasToProvideData: hasToProvideData,
                hasToProvideDataUntilExact: hasToProvideDataUntilExact,
                draftProcedureInProgress: draftProcedureInProgress,
                isAsignedAndNeedsToArrive: isAsignedAndNeedsToArrive,
                quicklyHasToContactAndArrive: quicklyHasToContactAndArrive,
                hasToAttendMedicalScreening: hasToAttendMedicalScreening,
                hasToAttendAdditionalMedScreening: hasToAttendAdditionalMedScreening,
                hasToProvideAddtionalInfoAfterAdditionalMedScreening: hasToAttendMedicalScreening,
                draftHasBeenPostponed: draftHasBeenPostponed,
                updatedOn: DateTime.Now - TimeSpan.FromMinutes(randomMinutes));


            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataDir, "draft-content-root/draft-page.html");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(fileContent);
            }

            _logger.LogInformation($"File created/updated at: DailyDataRefresher.cs");

        }

        private DataSet DeserializeDataSet(string serializedData)
        {
            DataSet newSet = new DataSet();
            newSet.Draftees = new List<Draftee>(); 
            newSet.CreatedOn = DateTime.Now;

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
                newSet.Draftees = new List<Draftee>();
                throw;
            }

            _logger.LogInformation($"tempDraftees.Count: {tempDraftees.Count}");



            foreach (var td in tempDraftees)
            {
                newSet.Draftees.Add(new Draftee()
                {
                    Position = int.Parse(td.Pos),
                    Number = td.Number,
                    Name = td.Name,
                    LastName = td.Lastname,
                    BirthDateYear = int.Parse(td.Bdate),
                    Department = int.Parse(td.Department),
                    Info = td.Info,
                });
            }


            return newSet;
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