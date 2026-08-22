using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;

public sealed class CompleteVehicleUsageCommand : CommandBase
{
    public CompleteVehicleUsageCommand(Guid vehicleId, Guid userId, DateTime endTime)
    {
        VehicleId = vehicleId;
        UserId = userId;
        EndTime = endTime;
    }

    public Guid VehicleId { get; }
    public Guid UserId { get; }
    public DateTime EndTime { get; }
}
