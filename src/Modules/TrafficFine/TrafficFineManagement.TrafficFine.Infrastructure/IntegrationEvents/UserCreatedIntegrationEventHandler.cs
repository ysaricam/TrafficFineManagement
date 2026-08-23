using Microsoft.Extensions.DependencyInjection;
using TrafficFineManagement.BuildingBlocks.Infrastructure.EventBus;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.Users.IntegrationEvents;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.IntegrationEvents;

public sealed class UserCreatedIntegrationEventHandler :
    IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserCreatedIntegrationEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(
        UserCreatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var userId = new UserId(integrationEvent.UserId);

        if (await repository.GetByIdAsync(userId, cancellationToken) is not null)
        {
            return;
        }

        await repository.AddAsync(
            User.Create(integrationEvent.UserId, (UserRole)integrationEvent.Role),
            cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
