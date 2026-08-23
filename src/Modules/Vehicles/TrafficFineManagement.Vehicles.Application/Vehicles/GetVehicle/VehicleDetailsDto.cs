using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;

public sealed record VehicleDetailsDto(
    Guid Id,
    string Plaka,
    string Brand,
    string Model,
    VehicleType Type,
    bool Status,
    IReadOnlyCollection<VehicleUserDto> Users);

public sealed record VehicleUserDto(
    Guid UserId,
    DateTime StartTime,
    DateTime? EndTime);
