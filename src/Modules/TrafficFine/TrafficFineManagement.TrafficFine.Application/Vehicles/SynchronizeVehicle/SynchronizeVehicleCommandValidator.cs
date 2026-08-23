using FluentValidation;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Vehicles.SynchronizeVehicle;

public sealed class SynchronizeVehicleCommandValidator : AbstractValidator<SynchronizeVehicleCommand>
{
    public SynchronizeVehicleCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
    }
}
