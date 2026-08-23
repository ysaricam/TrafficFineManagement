using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

public sealed class FineManagerApprovedNotification : DomainNotificationBase<FineManagerApprovedDomainEvent>
{
    public FineManagerApprovedNotification(FineManagerApprovedDomainEvent domainEvent)
        : this(domainEvent.Id, domainEvent.FineId.Value,
            domainEvent.PerformedByUserId.Value, domainEvent.Description, domainEvent.OccurredOn) { }

    [JsonConstructor]
    public FineManagerApprovedNotification(Guid id, Guid fineId,
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
