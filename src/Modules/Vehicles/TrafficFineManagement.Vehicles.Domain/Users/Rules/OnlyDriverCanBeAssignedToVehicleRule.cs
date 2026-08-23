using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Users.Rules;

public sealed class OnlyDriverCanBeAssignedToVehicleRule : IBusinessRule
{
    private readonly UserRole _role;

    public OnlyDriverCanBeAssignedToVehicleRule(UserRole role)
    {
        _role = role;
    }

    public string Message => "Only users with the Driver role can be assigned to a vehicle.";

    public bool IsBroken()
    {
        return _role != UserRole.Driver;
    }
}
