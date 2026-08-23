using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

public sealed class VehicleDto
{
    public Guid Id { get; set; }

    public string Plaka { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public VehicleType Type { get; set; }

    public bool Status { get; set; }

    public Guid? ActiveUserId { get; set; }

    public DateTime? ActiveUsageStartTime { get; set; }

    public DateTime LastModifiedAt { get; set; }
}
