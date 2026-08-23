using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using TrafficFineManagement.API.Modules.Vehicles;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllersWithViews();

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(
    VehiclesStartup.ConfigureContainer);

builder.Services.AddVehiclesProcessing();
builder.Services.AddVehiclesQuartz();
builder.Services.AddTrafficFineProcessing();
builder.Services.AddTrafficFineQuartz();

var vehiclesConnectionString = builder.Configuration.GetConnectionString("VehiclesConnectionString")
    ?? throw new InvalidOperationException("VehiclesConnectionString is not configured.");

builder.Services.AddVehiclesPersistence(vehiclesConnectionString);

var trafficFineConnectionString = builder.Configuration
    .GetConnectionString("TrafficFineConnectionString")
    ?? throw new InvalidOperationException("TrafficFineConnectionString is not configured.");

builder.Services.AddTrafficFinePersistence(trafficFineConnectionString);

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
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/vehicles"));

app.Run();
