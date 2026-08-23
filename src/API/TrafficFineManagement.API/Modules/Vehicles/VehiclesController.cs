using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

namespace TrafficFineManagement.API.Modules.Vehicles;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehiclesModule _vehiclesModule;

    public VehiclesController(IVehiclesModule vehiclesModule)
    {
        _vehiclesModule = vehiclesModule;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VehicleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicles(CancellationToken cancellationToken)
    {
        var vehicles = await _vehiclesModule.ExecuteQueryAsync(
            new GetAllVehiclesQuery(),
            cancellationToken);

        return Ok(vehicles);
    }

    [HttpGet("{vehicleId:guid}")]
    [ProducesResponseType(typeof(VehicleDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicle(
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehiclesModule.ExecuteQueryAsync(
            new GetVehicleQuery(vehicleId),
            cancellationToken);

        return vehicle is null ? NotFound() : Ok(vehicle);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVehicle(
        [FromBody] VehicleRequest request,
        CancellationToken cancellationToken)
    {
        var vehicleId = await _vehiclesModule.ExecuteCommandAsync(
            new VehicleCommand(
                request.Plaka,
                request.Brand,
                request.Model,
                request.Type),
            cancellationToken);

        return Ok(new VehicleResponse(vehicleId));
    }

    [HttpPost("{vehicleId:guid}/users")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddUser(
        Guid vehicleId,
        [FromBody] AddUserToVehicleRequest request,
        CancellationToken cancellationToken)
    {
        await _vehiclesModule.ExecuteCommandAsync(
            new AddUserToVehicleCommand(
                vehicleId,
                request.UserId,
                request.StartTime),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{vehicleId:guid}/users/{userId:guid}/complete")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteUsage(
        Guid vehicleId,
        Guid userId,
        [FromBody] CompleteVehicleUsageRequest request,
        CancellationToken cancellationToken)
    {
        await _vehiclesModule.ExecuteCommandAsync(
            new CompleteVehicleUsageCommand(
                vehicleId,
                userId,
                request.EndTime),
            cancellationToken);

        return NoContent();
    }
}
