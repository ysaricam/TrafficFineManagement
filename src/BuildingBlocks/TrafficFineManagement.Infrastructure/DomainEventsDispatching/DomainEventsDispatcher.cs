using MediatR;
using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;
using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.BuildingBlocks.Infrastructure.Serialization;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public sealed class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;
    private readonly IDomainEventsAccessor _domainEventsAccessor;
    private readonly IOutbox _outbox;
    private readonly IDomainNotificationsMapper _domainNotificationsMapper;

    public DomainEventsDispatcher(
        IMediator mediator,
        IDomainEventsAccessor domainEventsAccessor,
        IOutbox outbox,
        IDomainNotificationsMapper domainNotificationsMapper)
    {
        _mediator = mediator;
        _domainEventsAccessor = domainEventsAccessor;
        _outbox = outbox;
        _domainNotificationsMapper = domainNotificationsMapper;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();
        var domainEventNotifications = CreateDomainEventNotifications(domainEvents);

        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        AddDomainEventNotificationsToOutbox(domainEventNotifications);
    }

    private List<DomainEventNotification> CreateDomainEventNotifications(
        IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        List<DomainEventNotification> notifications = [];

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = _domainNotificationsMapper.GetNotificationType(
                domainEvent.GetType());

            if (notificationType is null)
            {
                continue;
            }

            var notification = Activator.CreateInstance(notificationType, domainEvent)
                as IDomainEventNotification
                ?? throw new InvalidOperationException(
                    $"Could not create domain notification '{notificationType.FullName}'.");

            notifications.Add(new DomainEventNotification(
                notification,
                domainEvent.OccurredOn));
        }

        return notifications;
    }

    private void AddDomainEventNotificationsToOutbox(
        IReadOnlyCollection<DomainEventNotification> domainEventNotifications)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        };

        foreach (var domainEventNotification in domainEventNotifications)
        {
            var notification = domainEventNotification.Notification;
            var type = _domainNotificationsMapper.GetName(notification.GetType())
                ?? throw new InvalidOperationException(
                    $"Domain notification '{notification.GetType().FullName}' is not mapped.");

            var data = JsonConvert.SerializeObject(notification, serializerSettings);

            _outbox.Add(new OutboxMessage(
                notification.Id,
                domainEventNotification.OccurredOn,
                type,
                data));
        }
    }

    private sealed record DomainEventNotification(
        IDomainEventNotification Notification,
        DateTime OccurredOn);
}
