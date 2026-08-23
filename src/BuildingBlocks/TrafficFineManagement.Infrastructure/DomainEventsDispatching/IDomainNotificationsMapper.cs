namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainNotificationsMapper
{
    string? GetName(Type type);

    Type? GetType(string name);

    Type? GetNotificationType(Type domainEventType);
}
