namespace TrafficFineManagement.API.Modules.Vehicles;

public sealed class VehicleRequest
{
    public string Plaka { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
}
