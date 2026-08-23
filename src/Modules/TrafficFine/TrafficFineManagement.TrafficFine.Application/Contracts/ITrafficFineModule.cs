namespace TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

public interface ITrafficFineModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default);

    Task ExecuteCommandAsync(
        ICommand command,
        CancellationToken cancellationToken = default);

    Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default);
}
