using Newtonsoft.Json;

namespace TrafficFineManagement.BuildingBlocks.Application.Events;

public abstract class DomainNotificationBase<TDomainEvent> :
    IDomainEventNotification<TDomainEvent>
{
    protected DomainNotificationBase(TDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }

    protected DomainNotificationBase(Guid id)
    {
        DomainEvent = default!;
        Id = id;
    }

    [JsonIgnore]
    public TDomainEvent DomainEvent { get; }

    public Guid Id { get; }
}
