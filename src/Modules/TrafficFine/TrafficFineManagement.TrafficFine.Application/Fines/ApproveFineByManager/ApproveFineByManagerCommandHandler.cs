using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;

public sealed class ApproveFineByManagerCommandHandler :
    ICommandHandler<ApproveFineByManagerCommand>
{
    private readonly IFineRepository _fineRepository;
    private readonly IUserRepository _userRepository;

    public ApproveFineByManagerCommandHandler(
        IFineRepository fineRepository,
        IUserRepository userRepository)
    {
        _fineRepository = fineRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(
        ApproveFineByManagerCommand request,
        CancellationToken cancellationToken)
    {
        var fine = await _fineRepository.GetByIdAsync(
            new FineId(request.FineId), cancellationToken)
            ?? throw new KeyNotFoundException($"Fine '{request.FineId}' was not found.");

        var performedByUserId = new UserId(request.PerformedByUserId);
        var performingUser = await _userRepository.GetByIdAsync(
            performedByUserId,
            cancellationToken);
        if (performingUser is null)
        {
            throw new KeyNotFoundException(
                $"Performing user '{request.PerformedByUserId}' was not found.");
        }

        if (!performingUser.IsInRole(UserRole.Manager, UserRole.Admin))
        {
            throw new UnauthorizedAccessException(
                "Only a manager or administrator can approve this step.");
        }

        fine.ApproveByManager(performedByUserId, request.Description);
    }
}
