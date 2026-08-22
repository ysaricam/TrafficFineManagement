using MediatR;

namespace TrafficFineManagement.BuildingBlocks.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}