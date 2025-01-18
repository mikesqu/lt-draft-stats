using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;


namespace WebApi.Controllers;

public class DashboardController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IConfiguration configuration, ILogger<DashboardController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetStaticSite()
    {
        return null;
    }



    [HttpGet("data-sets")]
    public async Task<IList<DataSet>> GetDataSets()
    {
        using var db = new DraftContext();

        _logger.LogInformation("Querying for a data sets");

        var dataSets = await db.DataSets
            .AsNoTracking()
            .ToListAsync();

        return dataSets;
    }



}
