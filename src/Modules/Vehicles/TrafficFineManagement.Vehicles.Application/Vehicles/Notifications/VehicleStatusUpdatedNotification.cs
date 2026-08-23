using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleStatusUpdatedNotification :
    DomainNotificationBase<VehicleStatusUpdatedDomainEvent>
{
    public VehicleStatusUpdatedNotification(VehicleStatusUpdatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id)
    {
        VehicleId = domainEvent.VehicleId.Value;
        Status = domainEvent.Status;
    }

    [JsonConstructor]
    public VehicleStatusUpdatedNotification(Guid id, Guid vehicleId, bool status)
        : base(id)
    {
        VehicleId = vehicleId;
        Status = status;
    }

    public Guid VehicleId { get; }

    public bool Status { get; }
}
