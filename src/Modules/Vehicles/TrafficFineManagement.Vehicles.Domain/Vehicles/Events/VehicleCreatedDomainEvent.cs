using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

public class VehicleCreatedDomainEvent : DomainEventBase
{

    public VehicleCreatedDomainEvent(VehicleId vehicleId)
    {
        VehicleId = vehicleId;
    }
    public VehicleId VehicleId { get; }
}