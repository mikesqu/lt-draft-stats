using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DraftContext : DbContext
{

    private readonly IConfiguration _config;
    private readonly ILogger<DraftContext> _logger;
    private readonly IHostEnvironment _hostEnv;

    public string DbPath { get; set; }

    public DbSet<DataSet> DataSets { get; set; }

    public DraftContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "draft-datasets.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<DataSet>(c =>
            {
                c.HasData(new DataSet(dataSetId: 1), new DataSet(dataSetId: 2));
            });


        // modelBuilder.Entity<Endpoint>(e =>
        // {
        //     e.HasIndex(e => e.Name).IsUnique();

        //     e.HasQueryFilter(e => !(e.EndpointId == 1));

        //     e.HasData(
        //         new Endpoint(string.Empty, endpointId: 1),
        //         new Endpoint("ProtoServiceController", endpointId: 2),
        //         new Endpoint("ProtoIPEndpoint", endpointId: 3),
        //         new Endpoint("ProtoWebServiceController", endpointId: 4),
        //         new Endpoint("ProtoSMAgent", endpointId: 5, isSMAgent: true)
        //     );
        // });
    }

}


public class DataSet
{

    public DataSet()
    {
    }

    public DataSet(int dataSetId)
    {
        DataSetId = dataSetId;
    }

    public int DataSetId { get; set; }
    public DateTime CreatedOn { get; set; }
    public IList<Draftee> Draftees { get; set; }
    public IList<KeyValueCombo> AdditionalProperties { get; set; }

}


public class Draftee
{
    public Draftee()
    {

    }
    
    public Draftee(int drafteeId)
    {
        DrafteeId = drafteeId;
    }

    public int DrafteeId { get; set; }
    public int Position { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int BirthDateYear { get; set; }
    public int Department { get; set; }
    public string Info { get; set; }
}
public class KeyValueCombo
{
    public int KeyValueComboId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}