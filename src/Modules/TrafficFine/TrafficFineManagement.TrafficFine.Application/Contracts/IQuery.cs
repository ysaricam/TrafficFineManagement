using MediatR;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
