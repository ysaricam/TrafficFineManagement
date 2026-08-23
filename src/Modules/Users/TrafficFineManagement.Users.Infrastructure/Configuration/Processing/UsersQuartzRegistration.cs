using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing.Outbox;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

public static class UsersQuartzRegistration
{
    private static readonly TimeSpan OutboxPollingInterval = TimeSpan.FromSeconds(2);

    public static IServiceCollection AddUsersQuartz(this IServiceCollection services)
    {
        var jobKey = new JobKey("Users-ProcessOutbox");

        services.AddQuartz(configuration =>
        {
            configuration.AddJob<ProcessOutboxJob>(options =>
                options.WithIdentity(jobKey));
            configuration.AddTrigger(options => options
                .ForJob(jobKey)
                .WithIdentity("Users-ProcessOutbox-trigger")
                .StartNow()
                .WithSimpleSchedule(schedule => schedule
                    .WithInterval(OutboxPollingInterval)
                    .RepeatForever()));
        });

        return services;
    }
}
