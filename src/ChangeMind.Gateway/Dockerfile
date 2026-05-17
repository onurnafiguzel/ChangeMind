# syntax=docker/dockerfile:1.7
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY Directory.Build.props* nuget.config* ./

COPY src/ChangeMind.Domain/ChangeMind.Domain.csproj           src/ChangeMind.Domain/
COPY src/ChangeMind.Application/ChangeMind.Application.csproj src/ChangeMind.Application/
COPY src/ChangeMind.Gateway/ChangeMind.Gateway.csproj         src/ChangeMind.Gateway/

RUN dotnet restore src/ChangeMind.Gateway/ChangeMind.Gateway.csproj

COPY src/ src/
RUN dotnet publish src/ChangeMind.Gateway/ChangeMind.Gateway.csproj \
    -c Release -o /app/publish \
    --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "ChangeMind.Gateway.dll"]
