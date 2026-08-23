using MediatR;

namespace TrafficFineManagement.Modules.Users.Application.Contracts;

public interface ICommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;
