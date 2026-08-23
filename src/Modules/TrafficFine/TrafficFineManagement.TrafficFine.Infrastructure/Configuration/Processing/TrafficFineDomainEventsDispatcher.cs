using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.BuildingBlocks.Infrastructure.Outbox;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;

public sealed class TrafficFineDomainEventsDispatcher
{
    private readonly DomainEventsDispatcher _dispatcher;

    public TrafficFineDomainEventsDispatcher(
        IMediator mediator,
        TrafficFineContext context,
        TrafficFineDomainNotificationsMapper notificationsMapper)
    {
        _dispatcher = new DomainEventsDispatcher(
            mediator,
            new DomainEventsAccessor(context),
            new OutboxAccessor(context),
            notificationsMapper);
    }

    public Task DispatchEventsAsync()
    {
        return _dispatcher.DispatchEventsAsync();
    }
}
