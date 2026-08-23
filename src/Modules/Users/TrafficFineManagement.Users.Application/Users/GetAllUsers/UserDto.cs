using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;

public sealed class UserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}
