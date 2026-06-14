# Banco de Dados

## Tecnologia

- SQL Server
- Entity Framework Core
- Migrations no projeto `MicrowaveApp.Infrastructure`

## Connection string de desenvolvimento

```text
Server=localhost,1433;Database=MicrowaveApp;User Id=sa;Password=Your_password123;TrustServerCertificate=True
```

## Subir aplicação com Docker

```bash
docker compose up -d
```

Esse comando sobe:

- SQL Server em `localhost:1433`
- API em `http://localhost:5167`
- Blazor em `http://localhost:5174`

Ao iniciar, a API aplica migrations pendentes e cadastra os 5 programas pré-definidos caso ainda não existam.

## Subir somente o SQL Server

```bash
docker compose up -d sqlserver
```

## Criar migration

```bash
dotnet ef migrations add InitialCreate \
  --project src/MicrowaveApp.Infrastructure/MicrowaveApp.Infrastructure.csproj \
  --startup-project src/MicrowaveApp.Infrastructure/MicrowaveApp.Infrastructure.csproj
```

## Aplicar banco

```bash
dotnet ef database update \
  --project src/MicrowaveApp.Infrastructure/MicrowaveApp.Infrastructure.csproj \
  --startup-project src/MicrowaveApp.Infrastructure/MicrowaveApp.Infrastructure.csproj
```

## Seed

Ao iniciar a API, o banco aplica migrations pendentes e cadastra os 5 programas pré-definidos caso ainda não existam.
