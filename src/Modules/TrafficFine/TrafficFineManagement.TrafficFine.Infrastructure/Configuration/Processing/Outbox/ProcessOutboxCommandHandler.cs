using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.DataAccess;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

public sealed class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;
    private readonly TrafficFineSqlConnectionFactory _connectionFactory;
    private readonly TrafficFineDomainNotificationsMapper _notificationsMapper;
    private readonly ILogger<ProcessOutboxCommandHandler> _logger;

    public ProcessOutboxCommandHandler(IMediator mediator,
        TrafficFineSqlConnectionFactory connectionFactory,
        TrafficFineDomainNotificationsMapper notificationsMapper,
        ILogger<ProcessOutboxCommandHandler> logger)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _notificationsMapper = notificationsMapper;
        _logger = logger;
    }

    public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetOpenConnection();

        const string selectSql = """
            SELECT "Id", "Type", "Data", "OccurredOn"
            FROM traffic_fines."OutboxMessages"
            WHERE "ProcessedDate" IS NULL
            ORDER BY "OccurredOn"
            """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(
            new CommandDefinition(selectSql, cancellationToken: cancellationToken));

        const string updateSql = """
            UPDATE traffic_fines."OutboxMessages"
            SET "ProcessedDate" = @Date
            WHERE "Id" = @Id
            """;

        foreach (var message in messages)
        {
            var notificationType = _notificationsMapper.GetType(message.Type)
                ?? throw new InvalidOperationException(
                    $"Domain notification type '{message.Type}' is not mapped.");

            var data = JObject.Parse(message.Data);
            data[nameof(IDomainEventNotification.OccurredOn)] ??= message.OccurredOn;

            var notification = JsonConvert.DeserializeObject(
                    data.ToString(Formatting.None), notificationType)
                as IDomainEventNotification
                ?? throw new InvalidOperationException(
                    $"Outbox message '{message.Id}' could not be deserialized.");

            using (_logger.BeginScope(new Dictionary<string, object>
                   {
                       ["OutboxMessageId"] = notification.Id
                   }))
            {
                await _mediator.Publish(notification, cancellationToken);
                await connection.ExecuteAsync(new CommandDefinition(updateSql,
                    new { Date = DateTime.UtcNow, message.Id },
                    cancellationToken: cancellationToken));
            }
        }
    }
}
