using System.Data;
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

        public DailyDataRefresher(ILogger<DailyDataRefresher> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
             Random r = new Random();
            int randomMinutes = (int) r.NextInt64(10, 90);
            TimeSpan delay = TimeSpan.FromMinutes(randomMinutes);
            await Task.Delay(delay);

            _logger.LogInformation($"starting {nameof(DailyDataRefresher)}.{nameof(Execute)} ... after delay {delay.TotalMinutes} minutes");

           
            // ---------------------------------------


            // download a dataset
            // use web browser headers
            
            string serializedResult = string.Empty;

            //deserialize dataset
            DataSet newSet = DeserializeDataSet(serializedResult);

            // store a dataset
            await StoreNewDataSet(newSet);


            // create a static site from the new data set


            // ----------------------------------------
            _logger.LogInformation($"finishing {nameof(DailyDataRefresher)}.{nameof(Execute)} ...");
        }

        private DataSet DeserializeDataSet(string serializedData)
        {
            DataSet newSet = new DataSet();
            newSet.CreatedOn = DateTime.Now;

            IList<Draftee> dataSetDraftees = new List<Draftee>();
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