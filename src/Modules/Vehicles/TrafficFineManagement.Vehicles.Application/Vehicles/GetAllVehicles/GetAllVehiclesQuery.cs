using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

public sealed class GetAllVehiclesQuery : IQuery<IReadOnlyCollection<VehicleDto>>
{
}
