using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;

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
}

public sealed record CreateFineRequest(Guid FinedUserId, Guid VehicleId,
    decimal Amount, string Currency, string ViolationCode, string Reason,
    DateTime FineDate, Guid CreatedByUserId);

public sealed record CreateFineResponse(Guid FineId);
