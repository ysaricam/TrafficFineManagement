using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure;

public sealed class TrafficFineContext : DbContext
{
    public TrafficFineContext(DbContextOptions<TrafficFineContext> options)
        : base(options)
    {
    }

    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
