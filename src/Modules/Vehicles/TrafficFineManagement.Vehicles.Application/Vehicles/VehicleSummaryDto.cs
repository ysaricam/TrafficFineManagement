namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

public sealed record VehicleSummaryDto(
    Guid Id,
    string Plaka,
    string Brand,
    string Model,
    bool Status);
