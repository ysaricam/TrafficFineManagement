using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.API.Authentication;
using TrafficFineManagement.API.Models.Authentication;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;

namespace TrafficFineManagement.API.Controllers;

public sealed class AuthenticationController : Controller
{
    private readonly IUsersModule _usersModule;

    public AuthenticationController(IUsersModule usersModule)
    {
        _usersModule = usersModule;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Redirect("/traffic-fines");
        }

        return View("~/Views/Authentication/Login.cshtml",
            new LoginInputModel { ReturnUrl = returnUrl });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginInputModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Authentication/Login.cshtml", input);
        }

        try
        {
            var user = await _usersModule.ExecuteQueryAsync(
                new AuthenticateUserQuery(input.Username, input.Password),
                cancellationToken);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                UserClaims.CreatePrincipal(user));

            return Url.IsLocalUrl(input.ReturnUrl)
                ? LocalRedirect(input.ReturnUrl!)
                : Redirect("/traffic-fines");
        }
        catch (InvalidCredentialsException)
        {
            ModelState.AddModelError(string.Empty,
                "Kullanıcı adı veya parola hatalı.");
            return View("~/Views/Authentication/Login.cshtml", input);
        }
    }

    [Authorize]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    [HttpGet("forbidden")]
    public IActionResult Forbidden()
    {
        return StatusCode(StatusCodes.Status403Forbidden);
    }
}
