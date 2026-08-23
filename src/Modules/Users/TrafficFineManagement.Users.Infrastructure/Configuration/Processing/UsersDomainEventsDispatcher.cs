using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.BuildingBlocks.Infrastructure.Outbox;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

public sealed class UsersDomainEventsDispatcher
{
    private readonly DomainEventsDispatcher _dispatcher;

    public UsersDomainEventsDispatcher(
        IMediator mediator,
        UsersContext context,
        UsersDomainNotificationsMapper notificationsMapper)
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
