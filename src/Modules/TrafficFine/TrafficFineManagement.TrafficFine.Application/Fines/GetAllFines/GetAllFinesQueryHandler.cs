using Dapper;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;

public sealed class GetAllFinesQueryHandler :
    IQueryHandler<GetAllFinesQuery, IReadOnlyCollection<FineDto>>
{
    private readonly ITrafficFineSqlConnectionFactory _connectionFactory;

    public GetAllFinesQueryHandler(ITrafficFineSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<FineDto>> Handle(
        GetAllFinesQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetOpenConnection();

        const string sql = """
            SELECT
                "Id",
                "FinedUserId",
                "VehicleId",
                "Amount",
                "Currency",
                "ViolationCode",
                "Reason",
                "FineDate",
                "Status",
                "CurrentAction"
            FROM traffic_fines."FineReadModel" AS fine
            ORDER BY
                (
                    SELECT MAX(history."ActionDate")
                    FROM traffic_fines."FineApprovalHistories" AS history
                    WHERE history."FineId" = fine."Id"
                ) DESC NULLS LAST,
                fine."Id"
            """;

        var fines = await connection.QueryAsync<FineDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return fines.AsList();
    }
}
