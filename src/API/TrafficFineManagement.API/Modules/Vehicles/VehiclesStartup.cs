using Autofac;
using TrafficFineManagement.API.Modules;
using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Notifications;

namespace TrafficFineManagement.API.Modules.Vehicles;

public static class VehiclesStartup
{
    public static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new VehiclesAutofacModule());
        builder.RegisterModule(new OutboxModule());

        var domainNotificationsMap = new BiDictionary<string, Type>();

        domainNotificationsMap.Add(
            nameof(VehicleCreatedNotification),
            typeof(VehicleCreatedNotification));
        domainNotificationsMap.Add(
            nameof(VehicleUserAddedNotification),
            typeof(VehicleUserAddedNotification));
        domainNotificationsMap.Add(
            nameof(VehicleStatusUpdatedNotification),
            typeof(VehicleStatusUpdatedNotification));

        builder.RegisterInstance<IDomainNotificationsMapper>(
                new DomainNotificationsMapper(domainNotificationsMap))
            .SingleInstance();
    }
}
