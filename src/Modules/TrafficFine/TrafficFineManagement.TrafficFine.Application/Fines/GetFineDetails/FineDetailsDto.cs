using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;

public sealed class FineDetailsDto
{
    public Guid Id { get; init; }
    public Guid FinedUserId { get; init; }
    public Guid VehicleId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string ViolationCode { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public DateTime FineDate { get; init; }
    public FineStatus Status { get; init; }
    public FineActionType CurrentAction { get; init; }
    public IReadOnlyCollection<FineApprovalHistoryDto> ApprovalHistory { get; set; } = [];
}
