using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

public sealed class AuthenticateUserQueryHandler :
    IQueryHandler<AuthenticateUserQuery, AuthenticatedUserDto>
{
    private const string DummyPasswordHash =
        "pbkdf2-sha256.600000.AAAAAAAAAAAAAAAAAAAAAA==." +
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticatedUserDto> Handle(
        AuthenticateUserQuery request,
        CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByUsernameAsync(
            normalizedUsername,
            cancellationToken);

        var passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user?.PasswordHash ?? DummyPasswordHash);

        if (user is null || !passwordIsValid)
        {
            throw new InvalidCredentialsException();
        }

        return new AuthenticatedUserDto(
            user.Id.Value,
            user.Name,
            user.Surname,
            user.Username,
            user.Role);
    }
}
