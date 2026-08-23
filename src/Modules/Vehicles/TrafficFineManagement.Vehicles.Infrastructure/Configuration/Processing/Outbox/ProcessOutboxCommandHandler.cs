using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing.Outbox;

public sealed class ProcessOutboxCommandHandler :
    ICommandHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IDomainNotificationsMapper _domainNotificationsMapper;
    private readonly ILogger<ProcessOutboxCommandHandler> _logger;

    public ProcessOutboxCommandHandler(
        IMediator mediator,
        ISqlConnectionFactory sqlConnectionFactory,
        IDomainNotificationsMapper domainNotificationsMapper,
        ILogger<ProcessOutboxCommandHandler> logger)
    {
        _mediator = mediator;
        _sqlConnectionFactory = sqlConnectionFactory;
        _domainNotificationsMapper = domainNotificationsMapper;
        _logger = logger;
    }

    public async Task Handle(
        ProcessOutboxCommand request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string selectSql = """
            SELECT
                "Id",
                "Type",
                "Data",
                "OccurredOn"
            FROM vehicles."OutboxMessages"
            WHERE "ProcessedDate" IS NULL
            ORDER BY "OccurredOn"
            """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(
            new CommandDefinition(
                selectSql,
                cancellationToken: cancellationToken));

        const string updateProcessedDateSql = """
            UPDATE vehicles."OutboxMessages"
            SET "ProcessedDate" = @Date
            WHERE "Id" = @Id
            """;

        foreach (var message in messages)
        {
            var notificationType = _domainNotificationsMapper.GetType(message.Type)
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
                    $"Outbox message '{message.Id}' could not be deserialized as a domain notification.");

            using (_logger.BeginScope(
                new Dictionary<string, object>
                {
                    ["OutboxMessageId"] = notification.Id
                }))
            {
                await _mediator.Publish(notification, cancellationToken);

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        updateProcessedDateSql,
                        new
                        {
                            Date = DateTime.UtcNow,
                            message.Id
                        },
                        cancellationToken: cancellationToken));
            }
        }
    }
}
