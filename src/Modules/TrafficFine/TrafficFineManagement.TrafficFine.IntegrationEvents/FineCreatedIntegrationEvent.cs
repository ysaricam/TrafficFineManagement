using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;

namespace TrafficFineManagement.Modules.TrafficFine.IntegrationEvents;

public sealed class FineCreatedIntegrationEvent : IntegrationEvent
{
    public FineCreatedIntegrationEvent(Guid id, DateTime occurredOn, Guid fineId,
        Guid finedUserId, Guid vehicleId, decimal amount, string currency,
        string violationCode, string reason, DateTime fineDate, Guid createdByUserId)
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
