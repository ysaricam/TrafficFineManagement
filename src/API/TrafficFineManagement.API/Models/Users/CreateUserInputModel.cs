using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Modules.Users.Domain.Users;

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

    [Required]
    public UserRole Role { get; init; }
}
