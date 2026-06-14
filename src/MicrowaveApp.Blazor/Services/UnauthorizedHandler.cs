using System.Net;
using Microsoft.AspNetCore.Components;

namespace MicrowaveApp.Blazor.Services;

public sealed class UnauthorizedHandler : DelegatingHandler
{
    private readonly IAuthState _authState;
    private readonly NavigationManager _nav;

    public UnauthorizedHandler(IAuthState authState, NavigationManager nav)
    {
        _authState = authState;
        _nav = nav;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized && _authState.IsAuthenticated)
        {
            await _authState.SignOutAsync();
            _nav.NavigateTo("/login", replace: true);
        }

        return response;
    }
}
