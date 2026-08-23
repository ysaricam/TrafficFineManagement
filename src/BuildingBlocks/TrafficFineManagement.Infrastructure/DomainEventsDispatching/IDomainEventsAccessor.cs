using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}
