using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

public sealed class FineRejectedDomainEvent : DomainEventBase
{
    public FineRejectedDomainEvent(
        FineId fineId,
        UserId performedByUserId,
        string rejectionReason)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        RejectionReason = rejectionReason;
    }

    public FineId FineId { get; }
    public UserId PerformedByUserId { get; }
    public string RejectionReason { get; }
}
