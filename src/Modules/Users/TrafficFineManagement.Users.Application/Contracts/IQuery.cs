using MediatR;

namespace TrafficFineManagement.Modules.Users.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>;
