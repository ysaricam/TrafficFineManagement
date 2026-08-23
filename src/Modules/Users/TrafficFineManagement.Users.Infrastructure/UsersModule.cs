using MediatR;
using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Infrastructure;

public sealed class UsersModule : IUsersModule
{
    private readonly IMediator _mediator;

    public UsersModule(IMediator mediator)
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
