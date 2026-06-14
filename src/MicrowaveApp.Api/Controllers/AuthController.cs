using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> RegisterAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }
}
