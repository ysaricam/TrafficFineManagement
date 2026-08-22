using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

public sealed class VehicleId : TypedIdValueBase
{
    public VehicleId(Guid value)
        : base(value)
    {
    }
}