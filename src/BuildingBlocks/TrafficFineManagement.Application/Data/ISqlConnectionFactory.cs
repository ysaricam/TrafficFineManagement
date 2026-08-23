using System.Data;

namespace TrafficFineManagement.BuildingBlocks.Application.Data;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}
