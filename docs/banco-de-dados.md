# Banco de Dados

## Tecnologia

- SQL Server
- Entity Framework Core
- Migrations no projeto `MicrowaveApp.Infrastructure`

## Connection string descriptografada de desenvolvimento

```text
Server=localhost,1433;Database=MicrowaveApp;User Id=sa;Password=Your_password123;TrustServerCertificate=True
```

## Connection string descriptografada no Docker

```text
Server=sqlserver,1433;Database=MicrowaveApp;User Id=sa;Password=Your_password123;TrustServerCertificate=True
```

## Connection string criptografada

A API lê preferencialmente `ConnectionStrings:DefaultConnectionEncrypted` e descriptografa o valor com `Security:ConnectionStringKey` usando AES.

Valores usados no ambiente de desenvolvimento:

- Chave: `MicrowaveApp-Level4-Development-Key-2026`
- Local: `ytpaS8G2TdXRGNvBVlF7nWeLufuce+nwBNcoSF5sfJ4yKBCgA+fBxNh3a+dhkf3O/FDtCL1m4VNhZZ7baWP1MxWcrGlJGcsnziFB2h2Dj0oZl6prycHQ9+djCc+fjctiwjkqrX1gE2eh5x2VfXWeDe8pUtA/iMVUoj6idCE3a3U=`
- Docker: `pnqyUCjlQnPTX3AgsPFHqF+YB+Rqxej/b8SP0TBrz4Oo9M2/aJ5WVzaCVkaoIacI/iMITchB7yo6vk9Cdcs90uVQZ8Ee2QMdyUnIaKOvZb9wQH3+vxop9R0hTw3Jaj9Qq+9Hh5PDb2lQ15qMQSAXP2wTe40HiAOYrCBFDo0LZmE=`

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
