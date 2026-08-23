namespace TrafficFineManagement.Modules.Users.Application.Users.CreateUser;

public sealed class UsernameAlreadyExistsException : Exception
{
    public UsernameAlreadyExistsException(string username)
        : base($"Username '{username}' is already in use.")
    {
    }
}
