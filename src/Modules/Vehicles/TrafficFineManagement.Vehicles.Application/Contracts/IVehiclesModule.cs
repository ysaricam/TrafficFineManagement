using TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Contracts;

public interface IVehiclesModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default);

    Task ExecuteCommandAsync(
        ICommand command,
        CancellationToken cancellationToken = default);

    Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default);
}
