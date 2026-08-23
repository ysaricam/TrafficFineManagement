using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.API.Infrastructure.Database;

public sealed class UserSeedHostedService : IHostedService
{
    private static readonly SeedUser[] SeedUsers =
    [
        new("Test", "Şoför", "driver", UserRole.Driver),
        new("Test", "Yönetici", "manager", UserRole.Manager),
        new("Test", "Finansçı", "finance", UserRole.Finance),
        new("Test", "Admin", "admin", UserRole.Admin),
        new("Test", "Ceza Görevlisi", "fineofficer", UserRole.FineOfficer)
    ];

    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UserSeedHostedService> _logger;

    public UserSeedHostedService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<UserSeedHostedService> logger)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue<bool>("UserSeed:Enabled"))
        {
            return;
        }

        var password = _configuration["UserSeed:Password"];
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "UserSeed is enabled but UserSeed:Password is not configured.");
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var usersModule = scope.ServiceProvider.GetRequiredService<IUsersModule>();
        var existingUsers = await usersModule.ExecuteQueryAsync(
            new GetAllUsersQuery(),
            cancellationToken);
        var existingUsernames = existingUsers
            .Select(user => user.Username)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var seedUser in SeedUsers)
        {
            if (existingUsernames.Contains(seedUser.Username))
            {
                continue;
            }

            await usersModule.ExecuteCommandAsync(
                new CreateUserCommand(
                    seedUser.Name,
                    seedUser.Surname,
                    seedUser.Username,
                    password,
                    seedUser.Role),
                cancellationToken);

            _logger.LogInformation(
                "Seed user {Username} was created with role {Role}.",
                seedUser.Username,
                seedUser.Role);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private sealed record SeedUser(
        string Name,
        string Surname,
        string Username,
        UserRole Role);
}
