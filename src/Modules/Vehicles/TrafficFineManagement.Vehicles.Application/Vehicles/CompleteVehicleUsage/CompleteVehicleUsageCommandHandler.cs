using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;

public sealed class CompleteVehicleUsageCommandHandler : ICommandHandler<CompleteVehicleUsageCommand>
{
    private readonly IVehicleRepository _vehicleRepository;

    public CompleteVehicleUsageCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task Handle(
        CompleteVehicleUsageCommand request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            new VehicleId(request.VehicleId),
            cancellationToken);

        if (vehicle is null)
        {
            throw new KeyNotFoundException($"Vehicle '{request.VehicleId}' was not found.");
        }

        vehicle.UpdateStatus(new UserId(request.UserId), request.EndTime);
    }
}
