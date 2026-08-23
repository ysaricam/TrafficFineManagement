namespace TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

    Task<Vehicle?> GetByIdAsync(
        VehicleId vehicleId,
        CancellationToken cancellationToken = default);
}
