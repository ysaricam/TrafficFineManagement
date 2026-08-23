using Dapper;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;

public sealed class GetFineDetailsQueryHandler :
    IQueryHandler<GetFineDetailsQuery, FineDetailsDto?>
{
    private readonly ITrafficFineSqlConnectionFactory _connectionFactory;

    public GetFineDetailsQueryHandler(ITrafficFineSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<FineDetailsDto?> Handle(
        GetFineDetailsQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetOpenConnection();

        const string sql = """
            SELECT
                "Id", "FinedUserId", "VehicleId", "Amount", "Currency",
                "ViolationCode", "Reason", "FineDate", "Status", "CurrentAction"
            FROM traffic_fines."FineReadModel"
            WHERE "Id" = @FineId;

            SELECT
                "PerformedByUserId", "ActionDate", "ActionType", "Description",
                "PreviousStatus", "NewStatus"
            FROM traffic_fines."FineApprovalHistories"
            WHERE "FineId" = @FineId
            ORDER BY "ActionDate", "Id";
            """;

        using var results = await connection.QueryMultipleAsync(
            new CommandDefinition(
                sql,
                new { request.FineId },
                cancellationToken: cancellationToken));

        var fine = await results.ReadSingleOrDefaultAsync<FineDetailsDto>();
        var history = (await results.ReadAsync<FineApprovalHistoryDto>()).AsList();

        if (fine is not null)
        {
            fine.ApprovalHistory = history;
        }

        return fine;
    }
}
