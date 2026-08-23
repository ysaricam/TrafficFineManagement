using Dapper;
using TrafficFineManagement.BuildingBlocks.Application.Data;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler :
    IQueryHandler<GetAllUsersQuery, IReadOnlyCollection<UserDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IReadOnlyCollection<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT "Id"
            FROM vehicles."Users"
            ORDER BY "Id";
            """;

        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var users = await connection.QueryAsync<UserDto>(
            new CommandDefinition(
                sql,
                cancellationToken: cancellationToken));

        return users.AsList();
    }
}
