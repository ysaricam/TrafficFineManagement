using Newtonsoft.Json;

namespace TrafficFineManagement.BuildingBlocks.Application.Events;

public abstract class DomainNotificationBase<TDomainEvent> :
    IDomainEventNotification<TDomainEvent>
{
    protected DomainNotificationBase(
        TDomainEvent domainEvent,
        Guid id,
        DateTime occurredOn)
    {
        DomainEvent = domainEvent;
        Id = id;
        OccurredOn = occurredOn;
    }

    protected DomainNotificationBase(Guid id, DateTime occurredOn)
    {
        DomainEvent = default!;
        Id = id;
        OccurredOn = occurredOn;
    }

    [JsonIgnore]
    public TDomainEvent DomainEvent { get; }

    public Guid Id { get; }

    public DateTime OccurredOn { get; }
}
