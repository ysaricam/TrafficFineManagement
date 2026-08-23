using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Vehicles.SynchronizeVehicle;

public sealed class SynchronizeVehicleCommand : CommandBase
{
    public SynchronizeVehicleCommand(Guid vehicleId)
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}
