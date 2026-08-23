using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;

namespace TrafficFineManagement.API.Modules.Users;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUsersModule _usersModule;

    public UsersController(IUsersModule usersModule)
    {
        _usersModule = usersModule;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _usersModule.ExecuteQueryAsync(
            new GetAllUsersQuery(), cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var userId = await _usersModule.ExecuteCommandAsync(
            new CreateUserCommand(
                request.Name,
                request.Surname,
                request.Username,
                request.Password,
                request.Role),
            cancellationToken);

        return Created($"/api/users/{userId}", new CreateUserResponse(userId));
    }

    [AllowAnonymous]
    [HttpPost("bootstrap")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Bootstrap(
        BootstrapAdminRequest request,
        CancellationToken cancellationToken)
    {
        var userId = await _usersModule.ExecuteCommandAsync(
            new BootstrapAdminCommand(
                request.Name,
                request.Surname,
                request.Username,
                request.Password),
            cancellationToken);

        return Created($"/api/users/{userId}", new CreateUserResponse(userId));
    }
}

public sealed record CreateUserResponse(Guid UserId);

public sealed record BootstrapAdminRequest(
    string Name,
    string Surname,
    string Username,
    string Password);
