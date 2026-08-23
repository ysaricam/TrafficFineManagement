using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.Vehicles;

public sealed class CreateVehicleInputModel
{
    [Required(ErrorMessage = "Plaka zorunludur.")]
    public string Plaka { get; init; } = string.Empty;

    [Required(ErrorMessage = "Marka zorunludur.")]
    public string Brand { get; init; } = string.Empty;

    [Required(ErrorMessage = "Model zorunludur.")]
    public string Model { get; init; } = string.Empty;
}
