using Microsoft.EntityFrameworkCore;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Infrastructure.Data;
using MicrowaveApp.Infrastructure.Repositories;
using MicrowaveApp.Infrastructure.Security;
using MicrowaveApp.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5035",
                "https://localhost:7169",
                "http://localhost:5174",
                "https://localhost:7174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<MicrowaveDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<Microwave>();
builder.Services.AddScoped<IMicrowaveService, MicrowaveService>();
builder.Services.AddScoped<IHeatingProgramService, HeatingProgramService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHeatingProgramRepository, HeatingProgramRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.UseAuthorization();

app.MapControllers();

await SeedDatabaseAsync(app);

app.Run();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MicrowaveDbContext>();

    await dbContext.Database.MigrateAsync();
    await HeatingProgramSeed.ApplyAsync(dbContext);
}
