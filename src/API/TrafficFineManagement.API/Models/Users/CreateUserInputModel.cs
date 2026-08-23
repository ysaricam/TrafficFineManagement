using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.Users;

public sealed class CreateUserInputModel
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Surname { get; init; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; init; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;

    public Guid VehicleId { get; init; }

    public DateTime? StartTime { get; init; }

    public int TimeZoneOffsetMinutes { get; init; }
}
