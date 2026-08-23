using Dapper;
using TrafficFineManagement.Modules.Users.Application.Contracts;

namespace TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler :
    IQueryHandler<GetAllUsersQuery, IReadOnlyCollection<UserDto>>
{
    private readonly IUsersSqlConnectionFactory _connectionFactory;

    public GetAllUsersQueryHandler(IUsersSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Id", "Name", "Surname", "Username", "Role"
            FROM users."Users"
            ORDER BY "Name", "Surname", "Id"
            """;

        using var connection = _connectionFactory.GetOpenConnection();
        var users = await connection.QueryAsync<UserDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return users.AsList();
    }
}
