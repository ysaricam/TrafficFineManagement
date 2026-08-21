using TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Contracts;

public interface IVehiclesModule
{
    Task<IReadOnlyCollection<VehiclesSummaryDto>>GetVehiclesSummariesAsync(
        CancellationToken cancellationToken = default);
}