using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using TrafficFineManagement.API.Modules.Vehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.DataAccess;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing;
using TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new VehiclesAutofacModule());
});

// MediaTr
builder.Services.AddMediatR(configuration =>
{
   configuration.RegisterServicesFromAssembly(typeof(IVehiclesModule).Assembly);
});

var vehiclesConnectionString = builder.Configuration.GetConnectionString("VehiclesConnectionString")
    ?? throw new InvalidOperationException("VehiclesConnectionString is not configured.");

builder.Services.AddVehiclesPersistence(vehiclesConnectionString);

// Validation
builder.Services.AddTransient<IValidator<VehicleCommand>, VehicleCommandValidator>();
builder.Services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
builder.Services.AddTransient<IValidator<AddUserToVehicleCommand>, AddUserToVehicleCommandValidator>();
builder.Services.AddTransient<IValidator<CompleteVehicleUsageCommand>, CompleteVehicleUsageCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

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

app.MapControllers();

app.Run();
