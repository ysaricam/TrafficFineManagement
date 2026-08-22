using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

public class VehicleStatusUpdated : DomainEventBase
{

    public VehicleStatusUpdated(VehicleId vehicleId, bool status)
    {
        VehicleId = vehicleId;
        Status = status;
    }
    public VehicleId VehicleId { get; }
    public bool Status;
}