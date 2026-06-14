using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Api.Controllers;

[ApiController]
[Route("api/heating-programs")]
[Authorize]
public sealed class HeatingProgramsController : ControllerBase
{
    private readonly IHeatingProgramService _heatingProgramService;

    public HeatingProgramsController(IHeatingProgramService heatingProgramService)
    {
        _heatingProgramService = heatingProgramService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<HeatingProgramResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var programs = await _heatingProgramService.GetAllAsync(cancellationToken);
        return Ok(programs);
    }

    [HttpPost]
    public async Task<ActionResult<HeatingProgramResponse>> CreateAsync(
        CreateHeatingProgramRequest request,
        CancellationToken cancellationToken)
    {
        var program = await _heatingProgramService.CreateAsync(request, cancellationToken);
        return Created($"/api/heating-programs/{program.Id}", program);
    }
}
