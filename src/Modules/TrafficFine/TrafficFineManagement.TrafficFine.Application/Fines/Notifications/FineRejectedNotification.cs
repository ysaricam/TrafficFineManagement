using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

public sealed class FineRejectedNotification : DomainNotificationBase<FineRejectedDomainEvent>
{
    public FineRejectedNotification(FineRejectedDomainEvent domainEvent)
        : this(domainEvent.Id, domainEvent.FineId.Value,
            domainEvent.PerformedByUserId.Value, domainEvent.RejectionReason,
            domainEvent.OccurredOn) { }

    [JsonConstructor]
    public FineRejectedNotification(Guid id, Guid fineId,
        Guid performedByUserId, string rejectionReason, DateTime occurredOn)
        : base(id, occurredOn)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        RejectionReason = rejectionReason;
    }

    public Guid FineId { get; }
    public Guid PerformedByUserId { get; }
    public string RejectionReason { get; }
}
