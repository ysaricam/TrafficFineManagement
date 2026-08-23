using FluentValidation;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.SynchronizeUser;

public sealed class SynchronizeUserCommandValidator : AbstractValidator<SynchronizeUserCommand>
{
    public SynchronizeUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Role).IsInEnum();
    }
}
