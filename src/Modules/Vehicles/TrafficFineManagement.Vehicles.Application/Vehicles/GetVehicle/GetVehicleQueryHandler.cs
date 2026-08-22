using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;

public sealed class GetVehicleQueryHandler : IQueryHandler<GetVehicleQuery, VehicleDetailsDto?>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleDetailsDto?> Handle(
        GetVehicleQuery request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            new VehicleId(request.VehicleId),
            cancellationToken);

        if (vehicle is null)
        {
            return null;
        }

        return new VehicleDetailsDto(
            vehicle.Id.Value,
            vehicle.Plaka,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Status,
            vehicle.Users
                .Select(user => new VehicleUserDto(
                    user.UserId.Value,
                    user.StartTime,
                    user.EndTime))
                .ToList());
    }
}
