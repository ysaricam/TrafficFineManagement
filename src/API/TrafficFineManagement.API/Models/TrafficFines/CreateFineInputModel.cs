using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.TrafficFines;

public sealed class CreateFineInputModel
{
    public Guid? VehicleId { get; init; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; init; }

    [Required, StringLength(3, MinimumLength = 3)]
    public string Currency { get; init; } = "TRY";

    [Required, StringLength(50)]
    public string ViolationCode { get; init; } = string.Empty;

    [Required, StringLength(1000)]
    public string Reason { get; init; } = string.Empty;

    [Required]
    public DateTime FineDate { get; init; }

    [Range(-840, 840)]
    public int? TimeZoneOffsetMinutes { get; init; }
}
