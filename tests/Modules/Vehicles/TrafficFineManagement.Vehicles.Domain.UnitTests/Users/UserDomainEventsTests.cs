using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Users.Events;

namespace TrafficFineManagement.Vehicles.Domain.UnitTests.Users;

public sealed class UserDomainEventsTests
{
    [Fact]
    public void Create_ShouldAddUserCreatedDomainEvent()
    {
        var userId = Guid.NewGuid();

        var user = User.Create(userId);

        var domainEvent = Assert.IsType<UserCreatedDomainEvent>(
            Assert.Single(user.DomainEvents!));

        Assert.Equal(userId, domainEvent.UserId.Value);
        Assert.NotEqual(Guid.Empty, domainEvent.Id);
        Assert.Equal(DateTimeKind.Utc, domainEvent.OccurredOn.Kind);
    }
}
