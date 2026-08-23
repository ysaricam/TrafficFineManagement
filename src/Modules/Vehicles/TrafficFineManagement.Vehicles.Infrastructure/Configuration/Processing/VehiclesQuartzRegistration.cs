using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing.Outbox;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;

public static class VehiclesQuartzRegistration
{
    private static readonly TimeSpan OutboxPollingInterval =
        TimeSpan.FromSeconds(2);

    public static IServiceCollection AddVehiclesQuartz(
        this IServiceCollection services)
    {
        var processOutboxJobKey = new JobKey(nameof(ProcessOutboxJob));

        services.AddQuartz(configuration =>
        {
            configuration.SchedulerName = "Vehicles";

            configuration.AddJob<ProcessOutboxJob>(options =>
                options.WithIdentity(processOutboxJobKey));

            configuration.AddTrigger(options => options
                .ForJob(processOutboxJobKey)
                .WithIdentity($"{nameof(ProcessOutboxJob)}-trigger")
                .StartNow()
                .WithSimpleSchedule(schedule => schedule
                    .WithInterval(OutboxPollingInterval)
                    .RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
