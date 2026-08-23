using MediatR;
using Quartz;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxJob : IJob
{
    private readonly IMediator _mediator;

    public ProcessOutboxJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return _mediator.Send(
            new ProcessOutboxCommand(),
            context.CancellationToken);
    }
}
