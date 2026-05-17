# syntax=docker/dockerfile:1.7
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY Directory.Build.props* nuget.config* ./

COPY src/ChangeMind.Domain/ChangeMind.Domain.csproj                 src/ChangeMind.Domain/
COPY src/ChangeMind.Application/ChangeMind.Application.csproj       src/ChangeMind.Application/
COPY src/ChangeMind.Infrastructure/ChangeMind.Infrastructure.csproj src/ChangeMind.Infrastructure/
COPY src/ChangeMind.Api/ChangeMind.Api.csproj                       src/ChangeMind.Api/

RUN dotnet restore src/ChangeMind.Api/ChangeMind.Api.csproj

COPY src/ src/
RUN dotnet publish src/ChangeMind.Api/ChangeMind.Api.csproj \
    -c Release -o /app/publish \
    --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
LABEL org.opencontainers.image.source="https://github.com/onurnafiguzel/ChangeMind"
LABEL org.opencontainers.image.description="ChangeMind API"
LABEL org.opencontainers.image.licenses="MIT"
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "ChangeMind.Api.dll"]
