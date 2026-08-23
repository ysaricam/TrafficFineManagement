using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;

public static class TrafficFineQuartzRegistration
{
    private static readonly TimeSpan OutboxPollingInterval = TimeSpan.FromSeconds(2);

    public static IServiceCollection AddTrafficFineQuartz(this IServiceCollection services)
    {
        var jobKey = new JobKey("TrafficFine-ProcessOutbox");

        services.AddQuartz(configuration =>
        {
            configuration.AddJob<ProcessOutboxJob>(options => options.WithIdentity(jobKey));
            configuration.AddTrigger(options => options
                .ForJob(jobKey)
                .WithIdentity("TrafficFine-ProcessOutbox-trigger")
                .StartNow()
                .WithSimpleSchedule(schedule => schedule
                    .WithInterval(OutboxPollingInterval)
                    .RepeatForever()));
        });

        return services;
    }
}
