namespace TrafficFineManagement.Modules.Users.Domain.Users;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<bool> HasAnyAsync(CancellationToken cancellationToken = default);
}
