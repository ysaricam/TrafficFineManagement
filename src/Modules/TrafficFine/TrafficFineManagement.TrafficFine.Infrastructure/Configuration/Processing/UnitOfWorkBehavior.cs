using MediatR;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ICommandBase)
        {
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        return response;
    }
}
