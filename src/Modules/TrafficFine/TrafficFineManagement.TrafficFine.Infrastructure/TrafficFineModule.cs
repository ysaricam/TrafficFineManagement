using MediatR;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure;

public sealed class TrafficFineModule : ITrafficFineModule
{
    private readonly IMediator _mediator;

    public TrafficFineModule(IMediator mediator)
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
