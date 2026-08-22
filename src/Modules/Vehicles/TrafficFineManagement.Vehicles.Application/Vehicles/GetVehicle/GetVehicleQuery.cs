using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;

public sealed class GetVehicleQuery : IQuery<VehicleDetailsDto?>
{
    public GetVehicleQuery(Guid vehicleId)
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}
