using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Rules;

public sealed class VehicleUsageEndTimeMustNotPrecedeStartTimeRule : IBusinessRule
{
    private readonly DateTime _startTime;
    private readonly DateTime _endTime;

    public VehicleUsageEndTimeMustNotPrecedeStartTimeRule(
        DateTime startTime,
        DateTime endTime)
    {
        _startTime = startTime;
        _endTime = endTime;
    }

    public string Message =>
        "Vehicle usage end time cannot be earlier than its start time.";

    public bool IsBroken()
    {
        return _endTime < _startTime;
    }
}
