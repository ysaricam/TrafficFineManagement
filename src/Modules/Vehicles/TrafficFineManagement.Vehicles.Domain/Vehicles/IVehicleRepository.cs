namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

    Task<Vehicle?> GetByIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
}
