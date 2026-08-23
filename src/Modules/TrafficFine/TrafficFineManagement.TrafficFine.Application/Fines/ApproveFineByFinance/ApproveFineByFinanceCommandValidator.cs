using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;

public sealed class ApproveFineByFinanceCommandValidator :
    AbstractValidator<ApproveFineByFinanceCommand>
{
    public ApproveFineByFinanceCommandValidator()
    {
        RuleFor(x => x.FineId).NotEmpty();
        RuleFor(x => x.PerformedByUserId).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
