using Autofac;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;

namespace TrafficFineManagement.API.Modules.Vehicles;

public sealed class VehiclesAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<VehiclesModule>()
            .As<IVehiclesModule>()
            .InstancePerLifetimeScope();
    }
}
