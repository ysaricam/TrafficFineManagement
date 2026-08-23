using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrafficFineManagement.API.Authentication;
using TrafficFineManagement.API.Infrastructure.Time;
using TrafficFineManagement.API.Models.TrafficFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.Users.Application.Contracts;
using TrafficFineManagement.Modules.Users.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleForUserAtTime;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetVehicleUserAtTime;

namespace TrafficFineManagement.API.Controllers;

[Route("traffic-fines")]
[Authorize]
public sealed class TrafficFinesPageController : Controller
{
    private readonly ITrafficFineModule _trafficFineModule;
    private readonly IVehiclesModule _vehiclesModule;
    private readonly IUsersModule _usersModule;

    public TrafficFinesPageController(
        ITrafficFineModule trafficFineModule,
        IVehiclesModule vehiclesModule,
        IUsersModule usersModule)
    {
        _trafficFineModule = trafficFineModule;
        _vehiclesModule = vehiclesModule;
        _usersModule = usersModule;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var fines = await _trafficFineModule.ExecuteQueryAsync(
            new GetAllFinesQuery(), cancellationToken);
        var vehicles = await _vehiclesModule.ExecuteQueryAsync(
            new GetAllVehiclesQuery(), cancellationToken);
        var users = await _usersModule.ExecuteQueryAsync(
            new GetAllUsersQuery(), cancellationToken);

        return View("~/Views/TrafficFines/Index.cshtml",
            new FineListViewModel(fines, vehicles, users));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Driver,FineOfficer,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateFineInputModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Ceza bilgileri eksik veya geçersiz.";
            return RedirectToAction(nameof(Index));
        }

        var fineDate = BrowserLocalTime.ToUtc(
            input.FineDate,
            input.TimeZoneOffsetMinutes);
        var createdByUserId = User.GetUserId();
        Guid finedUserId;
        Guid vehicleId;

        if (User.IsInRole("FineOfficer") || User.IsInRole("Admin"))
        {
            if (!input.VehicleId.HasValue || input.VehicleId == Guid.Empty)
            {
                TempData["Error"] = "Ceza oluşturmak için araç seçmelisiniz.";
                return RedirectToAction(nameof(Index));
            }

            var vehicleUser = await _vehiclesModule.ExecuteQueryAsync(
                new GetVehicleUserAtTimeQuery(input.VehicleId.Value, fineDate),
                cancellationToken);

            if (vehicleUser is null)
            {
                TempData["Error"] =
                    "Seçilen araçta ceza tarihinde atanmış bir sürücü bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            finedUserId = vehicleUser.UserId;
            vehicleId = input.VehicleId.Value;
        }
        else
        {
            var vehicle = await _vehiclesModule.ExecuteQueryAsync(
                new GetVehicleForUserAtTimeQuery(createdByUserId, fineDate),
                cancellationToken);

            if (vehicle is null)
            {
                TempData["Error"] =
                    "Ceza tarihinde kullandığınız bir araç bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            finedUserId = createdByUserId;
            vehicleId = vehicle.VehicleId;
        }

        var fineId = await _trafficFineModule.ExecuteCommandAsync(
            new CreateFineCommand(finedUserId, vehicleId, input.Amount,
                input.Currency, input.ViolationCode, input.Reason, fineDate,
                createdByUserId), cancellationToken);

        TempData["Success"] = $"Ceza kaydı oluşturuldu: {fineId}";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("manager-approve")]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveByManager(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByManagerCommand(
                input.FineId, User.GetUserId(), input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza yönetici tarafından onaylandı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("finance-approve")]
    [Authorize(Roles = "Finance,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveByFinance(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByFinanceCommand(
                input.FineId, User.GetUserId(), input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza finans tarafından onaylandı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("reject")]
    [Authorize(Roles = "Manager,Finance,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(
        RejectFineInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new RejectFineCommand(
                input.FineId, User.GetUserId(), input.RejectionReason),
            cancellationToken);
        TempData["Success"] = "Ceza reddedildi ve pasife alındı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("complete")]
    [Authorize(Roles = "FineOfficer,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new CompleteFineCommand(
                input.FineId, User.GetUserId(), input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza süreci tamamlandı.";
        return RedirectToAction(nameof(Index));
    }
}
