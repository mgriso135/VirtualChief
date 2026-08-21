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



From the repo root (/home/matteo/Documenti/projects/VirtualChief/VirtualChief):

## Standard startup (local real database)

```bash
VirtualChief/run-dev.sh
```

That single script is the standard way to start the app. It:
1. exports the connection seam against the local MariaDB/MySQL on **127.0.0.1:3306**,
   merged database **`virtualchief`** (user `matteo`), re-applying the env vars from
   scratch on every start so stale shell exports can never shadow them;
2. runs `dotnet run` → **http://localhost:5030** (Auth0 login).

Equivalent manual steps:

```bash
export VC_VCMAIN_CONN='Server=127.0.0.1;Port=3306;Uid=matteo;Pwd=hellas;Database=virtualchief'
export VC_MASTERDB_CONN='Server=127.0.0.1;Port=3306;Uid=matteo;Pwd=hellas;database='
cd VirtualChief && dotnet run
```

Tenant databases are created manually on the same server; their names must match
`virtualchief.workspaces.name` exactly (`kaizenkey`, `matteo`, `Testws`) because
tenant connections resolve as `database=<workspace name>`. Grant them to
`'matteo'@'localhost'`.

Pages:
| URL | |
|---|---|
| / | home + dynamic menu per user groups |
| /Clienti/Clienti | |
| /Commesse/commesse | |
| /Produzione/produzione | |
| /Reparti/listReparti | |
| /Users/listUsers | |
| /Analysis/analysis | |

Notes:
- The DB must be up before startup; domain classes open connections in their constructors/loaders.
- Without the two env vars, connections fall back to `App.config` (points at the seeded dev DB on port 3307).
- Self-contained test seed (no real data): `bash tests/provision-db.sh` starts a private MariaDB on 127.0.0.1:3307 — then point the two `VC_*CONN` vars at `Port=3307`.
- Stop with Ctrl+C; re-running provision-db.sh is safe anytime.