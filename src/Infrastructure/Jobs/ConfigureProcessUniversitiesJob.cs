using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.Jobs;

internal sealed class ConfigureProcessUniversitiesJob : IConfigureOptions<QuartzOptions>
{
    // Better to appsettings
    private const int HourInteval = 24;

    public void Configure(QuartzOptions options)
    {
        string jobName = typeof(ConfigureProcessUniversitiesJob).FullName!;

        options
            .AddJob<ProcessUniversitiesJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure.ForJob(jobName)
                .WithSimpleSchedule(schedule => 
                    schedule.WithIntervalInHours(HourInteval).RepeatForever()));
    }
}
