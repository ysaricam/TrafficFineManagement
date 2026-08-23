using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Application.Users.CreateUser;

public sealed class CreateUserCommand : CommandBase<Guid>
{
    public CreateUserCommand(
        string name,
        string surname,
        string username,
        string password,
        UserRole role)
    {
        Name = name;
        Surname = surname;
        Username = username;
        Password = password;
        Role = role;
    }

    public string Name { get; }

    public string Surname { get; }

    public string Username { get; }

    public string Password { get; }

    public UserRole Role { get; }
}
