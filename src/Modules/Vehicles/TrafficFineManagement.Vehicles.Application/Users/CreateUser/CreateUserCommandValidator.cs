using FluentValidation;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
