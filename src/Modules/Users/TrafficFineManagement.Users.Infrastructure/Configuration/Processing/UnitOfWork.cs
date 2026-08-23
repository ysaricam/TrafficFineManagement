using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly UsersContext _context;
    private readonly UsersDomainEventsDispatcher _domainEventsDispatcher;

    public UnitOfWork(
        UsersContext context,
        UsersDomainEventsDispatcher domainEventsDispatcher)
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
