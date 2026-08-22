namespace TrafficFineManagement.API.Modules.Vehicles;

public sealed class AddUserToVehicleRequest
{
    public Guid UserId { get; init; }
    public DateTime StartTime { get; init; }
}
