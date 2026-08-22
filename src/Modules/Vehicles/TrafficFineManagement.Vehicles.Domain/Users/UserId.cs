using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Users;

public sealed class UserId : TypedIdValueBase
{
    public UserId(Guid value)
        : base(value)
    {
    }
}