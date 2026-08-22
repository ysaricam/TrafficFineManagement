using MediatR;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure;

public sealed class VehiclesModule : IVehiclesModule
{
    private readonly IMediator _mediator;

    public VehiclesModule(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.Send(command, cancellationToken);
    }

    public Task ExecuteCommandAsync(
        ICommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.Send(command, cancellationToken);
    }

    public Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default)
    {
        return _mediator.Send(query, cancellationToken);
    }
}
