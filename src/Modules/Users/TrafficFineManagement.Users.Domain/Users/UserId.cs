using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Users.Domain.Users;

public sealed class UserId : TypedIdValueBase
{
    public UserId(Guid value)
        : base(value)
    {
    }
}
