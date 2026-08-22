using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;

public sealed class AddUserToVehicleCommandHandler : ICommandHandler<AddUserToVehicleCommand>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUserRepository _userRepository;

    public AddUserToVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IUserRepository userRepository)
    {
        _vehicleRepository = vehicleRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(
        AddUserToVehicleCommand request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            new VehicleId(request.VehicleId),
            cancellationToken);

        if (vehicle is null)
        {
            throw new KeyNotFoundException($"Vehicle '{request.VehicleId}' was not found.");
        }

        var user = await _userRepository.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User '{request.UserId}' was not found.");
        }

        vehicle.AddUser(user.Id, request.StartTime);
    }
}
