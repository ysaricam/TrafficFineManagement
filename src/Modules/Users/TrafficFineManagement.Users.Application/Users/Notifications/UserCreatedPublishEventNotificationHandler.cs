using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Users.IntegrationEvents;

namespace TrafficFineManagement.Modules.Users.Application.Users.Notifications;

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
                notification.UserId,
                notification.Name,
                notification.Surname,
                notification.Username,
                (int)notification.Role),
            cancellationToken);
    }
}
