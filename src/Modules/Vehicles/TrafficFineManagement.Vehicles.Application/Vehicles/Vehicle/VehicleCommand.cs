using TrafficFineManagement.Modules.Vehicles.Application.Contracts;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

public sealed class VehicleCommand : CommandBase<Guid>
{
    public VehicleCommand(
        string plaka,
        string brand,
        string model)
    {
        Plaka = plaka;
        Brand = brand;
        Model = model;
    }

    public string Plaka { get; }
    public string Brand { get; }
    public string Model { get; }
}
