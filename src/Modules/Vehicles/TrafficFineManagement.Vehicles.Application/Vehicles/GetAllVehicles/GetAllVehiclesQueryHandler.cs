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
            SELECT
                "Id",
                "Plaka",
                "Brand",
                "Model",
                "Type",
                "Status",
                "ActiveUserId",
                "ActiveUsageStartTime",
                "LastModifiedAt"
            FROM
            (
                SELECT DISTINCT ON ("Id")
                    "Id",
                    "Plaka",
                    "Brand",
                    "Model",
                    "Type",
                    "Status",
                    "LastModifiedAt",
                    CASE WHEN "EndTime" IS NULL THEN "UserId" END AS "ActiveUserId",
                    CASE WHEN "EndTime" IS NULL THEN "StartTime" END AS "ActiveUsageStartTime"
                FROM vehicles."VehicleReadModel"
                ORDER BY
                    "Id",
                    ("UserId" IS NOT NULL AND "EndTime" IS NULL) DESC,
                    "StartTime" DESC NULLS LAST
            ) AS vehicle
            ORDER BY "LastModifiedAt" DESC, "Id";
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var command = new CommandDefinition(
            sql,
            cancellationToken: cancellationToken);

        var vehicles = await connection.QueryAsync<VehicleDto>(command);

        return vehicles.AsList();
    }
}
