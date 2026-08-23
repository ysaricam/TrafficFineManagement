using Microsoft.Extensions.Hosting;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.Users.IntegrationEvents;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.IntegrationEvents;

public sealed class VehiclesIntegrationEventsHostedService : IHostedService
{
    private readonly IEventsBus _eventsBus;
    private readonly UserCreatedIntegrationEventHandler _userCreatedHandler;

    public VehiclesIntegrationEventsHostedService(
        IEventsBus eventsBus,
        UserCreatedIntegrationEventHandler userCreatedHandler)
    {
        _eventsBus = eventsBus;
        _userCreatedHandler = userCreatedHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _eventsBus.Subscribe<UserCreatedIntegrationEvent>(_userCreatedHandler);
        _eventsBus.StartConsuming();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
