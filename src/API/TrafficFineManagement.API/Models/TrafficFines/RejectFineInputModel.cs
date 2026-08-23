using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.TrafficFines;

public sealed class RejectFineInputModel
{
    [Required]
    public Guid FineId { get; init; }

    [Required]
    public Guid PerformedByUserId { get; init; }

    [Required, StringLength(1000)]
    public string RejectionReason { get; init; } = string.Empty;
}
