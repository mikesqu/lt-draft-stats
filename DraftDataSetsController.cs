using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using draft_data;


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
        using var db = new DraftContext();

        _logger.LogInformation("Querying for a last data set");

        var lastDataSet = await db.DataSets
            .AsNoTracking()
            .LastAsync();

        // calculate variables based on lastDataSet

        int hasToProvideData = 0;
        int hasToProvideDataUntilExact = 0;
        int draftProcedureInProgress = 0;
        int isAsignedAndNeedsToArrive = 0;
        int quicklyHasToContactAndArrive = 0;
        int hasToAttendMedicalScreening = 0;
        int draftHasBeenPostponed = 0;

        return Ok(DomainConstants.GetPage(
            hasToProvideData: hasToProvideData,
            hasToProvideDataUntilExact: hasToProvideDataUntilExact,
            draftProcedureInProgress: draftProcedureInProgress,
            isAsignedAndNeedsToArrive: isAsignedAndNeedsToArrive,
            quicklyHasToContactAndArrive: quicklyHasToContactAndArrive,
            hasToAttendMedicalScreening: hasToAttendMedicalScreening,
            draftHasBeenPostponed: draftHasBeenPostponed));

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