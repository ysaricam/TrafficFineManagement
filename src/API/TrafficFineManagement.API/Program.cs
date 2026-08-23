using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.API.Modules.Vehicles;
using TrafficFineManagement.API.Infrastructure.Database;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Users.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Users.Application.Users.AuthenticateUser;
using TrafficFineManagement.Modules.Users.Application.Users.BootstrapAdmin;
using TrafficFineManagement.Modules.Users.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllersWithViews();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "TrafficFineManagement.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/forbidden";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHostedService<DatabaseMigrationHostedService>();

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(
    VehiclesStartup.ConfigureContainer);

var quartzEnabled = builder.Configuration.GetValue("Quartz:Enabled", true);

builder.Services.AddVehiclesProcessing();
builder.Services.AddTrafficFineProcessing();
builder.Services.AddUsersProcessing();

if (quartzEnabled)
{
    builder.Services.AddVehiclesQuartz();
    builder.Services.AddTrafficFineQuartz();
    builder.Services.AddUsersQuartz();
}

var vehiclesConnectionString = builder.Configuration.GetConnectionString("VehiclesConnectionString")
    ?? throw new InvalidOperationException("VehiclesConnectionString is not configured.");

builder.Services.AddVehiclesPersistence(vehiclesConnectionString);

var trafficFineConnectionString = builder.Configuration
    .GetConnectionString("TrafficFineConnectionString")
    ?? throw new InvalidOperationException("TrafficFineConnectionString is not configured.");

builder.Services.AddTrafficFinePersistence(trafficFineConnectionString);

var usersConnectionString = builder.Configuration
    .GetConnectionString("UsersConnectionString")
    ?? throw new InvalidOperationException("UsersConnectionString is not configured.");

builder.Services.AddUsersPersistence(usersConnectionString);
builder.Services.AddHostedService<UserSeedHostedService>();
builder.Services.AddHostedService<DemoDataSeedHostedService>();

var app = builder.Build();

// Validation(Geçici)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var errors = exception.Errors
            .Select(error => error.ErrorMessage)
            .Distinct()
            .ToArray();

        await context.Response.WriteAsJsonAsync(new
        {
            title = "Command validation error",
            status = StatusCodes.Status400BadRequest,
            errors
        });
    }
    catch (BusinessRuleValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Business rule validation error",
            status = StatusCodes.Status400BadRequest,
            detail = exception.Message
        });
    }
    catch (KeyNotFoundException exception)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Resource not found",
            status = StatusCodes.Status404NotFound,
            detail = exception.Message
        });
    }
    catch (UsernameAlreadyExistsException exception)
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Username conflict",
            status = StatusCodes.Status409Conflict,
            detail = exception.Message
        });
    }
    catch (InvalidCredentialsException exception)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Authentication failed",
            status = StatusCodes.Status401Unauthorized,
            detail = exception.Message
        });
    }
    catch (BootstrapAlreadyCompletedException exception)
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Bootstrap conflict",
            status = StatusCodes.Status409Conflict,
            detail = exception.Message
        });
    }
    catch (UnauthorizedAccessException exception)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Access denied",
            status = StatusCodes.Status403Forbidden,
            detail = exception.Message
        });
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/traffic-fines"));

app.Run();

public partial class Program;
