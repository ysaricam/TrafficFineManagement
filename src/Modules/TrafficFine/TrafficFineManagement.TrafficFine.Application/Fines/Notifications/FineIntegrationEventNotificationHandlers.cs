using MediatR;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.TrafficFine.IntegrationEvents;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

public sealed class FineCreatedPublishEventNotificationHandler :
    INotificationHandler<FineCreatedNotification>
{
    private readonly IEventsBus _eventsBus;
    public FineCreatedPublishEventNotificationHandler(IEventsBus eventsBus) => _eventsBus = eventsBus;

    public Task Handle(FineCreatedNotification notification, CancellationToken cancellationToken) =>
        _eventsBus.Publish(new FineCreatedIntegrationEvent(Guid.NewGuid(), notification.OccurredOn,
            notification.FineId, notification.FinedUserId, notification.VehicleId,
            notification.Amount, notification.Currency, notification.ViolationCode,
            notification.Reason, notification.FineDate, notification.CreatedByUserId),
            cancellationToken);
}

public sealed class FineManagerApprovedPublishEventNotificationHandler :
    INotificationHandler<FineManagerApprovedNotification>
{
    private readonly IEventsBus _eventsBus;
    public FineManagerApprovedPublishEventNotificationHandler(IEventsBus eventsBus) => _eventsBus = eventsBus;

    public Task Handle(FineManagerApprovedNotification notification, CancellationToken cancellationToken) =>
        _eventsBus.Publish(new FineManagerApprovedIntegrationEvent(Guid.NewGuid(),
            notification.OccurredOn, notification.FineId, notification.PerformedByUserId,
            notification.Description), cancellationToken);
}

public sealed class FineFinanceApprovedPublishEventNotificationHandler :
    INotificationHandler<FineFinanceApprovedNotification>
{
    private readonly IEventsBus _eventsBus;
    public FineFinanceApprovedPublishEventNotificationHandler(IEventsBus eventsBus) => _eventsBus = eventsBus;

    public Task Handle(FineFinanceApprovedNotification notification, CancellationToken cancellationToken) =>
        _eventsBus.Publish(new FineFinanceApprovedIntegrationEvent(Guid.NewGuid(),
            notification.OccurredOn, notification.FineId, notification.PerformedByUserId,
            notification.Description), cancellationToken);
}

public sealed class FineRejectedPublishEventNotificationHandler :
    INotificationHandler<FineRejectedNotification>
{
    private readonly IEventsBus _eventsBus;
    public FineRejectedPublishEventNotificationHandler(IEventsBus eventsBus) => _eventsBus = eventsBus;

    public Task Handle(FineRejectedNotification notification, CancellationToken cancellationToken) =>
        _eventsBus.Publish(new FineRejectedIntegrationEvent(Guid.NewGuid(), notification.OccurredOn,
            notification.FineId, notification.PerformedByUserId, notification.RejectionReason),
            cancellationToken);
}

public sealed class FineCompletedPublishEventNotificationHandler :
    INotificationHandler<FineCompletedNotification>
{
    private readonly IEventsBus _eventsBus;
    public FineCompletedPublishEventNotificationHandler(IEventsBus eventsBus) => _eventsBus = eventsBus;

    public Task Handle(FineCompletedNotification notification, CancellationToken cancellationToken) =>
        _eventsBus.Publish(new FineCompletedIntegrationEvent(Guid.NewGuid(), notification.OccurredOn,
            notification.FineId, notification.PerformedByUserId, notification.Description),
            cancellationToken);
}
