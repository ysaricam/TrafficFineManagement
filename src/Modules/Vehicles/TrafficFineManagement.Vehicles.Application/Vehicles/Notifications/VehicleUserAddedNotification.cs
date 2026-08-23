using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleUserAddedNotification :
    DomainNotificationBase<VehicleAddUserDomainEvent>
{
    public VehicleUserAddedNotification(VehicleAddUserDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id, domainEvent.OccurredOn)
    {
        VehicleId = domainEvent.VehicleId.Value;
        UserId = domainEvent.UserId.Value;
        StartTime = domainEvent.StartTime;
    }

    [JsonConstructor]
    public VehicleUserAddedNotification(
        Guid id,
        Guid vehicleId,
        Guid userId,
        DateTime startTime,
        DateTime occurredOn)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
        UserId = userId;
        StartTime = startTime;
    }

    public Guid VehicleId { get; }

    public Guid UserId { get; }

    public DateTime StartTime { get; }
}
