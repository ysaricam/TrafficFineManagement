using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();
builder.Services.AddScoped<IVehiclesModule, VehiclesModule>();

var app = builder.Build();



app.UseHttpsRedirection();

app.MapControllers();

app.Run();

