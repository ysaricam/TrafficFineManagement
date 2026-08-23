using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleStatusUpdatedNotification :
    DomainNotificationBase<VehicleStatusUpdatedDomainEvent>
{
    public VehicleStatusUpdatedNotification(VehicleStatusUpdatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id, domainEvent.OccurredOn)
    {
        VehicleId = domainEvent.VehicleId.Value;
        Status = domainEvent.Status;
    }

    [JsonConstructor]
    public VehicleStatusUpdatedNotification(
        Guid id,
        Guid vehicleId,
        bool status,
        DateTime occurredOn)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
        Status = status;
    }

    public Guid VehicleId { get; }

    public bool Status { get; }
}
