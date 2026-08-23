using MediatR;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
