using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;

namespace TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;

public sealed class GetAllFinesQuery : QueryBase<IReadOnlyCollection<FineDto>>
{
}
