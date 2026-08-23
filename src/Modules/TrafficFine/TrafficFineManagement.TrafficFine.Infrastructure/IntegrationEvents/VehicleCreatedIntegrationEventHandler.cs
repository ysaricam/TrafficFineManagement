using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;
using TrafficFineManagement.Modules.Vehicles.IntegrationEvents;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.IntegrationEvents;

public sealed class VehicleCreatedIntegrationEventHandler :
    IIntegrationEventHandler<VehicleCreatedIntegrationEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public VehicleCreatedIntegrationEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(
        VehicleCreatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var vehicleId = new VehicleId(integrationEvent.VehicleId);

        if (await repository.GetByIdAsync(vehicleId, cancellationToken) is not null)
        {
            return;
        }

        await repository.AddAsync(Vehicle.Create(integrationEvent.VehicleId), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
