using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleCreatedNotification :
    DomainNotificationBase<VehicleCreatedDomainEvent>
{
    public VehicleCreatedNotification(VehicleCreatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id, domainEvent.OccurredOn)
    {
        VehicleId = domainEvent.VehicleId.Value;
    }

    [JsonConstructor]
    public VehicleCreatedNotification(
        Guid id,
        Guid vehicleId,
        DateTime occurredOn)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}
