using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;

public sealed class FineRejectionReasonMustBeProvidedRule : IBusinessRule
{
    private readonly string? _rejectionReason;

    public FineRejectionReasonMustBeProvidedRule(string? rejectionReason)
    {
        _rejectionReason = rejectionReason;
    }

    public string Message => "A rejection reason must be provided.";

    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_rejectionReason);
    }
}
