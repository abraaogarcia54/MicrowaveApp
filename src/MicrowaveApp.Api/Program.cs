using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MicrowaveApp.Api.Middleware;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Infrastructure.Data;
using MicrowaveApp.Infrastructure.Logging;
using MicrowaveApp.Infrastructure.Repositories;
using MicrowaveApp.Infrastructure.Security;
using MicrowaveApp.Infrastructure.Seed;
using MicrowaveApp.Infrastructure.State;

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
var connectionStringProtector = new ConnectionStringProtector();
var connectionString = ResolveConnectionString(builder.Configuration, connectionStringProtector);

builder.Services.AddDbContext<MicrowaveDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];

        var signingKey = JwtSecurityKeyFactory.Create(jwtKey!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MicrowaveApp",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "MicrowaveApp.Client",
            IssuerSigningKey = signingKey,
            IssuerSigningKeyResolver = (_, _, _, _) => [signingKey],
            TryAllIssuerSigningKeys = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<Microwave>();
builder.Services.AddScoped<IMicrowaveService, MicrowaveService>();
builder.Services.AddScoped<IMicrowaveServiceFactory, MicrowaveServiceFactory>();
builder.Services.AddScoped<IHeatingProgramService, HeatingProgramService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHeatingProgramRepository, HeatingProgramRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<IMicrowaveSessionStore, InMemoryMicrowaveSessionStore>();
builder.Services.AddSingleton<IExceptionLogger, FileExceptionLogger>();
builder.Services.AddSingleton(connectionStringProtector);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
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

static string ResolveConnectionString(IConfiguration configuration, ConnectionStringProtector protector)
{
    var protectedConnectionString = configuration.GetConnectionString("DefaultConnectionEncrypted");

    if (!string.IsNullOrWhiteSpace(protectedConnectionString))
    {
        var key = configuration["Security:ConnectionStringKey"]
            ?? throw new InvalidOperationException("A chave da connection string não foi configurada.");

        return protector.Unprotect(protectedConnectionString, key);
    }

    return configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string não configurada.");
}
