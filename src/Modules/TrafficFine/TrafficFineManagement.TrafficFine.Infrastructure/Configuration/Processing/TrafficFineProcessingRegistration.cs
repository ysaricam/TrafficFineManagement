using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.IntegrationEvents;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;

public static class TrafficFineProcessingRegistration
{
    public static IServiceCollection AddTrafficFineProcessing(
        this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(
                typeof(ITrafficFineModule).Assembly,
                typeof(TrafficFineProcessingRegistration).Assembly));

        services.AddScoped<ITrafficFineModule, TrafficFineModule>();
        services.AddTransient<IValidator<CreateFineCommand>, CreateFineCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        services.AddSingleton<UserCreatedIntegrationEventHandler>();
        services.AddSingleton<VehicleCreatedIntegrationEventHandler>();
        services.AddHostedService<TrafficFineIntegrationEventsHostedService>();

        return services;
    }
}
