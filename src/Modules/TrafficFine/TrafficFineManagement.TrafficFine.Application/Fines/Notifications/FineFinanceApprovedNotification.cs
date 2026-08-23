using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

public sealed class FineFinanceApprovedNotification : DomainNotificationBase<FineFinanceApprovedDomainEvent>
{
    public FineFinanceApprovedNotification(FineFinanceApprovedDomainEvent domainEvent)
        : this(domainEvent.Id, domainEvent.FineId.Value,
            domainEvent.PerformedByUserId.Value, domainEvent.Description, domainEvent.OccurredOn) { }

    [JsonConstructor]
    public FineFinanceApprovedNotification(Guid id, Guid fineId,
        Guid performedByUserId, string? description, DateTime occurredOn)
        : base(id, occurredOn)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        Description = description;
    }

    public Guid FineId { get; }
    public Guid PerformedByUserId { get; }
    public string? Description { get; }
}
