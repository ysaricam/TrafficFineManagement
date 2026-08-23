using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.SynchronizeUser;

public sealed class SynchronizeUserCommand : CommandBase
{
    public SynchronizeUserCommand(Guid userId, UserRole role)
    {
        UserId = userId;
        Role = role;
    }

    public Guid UserId { get; }
    public UserRole Role { get; }
}
