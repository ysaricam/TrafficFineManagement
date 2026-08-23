using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;
using TrafficFineManagement.Modules.Users.Domain.Users;
using TrafficFineManagement.Modules.Users.Infrastructure.Security;

namespace TrafficFineManagement.Users.Domain.UnitTests.Users;

public sealed class AuthenticationTests
{
    [Fact]
    public void PasswordHasher_ShouldHashAndVerifyPassword()
    {
        var hasher = new Pbkdf2PasswordHasher();

        var passwordHash = hasher.Hash("StrongPassword123!");

        Assert.DoesNotContain("StrongPassword123!", passwordHash);
        Assert.True(hasher.Verify("StrongPassword123!", passwordHash));
        Assert.False(hasher.Verify("WrongPassword123!", passwordHash));
        Assert.False(hasher.Verify("StrongPassword123!", "invalid-hash"));
    }

    [Fact]
    public async Task Authenticate_ShouldReturnUserForValidCredentials()
    {
        var hasher = new Pbkdf2PasswordHasher();
        var user = CreateUser(hasher.Hash("StrongPassword123!"));
        var handler = new AuthenticateUserQueryHandler(
            new FakeUserRepository(user),
            hasher);

        var result = await handler.Handle(
            new AuthenticateUserQuery(" TEST.USER ", "StrongPassword123!"),
            CancellationToken.None);

        Assert.Equal(user.Id.Value, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(UserRole.Manager, result.Role);
    }

    [Fact]
    public async Task Authenticate_ShouldRejectInvalidPassword()
    {
        var hasher = new Pbkdf2PasswordHasher();
        var user = CreateUser(hasher.Hash("StrongPassword123!"));
        var handler = new AuthenticateUserQueryHandler(
            new FakeUserRepository(user),
            hasher);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            handler.Handle(
                new AuthenticateUserQuery("test.user", "WrongPassword123!"),
                CancellationToken.None));
    }

    private static User CreateUser(string passwordHash)
    {
        return User.Create(
            Guid.NewGuid(),
            "Test",
            "User",
            "test.user",
            passwordHash,
            UserRole.Manager);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly User _user;

        public FakeUserRepository(User user)
        {
            _user = user;
        }

        public Task AddAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<bool> ExistsByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(username == _user.Username);
        }

        public Task<User?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(username == _user.Username ? _user : null);
        }

        public Task<bool> HasAnyAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
