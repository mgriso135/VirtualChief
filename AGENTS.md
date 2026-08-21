# Migration Context & Guidelines

## Tech Stack Target
- **Legacy Framework:** WebForms + MVC5 + Web API2, .NET Framework 4.0–4.8, ASMX web services, `packages.config` NuGet, IIS-only, Windows-only, mysql
- **Target Framework:** .NET 8, postgreSQL, Linux
- Detailed description and roadmap is in migration.md

## Which project to run
- **`VirtualChief/` is THE runnable app** (new ASP.NET Core UI, Auth0 login, group-based menu): `cd VirtualChief && dotnet run` → http://localhost:5030
- **`KisWebApp/` is legacy-only** — mid-migration shell that compiles on net10.0 with a compile allowlist (`Virtual Chief.csproj`); it has no auth/menu and must not be used as the daily app.
- Business layer: `KisApp.App_Sources/` links `KisWebApp/App_Sources/*.cs`; code-behind compile checks live in `tests/VirtualChief.CodeBehindCompileCheck`.

## Rules for Agents
1. **No Breaking Modifications Without Tests:** Always ensure a test exists before updating core functions.
2. **Backward Compatibility:** Keep public interface signatures intact where possible during early migration phases.
3. **Dependency Strategy:** Upgrade dependencies incrementally rather than modifying `package.json`/`requirements.txt` all at once.
4. **Verification Step:** Always run unit tests after applying a code block.