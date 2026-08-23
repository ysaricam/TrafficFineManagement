using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public sealed class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly DbContext _dbContext;

    public DomainEventsAccessor(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        return _dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents?.Count > 0)
            .SelectMany(entry => entry.Entity.DomainEvents!)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents?.Count > 0)
            .ToList();

        domainEntities.ForEach(entry => entry.Entity.ClearDomainEvents());
    }
}
