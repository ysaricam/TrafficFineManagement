using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Users.Domain.Users.Events;

public sealed class UserCreatedDomainEvent : DomainEventBase
{
    public UserCreatedDomainEvent(
        UserId userId,
        string name,
        string surname,
        string username,
        UserRole role)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Username = username;
        Role = role;
    }

    public UserId UserId { get; }

    public string Name { get; }

    public string Surname { get; }

    public string Username { get; }

    public UserRole Role { get; }
}
