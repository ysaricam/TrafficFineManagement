using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;

public sealed class CreateFineCommandHandler : ICommandHandler<CreateFineCommand, Guid>
{
    private readonly IFineRepository _fineRepository;
    private readonly IUserRepository _userRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public CreateFineCommandHandler(IFineRepository fineRepository,
        IUserRepository userRepository, IVehicleRepository vehicleRepository)
    {
        _fineRepository = fineRepository;
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Guid> Handle(
        CreateFineCommand request,
        CancellationToken cancellationToken)
    {
        var finedUserId = new UserId(request.FinedUserId);
        var createdByUserId = new UserId(request.CreatedByUserId);
        var vehicleId = new VehicleId(request.VehicleId);

        if (await _userRepository.GetByIdAsync(finedUserId, cancellationToken) is null)
        {
            throw new KeyNotFoundException($"Fined user '{request.FinedUserId}' was not found.");
        }

        if (await _userRepository.GetByIdAsync(createdByUserId, cancellationToken) is null)
        {
            throw new KeyNotFoundException($"Creating user '{request.CreatedByUserId}' was not found.");
        }

        if (await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken) is null)
        {
            throw new KeyNotFoundException($"Vehicle '{request.VehicleId}' was not found.");
        }

        var fine = Fine.Create(
            finedUserId,
            vehicleId,
            Money.Of(request.Amount, request.Currency),
            request.ViolationCode,
            request.Reason,
            request.FineDate,
            createdByUserId);

        await _fineRepository.AddAsync(fine, cancellationToken);

        return fine.Id.Value;
    }
}
