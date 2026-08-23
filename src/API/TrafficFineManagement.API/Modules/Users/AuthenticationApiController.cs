using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.API.Authentication;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

namespace TrafficFineManagement.API.Modules.Users;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationApiController : ControllerBase
{
    private readonly IUsersModule _usersModule;

    public AuthenticationApiController(IUsersModule usersModule)
    {
        _usersModule = usersModule;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _usersModule.ExecuteQueryAsync(
            new AuthenticateUserQuery(request.Username, request.Password),
            cancellationToken);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            UserClaims.CreatePrincipal(user));

        return NoContent();
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}

public sealed record LoginRequest(string Username, string Password);
