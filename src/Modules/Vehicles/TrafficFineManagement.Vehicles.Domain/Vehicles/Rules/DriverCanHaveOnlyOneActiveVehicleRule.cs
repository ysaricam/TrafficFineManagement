using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Rules;

public sealed class DriverCanHaveOnlyOneActiveVehicleRule : IBusinessRule
{
    private readonly bool _hasActiveVehicle;

    public DriverCanHaveOnlyOneActiveVehicleRule(bool hasActiveVehicle)
    {
        _hasActiveVehicle = hasActiveVehicle;
    }

    public string Message => "A driver can be assigned to only one active vehicle at a time.";

    public bool IsBroken()
    {
        return _hasActiveVehicle;
    }
}
