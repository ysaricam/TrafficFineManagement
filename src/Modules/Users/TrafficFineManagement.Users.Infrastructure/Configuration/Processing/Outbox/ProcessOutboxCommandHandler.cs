using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing.Outbox;

public sealed class ProcessOutboxCommandHandler : IRequestHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;
    private readonly IUsersSqlConnectionFactory _connectionFactory;
    private readonly UsersDomainNotificationsMapper _notificationsMapper;
    private readonly ILogger<ProcessOutboxCommandHandler> _logger;

    public ProcessOutboxCommandHandler(
        IMediator mediator,
        IUsersSqlConnectionFactory connectionFactory,
        UsersDomainNotificationsMapper notificationsMapper,
        ILogger<ProcessOutboxCommandHandler> logger)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _notificationsMapper = notificationsMapper;
        _logger = logger;
    }

    public async Task Handle(
        ProcessOutboxCommand request,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetOpenConnection();

        const string selectSql = """
            SELECT "Id", "Type", "Data", "OccurredOn"
            FROM users."OutboxMessages"
            WHERE "ProcessedDate" IS NULL
            ORDER BY "OccurredOn"
            """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(
            new CommandDefinition(selectSql, cancellationToken: cancellationToken));

        const string updateSql = """
            UPDATE users."OutboxMessages"
            SET "ProcessedDate" = @Date
            WHERE "Id" = @Id
            """;

        foreach (var message in messages)
        {
            var notificationType = _notificationsMapper.GetType(message.Type)
                ?? throw new InvalidOperationException(
                    $"Domain notification type '{message.Type}' is not mapped.");

            var notificationData = JObject.Parse(message.Data);
            notificationData[nameof(IDomainEventNotification.OccurredOn)] ??=
                message.OccurredOn;

            var notification = JsonConvert.DeserializeObject(
                    notificationData.ToString(Formatting.None),
                    notificationType)
                as IDomainEventNotification
                ?? throw new InvalidOperationException(
                    $"Outbox message '{message.Id}' could not be deserialized.");

            using (_logger.BeginScope(new Dictionary<string, object>
                   {
                       ["OutboxMessageId"] = notification.Id
                   }))
            {
                await _mediator.Publish(notification, cancellationToken);
                await connection.ExecuteAsync(new CommandDefinition(
                    updateSql,
                    new { Date = DateTime.UtcNow, message.Id },
                    cancellationToken: cancellationToken));
            }
        }
    }
}
