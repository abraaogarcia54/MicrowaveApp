using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Blazor;
using MicrowaveApp.Blazor.Services;
using MicrowaveApp.Domain.Entities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5167";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<Microwave>();
builder.Services.AddScoped<IMicrowaveService, MicrowaveService>();
builder.Services.AddScoped<IHeatingProgramService, HeatingProgramApiService>();
builder.Services.AddScoped<IHeatingProgramRepository, HeatingProgramApiRepository>();

await builder.Build().RunAsync();
