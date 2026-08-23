namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public interface IFineRepository
{
    Task AddAsync(Fine fine, CancellationToken cancellationToken = default);

    Task<Fine?> GetByIdAsync(
        FineId fineId,
        CancellationToken cancellationToken = default);
}
