using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.Modules.Users.Application.Users.Notifications;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Configuration.Processing;

public sealed class UsersDomainNotificationsMapper : IDomainNotificationsMapper
{
    private readonly DomainNotificationsMapper _mapper;

    public UsersDomainNotificationsMapper()
    {
        var map = new BiDictionary<string, Type>();
        map.Add(nameof(UserCreatedNotification), typeof(UserCreatedNotification));
        _mapper = new DomainNotificationsMapper(map);
    }

    public string? GetName(Type type) => _mapper.GetName(type);

    public Type? GetType(string name) => _mapper.GetType(name);

    public Type? GetNotificationType(Type domainEventType) =>
        _mapper.GetNotificationType(domainEventType);
}
