namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
