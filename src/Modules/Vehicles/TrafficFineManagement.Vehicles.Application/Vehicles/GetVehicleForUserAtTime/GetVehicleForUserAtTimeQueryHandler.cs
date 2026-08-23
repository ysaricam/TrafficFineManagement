using Dapper;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleForUserAtTime;

public sealed class GetVehicleForUserAtTimeQueryHandler :
    IQueryHandler<GetVehicleForUserAtTimeQuery, VehicleForUserAtTimeDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVehicleForUserAtTimeQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<VehicleForUserAtTimeDto?> Handle(
        GetVehicleForUserAtTimeQuery request,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT "Id" AS "VehicleId"
            FROM vehicles."VehicleReadModel"
            WHERE "UserId" = @UserId
              AND "StartTime" <= @AtTime
              AND ("EndTime" IS NULL OR "EndTime" > @AtTime)
            ORDER BY "StartTime" DESC
            LIMIT 1;
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();
        var command = new CommandDefinition(
            sql,
            new { request.UserId, request.AtTime },
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<VehicleForUserAtTimeDto>(
            command);
    }
}
