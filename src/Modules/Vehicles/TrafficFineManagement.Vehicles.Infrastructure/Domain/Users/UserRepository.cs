using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Users;

public sealed class UserRepository : IUserRepository
{
    private readonly VehiclesContext _context;

    public UserRepository(VehiclesContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
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
