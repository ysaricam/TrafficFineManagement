namespace TrafficFineManagement.Modules.Users.Application.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
