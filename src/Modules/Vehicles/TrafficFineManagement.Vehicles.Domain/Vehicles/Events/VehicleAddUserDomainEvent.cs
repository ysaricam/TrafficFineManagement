using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

public class VehicleAddUserDomainEvent : DomainEventBase
{
    public VehicleId VehicleId { get; }
    public UserId UserId { get; }

    public VehicleAddUserDomainEvent(VehicleId vehicleId, UserId userId)
    {
        VehicleId = vehicleId;
        UserId = userId;
    }
}