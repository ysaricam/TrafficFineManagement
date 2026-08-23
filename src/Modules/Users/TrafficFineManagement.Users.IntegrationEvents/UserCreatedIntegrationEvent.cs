using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.Users.IntegrationEvents;

public sealed class UserCreatedIntegrationEvent : IntegrationEvent
{
    public UserCreatedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid userId,
        string name,
        string surname,
        string username,
        int role)
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

    public int Role { get; }
}
