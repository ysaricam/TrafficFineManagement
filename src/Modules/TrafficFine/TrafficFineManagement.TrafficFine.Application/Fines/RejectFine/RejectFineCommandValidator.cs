using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;

public sealed class RejectFineCommandValidator : AbstractValidator<RejectFineCommand>
{
    public RejectFineCommandValidator()
    {
        RuleFor(x => x.FineId).NotEmpty();
        RuleFor(x => x.PerformedByUserId).NotEmpty();
        RuleFor(x => x.RejectionReason).NotEmpty().MaximumLength(1000);
    }
}
