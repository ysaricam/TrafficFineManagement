using System.Data;
using Npgsql;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.DataAccess;

public sealed class TrafficFineSqlConnectionFactory : ITrafficFineSqlConnectionFactory
{
    private readonly string _connectionString;

    public TrafficFineSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
