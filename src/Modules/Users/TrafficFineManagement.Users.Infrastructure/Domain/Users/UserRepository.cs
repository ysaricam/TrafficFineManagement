using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Domain.Users;

public sealed class UserRepository : IUserRepository
{
    private readonly UsersContext _context;

    public UserRepository(UsersContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return _context.Users.AnyAsync(
            user => user.Username == username,
            cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return _context.Users.SingleOrDefaultAsync(
            user => user.Username == username,
            cancellationToken);
    }

    public Task<bool> HasAnyAsync(CancellationToken cancellationToken = default)
    {
        return _context.Users.AnyAsync(cancellationToken);
    }
}
