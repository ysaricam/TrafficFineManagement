using System.Data;

namespace TrafficFineManagement.Modules.Users.Application.Contracts;

public interface IUsersSqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}
