using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.IntegrationEvents;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Users.SynchronizeUser;
using TrafficFineManagement.Modules.TrafficFine.Application.Vehicles.SynchronizeVehicle;

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
        services.AddTransient<IValidator<ApproveFineByManagerCommand>, ApproveFineByManagerCommandValidator>();
        services.AddTransient<IValidator<ApproveFineByFinanceCommand>, ApproveFineByFinanceCommandValidator>();
        services.AddTransient<IValidator<RejectFineCommand>, RejectFineCommandValidator>();
        services.AddTransient<IValidator<CompleteFineCommand>, CompleteFineCommandValidator>();
        services.AddTransient<IValidator<SynchronizeUserCommand>, SynchronizeUserCommandValidator>();
        services.AddTransient<IValidator<SynchronizeVehicleCommand>, SynchronizeVehicleCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        services.AddSingleton<UserCreatedIntegrationEventHandler>();
        services.AddSingleton<VehicleCreatedIntegrationEventHandler>();
        services.AddHostedService<TrafficFineIntegrationEventsHostedService>();

        return services;
    }
}
