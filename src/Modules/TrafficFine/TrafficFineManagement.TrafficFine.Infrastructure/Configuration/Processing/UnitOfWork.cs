using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TrafficFineContext _context;
    private readonly TrafficFineDomainEventsDispatcher _domainEventsDispatcher;

    public UnitOfWork(
        TrafficFineContext context,
        TrafficFineDomainEventsDispatcher domainEventsDispatcher)
    {
        _context = context;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventsDispatcher.DispatchEventsAsync();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
