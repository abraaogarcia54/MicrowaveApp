FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MicrowaveApp.sln ./
COPY src/MicrowaveApp.Api/MicrowaveApp.Api.csproj src/MicrowaveApp.Api/
COPY src/MicrowaveApp.Application/MicrowaveApp.Application.csproj src/MicrowaveApp.Application/
COPY src/MicrowaveApp.Domain/MicrowaveApp.Domain.csproj src/MicrowaveApp.Domain/
COPY src/MicrowaveApp.Infrastructure/MicrowaveApp.Infrastructure.csproj src/MicrowaveApp.Infrastructure/
COPY src/MicrowaveApp.Shared/MicrowaveApp.Shared.csproj src/MicrowaveApp.Shared/

RUN dotnet restore src/MicrowaveApp.Api/MicrowaveApp.Api.csproj

COPY src/ src/
RUN dotnet publish src/MicrowaveApp.Api/MicrowaveApp.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MicrowaveApp.Api.dll"]
