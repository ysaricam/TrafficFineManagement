using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Vehicles;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly VehiclesContext _context;

    public VehicleRepository(VehiclesContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
    }

    public Task<Vehicle?> GetByIdAsync(
        VehicleId vehicleId,
        CancellationToken cancellationToken = default)
    {
        return _context.Vehicles.FirstOrDefaultAsync(
            x => x.Id == vehicleId,
            cancellationToken);
    }

    public Task<bool> HasActiveVehicleAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return _context.Vehicles.AnyAsync(
            vehicle => EF.Property<List<VehicleUser>>(vehicle, "_users")
                .Any(usage =>
                    usage.UserId == userId &&
                    usage.EndTime == null),
            cancellationToken);
    }

}
