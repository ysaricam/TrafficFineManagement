using TrafficFineManagement.Modules.Vehicles.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

namespace TrafficFineManagement.API.Models.Vehicles;

public sealed class VehicleListViewModel
{
    public VehicleListViewModel(
        IReadOnlyCollection<VehicleDto> vehicles,
        IReadOnlyCollection<UserDto> users)
    {
        Vehicles = vehicles;
        Users = users;
    }

    public IReadOnlyCollection<VehicleDto> Vehicles { get; }

    public IReadOnlyCollection<UserDto> Users { get; }

    public IReadOnlyCollection<VehicleDto> AvailableVehicles =>
        Vehicles.Where(vehicle => !vehicle.Status).ToArray();

    public IReadOnlyCollection<VehicleDto> ActiveVehicles =>
        Vehicles.Where(vehicle =>
            vehicle.Status && vehicle.ActiveUserId.HasValue).ToArray();

    public int TotalCount => Vehicles.Count;

    public int InUseCount => Vehicles.Count(vehicle => vehicle.Status);

    public int AvailableCount => TotalCount - InUseCount;

    public int UserCount => Users.Count;
}
