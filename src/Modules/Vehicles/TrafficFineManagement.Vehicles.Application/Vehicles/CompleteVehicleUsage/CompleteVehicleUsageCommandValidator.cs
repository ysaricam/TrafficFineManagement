using FluentValidation;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;

public sealed class CompleteVehicleUsageCommandValidator : AbstractValidator<CompleteVehicleUsageCommand>
{
    public CompleteVehicleUsageCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
    }
}
