using Autofac;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;
using TrafficFineManagement.BuildingBlocks.Infrastructure.Outbox;
using TrafficFineManagement.Modules.Vehicles.Infrastructure;

namespace TrafficFineManagement.API.Modules;

public sealed class OutboxModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
                new OutboxAccessor(context.Resolve<VehiclesContext>()))
            .As<IOutbox>()
            .InstancePerLifetimeScope();
    }
}
