using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrafficFineManagement.API.Infrastructure.Time;
using TrafficFineManagement.API.Models.Vehicles;
using TrafficFineManagement.API.Models.Users;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.CreateUser;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.AddUserToVehicle;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.CompleteVehicleUsage;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.Vehicle;

namespace TrafficFineManagement.API.Controllers;

[Route("vehicles")]
[Authorize]
public sealed class VehiclesPageController : Controller
{
    private readonly IVehiclesModule _vehiclesModule;
    private readonly IUsersModule _usersModule;

    public VehiclesPageController(
        IVehiclesModule vehiclesModule,
        IUsersModule usersModule)
    {
        _vehiclesModule = vehiclesModule;
        _usersModule = usersModule;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var vehicles = await _vehiclesModule.ExecuteQueryAsync(
            new GetAllVehiclesQuery(),
            cancellationToken);
        var users = await _usersModule.ExecuteQueryAsync(
            new GetAllUsersQuery(),
            cancellationToken);

        return View(
            "~/Views/Vehicles/Index.cshtml",
            new VehicleListViewModel(vehicles, users));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVehicle(
        CreateVehicleInputModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Araç bilgileri eksik veya geçersiz.";
            return RedirectToAction(nameof(Index));
        }

        var vehicleId = await _vehiclesModule.ExecuteCommandAsync(
            new VehicleCommand(input.Plaka, input.Brand, input.Model),
            cancellationToken);

        TempData["Success"] = $"{input.Plaka} plakalı araç oluşturuldu.";
        TempData["CreatedVehicleId"] = vehicleId.ToString();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("users/create")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(
        CreateUserInputModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Kullanıcı bilgileri eksik veya geçersiz.";
            return RedirectToAction(nameof(Index));
        }

        var userId = await _usersModule.ExecuteCommandAsync(
            new CreateUserCommand(
                input.Name,
                input.Surname,
                input.Username,
                input.Password,
                input.Role),
            cancellationToken);

        TempData["Success"] = $"Yeni kullanıcı oluşturuldu: {userId}";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("users/assign")]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignUser(
        AssignVehicleUserInputModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid ||
            input.VehicleId == Guid.Empty ||
            input.UserId == Guid.Empty)
        {
            TempData["Error"] = "Araç ve kullanıcı seçimi zorunludur.";
            return RedirectToAction(nameof(Index));
        }

        var startTime = input.StartTime.HasValue
            ? BrowserLocalTime.ToUtc(
                input.StartTime.Value,
                input.TimeZoneOffsetMinutes)
            : DateTime.UtcNow;

        if (startTime > DateTime.UtcNow.AddMinutes(1))
        {
            TempData["Error"] = "Başlangıç zamanı gelecekte olamaz.";
            return RedirectToAction(nameof(Index));
        }

        await _vehiclesModule.ExecuteCommandAsync(
            new AddUserToVehicleCommand(
                input.VehicleId,
                input.UserId,
                startTime),
            cancellationToken);

        TempData["Success"] = "Kullanıcı araca atandı.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("users/complete")]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteUsage(
        Guid vehicleId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        if (vehicleId == Guid.Empty || userId == Guid.Empty)
        {
            TempData["Error"] = "Aktif kullanım bilgisi geçersiz.";
            return RedirectToAction(nameof(Index));
        }

        await _vehiclesModule.ExecuteCommandAsync(
            new CompleteVehicleUsageCommand(
                vehicleId,
                userId,
                DateTime.UtcNow),
            cancellationToken);

        TempData["Success"] = "Araç kullanımı tamamlandı.";

        return RedirectToAction(nameof(Index));
    }
}
