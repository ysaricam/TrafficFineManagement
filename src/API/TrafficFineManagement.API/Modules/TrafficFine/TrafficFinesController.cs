using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;

namespace TrafficFineManagement.API.Modules.TrafficFine;

[ApiController]
[Route("api/traffic-fines")]
public sealed class TrafficFinesController : ControllerBase
{
    private readonly ITrafficFineModule _trafficFineModule;

    public TrafficFinesController(ITrafficFineModule trafficFineModule)
    {
        _trafficFineModule = trafficFineModule;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<FineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var fines = await _trafficFineModule.ExecuteQueryAsync(
            new GetAllFinesQuery(), cancellationToken);

        return Ok(fines);
    }

    [HttpGet("{fineId:guid}")]
    [ProducesResponseType(typeof(FineDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid fineId,
        CancellationToken cancellationToken)
    {
        var fine = await _trafficFineModule.ExecuteQueryAsync(
            new GetFineDetailsQuery(fineId), cancellationToken);

        return fine is null ? NotFound() : Ok(fine);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateFineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateFine(
        [FromBody] CreateFineRequest request,
        CancellationToken cancellationToken)
    {
        var fineId = await _trafficFineModule.ExecuteCommandAsync(
            new CreateFineCommand(request.FinedUserId, request.VehicleId,
                request.Amount, request.Currency, request.ViolationCode,
                request.Reason, request.FineDate, request.CreatedByUserId),
            cancellationToken);

        return Ok(new CreateFineResponse(fineId));
    }

    [HttpPatch("{fineId:guid}/manager-approval")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveByManager(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByManagerCommand(
                fineId, request.PerformedByUserId, request.Description),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/finance-approval")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveByFinance(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByFinanceCommand(
                fineId, request.PerformedByUserId, request.Description),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reject(
        Guid fineId,
        [FromBody] RejectFineRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new RejectFineCommand(
                fineId, request.PerformedByUserId, request.RejectionReason),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Complete(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new CompleteFineCommand(
                fineId, request.PerformedByUserId, request.Description),
            cancellationToken);

        return NoContent();
    }
}

public sealed record CreateFineRequest(Guid FinedUserId, Guid VehicleId,
    decimal Amount, string Currency, string ViolationCode, string Reason,
    DateTime FineDate, Guid CreatedByUserId);

public sealed record CreateFineResponse(Guid FineId);

public sealed record FineActionRequest(Guid PerformedByUserId, string? Description);

public sealed record RejectFineRequest(Guid PerformedByUserId, string RejectionReason);
