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

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string filePath = Path.Combine(appDataPath, "draft-content-root/draft-page.html");

        if (!System.IO.File.Exists(filePath))
        {
            _logger.LogError($"File not found: {filePath}");
            return NotFound("The requested file was not found.");
        }

        string content = await System.IO.File.ReadAllTextAsync(filePath);
        return Content(content, "text/html");


    }



    [HttpGet("data-sets")]
    public async Task<DataSet> GetDataSets()
    {
        using var db = new DraftContext();

        _logger.LogInformation("Querying for a data sets");

        var dataSets = await db.DataSets
                .OrderBy(d => d.CreatedOn)
                .AsNoTracking()
                .Include(d => d.Draftees)
                .LastAsync();


        return dataSets;
    }



}