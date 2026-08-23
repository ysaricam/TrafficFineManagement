using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

public sealed record AuthenticatedUserDto(
    Guid Id,
    string Name,
    string Surname,
    string Username,
    UserRole Role);
