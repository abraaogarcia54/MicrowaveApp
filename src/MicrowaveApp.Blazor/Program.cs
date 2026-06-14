using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Blazor;
using MicrowaveApp.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5167";

builder.Services.AddScoped<UnauthorizedHandler>();
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<UnauthorizedHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
});
builder.Services.AddScoped<IAuthState, AuthState>();
builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<IMicrowaveService, MicrowaveApiService>();
builder.Services.AddScoped<IHeatingProgramService, HeatingProgramApiService>();

await builder.Build().RunAsync();
