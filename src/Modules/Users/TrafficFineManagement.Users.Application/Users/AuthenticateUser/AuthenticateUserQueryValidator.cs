using FluentValidation;

namespace TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

public sealed class AuthenticateUserQueryValidator : AbstractValidator<AuthenticateUserQuery>
{
    public AuthenticateUserQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(128);
    }
}
