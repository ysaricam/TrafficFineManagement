using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.API.Models.Vehicles;

public sealed class AssignVehicleUserInputModel
{
    public Guid VehicleId { get; init; }

    public Guid UserId { get; init; }

    public DateTime? StartTime { get; init; }

    [Range(-840, 840)]
    public int? TimeZoneOffsetMinutes { get; init; }
}
