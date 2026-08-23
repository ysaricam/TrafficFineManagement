using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleCreatedNotification :
    DomainNotificationBase<VehicleCreatedDomainEvent>
{
    public VehicleCreatedNotification(VehicleCreatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id)
    {
        VehicleId = domainEvent.VehicleId.Value;
    }

    [JsonConstructor]
    public VehicleCreatedNotification(Guid id, Guid vehicleId)
        : base(id)
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}
