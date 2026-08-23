using Newtonsoft.Json;
using TrafficFineManagement.BuildingBlocks.Application.Events;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.Notifications;

public sealed class FineCreatedNotification : DomainNotificationBase<FineCreatedDomainEvent>
{
    public FineCreatedNotification(FineCreatedDomainEvent domainEvent)
        : this(domainEvent.Id, domainEvent.FineId.Value, domainEvent.FinedUserId.Value,
            domainEvent.VehicleId.Value, domainEvent.Amount.Amount,
            domainEvent.Amount.Currency, domainEvent.ViolationCode, domainEvent.Reason,
            domainEvent.FineDate, domainEvent.CreatedByUserId.Value, domainEvent.OccurredOn)
    {
    }

    [JsonConstructor]
    public FineCreatedNotification(Guid id, Guid fineId, Guid finedUserId,
        Guid vehicleId, decimal amount, string currency, string violationCode,
        string reason, DateTime fineDate, Guid createdByUserId, DateTime occurredOn)
        : base(id, occurredOn)
    {
        FineId = fineId;
        FinedUserId = finedUserId;
        VehicleId = vehicleId;
        Amount = amount;
        Currency = currency;
        ViolationCode = violationCode;
        Reason = reason;
        FineDate = fineDate;
        CreatedByUserId = createdByUserId;
    }

    public Guid FineId { get; }
    public Guid FinedUserId { get; }
    public Guid VehicleId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string ViolationCode { get; }
    public string Reason { get; }
    public DateTime FineDate { get; }
    public Guid CreatedByUserId { get; }
}
