using TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Configuration.Processing.Outbox;

public sealed class TrafficFineDomainNotificationsMapper :
    IDomainNotificationsMapper
{
    private readonly DomainNotificationsMapper _mapper;

    public TrafficFineDomainNotificationsMapper()
    {
        var map = new BiDictionary<string, Type>();
        map.Add(nameof(FineCreatedNotification), typeof(FineCreatedNotification));
        map.Add(nameof(FineManagerApprovedNotification), typeof(FineManagerApprovedNotification));
        map.Add(nameof(FineFinanceApprovedNotification), typeof(FineFinanceApprovedNotification));
        map.Add(nameof(FineRejectedNotification), typeof(FineRejectedNotification));
        map.Add(nameof(FineCompletedNotification), typeof(FineCompletedNotification));
        _mapper = new DomainNotificationsMapper(map);
    }

    public string? GetName(Type type) => _mapper.GetName(type);

    public Type? GetType(string name) => _mapper.GetType(name);

    public Type? GetNotificationType(Type domainEventType) =>
        _mapper.GetNotificationType(domainEventType);
}
