namespace TrafficFineManagement.API.Models.Vehicles;

public sealed class AssignVehicleUserInputModel
{
    public Guid VehicleId { get; init; }

    public Guid UserId { get; init; }

    public DateTime? StartTime { get; init; }
}
