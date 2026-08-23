using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public sealed class Money : ValueObject
{
    private Money()
    {
    }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = string.Empty;

    public static Money Of(decimal amount, string currency)
    {
        CheckRule(new MoneyAmountMustBeGreaterThanZeroRule(amount));

        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        var normalizedCurrency = currency.Trim().ToUpperInvariant();

        if (normalizedCurrency.Length != 3 ||
            normalizedCurrency.Any(character => !char.IsAsciiLetter(character)))
        {
            throw new ArgumentException(
                "Currency must be a three-letter code.",
                nameof(currency));
        }

        return new Money(amount, normalizedCurrency);
    }
}
