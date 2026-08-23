using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.TrafficFine.IntegrationEvents;

public sealed class FineCompletedIntegrationEvent : IntegrationEvent
{
    public FineCompletedIntegrationEvent(Guid id, DateTime occurredOn,
        Guid fineId, Guid performedByUserId, string? description)
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
