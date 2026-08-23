using MediatR;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

public abstract class IntegrationEvent : INotification
{
    protected IntegrationEvent(Guid id, DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }

    public Guid Id { get; }

    public DateTime OccurredOn { get; }
}
