using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;

public sealed class GetAllUsersQuery : IQuery<IReadOnlyCollection<UserDto>>;
