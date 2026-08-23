using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    private UserRole _role;

    private User()
    {
    }

    private User(Guid id, UserRole role)
    {
        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role));
        }

        Id = new UserId(id);
        _role = role;
    }

    public UserId Id { get; private set; } = null!;

    public UserRole Role => _role;

    public static User Create(Guid id, UserRole role)
    {
        return new User(id, role);
    }

    public bool IsInRole(params UserRole[] roles)
    {
        return roles.Contains(_role);
    }
}
