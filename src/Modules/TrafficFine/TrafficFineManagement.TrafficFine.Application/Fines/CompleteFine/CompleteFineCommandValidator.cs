using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;

public sealed class CompleteFineCommandValidator : AbstractValidator<CompleteFineCommand>
{
    public CompleteFineCommandValidator()
    {
        RuleFor(x => x.FineId).NotEmpty();
        RuleFor(x => x.PerformedByUserId).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
