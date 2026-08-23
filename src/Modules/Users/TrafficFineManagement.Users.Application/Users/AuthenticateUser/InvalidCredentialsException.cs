namespace TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Username or password is incorrect.")
    {
    }
}
