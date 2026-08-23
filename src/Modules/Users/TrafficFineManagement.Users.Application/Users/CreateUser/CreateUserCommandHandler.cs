using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.CreateUser;

public sealed class CreateUserCommandHandler :
    ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToLowerInvariant();

        if (await _userRepository.ExistsByUsernameAsync(
                normalizedUsername,
                cancellationToken))
        {
            throw new UsernameAlreadyExistsException(normalizedUsername);
        }

        var userId = Guid.NewGuid();
        var user = User.Create(
            userId,
            request.Name,
            request.Surname,
            normalizedUsername,
            _passwordHasher.Hash(request.Password),
            request.Role);

        await _userRepository.AddAsync(user, cancellationToken);

        return user.Id.Value;
    }
}
