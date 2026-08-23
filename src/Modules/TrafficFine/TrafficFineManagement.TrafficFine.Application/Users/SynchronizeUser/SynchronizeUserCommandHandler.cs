using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Users.SynchronizeUser;

public sealed class SynchronizeUserCommandHandler : ICommandHandler<SynchronizeUserCommand>
{
    private readonly IUserRepository _userRepository;

    public SynchronizeUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(
        SynchronizeUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        if (await _userRepository.GetByIdAsync(userId, cancellationToken) is not null)
        {
            return;
        }

        await _userRepository.AddAsync(
            User.Create(request.UserId, request.Role),
            cancellationToken);
    }
}
