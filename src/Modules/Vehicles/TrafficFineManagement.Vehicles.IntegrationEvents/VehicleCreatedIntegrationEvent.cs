using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

public sealed class VehicleCreatedIntegrationEvent : IntegrationEvent
{
    public VehicleCreatedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid vehicleId)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}
