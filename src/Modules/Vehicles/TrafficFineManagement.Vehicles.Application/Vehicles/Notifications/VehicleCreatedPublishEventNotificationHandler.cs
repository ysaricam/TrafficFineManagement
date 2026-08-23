using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

public sealed class VehicleCreatedPublishEventNotificationHandler :
    INotificationHandler<VehicleCreatedNotification>
{
    private readonly IEventsBus _eventsBus;

    public VehicleCreatedPublishEventNotificationHandler(IEventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public Task Handle(
        VehicleCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        return _eventsBus.Publish(
            new VehicleCreatedIntegrationEvent(
                Guid.NewGuid(),
                notification.OccurredOn,
                notification.VehicleId),
            cancellationToken);
    }
}
