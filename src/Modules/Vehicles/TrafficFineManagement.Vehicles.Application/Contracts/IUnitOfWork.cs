namespace TrafficFineManagement.Modules.Vehicles.Application.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
