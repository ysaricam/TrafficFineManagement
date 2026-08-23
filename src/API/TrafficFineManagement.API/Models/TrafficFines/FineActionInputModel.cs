using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.TrafficFines;

public sealed class FineActionInputModel
{
    [Required]
    public Guid FineId { get; init; }

    [Required]
    public Guid PerformedByUserId { get; init; }

    [StringLength(1000)]
    public string? Description { get; init; }
}
