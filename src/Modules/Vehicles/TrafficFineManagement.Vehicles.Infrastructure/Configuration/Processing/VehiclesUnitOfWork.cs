using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;

public sealed class VehiclesUnitOfWork : IUnitOfWork
{
    private readonly VehiclesContext _vehiclesContext;

    public VehiclesUnitOfWork(VehiclesContext vehiclesContext)
    {
        _vehiclesContext = vehiclesContext;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _vehiclesContext.SaveChangesAsync(cancellationToken);
    }
}
