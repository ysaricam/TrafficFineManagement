namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing.Outbox;

public sealed class OutboxMessageDto
{
    public Guid Id { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Data { get; init; } = string.Empty;

    public DateTime OccurredOn { get; init; }
}
