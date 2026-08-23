using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

public sealed class Vehicle : Entity, IAggregateRoot
{
    private Vehicle()
    {
    }

    private Vehicle(Guid id)
    {
        Id = new VehicleId(id);
    }

    public VehicleId Id { get; private set; } = null!;

    public static Vehicle Create(Guid id)
    {
        return new Vehicle(id);
    }
}
