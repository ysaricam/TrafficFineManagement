using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Fines;

public sealed class FineRepository : IFineRepository
{
    private readonly TrafficFineContext _context;

    public FineRepository(TrafficFineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Fine fine,
        CancellationToken cancellationToken = default)
    {
        await _context.Fines.AddAsync(fine, cancellationToken);
    }

    public Task<Fine?> GetByIdAsync(
        FineId fineId,
        CancellationToken cancellationToken = default)
    {
        return _context.Fines.FirstOrDefaultAsync(
            fine => fine.Id == fineId,
            cancellationToken);
    }
}
