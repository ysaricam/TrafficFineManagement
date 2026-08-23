using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

public sealed class VehicleCommand : CommandBase<Guid>
{
    public VehicleCommand(
        string plaka,
        string brand,
        string model,
        VehicleType type)
    {
        Plaka = plaka;
        Brand = brand;
        Model = model;
        Type = type;
    }

    public string Plaka { get; }
    public string Brand { get; }
    public string Model { get; }
    public VehicleType Type { get; }
}
