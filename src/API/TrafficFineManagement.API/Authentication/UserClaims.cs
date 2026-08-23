using System.Security.Claims;
using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

namespace TrafficFineManagement.API.Authentication;

public static class UserClaims
{
    public static ClaimsPrincipal CreatePrincipal(AuthenticatedUserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        return new ClaimsPrincipal(new ClaimsIdentity(
            claims,
            Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new InvalidOperationException(
                "The authenticated user identifier is missing or invalid.");
    }
}
