using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.Vehicles.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

namespace TrafficFineManagement.API.Models.TrafficFines;

public sealed class FineListViewModel
{
    private readonly IReadOnlyDictionary<Guid, VehicleDto> _vehiclesById;

    public FineListViewModel(
        IReadOnlyCollection<FineDto> fines,
        IReadOnlyCollection<VehicleDto> vehicles,
        IReadOnlyCollection<UserDto> users)
    {
        Fines = fines;
        Vehicles = vehicles;
        Users = users;
        _vehiclesById = vehicles.ToDictionary(vehicle => vehicle.Id);
    }

    public IReadOnlyCollection<FineDto> Fines { get; }
    public IReadOnlyCollection<VehicleDto> Vehicles { get; }
    public IReadOnlyCollection<UserDto> Users { get; }

    public int TotalCount => Fines.Count;
    public int ActiveCount => Fines.Count(fine => fine.Status == FineStatus.Active);
    public int CompletedCount => Fines.Count(fine => fine.CurrentAction == FineActionType.Completed);
    public int RejectedCount => Fines.Count(fine => fine.CurrentAction == FineActionType.Rejected);

    public string GetVehicleLabel(Guid vehicleId)
    {
        return _vehiclesById.TryGetValue(vehicleId, out var vehicle)
            ? $"{vehicle.Plaka} · {vehicle.Brand} {vehicle.Model}"
            : vehicleId.ToString();
    }

    public static string GetActionLabel(FineActionType action)
    {
        return action switch
        {
            FineActionType.Created => "Yönetici onayı bekliyor",
            FineActionType.ManagerApproved => "Finans onayı bekliyor",
            FineActionType.FinanceApproved => "Tamamlanmayı bekliyor",
            FineActionType.Rejected => "Reddedildi",
            FineActionType.Completed => "Tamamlandı",
            _ => action.ToString()
        };
    }
}
