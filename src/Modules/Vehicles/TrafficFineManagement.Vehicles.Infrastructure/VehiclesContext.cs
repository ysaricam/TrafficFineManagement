using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure;

public sealed class VehiclesContext : DbContext
{
    public VehiclesContext(DbContextOptions<VehiclesContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
