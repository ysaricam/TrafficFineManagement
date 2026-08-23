using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;

public sealed class FineCreatedDomainEvent : DomainEventBase
{
    public FineCreatedDomainEvent(
        FineId fineId,
        UserId finedUserId,
        VehicleId vehicleId,
        Money amount,
        string violationCode,
        string reason,
        DateTime fineDate,
        UserId createdByUserId)
    {
        FineId = fineId;
        FinedUserId = finedUserId;
        VehicleId = vehicleId;
        Amount = amount;
        ViolationCode = violationCode;
        Reason = reason;
        FineDate = fineDate;
        CreatedByUserId = createdByUserId;
    }

    public FineId FineId { get; }
    public UserId FinedUserId { get; }
    public VehicleId VehicleId { get; }
    public Money Amount { get; }
    public string ViolationCode { get; }
    public string Reason { get; }
    public DateTime FineDate { get; }
    public UserId CreatedByUserId { get; }
}
