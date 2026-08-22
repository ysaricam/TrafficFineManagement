using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.CreateUser;

public sealed class CreateUserCommand : CommandBase
{
    public CreateUserCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
