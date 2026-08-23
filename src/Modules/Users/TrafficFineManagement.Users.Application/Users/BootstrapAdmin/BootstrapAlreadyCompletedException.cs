namespace TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

public sealed class BootstrapAlreadyCompletedException : Exception
{
    public BootstrapAlreadyCompletedException()
        : base("The initial administrator has already been created.")
    {
    }
}
