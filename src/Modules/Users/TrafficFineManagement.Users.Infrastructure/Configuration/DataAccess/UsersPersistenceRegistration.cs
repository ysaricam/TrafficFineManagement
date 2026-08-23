using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagament.BuildingBlocks.Infrastructure;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Domain.Users;
using TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Users.Infrastructure.Domain.Users;
using TrafficFineManagement.Modules.Users.Infrastructure.Security;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.DataAccess;

public static class UsersPersistenceRegistration
{
    public static IServiceCollection AddUsersPersistence(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<UsersContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.ReplaceService<IValueConverterSelector,
                StronglyTypedIdValueConverterSelector>();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UsersDomainEventsDispatcher>();
        services.AddSingleton<UsersDomainNotificationsMapper>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        var connectionFactory = new UsersSqlConnectionFactory(connectionString);
        services.AddSingleton<IUsersSqlConnectionFactory>(connectionFactory);

        return services;
    }
}
