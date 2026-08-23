using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

    Task<Vehicle?> GetByIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default);

    Task<bool> HasActiveVehicleAsync(
        UserId userId,
        CancellationToken cancellationToken = default);
}
