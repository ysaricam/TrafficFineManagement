using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

public sealed class ProcessOutboxCommand : CommandBase, IRecurringCommand
{
}
