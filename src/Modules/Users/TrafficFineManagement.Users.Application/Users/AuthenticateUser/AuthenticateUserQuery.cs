using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

public sealed class AuthenticateUserQuery : IQuery<AuthenticatedUserDto>
{
    public AuthenticateUserQuery(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; }

    public string Password { get; }
}
