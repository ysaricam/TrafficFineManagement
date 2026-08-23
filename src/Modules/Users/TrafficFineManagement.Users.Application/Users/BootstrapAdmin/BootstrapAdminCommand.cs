using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

public sealed class BootstrapAdminCommand : CommandBase<Guid>
{
    public BootstrapAdminCommand(
        string name,
        string surname,
        string username,
        string password)
    {
        Name = name;
        Surname = surname;
        Username = username;
        Password = password;
    }

    public string Name { get; }

    public string Surname { get; }

    public string Username { get; }

    public string Password { get; }
}
