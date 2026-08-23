using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.API.Models.Vehicles;

public sealed class VehicleListViewModel
{
    private readonly IReadOnlyDictionary<Guid, UserDto> _usersById;

    public VehicleListViewModel(
        IReadOnlyCollection<VehicleDto> vehicles,
        IReadOnlyCollection<UserDto> users)
    {
        Vehicles = vehicles;
        Users = users;
        _usersById = users.ToDictionary(user => user.Id);
    }

    public IReadOnlyCollection<VehicleDto> Vehicles { get; }

    public IReadOnlyCollection<UserDto> Users { get; }

    public IReadOnlyCollection<UserDto> Drivers =>
        Users.Where(user => user.Role ==
            TrafficFineManagement.Modules.Users.Domain.Users.UserRole.Driver)
            .ToArray();

    public IReadOnlyCollection<VehicleDto> AvailableVehicles =>
        Vehicles.Where(vehicle => !vehicle.Status).ToArray();

    public IReadOnlyCollection<VehicleDto> ActiveVehicles =>
        Vehicles.Where(vehicle =>
            vehicle.Status && vehicle.ActiveUserId.HasValue).ToArray();

    public int TotalCount => Vehicles.Count;

    public int InUseCount => Vehicles.Count(vehicle => vehicle.Status);

    public int AvailableCount => TotalCount - InUseCount;

    public int UserCount => Users.Count;

    public string GetUserLabel(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return "—";
        }

        return _usersById.TryGetValue(userId.Value, out var user)
            ? $"{user.Name} {user.Surname} (@{user.Username})"
            : userId.Value.ToString();
    }

    public static string GetVehicleTypeLabel(VehicleType type)
    {
        return type switch
        {
            VehicleType.Passenger => "Binek",
            VehicleType.Tractor => "Çekici",
            VehicleType.Trailer => "Dorse",
            VehicleType.Rental => "Kiralık araç",
            _ => type.ToString()
        };
    }
}
