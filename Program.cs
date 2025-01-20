using Microsoft.EntityFrameworkCore;
using Quartz;
using draft_data;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Console.WriteLine("crp: --- " + builder.Environment.ContentRootPath);

        // levelSwitch: new LoggingLevelSwitch(LogEventLevel.Warning), outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj} {NewLine} {Exception}"
        var serilogger = new LoggerConfiguration()
            .WriteTo.Async(a =>
                a.File(
                "Logs/log.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                shared: true,
                fileSizeLimitBytes: null,
                levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information))
            )
            .WriteTo.Async(a =>
                a.Console(
                    outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                    levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information)
                )
            )
            .CreateLogger();


        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(serilogger);


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
                    X509Certificate2 pathToCert = new X509Certificate2(builder.Configuration.GetConnectionString("cert-location"));
                    cOpts.ServerCertificate = pathToCert;
                });
            });
        }
        catch (Exception e)
        {
            throw new ArgumentException("Failed to find certificate to use");
        }



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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();


        app.UseRouting();
        app.MapControllers();

        app.UseHttpsRedirection();

        app.Run();
    }
}