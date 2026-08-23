using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;

public sealed class RejectFineCommand : CommandBase
{
    public RejectFineCommand(Guid fineId, Guid performedByUserId, string rejectionReason)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        RejectionReason = rejectionReason;
    }

    public Guid FineId { get; }
    public Guid PerformedByUserId { get; }
    public string RejectionReason { get; }
}
