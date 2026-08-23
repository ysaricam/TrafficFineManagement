using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.GetAllUsers;

public sealed class GetAllUsersQuery : IQuery<IReadOnlyCollection<UserDto>>
{
}
