using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public sealed class FineApprovalHistory : ValueObject
{
    private FineApprovalHistory()
    {
    }

    internal FineApprovalHistory(
        UserId performedByUserId,
        DateTime actionDate,
        FineActionType actionType,
        string? description,
        FineStatus previousStatus,
        FineStatus newStatus)
    {
        ArgumentNullException.ThrowIfNull(performedByUserId);

        if (actionDate == default)
        {
            throw new ArgumentException(
                "Action date must be specified.",
                nameof(actionDate));
        }

        PerformedByUserId = performedByUserId;
        ActionDate = NormalizeToUtc(actionDate);
        ActionType = actionType;
        Description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
    }

    public UserId PerformedByUserId { get; private set; } = null!;

    public DateTime ActionDate { get; private set; }

    public FineActionType ActionType { get; private set; }

    public string? Description { get; private set; }

    public FineStatus PreviousStatus { get; private set; }

    public FineStatus NewStatus { get; private set; }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime();
    }
}
