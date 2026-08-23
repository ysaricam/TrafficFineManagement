namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public enum FineActionType
{
    Created = 0,
    ManagerApproved = 1,
    FinanceApproved = 2,
    Rejected = 3,
    Completed = 4
}
