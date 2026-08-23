using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Users.SynchronizeUser;

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
