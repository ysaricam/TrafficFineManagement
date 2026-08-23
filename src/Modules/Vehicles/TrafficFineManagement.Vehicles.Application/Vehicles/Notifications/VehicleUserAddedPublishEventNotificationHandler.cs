using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleUserAddedPublishEventNotificationHandler :
    INotificationHandler<VehicleUserAddedNotification>
{
    private readonly IEventsBus _eventsBus;

    public VehicleUserAddedPublishEventNotificationHandler(IEventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public Task Handle(
        VehicleUserAddedNotification notification,
        CancellationToken cancellationToken)
    {
        return _eventsBus.Publish(
            new VehicleUserAddedIntegrationEvent(
                Guid.NewGuid(),
                notification.OccurredOn,
                notification.VehicleId,
                notification.UserId,
                notification.StartTime),
            cancellationToken);
    }
}
