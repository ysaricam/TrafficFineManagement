using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

public sealed class VehicleStatusUpdatedDomainEvent : DomainEventBase
{
    public VehicleStatusUpdatedDomainEvent(VehicleId vehicleId, bool status)
    {
        VehicleId = vehicleId;
        Status = status;
    }

    public VehicleId VehicleId { get; }

    public bool Status { get; }
}
