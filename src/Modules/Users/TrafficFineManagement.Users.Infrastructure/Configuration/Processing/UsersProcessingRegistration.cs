using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;
using TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

public static class UsersProcessingRegistration
{
    public static IServiceCollection AddUsersProcessing(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(
                typeof(IUsersModule).Assembly,
                typeof(UsersProcessingRegistration).Assembly));

        services.AddScoped<IUsersModule, UsersModule>();
        services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddTransient<IValidator<AuthenticateUserQuery>, AuthenticateUserQueryValidator>();
        services.AddTransient<IValidator<BootstrapAdminCommand>, BootstrapAdminCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return services;
    }
}
