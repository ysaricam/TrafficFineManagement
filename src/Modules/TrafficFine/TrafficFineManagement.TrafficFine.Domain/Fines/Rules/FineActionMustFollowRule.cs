using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

public sealed class FineActionMustFollowRule : IBusinessRule
{
    private readonly FineActionType _currentAction;
    private readonly FineActionType _expectedCurrentAction;
    private readonly FineActionType _newAction;

    public FineActionMustFollowRule(
        FineActionType currentAction,
        FineActionType expectedCurrentAction,
        FineActionType newAction)
    {
        _currentAction = currentAction;
        _expectedCurrentAction = expectedCurrentAction;
        _newAction = newAction;
    }

    public string Message =>
        $"Fine action '{_newAction}' can only follow '{_expectedCurrentAction}'.";

    public bool IsBroken()
    {
        return _currentAction != _expectedCurrentAction;
    }
}
