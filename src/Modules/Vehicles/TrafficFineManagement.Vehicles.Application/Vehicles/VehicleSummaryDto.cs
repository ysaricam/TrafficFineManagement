namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

public sealed record VehiclesSummaryDto(
    Guid Id,
    string Plaka,
    string Brand,
    string Model);