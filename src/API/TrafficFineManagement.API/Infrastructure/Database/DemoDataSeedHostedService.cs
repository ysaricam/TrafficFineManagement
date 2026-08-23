using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Users.SynchronizeUser;
using TrafficFineManagement.Modules.TrafficFine.Application.Vehicles.SynchronizeVehicle;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Users.SynchronizeUser;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;
using FineActionType = TrafficFineManagement.Modules.TrafficFine.Domain.Fines.FineActionType;
using TrafficFineUserRole = TrafficFineManagement.Modules.TrafficFine.Domain.Users.UserRole;
using UserDto = TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers.UserDto;
using VehicleDto = TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles.VehicleDto;
using VehicleType = TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType;
using VehiclesUserRole = TrafficFineManagement.Modules.Vehicles.Domain.Users.UserRole;

namespace TrafficFineManagement.API.Infrastructure.Database;

public sealed class DemoDataSeedHostedService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DemoDataSeedHostedService> _logger;

    public DemoDataSeedHostedService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<DemoDataSeedHostedService> logger)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue<bool>("DemoDataSeed:Enabled"))
        {
            return;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var usersModule = scope.ServiceProvider.GetRequiredService<IUsersModule>();
        var vehiclesModule = scope.ServiceProvider.GetRequiredService<IVehiclesModule>();
        var trafficFineModule = scope.ServiceProvider.GetRequiredService<ITrafficFineModule>();

        var users = await usersModule.ExecuteQueryAsync(
            new GetAllUsersQuery(),
            cancellationToken);
        var usersByUsername = users.ToDictionary(
            user => user.Username,
            StringComparer.OrdinalIgnoreCase);

        var requiredUsernames = new[]
        {
            "driver", "driver.ayse", "driver.mehmet", "driver.elif", "driver.can",
            "manager", "finance", "admin", "fineofficer"
        };

        foreach (var username in requiredUsernames)
        {
            if (!usersByUsername.ContainsKey(username))
            {
                throw new InvalidOperationException(
                    $"Demo data seed requires the '{username}' seed user.");
            }
        }

        foreach (var user in usersByUsername.Values
                     .Where(user => requiredUsernames.Contains(
                         user.Username,
                         StringComparer.OrdinalIgnoreCase)))
        {
            await vehiclesModule.ExecuteCommandAsync(
                new TrafficFineManagement.Modules.Vehicles.Application.Users
                    .SynchronizeUser.SynchronizeUserCommand(
                        user.Id,
                        (VehiclesUserRole)(int)user.Role),
                cancellationToken);
            await trafficFineModule.ExecuteCommandAsync(
                new TrafficFineManagement.Modules.TrafficFine.Application.Users
                    .SynchronizeUser.SynchronizeUserCommand(
                        user.Id,
                        (TrafficFineUserRole)(int)user.Role),
                cancellationToken);
        }

        var vehicleSeeds = new[]
        {
            new SeedVehicle("34 DEMO 001", "Toyota", "Corolla", VehicleType.Passenger),
            new SeedVehicle("34 DEMO 002", "Volvo", "FH16", VehicleType.Tractor),
            new SeedVehicle("34 DEMO 003", "Schmitz", "S.CF", VehicleType.Trailer),
            new SeedVehicle("34 DEMO 004", "Renault", "Clio", VehicleType.Rental)
        };

        var existingVehicles = await vehiclesModule.ExecuteQueryAsync(
            new GetAllVehiclesQuery(),
            cancellationToken);
        var vehiclesByPlate = existingVehicles.ToDictionary(
            vehicle => vehicle.Plaka,
            StringComparer.OrdinalIgnoreCase);

        foreach (var seed in vehicleSeeds)
        {
            if (vehiclesByPlate.ContainsKey(seed.Plaka))
            {
                continue;
            }

            var vehicleId = await vehiclesModule.ExecuteCommandAsync(
                new VehicleCommand(seed.Plaka, seed.Brand, seed.Model, seed.Type),
                cancellationToken);
            vehiclesByPlate[seed.Plaka] = new VehicleDto
            {
                Id = vehicleId,
                Plaka = seed.Plaka,
                Brand = seed.Brand,
                Model = seed.Model,
                Type = seed.Type
            };
        }

        foreach (var vehicle in vehiclesByPlate.Values
                     .Where(vehicle => vehicleSeeds.Any(seed =>
                         seed.Plaka.Equals(vehicle.Plaka, StringComparison.OrdinalIgnoreCase))))
        {
            await trafficFineModule.ExecuteCommandAsync(
                new SynchronizeVehicleCommand(vehicle.Id),
                cancellationToken);
        }

        var now = DateTime.UtcNow;
        var passenger = vehiclesByPlate["34 DEMO 001"];
        var tractor = vehiclesByPlate["34 DEMO 002"];
        var trailer = vehiclesByPlate["34 DEMO 003"];
        var rental = vehiclesByPlate["34 DEMO 004"];

        await EnsureHistoricalUsageAsync(
            vehiclesModule, passenger, usersByUsername["driver.can"],
            now.AddDays(-45), now.AddDays(-35), cancellationToken);
        await EnsureHistoricalUsageAsync(
            vehiclesModule, tractor, usersByUsername["driver.ayse"],
            now.AddDays(-30), now.AddDays(-20), cancellationToken);
        await EnsureHistoricalUsageAsync(
            vehiclesModule, trailer, usersByUsername["driver.mehmet"],
            now.AddDays(-20), now.AddDays(-10), cancellationToken);
        await EnsureHistoricalUsageAsync(
            vehiclesModule, rental, usersByUsername["driver.elif"],
            now.AddDays(-10), now.AddDays(-5), cancellationToken);
        await EnsureActiveUsageAsync(
            vehiclesModule, passenger, usersByUsername["driver"],
            now.AddDays(-7), cancellationToken);

        var existingFines = await trafficFineModule.ExecuteQueryAsync(
            new GetAllFinesQuery(),
            cancellationToken);
        var finesByCode = existingFines.ToDictionary(
            fine => fine.ViolationCode,
            StringComparer.OrdinalIgnoreCase);

        var manager = usersByUsername["manager"];
        var finance = usersByUsername["finance"];
        var fineOfficer = usersByUsername["fineofficer"];

        var fineSeeds = new[]
        {
            new SeedFine("DEMO-CREATED", "Hız sınırı ihlali", passenger,
                usersByUsername["driver"], now.AddDays(-1), FineActionType.Created),
            new SeedFine("DEMO-MANAGER", "Hatalı şerit kullanımı", tractor,
                usersByUsername["driver.ayse"], now.AddDays(-25), FineActionType.ManagerApproved),
            new SeedFine("DEMO-FINANCE", "Kırmızı ışık ihlali", trailer,
                usersByUsername["driver.mehmet"], now.AddDays(-15), FineActionType.FinanceApproved),
            new SeedFine("DEMO-REJECTED", "Park yasağı ihlali", rental,
                usersByUsername["driver.elif"], now.AddDays(-7), FineActionType.Rejected),
            new SeedFine("DEMO-COMPLETED", "Takip mesafesi ihlali", passenger,
                usersByUsername["driver.can"], now.AddDays(-40), FineActionType.Completed)
        };

        foreach (var seed in fineSeeds)
        {
            await EnsureFineAsync(
                trafficFineModule,
                finesByCode,
                seed,
                fineOfficer,
                manager,
                finance,
                cancellationToken);
        }

        _logger.LogInformation(
            "Demo data seed completed with {VehicleCount} vehicles and {FineCount} fine scenarios.",
            vehicleSeeds.Length,
            fineSeeds.Length);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task EnsureHistoricalUsageAsync(
        IVehiclesModule vehiclesModule,
        VehicleDto vehicle,
        UserDto driver,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken)
    {
        var details = await vehiclesModule.ExecuteQueryAsync(
            new GetVehicleQuery(vehicle.Id),
            cancellationToken);
        if (details is null || details.Users.Any(usage =>
                usage.UserId == driver.Id && usage.EndTime.HasValue))
        {
            return;
        }

        if (details.Status)
        {
            return;
        }

        await vehiclesModule.ExecuteCommandAsync(
            new AddUserToVehicleCommand(vehicle.Id, driver.Id, startTime),
            cancellationToken);
        await vehiclesModule.ExecuteCommandAsync(
            new CompleteVehicleUsageCommand(vehicle.Id, driver.Id, endTime),
            cancellationToken);
    }

    private static async Task EnsureActiveUsageAsync(
        IVehiclesModule vehiclesModule,
        VehicleDto vehicle,
        UserDto driver,
        DateTime startTime,
        CancellationToken cancellationToken)
    {
        var details = await vehiclesModule.ExecuteQueryAsync(
            new GetVehicleQuery(vehicle.Id),
            cancellationToken);
        if (details is null || details.Users.Any(usage =>
                usage.UserId == driver.Id && !usage.EndTime.HasValue))
        {
            return;
        }

        if (details.Status)
        {
            return;
        }

        await vehiclesModule.ExecuteCommandAsync(
            new AddUserToVehicleCommand(vehicle.Id, driver.Id, startTime),
            cancellationToken);
    }

    private static async Task EnsureFineAsync(
        ITrafficFineModule trafficFineModule,
        IDictionary<string, FineDto> finesByCode,
        SeedFine seed,
        UserDto fineOfficer,
        UserDto manager,
        UserDto finance,
        CancellationToken cancellationToken)
    {
        if (!finesByCode.TryGetValue(seed.Code, out var fine))
        {
            var fineId = await trafficFineModule.ExecuteCommandAsync(
                new CreateFineCommand(
                    seed.Driver.Id,
                    seed.Vehicle.Id,
                    1500m,
                    "TRY",
                    seed.Code,
                    seed.Reason,
                    seed.FineDate,
                    fineOfficer.Id),
                cancellationToken);
            fine = new FineDto
            {
                Id = fineId,
                ViolationCode = seed.Code,
                CurrentAction = FineActionType.Created
            };
            finesByCode[seed.Code] = fine;
        }

        var currentAction = fine.CurrentAction;
        if (seed.TargetAction == FineActionType.Rejected &&
            currentAction is FineActionType.Created or FineActionType.ManagerApproved)
        {
            await trafficFineModule.ExecuteCommandAsync(
                new RejectFineCommand(
                    fine.Id,
                    manager.Id,
                    "Demo senaryosu: yönetici tarafından reddedildi."),
                cancellationToken);
            return;
        }

        if (currentAction == FineActionType.Created &&
            seed.TargetAction is FineActionType.ManagerApproved
                or FineActionType.FinanceApproved
                or FineActionType.Completed)
        {
            await trafficFineModule.ExecuteCommandAsync(
                new ApproveFineByManagerCommand(
                    fine.Id,
                    manager.Id,
                    "Demo senaryosu: yönetici onayı."),
                cancellationToken);
            currentAction = FineActionType.ManagerApproved;
        }

        if (currentAction == FineActionType.ManagerApproved &&
            seed.TargetAction is FineActionType.FinanceApproved
                or FineActionType.Completed)
        {
            await trafficFineModule.ExecuteCommandAsync(
                new ApproveFineByFinanceCommand(
                    fine.Id,
                    finance.Id,
                    "Demo senaryosu: finans onayı."),
                cancellationToken);
            currentAction = FineActionType.FinanceApproved;
        }

        if (currentAction == FineActionType.FinanceApproved &&
            seed.TargetAction == FineActionType.Completed)
        {
            await trafficFineModule.ExecuteCommandAsync(
                new CompleteFineCommand(
                    fine.Id,
                    fineOfficer.Id,
                    "Demo senaryosu: süreç tamamlandı."),
                cancellationToken);
        }
    }

    private sealed record SeedVehicle(
        string Plaka,
        string Brand,
        string Model,
        VehicleType Type);

    private sealed record SeedFine(
        string Code,
        string Reason,
        VehicleDto Vehicle,
        UserDto Driver,
        DateTime FineDate,
        FineActionType TargetAction);
}
