using Autofac;
using Autofac.Extensions.DependencyInjection;
using TrafficFineManagement.API.Modules.Vehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new VehiclesAutofacModule());
});

var app = builder.Build();



app.UseHttpsRedirection();

app.MapControllers();

app.Run();

