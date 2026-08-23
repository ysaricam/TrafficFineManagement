using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;

public sealed class CreateFineCommand : CommandBase<Guid>
{
    public CreateFineCommand(Guid finedUserId, Guid vehicleId, decimal amount,
        string currency, string violationCode, string reason, DateTime fineDate,
        Guid createdByUserId)
    {
        FinedUserId = finedUserId;
        VehicleId = vehicleId;
        Amount = amount;
        Currency = currency;
        ViolationCode = violationCode;
        Reason = reason;
        FineDate = fineDate;
        CreatedByUserId = createdByUserId;
    }

    public Guid FinedUserId { get; }
    public Guid VehicleId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string ViolationCode { get; }
    public string Reason { get; }
    public DateTime FineDate { get; }
    public Guid CreatedByUserId { get; }
}
