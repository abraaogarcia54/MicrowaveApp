using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Api.Controllers;

[ApiController]
[Route("api/microwave")]
[Authorize]
public sealed class MicrowaveController : ControllerBase
{
    private readonly IMicrowaveServiceFactory _factory;

    public MicrowaveController(IMicrowaveServiceFactory factory)
    {
        _factory = factory;
    }

    [HttpPost("quick-start")]
    public async Task<ActionResult<HeatingSessionResponse>> QuickStartAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.QuickStartAsync(cancellationToken));
    }

    [HttpPost("start")]
    public async Task<ActionResult<HeatingSessionResponse>> StartAsync(
        StartHeatingRequest request,
        CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.StartAsync(request, cancellationToken));
    }

    [HttpPost("programs/{programId:int}/start")]
    public async Task<ActionResult<HeatingSessionResponse>> StartProgramAsync(
        int programId,
        CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.StartProgramAsync(programId, cancellationToken));
    }

    [HttpPost("add-time")]
    public async Task<ActionResult<HeatingSessionResponse>> AddTimeAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.AddTimeAsync(cancellationToken));
    }

    [HttpPost("pause")]
    public async Task<ActionResult<HeatingSessionResponse>> PauseAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.PauseAsync(cancellationToken));
    }

    [HttpPost("pause-or-cancel")]
    public async Task<ActionResult<HeatingSessionResponse?>> PauseOrCancelAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.PauseOrCancelAsync(cancellationToken));
    }

    [HttpPost("resume")]
    public async Task<ActionResult<HeatingSessionResponse>> ResumeAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.ResumeAsync(cancellationToken));
    }

    [HttpPost("advance")]
    public async Task<ActionResult<HeatingSessionResponse>> AdvanceAsync(
        [FromQuery] int seconds = 1,
        CancellationToken cancellationToken = default)
    {
        var service = await CreateServiceAsync(cancellationToken);
        return Ok(await service.AdvanceAsync(seconds, cancellationToken));
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelAsync(CancellationToken cancellationToken)
    {
        var service = await CreateServiceAsync(cancellationToken);
        await service.CancelAsync(cancellationToken);
        return NoContent();
    }

    private async Task<IMicrowaveService> CreateServiceAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Usuário não autenticado.");

        return await _factory.CreateForUserAsync(userId, cancellationToken);
    }
}
