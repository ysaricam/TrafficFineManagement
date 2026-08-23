using FluentValidation;

namespace TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

public sealed class BootstrapAdminCommandValidator : AbstractValidator<BootstrapAdminCommand>
{
    public BootstrapAdminCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Username)
            .NotEmpty().MinimumLength(3).MaximumLength(50)
            .Matches("^[a-zA-Z0-9._-]+$");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
    }
}
