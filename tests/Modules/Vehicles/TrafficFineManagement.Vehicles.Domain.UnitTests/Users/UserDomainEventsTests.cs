using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Vehicles.Domain.UnitTests.Users;

public sealed class UserProjectionTests
{
    [Fact]
    public void Create_ShouldPreserveSourceUserId()
    {
        var userId = Guid.NewGuid();

        var user = User.Create(userId, UserRole.Driver);

        Assert.Equal(userId, user.Id.Value);
        Assert.Equal(UserRole.Driver, user.Role);
        Assert.Null(user.DomainEvents);
    }
}
