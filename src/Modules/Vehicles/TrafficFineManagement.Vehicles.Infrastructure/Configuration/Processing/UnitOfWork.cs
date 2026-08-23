using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly VehiclesContext _vehiclesContext;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;

    public UnitOfWork(
        VehiclesContext vehiclesContext,
        IDomainEventsDispatcher domainEventsDispatcher)
    {
        _vehiclesContext = vehiclesContext;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventsDispatcher.DispatchEventsAsync();
        await _vehiclesContext.SaveChangesAsync(cancellationToken);
    }
}
