using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;

public sealed class AddUserToVehicleCommand : CommandBase
{
    public AddUserToVehicleCommand(Guid vehicleId, Guid userId, DateTime startTime)
    {
        VehicleId = vehicleId;
        UserId = userId;
        StartTime = startTime;
    }

    public Guid VehicleId { get; }
    public Guid UserId { get; }
    public DateTime StartTime { get; }
}
