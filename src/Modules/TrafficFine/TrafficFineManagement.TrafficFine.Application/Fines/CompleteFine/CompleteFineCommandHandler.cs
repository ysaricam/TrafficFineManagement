using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;

public sealed class CompleteFineCommandHandler : ICommandHandler<CompleteFineCommand>
{
    private readonly IFineRepository _fineRepository;
    private readonly IUserRepository _userRepository;

    public CompleteFineCommandHandler(IFineRepository fineRepository, IUserRepository userRepository)
    {
        _fineRepository = fineRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(CompleteFineCommand request, CancellationToken cancellationToken)
    {
        var fine = await _fineRepository.GetByIdAsync(
            new FineId(request.FineId), cancellationToken)
            ?? throw new KeyNotFoundException($"Fine '{request.FineId}' was not found.");

        var performedByUserId = new UserId(request.PerformedByUserId);
        if (await _userRepository.GetByIdAsync(performedByUserId, cancellationToken) is null)
        {
            throw new KeyNotFoundException(
                $"Performing user '{request.PerformedByUserId}' was not found.");
        }

        fine.Complete(performedByUserId, request.Description);
    }
}
