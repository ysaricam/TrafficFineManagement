using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

public sealed class VehicleUserAddedIntegrationEvent : IntegrationEvent
{
    public VehicleUserAddedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid vehicleId,
        Guid userId,
        DateTime startTime)
        : base(id, occurredOn)
    {
        VehicleId = vehicleId;
        UserId = userId;
        StartTime = startTime;
    }

    public Guid VehicleId { get; }

    public Guid UserId { get; }

    public DateTime StartTime { get; }
}
