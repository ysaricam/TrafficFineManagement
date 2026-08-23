using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Vehicles.SynchronizeVehicle;

public sealed class SynchronizeVehicleCommandHandler : ICommandHandler<SynchronizeVehicleCommand>
{
    private readonly IVehicleRepository _vehicleRepository;

    public SynchronizeVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task Handle(
        SynchronizeVehicleCommand request,
        CancellationToken cancellationToken)
    {
        var vehicleId = new VehicleId(request.VehicleId);
        if (await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken) is not null)
        {
            return;
        }

        await _vehicleRepository.AddAsync(
            Vehicle.Create(request.VehicleId),
            cancellationToken);
    }
}
