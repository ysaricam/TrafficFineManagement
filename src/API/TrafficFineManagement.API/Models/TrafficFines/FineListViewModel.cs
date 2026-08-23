using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

namespace TrafficFineManagement.API.Models.TrafficFines;

public sealed class FineListViewModel
{
    private readonly IReadOnlyDictionary<Guid, VehicleDto> _vehiclesById;
    private readonly IReadOnlyDictionary<Guid, UserDto> _usersById;

    public FineListViewModel(
        IReadOnlyCollection<FineDto> fines,
        IReadOnlyCollection<VehicleDto> vehicles,
        IReadOnlyCollection<UserDto> users)
    {
        Fines = fines;
        Vehicles = vehicles;
        Users = users;
        _vehiclesById = vehicles.ToDictionary(vehicle => vehicle.Id);
        _usersById = users.ToDictionary(user => user.Id);
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

    public string GetVehicleTypeLabel(Guid vehicleId)
    {
        return _vehiclesById.TryGetValue(vehicleId, out var vehicle)
            ? VehicleTypeLabel(vehicle.Type)
            : "—";
    }

    public string GetUserLabel(Guid userId)
    {
        return _usersById.TryGetValue(userId, out var user)
            ? $"{user.Name} {user.Surname} (@{user.Username})"
            : userId.ToString();
    }

    private static string VehicleTypeLabel(
        TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType type)
    {
        return type switch
        {
            TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType.Passenger => "Binek",
            TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType.Tractor => "Çekici",
            TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType.Trailer => "Dorse",
            TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.VehicleType.Rental => "Kiralık araç",
            _ => type.ToString()
        };
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
