FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MicrowaveApp.sln ./
COPY src/MicrowaveApp.Blazor/MicrowaveApp.Blazor.csproj src/MicrowaveApp.Blazor/
COPY src/MicrowaveApp.Application/MicrowaveApp.Application.csproj src/MicrowaveApp.Application/
COPY src/MicrowaveApp.Domain/MicrowaveApp.Domain.csproj src/MicrowaveApp.Domain/
COPY src/MicrowaveApp.Shared/MicrowaveApp.Shared.csproj src/MicrowaveApp.Shared/

RUN dotnet restore src/MicrowaveApp.Blazor/MicrowaveApp.Blazor.csproj

COPY src/ src/
RUN dotnet publish src/MicrowaveApp.Blazor/MicrowaveApp.Blazor.csproj -c Release -o /app/publish --no-restore

FROM nginx:1.27-alpine AS final
COPY src/MicrowaveApp.Blazor/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

EXPOSE 80
