using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;

public sealed class CreateFineCommandValidator : AbstractValidator<CreateFineCommand>
{
    public CreateFineCommandValidator()
    {
        RuleFor(x => x.FinedUserId).NotEmpty();
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.CreatedByUserId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.ViolationCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.FineDate).NotEqual(default(DateTime));
    }
}
