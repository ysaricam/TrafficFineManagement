using System.Data;
using Npgsql;
using TrafficFineManagement.BuildingBlocks.Application.Data;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.Data;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
