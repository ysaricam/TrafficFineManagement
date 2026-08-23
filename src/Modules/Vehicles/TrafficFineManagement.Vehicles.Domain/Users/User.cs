using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public UserId Id { get; private set; } = null!;
    private UserRole _role;

    public UserRole Role => _role;

    private User() { }

    public static User Create(Guid id, UserRole role)
    {
        return new User(id, role);
    }

    private User(Guid id, UserRole role)
    {
        Id = new UserId(id);
        _role = role;
    }
}
