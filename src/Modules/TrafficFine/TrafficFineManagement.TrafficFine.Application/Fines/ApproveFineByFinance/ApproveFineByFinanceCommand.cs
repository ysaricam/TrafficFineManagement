using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;

public sealed class ApproveFineByFinanceCommand : CommandBase
{
    public ApproveFineByFinanceCommand(
        Guid fineId,
        Guid performedByUserId,
        string? description)
    {
        FineId = fineId;
        PerformedByUserId = performedByUserId;
        Description = description;
    }

    public Guid FineId { get; }
    public Guid PerformedByUserId { get; }
    public string? Description { get; }
}
