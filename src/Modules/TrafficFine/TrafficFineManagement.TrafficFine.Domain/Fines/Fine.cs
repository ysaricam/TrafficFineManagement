using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Events;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines.Rules;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Domain.Fines;

public sealed class Fine : Entity, IAggregateRoot
{
    private UserId _finedUserId = null!;
    private VehicleId _vehicleId = null!;
    private Money _amount = null!;
    private string _violationCode = string.Empty;
    private string _reason = string.Empty;
    private DateTime _fineDate;
    private FineStatus _status;
    private FineActionType _currentAction;
    private readonly List<FineApprovalHistory> _approvalHistory;

    private Fine()
    {
        _approvalHistory = [];
    }

    private Fine(
        FineId id,
        UserId finedUserId,
        VehicleId vehicleId,
        Money amount,
        string violationCode,
        string reason,
        DateTime fineDate,
        UserId createdByUserId)
    {
        ArgumentNullException.ThrowIfNull(finedUserId);
        ArgumentNullException.ThrowIfNull(vehicleId);
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentException.ThrowIfNullOrWhiteSpace(violationCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (fineDate == default)
        {
            throw new ArgumentException(
                "Fine date must be specified.",
                nameof(fineDate));
        }

        ArgumentNullException.ThrowIfNull(createdByUserId);

        Id = id;
        _finedUserId = finedUserId;
        _vehicleId = vehicleId;
        _amount = amount;
        _violationCode = violationCode.Trim();
        _reason = reason.Trim();
        _fineDate = NormalizeToUtc(fineDate);
        _status = FineStatus.Active;
        _currentAction = FineActionType.Created;
        _approvalHistory = [];

        AddApprovalHistory(
            createdByUserId,
            FineActionType.Created,
            description: null,
            previousStatus: FineStatus.Active,
            newStatus: FineStatus.Active);

        AddDomainEvent(new FineCreatedDomainEvent(
            Id,
            _finedUserId,
            _vehicleId,
            _amount,
            _violationCode,
            _reason,
            _fineDate,
            createdByUserId));
    }

    public FineId Id { get; private set; } = null!;

    public UserId FinedUserId => _finedUserId;

    public VehicleId VehicleId => _vehicleId;

    public Money Amount => _amount;

    public string ViolationCode => _violationCode;

    public string Reason => _reason;

    public DateTime FineDate => _fineDate;

    public FineStatus Status => _status;

    public FineActionType CurrentAction => _currentAction;

    public IReadOnlyCollection<FineApprovalHistory> ApprovalHistory =>
        _approvalHistory.AsReadOnly();

    public static Fine Create(
        UserId finedUserId,
        VehicleId vehicleId,
        Money amount,
        string violationCode,
        string reason,
        DateTime fineDate,
        UserId createdByUserId)
    {
        return new Fine(
            new FineId(Guid.NewGuid()),
            finedUserId,
            vehicleId,
            amount,
            violationCode,
            reason,
            fineDate,
            createdByUserId);
    }

    public void ApproveByManager(
        UserId performedByUserId,
        string? description = null)
    {
        Transition(
            expectedCurrentAction: FineActionType.Created,
            newAction: FineActionType.ManagerApproved,
            newStatus: FineStatus.Active,
            performedByUserId,
            description);

        AddDomainEvent(new FineManagerApprovedDomainEvent(
            Id,
            performedByUserId,
            description));
    }

    public void ApproveByFinance(
        UserId performedByUserId,
        string? description = null)
    {
        Transition(
            expectedCurrentAction: FineActionType.ManagerApproved,
            newAction: FineActionType.FinanceApproved,
            newStatus: FineStatus.Active,
            performedByUserId,
            description);

        AddDomainEvent(new FineFinanceApprovedDomainEvent(
            Id,
            performedByUserId,
            description));
    }

    public void Reject(
        UserId performedByUserId,
        string rejectionReason)
    {
        CheckRule(new FineMustBeActiveRule(_status));
        CheckRule(new FineCanOnlyBeRejectedDuringApprovalRule(_currentAction));
        CheckRule(new FineRejectionReasonMustBeProvidedRule(rejectionReason));

        RecordAction(
            FineActionType.Rejected,
            FineStatus.Passive,
            performedByUserId,
            rejectionReason);

        AddDomainEvent(new FineRejectedDomainEvent(
            Id,
            performedByUserId,
            rejectionReason));
    }

    public void Complete(
        UserId performedByUserId,
        string? description = null)
    {
        Transition(
            expectedCurrentAction: FineActionType.FinanceApproved,
            newAction: FineActionType.Completed,
            newStatus: FineStatus.Passive,
            performedByUserId,
            description);

        AddDomainEvent(new FineCompletedDomainEvent(
            Id,
            performedByUserId,
            description));
    }

    private void Transition(
        FineActionType expectedCurrentAction,
        FineActionType newAction,
        FineStatus newStatus,
        UserId performedByUserId,
        string? description)
    {
        CheckRule(new FineMustBeActiveRule(_status));
        CheckRule(new FineActionMustFollowRule(
            _currentAction,
            expectedCurrentAction,
            newAction));

        RecordAction(
            newAction,
            newStatus,
            performedByUserId,
            description);
    }

    private void RecordAction(
        FineActionType actionType,
        FineStatus newStatus,
        UserId performedByUserId,
        string? description)
    {
        ArgumentNullException.ThrowIfNull(performedByUserId);

        var previousStatus = _status;
        _status = newStatus;
        _currentAction = actionType;

        AddApprovalHistory(
            performedByUserId,
            actionType,
            description,
            previousStatus,
            newStatus);
    }

    private void AddApprovalHistory(
        UserId performedByUserId,
        FineActionType actionType,
        string? description,
        FineStatus previousStatus,
        FineStatus newStatus)
    {
        _approvalHistory.Add(new FineApprovalHistory(
            performedByUserId,
            DateTime.UtcNow,
            actionType,
            description,
            previousStatus,
            newStatus));
    }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime();
    }
}
