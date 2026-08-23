using System.Data;
using Npgsql;
using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.DataAccess;

public sealed class UsersSqlConnectionFactory : IUsersSqlConnectionFactory
{
    private readonly string _connectionString;

    public UsersSqlConnectionFactory(string connectionString)
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
