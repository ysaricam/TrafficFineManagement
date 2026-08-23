using TrafficFineManagement.Modules.Users.Domain.Users;
using TrafficFineManagement.Modules.Users.Domain.Users.Events;

namespace TrafficFineManagement.Users.Domain.UnitTests.Users;

public sealed class UserTests
{
    [Fact]
    public void Create_ShouldSetFieldsAndAddDomainEvent()
    {
        var userId = Guid.NewGuid();

        var user = User.Create(
            userId,
            "  Yasin ",
            " Test ",
            " YASIN.TEST ",
            "password-hash",
            UserRole.Admin);

        Assert.Equal(userId, user.Id.Value);
        Assert.Equal("Yasin", user.Name);
        Assert.Equal("Test", user.Surname);
        Assert.Equal("yasin.test", user.Username);
        Assert.Equal("password-hash", user.PasswordHash);
        Assert.Equal(UserRole.Admin, user.Role);

        var domainEvent = Assert.IsType<UserCreatedDomainEvent>(
            Assert.Single(user.DomainEvents!));
        Assert.Equal(user.Id, domainEvent.UserId);
        Assert.Equal(user.Username, domainEvent.Username);
        Assert.Equal(user.Role, domainEvent.Role);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenRequiredValueIsEmpty_ShouldThrow(string value)
    {
        Assert.Throws<ArgumentException>(() => User.Create(
            Guid.NewGuid(),
            value,
            "Test",
            "user",
            "password-hash",
            UserRole.Driver));
    }

    [Fact]
    public void Create_WhenRoleIsInvalid_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => User.Create(
            Guid.NewGuid(),
            "Yasin",
            "Test",
            "user",
            "password-hash",
            (UserRole)99));
    }
}
