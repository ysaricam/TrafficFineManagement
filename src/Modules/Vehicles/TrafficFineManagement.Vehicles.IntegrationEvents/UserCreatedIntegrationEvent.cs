using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

public sealed class UserCreatedIntegrationEvent : IntegrationEvent
{
    public UserCreatedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid userId)
        : base(id, occurredOn)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
