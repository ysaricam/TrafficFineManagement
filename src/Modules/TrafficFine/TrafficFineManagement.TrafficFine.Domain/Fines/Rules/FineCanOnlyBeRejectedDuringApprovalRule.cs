using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

public sealed class FineCanOnlyBeRejectedDuringApprovalRule : IBusinessRule
{
    private readonly FineActionType _currentAction;

    public FineCanOnlyBeRejectedDuringApprovalRule(
        FineActionType currentAction)
    {
        _currentAction = currentAction;
    }

    public string Message =>
        "A fine can only be rejected during manager or finance approval.";

    public bool IsBroken()
    {
        return _currentAction is not FineActionType.Created and
            not FineActionType.ManagerApproved;
    }
}
