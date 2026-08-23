using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrafficFineManagement.API.Authentication;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetFineDetails;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleForUserAtTime;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleUserAtTime;

namespace TrafficFineManagement.API.Modules.TrafficFine;

[ApiController]
[Route("api/traffic-fines")]
[Authorize]
public sealed class TrafficFinesController : ControllerBase
{
    private readonly ITrafficFineModule _trafficFineModule;
    private readonly IVehiclesModule _vehiclesModule;

    public TrafficFinesController(
        ITrafficFineModule trafficFineModule,
        IVehiclesModule vehiclesModule)
    {
        _trafficFineModule = trafficFineModule;
        _vehiclesModule = vehiclesModule;
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
    [Authorize(Roles = "Driver,FineOfficer,Admin")]
    [ProducesResponseType(typeof(CreateFineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateFine(
        [FromBody] CreateFineRequest request,
        CancellationToken cancellationToken)
    {
        var fineDate = NormalizeToUtc(request.FineDate);
        var createdByUserId = User.GetUserId();
        Guid finedUserId;
        Guid vehicleId;

        if (User.IsInRole("FineOfficer") || User.IsInRole("Admin"))
        {
            if (!request.VehicleId.HasValue || request.VehicleId == Guid.Empty)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Araç seçilmedi",
                    detail: "Ceza görevlisi veya Admin ceza oluştururken araç seçmelidir.");
            }

            var vehicleUser = await _vehiclesModule.ExecuteQueryAsync(
                new GetVehicleUserAtTimeQuery(request.VehicleId.Value, fineDate),
                cancellationToken);

            if (vehicleUser is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Sürücü bulunamadı",
                    detail: "Seçilen araçta ceza tarihinde atanmış bir sürücü bulunamadı.");
            }

            finedUserId = vehicleUser.UserId;
            vehicleId = request.VehicleId.Value;
        }
        else
        {
            var vehicle = await _vehiclesModule.ExecuteQueryAsync(
                new GetVehicleForUserAtTimeQuery(createdByUserId, fineDate),
                cancellationToken);

            if (vehicle is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Araç kullanımı bulunamadı",
                    detail: "Ceza tarihinde kullandığınız bir araç bulunamadı.");
            }

            finedUserId = createdByUserId;
            vehicleId = vehicle.VehicleId;
        }

        var fineId = await _trafficFineModule.ExecuteCommandAsync(
            new CreateFineCommand(finedUserId, vehicleId,
                request.Amount, request.Currency, request.ViolationCode,
                request.Reason, fineDate, createdByUserId),
            cancellationToken);

        return Ok(new CreateFineResponse(fineId));
    }

    [HttpPatch("{fineId:guid}/manager-approval")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveByManager(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByManagerCommand(
                fineId, User.GetUserId(), request.Description),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/finance-approval")]
    [Authorize(Roles = "Finance,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveByFinance(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByFinanceCommand(
                fineId, User.GetUserId(), request.Description),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/reject")]
    [Authorize(Roles = "Manager,Finance,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reject(
        Guid fineId,
        [FromBody] RejectFineRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new RejectFineCommand(
                fineId, User.GetUserId(), request.RejectionReason),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{fineId:guid}/complete")]
    [Authorize(Roles = "FineOfficer,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Complete(
        Guid fineId,
        [FromBody] FineActionRequest request,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new CompleteFineCommand(
                fineId, User.GetUserId(), request.Description),
            cancellationToken);

        return NoContent();
    }
    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}

public sealed record CreateFineRequest(Guid? VehicleId,
    decimal Amount, string Currency, string ViolationCode, string Reason,
    DateTime FineDate);

public sealed record CreateFineResponse(Guid FineId);

public sealed record FineActionRequest(string? Description);

public sealed record RejectFineRequest(string RejectionReason);
