using TrafficFineManagement.Modules.Vehicles.Domain.Users.Events;
using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public UserId Id { get; private set; } = null!;

    private User() { }

    public static User Create(Guid id)
    {
        return new User(id);
    }

    private User(Guid id)
    {
        Id = new UserId(id);

        AddDomainEvent(new UserCreatedDomainEvent(Id));
    }
}
