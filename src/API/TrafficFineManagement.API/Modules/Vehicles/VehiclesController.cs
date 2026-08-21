using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles;

namespace TrafficFineManagament.API.Modules.Vehicles;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehiclesModule _vehiclesModule;

    public VehiclesController(IVehiclesModule vehiclesModule)
    {
        _vehiclesModule = vehiclesModule;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VehiclesSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicles(CancellationToken cancellationToken)
    {
        var vehicles = await _vehiclesModule.GetVehiclesSummariesAsync(cancellationToken);

        return Ok(vehicles);
    }
}