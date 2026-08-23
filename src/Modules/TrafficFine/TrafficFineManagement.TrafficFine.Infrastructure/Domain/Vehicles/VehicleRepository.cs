using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Vehicles;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly TrafficFineContext _context;

    public VehicleRepository(TrafficFineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Vehicle vehicle,
        CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
    }

    public Task<Vehicle?> GetByIdAsync(
        VehicleId vehicleId,
        CancellationToken cancellationToken = default)
    {
        return _context.Vehicles.FirstOrDefaultAsync(
            vehicle => vehicle.Id == vehicleId,
            cancellationToken);
    }
}
