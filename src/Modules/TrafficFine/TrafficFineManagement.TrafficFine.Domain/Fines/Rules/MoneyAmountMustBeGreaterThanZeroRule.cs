using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

public sealed class MoneyAmountMustBeGreaterThanZeroRule : IBusinessRule
{
    private readonly decimal _amount;

    public MoneyAmountMustBeGreaterThanZeroRule(decimal amount)
    {
        _amount = amount;
    }

    public string Message => "Money amount must be greater than zero.";

    public bool IsBroken()
    {
        return _amount <= 0;
    }
}
