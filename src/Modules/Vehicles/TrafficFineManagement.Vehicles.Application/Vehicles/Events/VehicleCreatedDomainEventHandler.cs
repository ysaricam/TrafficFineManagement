using MediatR;
using Microsoft.Extensions.Logging;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Events;

public sealed class VehicleCreatedDomainEventHandler :
    INotificationHandler<VehicleCreatedDomainEvent>
{
    private readonly ILogger<VehicleCreatedDomainEventHandler> _logger;

    public VehicleCreatedDomainEventHandler(
        ILogger<VehicleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        VehicleCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Vehicle {VehicleId} was created. Domain event: {DomainEventId}",
            notification.VehicleId.Value,
            notification.Id);

        return Task.CompletedTask;
    }
}
