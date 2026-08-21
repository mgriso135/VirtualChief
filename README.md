# VirtualChief - .NET 10 Migration

This project has been migrated from .NET Framework 4.8 (IIS-only, Windows-only) to .NET 10 (Kestrel, Linux, cross-platform).

## Project Structure

```
VirtualChief/
├── VirtualChief/              # Main .NET 10 Web App
│   ├── Program.cs            # Entry point
│   ├── appsettings.json      # Configuration
│   ├── KisApp.App_Code/      # Class library: UserAccount, UserList
│   ├── KisApp.App_Sources/   # Class library: Analysis, clienti, produzione
│   ├── Pages/                # Razor Pages (.cshtml + .cshtml.cs)
│   │   ├── Analysis/
│   │   │   ├── analysis.cshtml
│   │   │   └── analysis.cshtml.cs
│   │   ├── Clienti/
│   │   │   ├── Clienti.cshtml
│   │   │   └── Clienti.cshtml.cs
│   │   └── Commesse/
│   │       ├── commesse.cshtml
│   │       └── CommesseModel.cshtml.cs
│   └── VirtualChief.csproj   # Project file (.NET 10)
├── KisApp.App_Code/          # Class library project
│   ├── UserAccount.cs
│   ├── UserList.cs
│   └── KisApp.App_Code.csproj
├── KisApp.App_Sources/       # Class library project
│   ├── Analysis.cs
│   ├── clienti.cs
│   ├── produzione.cs
│   └── KisApp.App_Sources.csproj
├── Archive/                  # Archived legacy files
│   ├── About.aspx, About.aspx.cs
│   ├── Admin/                # .aspx and .ascx files
│   ├── Analysis/             # .aspx and .ascx files
│   ├── Commesse/             # .aspx and .ascx files
│   └── ... (other subdirectories)
├── KisWebApp/                # Original .NET Framework 4.8 project (legacy)
│   ├── About.aspx, About.aspx.cs
   └── ... (all .aspx and .ascx files archived)
└── packages.config           # NuGet packages (Framework 4.8)
```

## How to Run the Application Manually

### Option 1: Using dotnet run

```bash
# Navigate to the project directory
cd /home/matteo/Documenti/projects/VirtualChief/VirtualChief/VirtualChief

# Run the application
dotnet run
```

The application will start and listen on:
- **HTTP**: http://localhost:5030
- **HTTPS**: https://localhost:7039 (if configured)

### Option 2: Using dotnet build + dotnet exec

```bash
# Build the project
cd /home/matteo/Documenti/projects/VirtualChief/VirtualChief/VirtualChief
dotnet build

# Run the compiled DLL
dotnet bin/Debug/net10.0/VirtualChief.dll
```

### Option 3: Using the launched settings profile

The `Properties/launchSettings.json` configures:
- **HTTP**: http://localhost:5030
- **HTTPS**: https://localhost:7039

To run with these settings:
```bash
dotnet run --launch-profile http
# or
dotnet run --launch-profile https
```

### Option 4: Set environment variables manually

```bash
cd /home/matteo/Documenti/projects/VirtualChief/VirtualChief/VirtualChief
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

## Configuration

### appsettings.json
The application uses standard ASP.NET Core configuration:
- `Logging`: Log levels for Default and Microsoft.AspNetCore
- `AllowedHosts`: "*" (allows all hosts)

No additional appsettings are currently required. The `KisApp.App_Sources` libraries use a hardcoded connection string `"Server=localhost;Database=virtualchief;User=root;"` for MySQL connectivity.

## Project Overview

### Migration Details

- **From**: .NET Framework 4.8 (IIS, Windows, MySql.Data)
- **To**: .NET 10 (Kestrel, Linux, MySqlConnector)
- **Legacy files archived**: 56 .aspx + 168 .ascx files moved to Archive/
- **New Razor Pages**: Created using `KisApp.App_Code` and `KisApp.App_Sources` namespaces

### Libraries

- **KisApp.App_Code**: Contains `UserAccount` (permission validation) and `UserList` (user management)
- **KisApp.App_Sources**: Contains `Analysis` (production analysis), `clienti` (client management), `produzione` (production management) with MySqlConnector

### Pages

- **Analysis**: `analysis.cshtml + .cshtml.cs` - Production analysis dashboard
- **Clienti**: `Clienti.cshtml + .cshtml.cs` - Client management list
- **Commesse**: `commesse.cshtml + .cshtml.cs` - Commessa management list

## Development

### Prerequisites

- .NET 10 SDK installed (`dotnet --version` should show 10.0.x)
- MySQL server running (for KisApp.App_Sources data access)

### Building

```bash
dotnet build
```

### Running Tests (if any)

Check for test projects in the solution.

### Adding New Pages

1. Create `.cshtml` Razor Page in `Pages/` directory
2. Create corresponding `.cshtml.cs` PageModel
3. Add `using KisApp.App_Code;` and `using KisApp.App_Sources;` directives
4. Build and run to verify