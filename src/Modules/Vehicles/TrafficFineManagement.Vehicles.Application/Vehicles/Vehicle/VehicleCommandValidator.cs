using FluentValidation;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

public sealed class VehicleCommandValidator : AbstractValidator<VehicleCommand>
{
    public VehicleCommandValidator()
    {
        RuleFor(x => x.Plaka)
            .NotEmpty()
            .WithMessage("Plaka cannot be empty.");

        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Brand cannot be empty.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model cannot be empty.");
    }
}
