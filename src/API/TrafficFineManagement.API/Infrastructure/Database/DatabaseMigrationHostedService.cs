using System.Security.Cryptography;
using System.Text;
using Npgsql;

namespace TrafficFineManagement.API.Infrastructure.Database;

public sealed class DatabaseMigrationHostedService : IHostedService
{
    private const long AdvisoryLockKey = 7_641_903_241_325;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<DatabaseMigrationHostedService> _logger;

    public DatabaseMigrationHostedService(
        IConfiguration configuration,
        IHostEnvironment environment,
        ILogger<DatabaseMigrationHostedService> logger)
    {
        _configuration = configuration;
        _environment = environment;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue<bool>("DatabaseMigrations:Enabled"))
        {
            _logger.LogInformation("Automatic database migrations are disabled.");
            return;
        }

        var connectionStringName = _configuration[
                "DatabaseMigrations:ConnectionStringName"]
            ?? "UsersConnectionString";
        var connectionString = _configuration.GetConnectionString(
                connectionStringName)
            ?? throw new InvalidOperationException(
                $"Migration connection string '{connectionStringName}' is not configured.");
        var scriptsDirectory = ResolveScriptsDirectory();

        if (!Directory.Exists(scriptsDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Database migration directory '{scriptsDirectory}' was not found " +
                $"for environment '{_environment.EnvironmentName}'.");
        }

        var scriptPaths = Directory.GetFiles(scriptsDirectory, "*.sql")
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await AcquireLockAsync(connection, cancellationToken);

        try
        {
            await EnsureHistoryTableAsync(connection, cancellationToken);

            foreach (var scriptPath in scriptPaths)
            {
                await ApplyScriptAsync(connection, scriptPath, cancellationToken);
            }
        }
        finally
        {
            await ReleaseLockAsync(connection, cancellationToken);
        }
    }

    private string ResolveScriptsDirectory()
    {
        var outputDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "Database",
            "Scripts");

        if (Directory.Exists(outputDirectory))
        {
            return outputDirectory;
        }

        return Path.GetFullPath(Path.Combine(
            _environment.ContentRootPath,
            "..",
            "..",
            "Database",
            "Scripts"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task AcquireLockAsync(
        NpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT pg_advisory_lock(@LockKey)", connection);
        command.Parameters.AddWithValue("LockKey", AdvisoryLockKey);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task ReleaseLockAsync(
        NpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT pg_advisory_unlock(@LockKey)", connection);
        command.Parameters.AddWithValue("LockKey", AdvisoryLockKey);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task EnsureHistoryTableAsync(
        NpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        const string sql = """
            CREATE SCHEMA IF NOT EXISTS app;
            CREATE TABLE IF NOT EXISTS app."SchemaMigrations"
            (
                "Name" character varying(255) NOT NULL,
                "Checksum" character varying(64) NOT NULL,
                "AppliedOn" timestamp with time zone NOT NULL,
                CONSTRAINT "PK_SchemaMigrations" PRIMARY KEY ("Name")
            );
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task ApplyScriptAsync(
        NpgsqlConnection connection,
        string scriptPath,
        CancellationToken cancellationToken)
    {
        var name = Path.GetFileName(scriptPath);
        var originalScript = await File.ReadAllTextAsync(
            scriptPath,
            cancellationToken);
        var checksum = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(originalScript)));
        var appliedChecksum = await GetAppliedChecksumAsync(
            connection,
            name,
            cancellationToken);

        if (appliedChecksum is not null)
        {
            if (!string.Equals(appliedChecksum, checksum, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Applied migration '{name}' has been modified.");
            }

            return;
        }

        var executableScript = RemoveTransactionMarkers(originalScript);
        await using var transaction = await connection.BeginTransactionAsync(
            cancellationToken);

        await using (var migrationCommand = new NpgsqlCommand(
                         executableScript,
                         connection,
                         transaction))
        {
            await migrationCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        const string insertSql = """
            INSERT INTO app."SchemaMigrations" ("Name", "Checksum", "AppliedOn")
            VALUES (@Name, @Checksum, @AppliedOn)
            """;
        await using (var historyCommand = new NpgsqlCommand(
                         insertSql,
                         connection,
                         transaction))
        {
            historyCommand.Parameters.AddWithValue("Name", name);
            historyCommand.Parameters.AddWithValue("Checksum", checksum);
            historyCommand.Parameters.AddWithValue("AppliedOn", DateTime.UtcNow);
            await historyCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        _logger.LogInformation("Applied database migration {MigrationName}.", name);
    }

    private static async Task<string?> GetAppliedChecksumAsync(
        NpgsqlConnection connection,
        string name,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Checksum"
            FROM app."SchemaMigrations"
            WHERE "Name" = @Name
            """;
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("Name", name);
        return await command.ExecuteScalarAsync(cancellationToken) as string;
    }

    private static string RemoveTransactionMarkers(string script)
    {
        return string.Join(
            Environment.NewLine,
            script.Split('\n')
                .Where(line =>
                {
                    var value = line.Trim();
                    return !value.Equals("BEGIN;", StringComparison.OrdinalIgnoreCase) &&
                           !value.Equals("COMMIT;", StringComparison.OrdinalIgnoreCase);
                }));
    }
}
