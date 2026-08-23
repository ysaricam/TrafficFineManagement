using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.TrafficFine.IntegrationEvents;

public sealed class FineRejectedIntegrationEvent : IntegrationEvent
{
    public FineRejectedIntegrationEvent(Guid id, DateTime occurredOn,
        Guid fineId, Guid performedByUserId, string rejectionReason)
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
