using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Users.Events;

public class UserCreatedDomainEvent : DomainEventBase
{
    public UserId UserId { get; }

    public UserCreatedDomainEvent(UserId userId)
    {
        UserId = userId;
    }
}