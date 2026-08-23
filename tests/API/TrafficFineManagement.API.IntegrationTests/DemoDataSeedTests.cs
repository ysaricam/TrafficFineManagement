using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace TrafficFineManagement.API.IntegrationTests;

public sealed class DemoDataSeedTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("traffic_fine_management_demo_seed_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    private WebApplicationFactory<Program> _factory = null!;

    public async Task InitializeAsync()
    {
        await _database.StartAsync();
        _factory = new DemoSeedApplicationFactory(_database.GetConnectionString());
        using var client = _factory.CreateClient();
        using var response = await client.GetAsync("/");
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _database.DisposeAsync();
    }

    [Fact]
    public async Task DemoSeed_ShouldProvideAllTestScenarios()
    {
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        Assert.True(configuration.GetValue<bool>("UserSeed:Enabled"));
        Assert.True(configuration.GetValue<bool>("DemoDataSeed:Enabled"));

        await using var connection = new NpgsqlConnection(
            _database.GetConnectionString());
        await connection.OpenAsync();

        Assert.Equal(4, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(*)
            FROM users."Users"
            WHERE "Username" IN
                ('driver.ayse', 'driver.mehmet', 'driver.elif', 'driver.can')
            """));

        Assert.Equal(4, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(*)
            FROM vehicles."Vehicles"
            WHERE "Plaka" LIKE '34 DEMO %'
            """));
        Assert.Equal(4, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(DISTINCT "Type")
            FROM vehicles."Vehicles"
            WHERE "Plaka" LIKE '34 DEMO %'
            """));
        Assert.Equal(1, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(*)
            FROM vehicles."Vehicles"
            WHERE "Plaka" LIKE '34 DEMO %'
              AND "Status" = TRUE
            """));
        Assert.Equal(4, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(DISTINCT vehicle."Id")
            FROM vehicles."Vehicles" AS vehicle
            JOIN vehicles."VehicleUsers" AS usage
              ON usage."VehicleId" = vehicle."Id"
            WHERE vehicle."Plaka" LIKE '34 DEMO %'
              AND usage."EndTime" IS NOT NULL
            """));

        Assert.Equal(5, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(*)
            FROM traffic_fines."Fines"
            WHERE "ViolationCode" LIKE 'DEMO-%'
            """));
        Assert.Equal(5, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(DISTINCT "CurrentAction")
            FROM traffic_fines."Fines"
            WHERE "ViolationCode" LIKE 'DEMO-%'
            """));
        Assert.Equal(5, await ExecuteCountAsync(connection,
            """
            SELECT COUNT(*)
            FROM traffic_fines."Fines" AS fine
            WHERE fine."ViolationCode" LIKE 'DEMO-%'
              AND EXISTS
              (
                  SELECT 1
                  FROM vehicles."VehicleUsers" AS usage
                  WHERE usage."VehicleId" = fine."VehicleId"
                    AND usage."UserId" = fine."FinedUserId"
                    AND fine."FineDate" >= usage."StartTime"
                    AND (usage."EndTime" IS NULL OR fine."FineDate" <= usage."EndTime")
              )
            """));
    }

    private static async Task<int> ExecuteCountAsync(
        NpgsqlConnection connection,
        string sql)
    {
        await using var command = new NpgsqlCommand(sql, connection);
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    private sealed class DemoSeedApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;

        public DemoSeedApplicationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .UseSetting(
                    "ConnectionStrings:VehiclesConnectionString",
                    _connectionString)
                .UseSetting(
                    "ConnectionStrings:TrafficFineConnectionString",
                    _connectionString)
                .UseSetting(
                    "ConnectionStrings:UsersConnectionString",
                    _connectionString)
                .UseSetting("DatabaseMigrations:Enabled", "true")
                .UseSetting(
                    "DatabaseMigrations:ConnectionStringName",
                    "UsersConnectionString")
                .UseSetting("Quartz:Enabled", "false")
                .UseSetting("UserSeed:Enabled", "true")
                .UseSetting("UserSeed:Password", "Test123!")
                .UseSetting("DemoDataSeed:Enabled", "true");
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:VehiclesConnectionString"] = _connectionString,
                    ["ConnectionStrings:TrafficFineConnectionString"] = _connectionString,
                    ["ConnectionStrings:UsersConnectionString"] = _connectionString,
                    ["DatabaseMigrations:Enabled"] = "true",
                    ["DatabaseMigrations:ConnectionStringName"] = "UsersConnectionString",
                    ["Quartz:Enabled"] = "false",
                    ["UserSeed:Enabled"] = "true",
                    ["UserSeed:Password"] = "Test123!",
                    ["DemoDataSeed:Enabled"] = "true"
                });
            });
        }
    }
}
