using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;

public sealed class ApproveFineByManagerCommand : CommandBase
{
    public ApproveFineByManagerCommand(
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
