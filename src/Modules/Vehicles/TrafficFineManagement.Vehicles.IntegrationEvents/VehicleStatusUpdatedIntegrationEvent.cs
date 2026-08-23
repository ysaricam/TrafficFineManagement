using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

public sealed class VehicleStatusUpdatedIntegrationEvent : IntegrationEvent
{
    public VehicleStatusUpdatedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid vehicleId,
        bool status)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
        Status = status;
    }

    public Guid VehicleId { get; }

    public bool Status { get; }
}
