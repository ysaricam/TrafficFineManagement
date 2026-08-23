using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public sealed class FineId : TypedIdValueBase
{
    public FineId(Guid value)
        : base(value)
    {
    }
}
