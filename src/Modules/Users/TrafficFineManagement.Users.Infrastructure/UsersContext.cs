using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Infrastructure;

public sealed class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
