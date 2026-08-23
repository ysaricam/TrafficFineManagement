using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.Notifications;

public sealed class UserCreatedPublishEventNotificationHandler :
    INotificationHandler<UserCreatedNotification>
{
    private readonly IEventsBus _eventsBus;

    public UserCreatedPublishEventNotificationHandler(IEventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public Task Handle(
        UserCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        return _eventsBus.Publish(
            new UserCreatedIntegrationEvent(
                Guid.NewGuid(),
                notification.OccurredOn,
                notification.UserId),
            cancellationToken);
    }
}
