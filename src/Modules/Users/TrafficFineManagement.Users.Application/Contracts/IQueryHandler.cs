using MediatR;

namespace TrafficFineManagement.Modules.Users.Application.Contracts;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;
