using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;

public sealed class ApproveFineByManagerCommandValidator :
    AbstractValidator<ApproveFineByManagerCommand>
{
    public ApproveFineByManagerCommandValidator()
    {
        RuleFor(x => x.FineId).NotEmpty();
        RuleFor(x => x.PerformedByUserId).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
