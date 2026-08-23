using MediatR;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface ICommandBase
{
    Guid Id { get; }
}

public interface ICommand<out TResult> : IRequest<TResult>, ICommandBase
{
}

public interface ICommand : IRequest, ICommandBase
{
}
