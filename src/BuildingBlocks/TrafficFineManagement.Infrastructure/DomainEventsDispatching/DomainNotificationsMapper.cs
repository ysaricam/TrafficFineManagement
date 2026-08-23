using TrafficFineManagement.BuildingBlocks.Application.Events;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public sealed class DomainNotificationsMapper : IDomainNotificationsMapper
{
    private readonly BiDictionary<string, Type> _domainNotificationsMap;

    public DomainNotificationsMapper(
        BiDictionary<string, Type> domainNotificationsMap)
    {
        _domainNotificationsMap = domainNotificationsMap;
    }

    public string? GetName(Type type)
    {
        return _domainNotificationsMap.TryGetBySecond(type, out var name)
            ? name
            : null;
    }

    public Type? GetType(string name)
    {
        return _domainNotificationsMap.TryGetByFirst(name, out var type)
            ? type
            : null;
    }

    public Type? GetNotificationType(Type domainEventType)
    {
        return _domainNotificationsMap.SecondValues.SingleOrDefault(
            notificationType => notificationType
                .GetInterfaces()
                .Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() ==
                        typeof(IDomainEventNotification<>) &&
                    interfaceType.GenericTypeArguments[0] == domainEventType));
    }
}
