namespace TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> :
    IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);
}

public interface IIntegrationEventHandler
{
}
