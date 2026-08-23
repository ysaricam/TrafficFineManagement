using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleStatusUpdatedPublishEventNotificationHandler :
    INotificationHandler<VehicleStatusUpdatedNotification>
{
    private readonly IEventsBus _eventsBus;

    public VehicleStatusUpdatedPublishEventNotificationHandler(IEventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public Task Handle(
        VehicleStatusUpdatedNotification notification,
        CancellationToken cancellationToken)
    {
        return _eventsBus.Publish(
            new VehicleStatusUpdatedIntegrationEvent(
                Guid.NewGuid(),
                notification.OccurredOn,
                notification.VehicleId,
                notification.Status),
            cancellationToken);
    }
}
