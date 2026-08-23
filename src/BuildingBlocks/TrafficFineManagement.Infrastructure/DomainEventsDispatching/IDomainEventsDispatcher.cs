namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}
