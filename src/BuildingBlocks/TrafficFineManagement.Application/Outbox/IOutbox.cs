namespace TrafficFineManagement.BuildingBlocks.Application.Outbox;

public interface IOutbox
{
    void Add(OutboxMessage message);

    Task Save();
}
