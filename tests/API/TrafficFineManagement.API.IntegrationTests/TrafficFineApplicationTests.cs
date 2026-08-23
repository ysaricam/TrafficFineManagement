using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Testcontainers.PostgreSql;

namespace TrafficFineManagement.API.IntegrationTests;

public sealed class TrafficFineApplicationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("traffic_fine_management_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    private WebApplicationFactory<Program> _factory = null!;

    public async Task InitializeAsync()
    {
        await _database.StartAsync();
        _factory = new TestApplicationFactory(_database.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _database.DisposeAsync();
    }

    [Fact]
    public async Task FullWorkflow_ShouldAuthenticateProjectEventsAndEnforceRoles()
    {
        using var adminClient = CreateClient();
        await AssertMigrationHistoryAsync(expectedMigrationCount: 14);

        var bootstrapResponse = await adminClient.PostAsJsonAsync(
            "/api/users/bootstrap",
            new
            {
                Name = "System",
                Surname = "Admin",
                Username = "admin",
                Password = "AdminPassword123!"
            });
        Assert.Equal(HttpStatusCode.Created, bootstrapResponse.StatusCode);

        var secondBootstrapResponse = await adminClient.PostAsJsonAsync(
            "/api/users/bootstrap",
            new
            {
                Name = "Second",
                Surname = "Admin",
                Username = "second.admin",
                Password = "AdminPassword123!"
            });
        Assert.Equal(HttpStatusCode.Conflict, secondBootstrapResponse.StatusCode);

        var wrongPasswordResponse = await adminClient.PostAsJsonAsync(
            "/api/auth/login",
            new { Username = "admin", Password = "WrongPassword123!" });
        Assert.Equal(HttpStatusCode.Unauthorized, wrongPasswordResponse.StatusCode);

        await LoginAsync(adminClient, "admin", "AdminPassword123!");

        var driverId = await CreateUserAsync(
            adminClient, "Driver", "User", "driver", "DriverPassword123!", 0);
        var managerId = await CreateUserAsync(
            adminClient, "Manager", "User", "manager", "ManagerPassword123!", 1);
        await CreateUserAsync(
            adminClient, "Finance", "User", "finance", "FinancePassword123!", 2);
        await CreateUserAsync(
            adminClient, "Fine", "Officer", "fineofficer", "OfficerPassword123!", 4);

        var vehicleResponse = await adminClient.PostAsJsonAsync(
            "/api/vehicles",
            new { Plaka = "34 TEST 001", Brand = "Test", Model = "Vehicle" });
        vehicleResponse.EnsureSuccessStatusCode();
        var vehicleId = await ReadGuidAsync(vehicleResponse, "vehicleId");

        await WaitForTrafficFineProjectionsAsync(
            expectedUserCount: 5,
            vehicleId);

        var usageStartTime = DateTime.UtcNow.AddMinutes(-1);
        var assignManagerResponse = await adminClient.PostAsJsonAsync(
            $"/api/vehicles/{vehicleId}/users",
            new { UserId = managerId, StartTime = usageStartTime });
        Assert.Equal(HttpStatusCode.BadRequest, assignManagerResponse.StatusCode);

        var assignDriverResponse = await adminClient.PostAsJsonAsync(
            $"/api/vehicles/{vehicleId}/users",
            new { UserId = driverId, StartTime = usageStartTime });
        Assert.Equal(HttpStatusCode.NoContent, assignDriverResponse.StatusCode);

        using var driverClient = CreateClient();
        await LoginAsync(driverClient, "driver", "DriverPassword123!");
        var createFineResponse = await driverClient.PostAsJsonAsync(
            "/api/traffic-fines",
            CreateFineRequest());
        createFineResponse.EnsureSuccessStatusCode();
        var fineId = await ReadGuidAsync(createFineResponse, "fineId");

        var newerFineResponse = await driverClient.PostAsJsonAsync(
            "/api/traffic-fines",
            CreateFineRequest());
        newerFineResponse.EnsureSuccessStatusCode();
        var newerFineId = await ReadGuidAsync(newerFineResponse, "fineId");

        using var managerClient = CreateClient();
        await LoginAsync(managerClient, "manager", "ManagerPassword123!");
        var forbiddenResponse = await managerClient.PostAsJsonAsync(
            "/api/traffic-fines",
            CreateFineRequest());
        Assert.Equal(HttpStatusCode.Forbidden, forbiddenResponse.StatusCode);

        using var financeClient = CreateClient();
        await LoginAsync(financeClient, "finance", "FinancePassword123!");
        using var fineOfficerClient = CreateClient();
        await LoginAsync(fineOfficerClient, "fineofficer", "OfficerPassword123!");
        var prematureFinanceRejectResponse = await financeClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/reject",
            new { RejectionReason = "Finance cannot reject before manager approval" });
        Assert.Equal(HttpStatusCode.Forbidden, prematureFinanceRejectResponse.StatusCode);

        var managerApprovalResponse = await managerClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/manager-approval",
            new { Description = "Manager approved" });
        Assert.Equal(HttpStatusCode.NoContent, managerApprovalResponse.StatusCode);

        var orderedFines = await managerClient.GetFromJsonAsync<JsonElement>(
            "/api/traffic-fines");
        Assert.Equal(fineId, orderedFines[0].GetProperty("id").GetGuid());
        Assert.Equal(newerFineId, orderedFines[1].GetProperty("id").GetGuid());

        var lateManagerRejectResponse = await managerClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/reject",
            new { RejectionReason = "Manager cannot reject after manager approval" });
        Assert.Equal(HttpStatusCode.Forbidden, lateManagerRejectResponse.StatusCode);

        var financeApprovalResponse = await financeClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/finance-approval",
            new { Description = "Finance approved" });
        Assert.Equal(HttpStatusCode.NoContent, financeApprovalResponse.StatusCode);

        var rejectAfterApprovalsResponse = await financeClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/reject",
            new { RejectionReason = "Cannot reject after finance approval" });
        Assert.Equal(HttpStatusCode.Forbidden, rejectAfterApprovalsResponse.StatusCode);

        var forbiddenFinanceCompleteResponse = await financeClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/complete",
            new { Description = "Completed" });
        Assert.Equal(HttpStatusCode.Forbidden, forbiddenFinanceCompleteResponse.StatusCode);

        var completeResponse = await fineOfficerClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{fineId}/complete",
            new { Description = "Completed by fine officer" });
        Assert.Equal(HttpStatusCode.NoContent, completeResponse.StatusCode);

        var details = await financeClient.GetFromJsonAsync<JsonElement>(
            $"/api/traffic-fines/{fineId}");
        Assert.Equal(driverId, details.GetProperty("finedUserId").GetGuid());
        Assert.Equal(4, details.GetProperty("currentAction").GetInt32());
        Assert.Equal(1, details.GetProperty("status").GetInt32());
        Assert.Equal(4, details.GetProperty("approvalHistory").GetArrayLength());

        var managerRejectResponse = await managerClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{newerFineId}/reject",
            new { RejectionReason = "Rejected during manager approval" });
        Assert.Equal(HttpStatusCode.NoContent, managerRejectResponse.StatusCode);

        var financeRejectFineResponse = await driverClient.PostAsJsonAsync(
            "/api/traffic-fines",
            CreateFineRequest());
        financeRejectFineResponse.EnsureSuccessStatusCode();
        var financeRejectFineId = await ReadGuidAsync(
            financeRejectFineResponse,
            "fineId");
        var approveForFinanceResponse = await managerClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{financeRejectFineId}/manager-approval",
            new { Description = "Sent to finance" });
        Assert.Equal(HttpStatusCode.NoContent, approveForFinanceResponse.StatusCode);
        var financeRejectResponse = await financeClient.PatchAsJsonAsync(
            $"/api/traffic-fines/{financeRejectFineId}/reject",
            new { RejectionReason = "Rejected during finance approval" });
        Assert.Equal(HttpStatusCode.NoContent, financeRejectResponse.StatusCode);

        var officerCreateFineResponse = await fineOfficerClient.PostAsJsonAsync(
            "/api/traffic-fines",
            CreateFineRequest(vehicleId));
        officerCreateFineResponse.EnsureSuccessStatusCode();
        var officerCreatedFineId = await ReadGuidAsync(
            officerCreateFineResponse,
            "fineId");
        var officerCreatedFine = await fineOfficerClient.GetFromJsonAsync<JsonElement>(
            $"/api/traffic-fines/{officerCreatedFineId}");
        Assert.Equal(driverId, officerCreatedFine.GetProperty("finedUserId").GetGuid());
        Assert.Equal(vehicleId, officerCreatedFine.GetProperty("vehicleId").GetGuid());

        var completeUsageResponse = await managerClient.PatchAsJsonAsync(
            $"/api/vehicles/{vehicleId}/users/{driverId}/complete",
            new { EndTime = DateTime.UtcNow });
        Assert.Equal(HttpStatusCode.NoContent, completeUsageResponse.StatusCode);

        var reassignDriverResponse = await managerClient.PostAsJsonAsync(
            $"/api/vehicles/{vehicleId}/users",
            new { UserId = driverId, StartTime = DateTime.UtcNow });
        Assert.Equal(HttpStatusCode.NoContent, reassignDriverResponse.StatusCode);

        var vehicleDetails = await managerClient.GetFromJsonAsync<JsonElement>(
            $"/api/vehicles/{vehicleId}");
        var vehicleUsages = vehicleDetails.GetProperty("users");
        Assert.Equal(2, vehicleUsages.GetArrayLength());
        Assert.NotEqual(
            vehicleUsages[0].GetProperty("startTime").GetDateTime(),
            vehicleUsages[1].GetProperty("startTime").GetDateTime());
        Assert.Equal(JsonValueKind.Null, vehicleUsages[1].GetProperty("endTime").ValueKind);

        await AssertMigrationHistoryAsync(expectedMigrationCount: 14);
    }

    private HttpClient CreateClient()
    {
        return _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });
    }

    private static async Task LoginAsync(
        HttpClient client,
        string username,
        string password)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { Username = username, Password = password });
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static async Task<Guid> CreateUserAsync(
        HttpClient client,
        string name,
        string surname,
        string username,
        string password,
        int role)
    {
        var response = await client.PostAsJsonAsync(
            "/api/users",
            new { Name = name, Surname = surname, Username = username, Password = password, Role = role });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return await ReadGuidAsync(response, "userId");
    }

    private static object CreateFineRequest(Guid? vehicleId = null)
    {
        return new
        {
            VehicleId = vehicleId,
            Amount = 1500m,
            Currency = "TRY",
            ViolationCode = "TEST-001",
            Reason = "Integration test",
            FineDate = DateTime.UtcNow
        };
    }

    private static async Task<Guid> ReadGuidAsync(
        HttpResponseMessage response,
        string propertyName)
    {
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty(propertyName).GetGuid();
    }

    private async Task WaitForTrafficFineProjectionsAsync(
        int expectedUserCount,
        Guid vehicleId)
    {
        var deadline = DateTime.UtcNow.AddSeconds(20);

        while (DateTime.UtcNow < deadline)
        {
            await using var connection = new NpgsqlConnection(
                _database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                """
                SELECT
                    (SELECT COUNT(*) FROM traffic_fines."Users") >= @UserCount
                    AND EXISTS
                    (
                        SELECT 1
                        FROM traffic_fines."Vehicles"
                        WHERE "Id" = @VehicleId
                    )
                """,
                connection);
            command.Parameters.AddWithValue("UserCount", expectedUserCount);
            command.Parameters.AddWithValue("VehicleId", vehicleId);

            if (await command.ExecuteScalarAsync() is true)
            {
                return;
            }

            await Task.Delay(250);
        }

        throw new TimeoutException(
            "Users and vehicle were not projected to the TrafficFine module in time.");
    }

    private async Task AssertMigrationHistoryAsync(int expectedMigrationCount)
    {
        await using var connection = new NpgsqlConnection(
            _database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT COUNT(*) FROM app.\"SchemaMigrations\"",
            connection);
        var count = Convert.ToInt32(await command.ExecuteScalarAsync());
        Assert.Equal(expectedMigrationCount, count);
    }

    private sealed class TestApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;

        public TestApplicationFactory(string connectionString)
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
                    "UsersConnectionString");
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:VehiclesConnectionString"] = _connectionString,
                    ["ConnectionStrings:TrafficFineConnectionString"] = _connectionString,
                    ["ConnectionStrings:UsersConnectionString"] = _connectionString,
                    ["DatabaseMigrations:Enabled"] = "true",
                    ["DatabaseMigrations:ConnectionStringName"] = "UsersConnectionString"
                });
            });
        }
    }
}
