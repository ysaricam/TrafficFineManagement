using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure;

public sealed class VehiclesModule : IVehiclesModule
{
    public Task<IReadOnlyCollection<VehiclesSummaryDto>>GetVehiclesSummariesAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<VehiclesSummaryDto> vehicles =
        [
            new VehiclesSummaryDto(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "34 YSN 34",
                "Ford",
                "Focus"),
            new VehiclesSummaryDto(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "41 YS 41",
                "Kia",
                "Sportage")
        ];
        
        return Task.FromResult(vehicles);
    }
}
