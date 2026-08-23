using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleForUserAtTime;

public sealed class GetVehicleForUserAtTimeQuery :
    IQuery<VehicleForUserAtTimeDto?>
{
    public GetVehicleForUserAtTimeQuery(Guid userId, DateTime atTime)
    {
        UserId = userId;
        AtTime = NormalizeToUtc(atTime);
    }

    public Guid UserId { get; }

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

public sealed record VehicleForUserAtTimeDto(Guid VehicleId);
