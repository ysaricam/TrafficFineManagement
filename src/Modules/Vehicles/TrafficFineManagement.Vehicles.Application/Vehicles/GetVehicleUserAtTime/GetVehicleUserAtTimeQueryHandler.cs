using Dapper;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleUserAtTime;

public sealed class GetVehicleUserAtTimeQueryHandler :
    IQueryHandler<GetVehicleUserAtTimeQuery, VehicleUserAtTimeDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVehicleUserAtTimeQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<VehicleUserAtTimeDto?> Handle(
        GetVehicleUserAtTimeQuery request,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                "UserId",
                "StartTime",
                "EndTime"
            FROM vehicles."VehicleReadModel"
            WHERE "Id" = @VehicleId
              AND "UserId" IS NOT NULL
              AND "StartTime" <= @AtTime
              AND ("EndTime" IS NULL OR "EndTime" > @AtTime)
            ORDER BY "StartTime" DESC
            LIMIT 1;
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();
        var command = new CommandDefinition(
            sql,
            new { request.VehicleId, request.AtTime },
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<VehicleUserAtTimeDto>(
            command);
    }
}
