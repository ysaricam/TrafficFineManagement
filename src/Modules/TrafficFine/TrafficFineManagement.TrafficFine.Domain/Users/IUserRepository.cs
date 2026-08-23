namespace TrafficFineManagement.Modules.TrafficFine.Domain.Users;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);
}
