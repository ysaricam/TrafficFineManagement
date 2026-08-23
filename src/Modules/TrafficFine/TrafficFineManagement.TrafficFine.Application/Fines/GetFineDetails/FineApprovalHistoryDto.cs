using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;

public sealed class FineApprovalHistoryDto
{
    public Guid PerformedByUserId { get; init; }
    public DateTime ActionDate { get; init; }
    public FineActionType ActionType { get; init; }
    public string? Description { get; init; }
    public FineStatus PreviousStatus { get; init; }
    public FineStatus NewStatus { get; init; }
}
