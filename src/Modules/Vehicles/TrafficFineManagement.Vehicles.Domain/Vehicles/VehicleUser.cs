using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

public class VehicleUser : Entity
{
    public UserId UserId { get; private set; } = null!;
    private DateTime _startTime;
    private DateTime? _endTime;

    public DateTime StartTime => _startTime;
    public DateTime? EndTime => _endTime;

    private VehicleUser() { }

    internal static VehicleUser Create(UserId userId, DateTime startTime)
    {
        return new VehicleUser(userId, startTime);
    }

    private VehicleUser(UserId userId, DateTime startTime)
    {
        UserId = userId;
        _startTime = startTime;
    }

    internal void Complete(DateTime endTime)
    {
        _endTime = endTime;
    }
}
