using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.API.Models.TrafficFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Contracts;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByFinance;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.ApproveFineByManager;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CompleteFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.CreateFine;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.GetAllFines;
using TrafficFineManagement.Modules.TrafficFine.Application.Fines.RejectFine;
using TrafficFineManagement.Modules.Vehicles.Application.Contracts;
using TrafficFineManagement.Modules.Vehicles.Application.Users.GetAllUsers;
using TrafficFineManagement.Modules.Vehicles.Application.Vehicles.GetAllVehicles;

namespace TrafficFineManagement.API.Controllers;

[Route("traffic-fines")]
public sealed class TrafficFinesPageController : Controller
{
    private readonly ITrafficFineModule _trafficFineModule;
    private readonly IVehiclesModule _vehiclesModule;

    public TrafficFinesPageController(
        ITrafficFineModule trafficFineModule,
        IVehiclesModule vehiclesModule)
    {
        _trafficFineModule = trafficFineModule;
        _vehiclesModule = vehiclesModule;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var fines = await _trafficFineModule.ExecuteQueryAsync(
            new GetAllFinesQuery(), cancellationToken);
        var vehicles = await _vehiclesModule.ExecuteQueryAsync(
            new GetAllVehiclesQuery(), cancellationToken);
        var users = await _vehiclesModule.ExecuteQueryAsync(
            new GetAllUsersQuery(), cancellationToken);

        return View("~/Views/TrafficFines/Index.cshtml",
            new FineListViewModel(fines, vehicles, users));
    }

    [HttpPost("create")]
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

        var fineDate = DateTime.SpecifyKind(input.FineDate, DateTimeKind.Utc);
        var fineId = await _trafficFineModule.ExecuteCommandAsync(
            new CreateFineCommand(input.FinedUserId, input.VehicleId, input.Amount,
                input.Currency, input.ViolationCode, input.Reason, fineDate,
                input.CreatedByUserId), cancellationToken);

        TempData["Success"] = $"Ceza kaydı oluşturuldu: {fineId}";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("manager-approve")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveByManager(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByManagerCommand(
                input.FineId, input.PerformedByUserId, input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza yönetici tarafından onaylandı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("finance-approve")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveByFinance(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new ApproveFineByFinanceCommand(
                input.FineId, input.PerformedByUserId, input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza finans tarafından onaylandı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("reject")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(
        RejectFineInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new RejectFineCommand(
                input.FineId, input.PerformedByUserId, input.RejectionReason),
            cancellationToken);
        TempData["Success"] = "Ceza reddedildi ve pasife alındı.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("complete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(
        FineActionInputModel input,
        CancellationToken cancellationToken)
    {
        await _trafficFineModule.ExecuteCommandAsync(
            new CompleteFineCommand(
                input.FineId, input.PerformedByUserId, input.Description),
            cancellationToken);
        TempData["Success"] = "Ceza süreci tamamlandı.";
        return RedirectToAction(nameof(Index));
    }
}
