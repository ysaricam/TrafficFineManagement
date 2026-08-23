using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

public sealed class BootstrapAdminCommandHandler :
    ICommandHandler<BootstrapAdminCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public BootstrapAdminCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        BootstrapAdminCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.HasAnyAsync(cancellationToken))
        {
            throw new BootstrapAlreadyCompletedException();
        }

        var user = User.Create(
            Guid.NewGuid(),
            request.Name,
            request.Surname,
            request.Username,
            _passwordHasher.Hash(request.Password),
            UserRole.Admin);

        await _userRepository.AddAsync(user, cancellationToken);
        return user.Id.Value;
    }
}
