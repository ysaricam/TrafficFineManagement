using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagament.BuildingBlocks.Infrastructure;
using TrafficFineManagement.BuildingBlocks.Infrastructure.Data;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Vehicles;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.DataAccess;

public static class TrafficFinePersistenceRegistration
{
    public static IServiceCollection AddTrafficFinePersistence(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<TrafficFineContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.AddScoped<IFineRepository, FineRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<TrafficFineDomainEventsDispatcher>();
        services.AddSingleton<TrafficFineDomainNotificationsMapper>();
        var sqlConnectionFactory = new TrafficFineSqlConnectionFactory(connectionString);
        services.AddSingleton(sqlConnectionFactory);
        services.AddSingleton<ITrafficFineSqlConnectionFactory>(sqlConnectionFactory);

        return services;
    }
}
