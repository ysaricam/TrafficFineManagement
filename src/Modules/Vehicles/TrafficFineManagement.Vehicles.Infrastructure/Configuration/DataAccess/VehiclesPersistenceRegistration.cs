using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagament.BuildingBlocks.Infrastructure;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.DataAccess;

public static class VehiclesPersistenceRegistration
{
    public static IServiceCollection AddVehiclesPersistence(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<VehiclesContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, VehiclesUnitOfWork>();

        return services;
    }
}
