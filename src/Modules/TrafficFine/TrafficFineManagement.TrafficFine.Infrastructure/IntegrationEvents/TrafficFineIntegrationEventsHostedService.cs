using Microsoft.Extensions.Hosting;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.IntegrationEvents;

public sealed class TrafficFineIntegrationEventsHostedService : IHostedService
{
    private readonly IEventsBus _eventsBus;
    private readonly UserCreatedIntegrationEventHandler _userCreatedHandler;
    private readonly VehicleCreatedIntegrationEventHandler _vehicleCreatedHandler;

    public TrafficFineIntegrationEventsHostedService(
        IEventsBus eventsBus,
        UserCreatedIntegrationEventHandler userCreatedHandler,
        VehicleCreatedIntegrationEventHandler vehicleCreatedHandler)
    {
        _eventsBus = eventsBus;
        _userCreatedHandler = userCreatedHandler;
        _vehicleCreatedHandler = vehicleCreatedHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _eventsBus.Subscribe<UserCreatedIntegrationEvent>(_userCreatedHandler);
        _eventsBus.Subscribe<VehicleCreatedIntegrationEvent>(_vehicleCreatedHandler);
        _eventsBus.StartConsuming();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
