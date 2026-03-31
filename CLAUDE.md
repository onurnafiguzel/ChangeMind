# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ChangeMind is an ASP.NET Core 10.0 API project following **Clean Architecture** principles with four main projects:

- **ChangeMind.Api**: ASP.NET Core entry point with controllers and HTTP middleware
- **ChangeMind.Application**: Use cases and business logic orchestration
- **ChangeMind.Domain**: Core entities and domain logic
- **ChangeMind.Infrastructure**: External service integrations (database, APIs, etc.)

## Development Commands

### Build & Run

```bash
# Build the entire solution
dotnet build

# Run the API in development mode
dotnet run --project src/ChangeMind.Api

# Run with specific launch profile (http or https)
dotnet run --project src/ChangeMind.Api --launch-profile http
```

The API starts on:
- **HTTP**: http://localhost:5123
- **HTTPS**: https://localhost:7184

### Project-Specific Commands

```bash
# Build a specific project
dotnet build src/ChangeMind.Api
dotnet build src/ChangeMind.Application

# List all projects in solution
dotnet sln ChangeMind.slnx list
```

## Architecture

### Project Dependencies

```
ChangeMind.Api
    └── references: Application
    └── entry point: Program.cs

ChangeMind.Application
    └── references: Domain
    └── contains: use cases, services, orchestration

ChangeMind.Domain
    └── references: none (core layer)
    └── contains: entities, interfaces, business rules

ChangeMind.Infrastructure
    └── references: Domain
    └── contains: implementations (DB, external APIs, logging, etc.)
```

### Key Configuration

- **Target Framework**: .NET 10.0
- **Language Features**:
  - Implicit usings enabled (`global using` auto-imports common namespaces)
  - Nullable reference types enabled (strict null checking)
  - Latest C# language version
- **OpenAPI/Swagger**: Enabled in development (accessible via `/openapi`)

## Configuration & Environment

### App Settings

- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development overrides (loaded when `ASPNETCORE_ENVIRONMENT=Development`)

### Launch Settings

Located in `src/ChangeMind.Api/Properties/launchSettings.json`. Both http and https profiles have:
- `dotnetRunMessages: true` (shows startup information)
- `launchBrowser: false` (doesn't auto-open browser)

## Important Notes

- **Solution File**: Uses `.slnx` format (Visual Studio solution format, compatible with VS Code)
- **Central Package Management**: `Directory.Packages.props` manages package versions centrally
- **Implicit Usings**: Don't explicitly `using` common namespaces like `System`, `System.Linq`, etc. — they're auto-imported
- **Nullable Context**: All code must respect nullable reference types; use `?` for nullable types
