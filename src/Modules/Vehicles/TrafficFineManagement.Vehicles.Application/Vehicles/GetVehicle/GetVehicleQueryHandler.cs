using Dapper;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;

public sealed class GetVehicleQueryHandler : IQueryHandler<GetVehicleQuery, VehicleDetailsDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVehicleQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<VehicleDetailsDto?> Handle(
        GetVehicleQuery request,
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
            WHERE "Id" = @VehicleId;

            SELECT
                "UserId",
                "StartTime",
                "EndTime"
            FROM vehicles."VehicleReadModel"
            WHERE "Id" = @VehicleId
              AND "UserId" IS NOT NULL
            ORDER BY "StartTime";
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var command = new CommandDefinition(
            sql,
            new { request.VehicleId },
            cancellationToken: cancellationToken);

        using var result = await connection.QueryMultipleAsync(command);

        var vehicle = await result.ReadSingleOrDefaultAsync<VehicleDto>();

        if (vehicle is null)
        {
            return null;
        }

        var users = (await result.ReadAsync<VehicleUserDto>()).AsList();

        return new VehicleDetailsDto(
            vehicle.Id,
            vehicle.Plaka,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Status,
            users);
    }
}
