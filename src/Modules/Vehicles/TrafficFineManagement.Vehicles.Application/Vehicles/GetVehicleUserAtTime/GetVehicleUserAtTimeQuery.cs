using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleUserAtTime;

public sealed class GetVehicleUserAtTimeQuery : IQuery<VehicleUserAtTimeDto?>
{
    public GetVehicleUserAtTimeQuery(Guid vehicleId, DateTime atTime)
    {
        VehicleId = vehicleId;
        AtTime = NormalizeToUtc(atTime);
    }

    public Guid VehicleId { get; }

    public DateTime AtTime { get; }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}

public sealed record VehicleUserAtTimeDto(
    Guid UserId,
    DateTime StartTime,
    DateTime? EndTime);
