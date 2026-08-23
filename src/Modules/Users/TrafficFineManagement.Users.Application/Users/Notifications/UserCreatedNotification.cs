using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Users.Domain.Users;
using TrafficFineManagement.Modules.Users.Domain.Users.Events;

namespace TrafficFineManagement.Modules.Users.Application.Users.Notifications;

public sealed class UserCreatedNotification :
    DomainNotificationBase<UserCreatedDomainEvent>
{
    public UserCreatedNotification(UserCreatedDomainEvent domainEvent)
        : base(domainEvent, domainEvent.Id, domainEvent.OccurredOn)
    {
        UserId = domainEvent.UserId.Value;
        Name = domainEvent.Name;
        Surname = domainEvent.Surname;
        Username = domainEvent.Username;
        Role = domainEvent.Role;
    }

    [JsonConstructor]
    public UserCreatedNotification(
        Guid id,
        DateTime occurredOn,
        Guid userId,
        string name,
        string surname,
        string username,
        UserRole role)
        : base(id, occurredOn)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Username = username;
        Role = role;
    }

    public Guid UserId { get; }

    public string Name { get; }

    public string Surname { get; }

    public string Username { get; }

    public UserRole Role { get; }
}
