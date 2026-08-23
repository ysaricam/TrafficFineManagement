using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

public sealed class InMemoryEventBusClient : IEventsBus
{
    private readonly ConcurrentDictionary<
        Type,
        ConcurrentDictionary<IIntegrationEventHandler, byte>> _handlers = new();
    private readonly ILogger<InMemoryEventBusClient> _logger;

    public InMemoryEventBusClient(ILogger<InMemoryEventBusClient> logger)
    {
        _logger = logger;
    }

    public async Task Publish<TIntegrationEvent>(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IntegrationEvent
    {
        _logger.LogInformation(
            "Publishing integration event {IntegrationEventType} with id {IntegrationEventId}",
            integrationEvent.GetType().FullName,
            integrationEvent.Id);

        if (!_handlers.TryGetValue(
                integrationEvent.GetType(),
                out var registeredHandlers))
        {
            return;
        }

        var tasks = registeredHandlers.Keys
            .OfType<IIntegrationEventHandler<TIntegrationEvent>>()
            .Select(handler => handler.Handle(
                integrationEvent,
                cancellationToken));

        await Task.WhenAll(tasks);
    }

    public void Subscribe<TIntegrationEvent>(
        IIntegrationEventHandler<TIntegrationEvent> handler)
        where TIntegrationEvent : IntegrationEvent
    {
        var registeredHandlers = _handlers.GetOrAdd(
            typeof(TIntegrationEvent),
            _ => new ConcurrentDictionary<IIntegrationEventHandler, byte>());

        registeredHandlers.TryAdd(handler, 0);
    }

    public void StartConsuming()
    {
    }

    public void Dispose()
    {
    }
}
