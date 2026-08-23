using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;

public sealed class GetFineDetailsQuery : QueryBase<FineDetailsDto?>
{
    public GetFineDetailsQuery(Guid fineId)
    {
        FineId = fineId;
    }

    public Guid FineId { get; }
}
