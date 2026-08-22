using MediatR;

namespace TrafficFineManagement.Modules.Vehicles.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
