using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Vehicles.Domain.Users.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.Notifications;

public sealed class UserCreatedNotification :
    DomainNotificationBase<UserCreatedDomainEvent>
{
    public UserCreatedNotification(UserCreatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id, domainEvent.OccurredOn)
    {
        UserId = domainEvent.UserId.Value;
    }

    [JsonConstructor]
    public UserCreatedNotification(
        Guid id,
        Guid userId,
        DateTime occurredOn)
        : base(id, occurredOn)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
