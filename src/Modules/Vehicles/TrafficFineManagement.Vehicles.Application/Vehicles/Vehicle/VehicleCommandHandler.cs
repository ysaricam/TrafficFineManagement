using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;
using DomainVehicle = TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Vehicle;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

public sealed class VehicleCommandHandler : ICommandHandler<VehicleCommand, Guid>
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Guid> Handle(
        VehicleCommand request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var vehicle = DomainVehicle.Create(
            request.Plaka,
            request.Brand,
            request.Model);

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        return vehicle.Id.Value;
    }
}
