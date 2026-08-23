using MediatR;

namespace TrafficFineManagement.BuildingBlocks.Application.Events;

public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
{
    TEventType DomainEvent { get; }
}

public interface IDomainEventNotification : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}
