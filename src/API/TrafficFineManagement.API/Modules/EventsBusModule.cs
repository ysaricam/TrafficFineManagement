using Autofac;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.API.Modules;

public sealed class EventsBusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<InMemoryEventBusClient>()
            .As<IEventsBus>()
            .SingleInstance();
    }
}
