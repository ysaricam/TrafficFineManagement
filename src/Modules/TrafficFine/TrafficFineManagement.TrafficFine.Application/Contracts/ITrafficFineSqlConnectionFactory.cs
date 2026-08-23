using System.Data;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface ITrafficFineSqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}
