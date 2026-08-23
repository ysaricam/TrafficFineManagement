using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.Outbox;

public sealed class OutboxAccessor : IOutbox
{
    private readonly DbContext _dbContext;

    public OutboxAccessor(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(OutboxMessage message)
    {
        _dbContext.Set<OutboxMessage>().Add(message);
    }

    public Task Save()
    {
        return _dbContext.SaveChangesAsync();
    }
}
