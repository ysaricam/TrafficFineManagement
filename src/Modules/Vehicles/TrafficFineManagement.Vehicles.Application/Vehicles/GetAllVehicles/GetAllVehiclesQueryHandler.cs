using Dapper;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler :
    IQueryHandler<GetAllVehiclesQuery, IReadOnlyCollection<VehicleDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllVehiclesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IReadOnlyCollection<VehicleDto>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT DISTINCT
                "Id",
                "Plaka",
                "Brand",
                "Model",
                "Status"
            FROM vehicles."VehicleReadModel"
            ORDER BY "Plaka", "Id";
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var command = new CommandDefinition(
            sql,
            cancellationToken: cancellationToken);

        var vehicles = await connection.QueryAsync<VehicleDto>(command);

        return vehicles.AsList();
    }
}
