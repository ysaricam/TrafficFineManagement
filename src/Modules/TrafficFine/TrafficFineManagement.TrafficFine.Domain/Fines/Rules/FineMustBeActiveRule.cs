using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

public sealed class FineMustBeActiveRule : IBusinessRule
{
    private readonly FineStatus _status;

    public FineMustBeActiveRule(FineStatus status)
    {
        _status = status;
    }

    public string Message => "Only an active fine can be processed.";

    public bool IsBroken()
    {
        return _status != FineStatus.Active;
    }
}
