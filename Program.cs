using Microsoft.EntityFrameworkCore;
using Quartz;
using draft_data;
using Serilog;
using Serilog.Core;
using Serilog.Events;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // levelSwitch: new LoggingLevelSwitch(LogEventLevel.Warning), outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj} {NewLine} {Exception}"
        var serilogger = new LoggerConfiguration()
            .WriteTo.Async(a =>
                a.File(builder.Environment.ContentRootPath + "Logs/log.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                shared: true,
                fileSizeLimitBytes: null,
                levelSwitch: new LoggingLevelSwitch(LogEventLevel.Warning))
            )
            .WriteTo.Async(a =>
                a.Console(
                    outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                    levelSwitch: new LoggingLevelSwitch(LogEventLevel.Warning)
                )
            ).Filter.ByExcluding((logEvent) =>
            {
                string message = logEvent.MessageTemplate.Render(logEvent.Properties).ToLower();

                if (message.Contains("select "))
                {
                    if
                    (
                        message.Contains("insert ") || message.Contains("update ") || message.ToLower().Contains("delete ")
                    )
                    {
                        return false;
                    }
                    else
                    {
                        // exclude because doesnt contain data modification statements
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            })
            .CreateLogger();



        builder.Logging.AddSerilog(serilogger);


        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddControllers();


        builder.Services.AddDbContext<DraftContext>();


        string hourIntervalStrg = builder.Configuration.GetConnectionString("checker-interval-inhours");

        int intervalInHours = int.Parse(hourIntervalStrg);

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


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.MapControllers();

        app.UseHttpsRedirection();

        app.Run();
    }
}