using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Users;

public sealed class UserRepository : IUserRepository
{
    private readonly TrafficFineContext _context;

    public UserRepository(TrafficFineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public Task<User?> GetByIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(
            user => user.Id == userId,
            cancellationToken);
    }
}
