namespace TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

public interface IEventsBus : IDisposable
{
    Task Publish<TIntegrationEvent>(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IntegrationEvent;

    void Subscribe<TIntegrationEvent>(
        IIntegrationEventHandler<TIntegrationEvent> handler)
        where TIntegrationEvent : IntegrationEvent;

    void StartConsuming();
}
