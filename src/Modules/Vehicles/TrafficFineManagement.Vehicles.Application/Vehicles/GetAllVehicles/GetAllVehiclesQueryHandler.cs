using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler :
    IQueryHandler<GetAllVehiclesQuery, IReadOnlyCollection<VehicleSummaryDto>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetAllVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyCollection<VehicleSummaryDto>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);

        return vehicles
            .Select(vehicle => new VehicleSummaryDto(
                vehicle.Id.Value,
                vehicle.Plaka,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Status))
            .ToList();
    }
}
