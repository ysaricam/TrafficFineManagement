using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

public sealed class FineManagerApprovedDomainEvent : DomainEventBase
{
    public FineManagerApprovedDomainEvent(
        FineId fineId,
        UserId performedByUserId,
        string? description)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        Description = description;
    }

    public FineId FineId { get; }
    public UserId PerformedByUserId { get; }
    public string? Description { get; }
}
