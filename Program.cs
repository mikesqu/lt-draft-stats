using Microsoft.EntityFrameworkCore;
using Quartz;
using draft_data;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppDomain.CurrentDomain.BaseDirectory
        });


        Console.WriteLine("crp: --- " + builder.Environment.ContentRootPath);

        // levelSwitch: new LoggingLevelSwitch(LogEventLevel.Warning), outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj} {NewLine} {Exception}"
        var serilogger = new LoggerConfiguration()
            .WriteTo.Async(a =>
                a.File(
                "Logs/log.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] Request-id: {RequestId} {SourceContext} {Message:lj}{NewLine}{Exception} ",
                shared: true,
                fileSizeLimitBytes: null,
                levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information))
            )
            .CreateLogger();


        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(serilogger);

        builder.Services.AddHttpLogging(o => { });

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddControllers();

        builder.Services.AddHttpClient();

        try
        {
            builder.WebHost.ConfigureKestrel(opts =>
            {
                opts.ConfigureHttpsDefaults(cOpts =>
                {
                    var certPem = File.ReadAllText(builder.Configuration.GetConnectionString("cert-pem-location"));
                    var keyPem = File.ReadAllText(builder.Configuration.GetConnectionString("priv-pem-location"));
                    
                    X509Certificate2 pathToCert = X509Certificate2.CreateFromPem(certPem, keyPem);
                    cOpts.ServerCertificate = pathToCert;
                });

            });
        }
        catch (Exception e)
        {
            throw new ArgumentException("Failed to find certificate to use");
        }

        builder.Host.UseSystemd();



        builder.Services.AddDbContext<DraftContext>();


        string hourIntervalStrg = builder.Configuration.GetConnectionString("checker-interval-inhours");

        int intervalInHours = int.Parse(hourIntervalStrg);

        if (builder.Configuration.GetConnectionString("enable-job") == "true")
        {


            builder.Services.AddQuartz(q =>
                                {
                                    var jobKey = new JobKey("Run the dataset processing job");
                                    q.AddJob<DailyDataRefresher>(opts =>
                                    {
                                        opts.WithIdentity(jobKey);
                                    });

                                    q.AddTrigger(opts =>
                                    {
                                        opts.ForJob(jobKey)
                                        .WithIdentity(jobKey.Name + "trigger")
                                        .StartNow()
                                        .WithSimpleSchedule(x =>
                                        {
                                            x.WithInterval(TimeSpan.FromHours(intervalInHours))
                                            .RepeatForever();

                                        });
                                    });
                                });

            builder.Services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
                q.AwaitApplicationStarted = true;
            });
        }



        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            using var context = services.GetRequiredService<DraftContext>();

            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                logger.LogWarning("Unhandled error occurred while migrating the database: {e}", e);
                context.Dispose();
                throw;
            }

        }

        app.UseMiddleware<LoggingMiddleware>();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.MapControllers();


        app.Run();
    }
}