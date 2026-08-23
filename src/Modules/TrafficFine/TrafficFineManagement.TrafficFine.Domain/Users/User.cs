using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    private User()
    {
    }

    private User(Guid id)
    {
        Id = new UserId(id);
    }

    public UserId Id { get; private set; } = null!;

    public static User Create(Guid id)
    {
        return new User(id);
    }
}
