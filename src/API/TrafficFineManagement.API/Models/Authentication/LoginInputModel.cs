using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.Authentication;

public sealed class LoginInputModel
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    public string? ReturnUrl { get; init; }
}
