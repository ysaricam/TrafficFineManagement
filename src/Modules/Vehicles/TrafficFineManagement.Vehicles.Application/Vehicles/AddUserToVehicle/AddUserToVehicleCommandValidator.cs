using FluentValidation;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;

public sealed class AddUserToVehicleCommandValidator : AbstractValidator<AddUserToVehicleCommand>
{
    public AddUserToVehicleCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
    }
}
