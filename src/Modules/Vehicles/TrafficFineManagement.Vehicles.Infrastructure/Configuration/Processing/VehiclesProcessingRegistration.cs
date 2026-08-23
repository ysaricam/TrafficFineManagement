using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Validation;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;

public static class VehiclesProcessingRegistration
{
    public static IServiceCollection AddVehiclesProcessing(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(
                typeof(IVehiclesModule).Assembly,
                typeof(VehiclesProcessingRegistration).Assembly));

        services.AddTransient<IValidator<VehicleCommand>, VehicleCommandValidator>();
        services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddTransient<IValidator<AddUserToVehicleCommand>, AddUserToVehicleCommandValidator>();
        services.AddTransient<IValidator<CompleteVehicleUsageCommand>, CompleteVehicleUsageCommandValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return services;
    }
}
