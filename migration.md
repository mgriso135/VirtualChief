# VirtualChief — Functional Report & Modernization Plan

> Repository: `Virtual Chief.sln` (Visual Studio 2019, .NET Framework 4.0–4.8)
> Product: **VirtualChief** (formerly "Kaizen Indicator System") — a multi-tenant
> MES / Andon / Kaizen production-monitoring platform for manufacturing plants.
>
> This document contains: (1) a comprehensive report of what the software does,
> (2) a technological review of the current state, and (3) a phased roadmap to
> modernize the platform **without changing the UI or any business function**.

---

## 1. Software Overview

VirtualChief is a web-based shop-floor / production-monitoring suite. One ASP.NET
web application (`KisWebApp`) provides the product UI and business logic; ~11
run-once console applications act as integration agents and scheduler triggers
(polling kanban systems, ERPs, exporting production data). MySQL 8 is the
database. The system is multi-tenant: every tenant (customer plant) gets its own
MySQL database, while a central `vcmain` database holds accounts, workspaces,
roles and the menu tree.

External integrations:
- **KanbanBox** — cloud kanban system (REST API, card polling, auto production launch)
- **SIAV CPM** — production performance software (client-cert + bearer token, `.dot` graph download, events export into a dedicated MySQL DB)
- **Finestra3000 / Omnia3000** — ERP (XML order import from a drop folder)
- **Auth0** — identity provider (OpenID Connect login)
- **Google SMTP** — transactional e-mail (delays, warnings, license, quality, integration errors)

Deployment model: IIS-hosted ASP.NET app + Windows Task Scheduler invoking the
console agents.

---

## 2. Functional Report

### 2.1 Web application (`KisWebApp`)

Hybrid **WebForms + ASP.NET MVC 5 + Web API 2**, target framework **.NET Framework 4.8**.
Scale: ~1,481 C# files, 114 `.aspx`, 172 `.ascx`, 154 `.cshtml`, 7 `.asmx`,
48 controllers. Main libraries: EF 6.4.4 + MySql.Data 8.0.24 (raw ADO.NET over
MySQL is the primary data path), OWIN + Auth0 OpenID Connect, jQuery 3.6,
Bootstrap 4.6, jQuery UI, Google Charts, D3/graphviz, iTextSharp (PDF), GenCode128
(barcodes), AjaxControlToolkit (legacy).

#### 2.1.1 Authentication, tenancy & licensing
- **Auth0 login** (OWIN `Startup.cs`): OpenID Connect, cookie auth. On first login the
  user is auto-provisioned (`useraccounts` in `vcmain`), a default workspace is
  assigned, and account activation / invites / email-verification flows are handled
  by the `AccountsMgm` area. Workspace switching supported.
- **Legacy forms login** (`Login/login.aspx`) still present for the old user/password model.
- **Licensing**: per-tenant `ExpiryDate` in the `configurazione` table. `Site.Master`
  shows a warning banner ≤30 days before expiry and redirects to `LicenseExpired.aspx`
  once expired. `Eventi/Licensing.asmx` emails the Admin group about renewal.
- **Tenancy**: connection string per workspace built at runtime by replacing the
  `database=` part of a base connection string (`Dati.GetConnectionString` in
  `App_Sources/data.cs`); global `vcmain` DB via `Dati.VCMainConn()`.
- **i18n**: per-user language (en / it / es / es-AR) via `CurrentUICulture` in
  `Global.asax.cs`, resx resource files, localized "whatsnew" tips.

#### 2.1.2 Functional modules (WebForms pages + MVC areas)
- **Customers / CRM** (`Clienti/`, `Customers` area) — customer master data,
  contacts, emails, phones, customer portfolio.
- **Sales orders / Commesse** (`Commesse/`, `SalesOrders` area) — order creation
  wizards, PERT scheduling, workload checks, delivery dates, article-linked alarms,
  barcode label printing, manual task rescheduling (PDF via iTextSharp).
- **Processes & products** (`Processi/`, `Products`, `Parts` areas) — processes,
  variants, cycle/unload times (`tempiciclo`), PERT precedence editing, microsteps,
  product and task parameters.
- **Production** (`Produzione/`) — production planning board, launch-in-production
  with simulation (`SimulaIntroduzioneInProduzione`, `LanciaInProduzione`), workload
  simulation across departments, task status tracking, production history and
  "exhume" of finished products, production details per product.
- **Shop floor & organization** (`Reparti/`, `Postazioni/`, `Operatori/`) —
  departments, workstations, shift calendars / holidays / overtime, operator↔station
  registration, cycle-time activities.
- **Operator UI** — legacy barcode check-in/out pages (`AzioniBarcode*`,
  `checkInPostazione_OLD`, `doTasksPostazione_OLD`) and the modern **WebGemba**
  `Workplace` area: task lists (available / in execution / assigned), station
  check-in, per-workstation KPIs, operator notes. Work instructions with file uploads.
- **Andon boards** (`Andon/`, `Andon` area) — per-department live boards: current
  shift, active operators, production indicator; configurable scroll type, username
  format, show-active-users flag.
- **Analytics & KPIs** (`Analysis/`, `Analysis` area) — global KPIs (delays, lead
  time, quantities, warnings/NCs/improvement actions), operator productivity,
  product analysis, production history, Pareto / customer portfolios, cost per
  commessa, **Process Mining** view (D3 + graphviz).
- **Quality** (`Quality` area) — non-conformities board/CRUD, NC analysis charts
  (per-period, Pareto, causes), non-conformity types/causes, corrective actions,
  improvement actions with file attachments.
- **Free time measurement** (`FreeTimeMeasurement` area) — stopwatch-style task time
  measurements, multi-file upload, SPC-style analysis views.
- **Events / alarms** (`Eventi/`) — warning & delay event configuration scoped per
  department / order / article, with per-group and per-user e-mail subscriptions.
- **Admin & config** (`Admin/`, `Config` area) — menu tree management, KPI admin,
  report configuration, order-status mapping, logo / timezone config, first-run
  configuration wizard (`Configuration/`), measurement units, no-productive tasks,
  departments' auto-pause settings.
- **Personal area** (`Personal/`) — password, contacts, language, alerts, post-login
  destination URL.
- **Users & permissions** (`Users/`, `Users` area) — users, groups, permissions,
  work-hours registration, disabled users, checksum-based activation.

#### 2.1.3 Web services exposed by the app (7 ASMX)
| Service | Purpose |
|---|---|
| `Eventi/Licensing.asmx` | License-expiry e-mail alerts to Admin group |
| `Eventi/Warning.asmx` | Finds open production warnings, emails configured recipients, marks as `segnalato` |
| `Eventi/Ritardi.asmx` | Finds late tasks, emails delay notifications, marks as `segnalato` |
| `Eventi/QualityModuleEvents.asmx` | Late improvement / corrective action reminder e-mails |
| `KanbanBox/KanbanBoxReader.asmx` | Pulls released kanban cards from KanbanBox, creates orders, launches production, updates card status |
| `KanbanBox/KanbanBoxCheckHealth.asmx` | KIS↔KanbanBox consistency checks + e-mails |
| `Processi/getProcessData.asmx` | AJAX endpoint for the PERT editor (cycle times, precedences) |

#### 2.1.4 Scheduled HTTP endpoints (called by console agents, `X-API-KEY` guarded)
- `DelaysAlarmController` — late-task detection + delay e-mails (duplicate of `Ritardi.asmx`).
- `AutoPauseTasksController` — auto-pauses running tasks outside department work shifts.
- `EventsExportController` — exports finished-task events / converts events to
  timespans for SIAV.
- `RemoteSalesOrderController` / `ThirdPartySalesOrdersController` — third-party
  sales-order import (auto-create customer, plan and launch production).
- `ConfigController` — API-key guarded get/set of license expiry date and module summaries.

### 2.2 Console scheduler / integration apps

All are **run-once console executables** intended for Windows Task Scheduler
(no Windows Services, no timers/loops). Most real logic lives server-side in
`KisWebApp`; the agents are thin triggers.

| Project | Framework | Function |
|---|---|---|
| `KISScheduler` | .NET 4.0 | Triggers `Ritardi.asmx` + `Warning.asmx` (delay/warning e-mails) |
| `KISLicenseCheck` | .NET 4.5.2 | Triggers `Licensing.asmx` (license renewal e-mails) |
| `KISQualityEventsCheck` | .NET 4.5.2 | Triggers `QualityModuleEvents.asmx` (quality reminder e-mails) |
| `KanbanBoxReaderScheduler` | .NET 4.0 | Polls KanbanBox REST for released cards → creates commesse → launches production → marks cards in-process; rollback on failure |
| `KanbanBoxCheckHealthScheduler` | .NET 4.0 | KIS↔KanbanBox consistency checks (non-existent part numbers, ghost products, non-updated cards, non-existent customers) → e-mails managers |
| `VCAutoPauseTasks` | .NET 4.6.1 | Calls `AutoPauseTasks` API; logs to `c:\temp\AutoPauseLog.txt` |
| `VCAlarmsEvents` | .NET 4.6.1 | HTTP replacement for the delays alarm (duplicate; currently broken entry point) |
| `ThirdPartySalesOrders_Finestra3000` | .NET 4.6.1 | Reads Finestra3000/Omnia3000 XML orders from a drop folder, imports orders, auto-creates customers, plans & launches production; archives XML on success; error e-mails |
| `VCProductionEventsExport-SIAV` | .NET 4.6.1 | Exports finished-task events, derives start/end timespans, writes ~80 columns into `siav_exportevents.longatoexport` MySQL DB for SIAV |
| `SIAV-GetDotFile` | .NET 4.6.1 | Authenticates to SIAV CPM (client cert + bearer token), downloads production-performance `.dot` graphs |
| `ConsoleApp1`, `VCAlarmEvents` (netcoreapp2.0) | — | **Broken/abandoned scaffolding**, not in the solution |

### 2.3 Database

MySQL 8.0.11 (`mysqldump` exports):
- **`kaizenkey`** — production tenant schema: **90 tables, 10 views**.
- **`vcmain`** — central multi-tenant DB: **13 tables** (`workspaces`,
  `useraccounts`, `useraccountworkspaces`, `groupss`, `permissions`, `menualbero`, ...).
- **`vc_dev`** — development tenant snapshot: **84 tables, 1 view** (older/different
  evolution; contains `accounts`, `taskspostazioni_old`; lacks `freemeasurements*`,
  `microsteps`, `taskstimespans`, `noproductivetasks`).

**No stored procedures, functions or triggers** exist in any dump — all business
logic lives in the C# application layer.

Main domain areas (from `kaizenkey`):
- CRM: `anagraficaclienti`, `contatticlienti`, `contatticlienti_email`, `contatticlienti_phone`
- Orders: `commesse`, `productionplan`, `varianti`, `variantiprocessi`
- Tasks/time: `tasksproduzione`, `taskstimespans`, `taskuser`, `taskparameters`,
  `tempiciclo`, `microsteps`, `task_microsteps`, `tasksmanuals`, `noproductivetasks`, `taskreschedulelog`
- Organization: `reparti`, `postazioni`, `turniproduzione`, `orarilavoroturni`,
  `straordinarifestivita`, `registrooperatoripostazioni`
- Processes: `processo`, `processipadrifigli`, `precedenzeprocessi`, `relazioniprocessi`
- Event log/alarms: `registroeventiproduzione`, `registroeventitaskproduzione`, `warningproduzione`
- Quality/Kaizen: `noncompliances`, `noncompliancestypes`, `noncompliancescause`,
  `correctiveactions`, `improvementactions`, `kpi_description`, `kpi_record`
- Free measurements: `freemeasurements`, `freemeasurements_tasks*`
- Notification subscriptions: `eventoarticolo*`, `eventocommessa*`, `eventoreparto*`
- Work instructions: `manuals`, `workinstructionslabel`, `measurementunits`
- Users/security: `users`, `useremail`, `groupss`, `groupusers`, `userslog`, `permessi`, `menuvoci`, `menugruppi`, `menualbero`
- Config: `configurazione`, `homeboxesregistro`, `homeboxesuser`, `syslog`

### 2.4 Key source files
- Data-access hub: `KisWebApp/App_Sources/data.cs` (`Dati.Dati`, `Dati.Utilities`)
- EF6 DbContext + entities: `KisWebApp/App_DB/VCContext.cs` (+ ~84 POCOs)
- Business/domain layer (namespace `KIS.App_Code`): `App_Sources/{clienti,commesse,produzione,processi,reparti,postazioni,configurazione,users,Account,eventi,quality,Analysis,andon,FreeTimeMeasurement,inputpoints,KanbanBox,parts,kpi,WorkInstructions,NoProductiveTasks,menu,permessi,relazioni}.cs`
- Auth/OWIN: `KisWebApp/Startup.cs`, `Areas/AccountsMgm/Controllers/AccountController.cs`
- Licensing: `App_Sources/configurazione.cs` (`KISConfig.ExpiryDate`), `Site.Master.cs`, `Eventi/Licensing.asmx.cs`
- Scheduled endpoints: `Controllers/{DelaysAlarmController,AutoPauseTasksController,EventsExportController,RemoteSalesOrderController,ThirdPartySalesOrdersController}.cs`
- Config: `Web.config`, `packages.config`, `App_Start/{WebApiConfig,Routeconfig}.cs`, `Global.asax.cs`

---

## 3. Technological Review

### 3.1 Strengths
- Mature, working production system with a rich, well-modeled domain (~90 tables).
- Three real external integrations (KanbanBox, SIAV, Finestra3000).
- Already part-way through a WebForms → MVC5 + Auth0 migration.

### 3.2 Weaknesses / risks
| Category | Findings |
|---|---|
| **Stack** | WebForms + MVC5 + Web API2, .NET Framework 4.0–4.8, ASMX web services, `packages.config` NuGet, IIS-only, Windows-only. CI (`dotnet.yml`, .NET 5, Ubuntu) cannot build these projects. |
| **Security** | API keys, DB and SMTP passwords hardcoded in source/configs; SQL built by string concatenation (injection surface); Web API JSON `TypeNameHandling.All` (deserialization risk); SIAV tool disables SSL validation; Auth0 redirect points to localhost. |
| **Duplication** | Delays logic duplicated (ASMX + Web API); two parallel "AlarmEvents" projects (both broken); ~80-field DTOs copied; orphan projects `ConsoleApp1` and `VCAlarmEvents` (netcoreapp2.0). |
| **Dead code** | `.suo` (VS2012), `UpgradeLog*` + `_UpgradeReport_Files` (VS2010 leftovers), `WebApplication1` references, large commented-out blocks, `Packages.dgml` empty stub. |
| **Operations** | Scheduling via Windows Task Scheduler + hardcoded localhost endpoints; fixed `C:\temp` log paths; Gmail SMTP with plaintext credentials. |
| **Engineering** | No tests; no DI container; no logging framework; EF6 present but barely used (raw ADO.NET everywhere). |

---

## 4. Modernization Roadmap

> **Guiding principle:** modernize the platform *underneath* the existing UI and
> business logic. Screens, workflows, URLs, emails and behavior stay identical —
> only the plumbing changes. Each phase is verifiable independently.

### Phase 0 — Stabilize (no architectural change, do first)
1. Rotate all hardcoded secrets (API keys, SMTP passwords, SIAV credentials, DB
   passwords) out of source into configuration / a secret store.
2. Parameterize all SQL built by concatenation; remove injection surface.
3. Harden the API: remove `TypeNameHandling.All`, restore SSL validation in the SIAV tool.
4. Delete orphan projects (`ConsoleApp1`, `VCAlarmEvents` netcoreapp2.0) and legacy
   upgrade artifacts (`UpgradeLog*`, `_UpgradeReport_Files`, `.suo`).
5. Fix the two broken scheduler entry points (`VCAlarmsEvents` `Main`/`MainAsync`).
6. Make CI real: build on Windows runners (or defer until Phase 3).

### Phase 1 — Same-stack modernization (biggest value, low risk)
1. Migrate NuGet from `packages.config` to **PackageReference**; pin latest versions
   compatible with .NET Framework 4.8.
2. Introduce **Dapper** over the existing SQL in `App_Sources` (keep the queries;
   eliminate ADO.NET boilerplate). Optionally add **EF Core existing-database**
   later. Semantics of every query unchanged.
3. Extract duplicated logic (delays/warnings, SIAV export DTOs) into a **shared
   class library** used by both the ASMX/WCF and Web API paths; keep endpoints.
4. Centralize configuration with a typed config reader over `Web.config` (same values).
5. Add **structured logging (Serilog)** and error capture across `data.cs`,
   controllers and console agents — no UI impact.

### Phase 2 — Move off WebForms, keep the same screens
1. WebForms pages are thin shells over `.ascx` controls calling `App_Sources`.
   Convert page-by-page to **Razor pages / MVC views** rendering identical markup.
2. Use a screenshot/visual-regression harness to guarantee pixel-level parity — the
   UI is unchanged for users.
3. Convert the 7 `.asmx` services to **Web API controllers**, keeping the same
   routes/contracts via compatibility endpoints so the console agents keep working.

### Phase 3 — Modern runtime, same behavior
1. Re-target the web app to **.NET 8 / ASP.NET Core** using official MVC5→Core
   migration guidance (WebForms pages → Razor Pages). Keep the same HTML/CSS/JS
   assets (Bootstrap, jQuery, Google Charts, D3 — all cross-platform).
2. MySQL via **Pomelo EF Core** provider or Dapper — no query rewrite required.
3. Replace the 11 console schedulers with a single **.NET 8 worker service**
   hosting the same jobs as configurable cron/timer jobs (same logic, same calls,
   same emails). Removes Windows Task Scheduler + localhost coupling.
4. Keep Auth0; swap OWIN for ASP.NET Core OpenID Connect middleware with the same
   Auth0 client config and claims — login UX unchanged.

### Phase 4 — Hardening & engineering practices (invisible to users)
1. Expand automated tests to full coverage per the Testing Strategy below.
2. Introduce DI (Microsoft.Extensions.DependencyInjection) in the web app; add
   health checks and centralized logging.
3. Extract the tenant-connection-swap logic into a small, tested database factory.
4. **Never rewrite domain logic** — this is a "lift & shift" onto modern frameworks;
   the ~90-table schema and all business rules remain as-is.

---

## 5. Testing Strategy (before / during / after migration)

The codebase has **no test coverage today** and **no documentation of business
rules**. Tests written now serve two purposes: they lock the behavior that carries
all the migration risk (domain logic + SQL that you keep), and they become an
executable spec of the undocumented rules. Priority is **by surviving behavior,
not by file** — do not gold-plate the old stack. Coverage is tiered per layer so
that every test either survives the migration or is written against the layer
that will be re-verified after it.

### 5.1 Timing principle

| When | What to write | Why |
|---|---|---|
| **Before** | DB characterization tests, domain-layer tests, endpoint contract tests, screenshot/visual-regression fixture baseline | Locks behavior that survives; becomes the migration safety net |
| **During** | Dialect-equivalence re-run of DB tests (same suite on MySQL then PostgreSQL), visual-regression diffs per converted page | Proves queries and pixels are unchanged |
| **After** | Razor pages, Kestrel hosting, worker-service jobs, Docker/nginx, Serilog/DI/health checks | Platform-specific code does not exist yet |

### 5.2 App_Sources — tiered class-level coverage (28 files, ~51,700 lines)

**Tier 1 — Domain logic: class-level tests BEFORE migration (high risk, survives migration, highest value)**
`produzione.cs`, `processi.cs`, `commesse.cs`, `quality.cs`, `clienti.cs`,
`reparti.cs`, `eventi.cs`, `postazioni.cs`, `inputpoints.cs`, `KanbanBox.cs`,
`Account.cs`, `Analysis.cs`, `FreeTimeMeasurement.cs`, `NoProductiveTasks.cs`,
`parts.cs`, `kpi.cs`, `WorkInstructions.cs`

> Deployed on Linux without Windows/IIS by compiling the unmodified App_Sources
> into a test assembly via shims — see §5.5 (`tests/VirtualChief.DomainTests/`).

**Tier 2 — Thin SQL/config wrappers: covered by the DB-level suite, NOT by class tests**
`configurazione.cs`, `menu.cs`, `permessi.cs`, `users.cs`, `relazioni.cs`,
`data.cs`
Their behavior is "run query X, map result." The DB characterization tests already
exercise the exact SQL; class tests would be duplicate effort.

**Tier 3 — UI/output glue replaced during migration: NO tests before**
`ImageHandler.cs` (HTTP handler), `iTextSharpEventHandler.cs` (PDF generation),
`FilesViewModel.cs` (DTO), `FilesHelper.cs` (file I/O)
Re-implemented during WebForms→Razor/Kestrel work; tests here would be sunk cost
and a failing liability while the code is replaced.

### 5.3 Controllers — tiered coverage (root `Controllers/` + `Areas/*/Controllers/`)

Controllers are the orchestration layer over the domain classes. Unlike the
domain layer, controllers **will be reworked** during the migration (routing,
model binding, DI, async signatures), so "test now, reuse after" is weaker here.
Invest in URL-level HTTP tests (they survive) and in the fat orchestrators only.

**Tier A — Fat orchestrating MVC controllers: real logic — extract to services and test the services**
`Quality/ImprovementActionsController.cs` (2,286), `Workplace/WebGembaController.cs`
(1,766), `SalesOrders/SalesOrderController.cs` (1,745),
`Quality/NonCompliancesAnalysisController.cs` (1,703),
`Quality/NonCompliancesController.cs` (1,192),
`FreeTimeMeasurement/FreeMeasurementController.cs` (1,169),
`Products/ProductsController.cs` (1,052), `Analysis/GlobalKPIsController.cs` (953),
`Analysis/ProductAnalysisController.cs` (698), `Analysis/ProductionWorkloadController.cs`
(605), `Analysis/TaskAnalysisController.cs` (564), `Users/UsersController.cs` (607)
Controller-level unit tests on MVC5 have low transfer value; prefer extracting the
logic into services (they already call `App_Sources`) and testing the services.

**Tier B — Web API controllers: HTTP/contract tests, NOT class tests**
Root `Controllers/`: `customers.cs`, `products.cs`, `RemoteSalesOrderController.cs`,
`ThirdPartySalesOrdersController.cs`, `DelaysAlarmController.cs`,
`AutoPauseTasksController.cs`, `EventsExportController.cs`
`X-API-KEY`-guarded endpoints the schedulers call. Write URL-level contract tests
(request → status code → JSON shape → DB effect); they survive the migration and
double as the scheduler→API contract.

**Tier C — Small CRUD/config MVC controllers: end-to-end HTTP flow tests, NOT per-file unit tests**
Most remaining Areas controllers (`Config/*`, `Andon/*`, `Products/ProductParameters*`,
`Quality/Home`, `Home/Default`, `Parts`, `Personal`, `Departments`, `Production/*`,
`Customers`, `AccountsMgm/Workspaces`, small `Analysis/*`). A few HTTP flow tests
per area (login → navigate → action persists → status code) beat dozens of brittle
unit tests and are exactly what is re-run after migration to catch route /
model-binding / auth regressions.

**Tier D — Skip: scaffolding / experiments / test pages**
`TestController.cs`, `TestClass.cs`, `CustomersTest.aspx(.cs)` (WebForms test page),
`Areas/Test/CustomerTestController.cs`, `Auth0Test/AccountController.cs`,
`UtentiController.cs`, `NonComplianceTypesController.cs` — dead/demo code; delete
or ignore, never test.

### 5.4 Coverage by migration risk (summary)

- **Highest ROI before migration:** DB characterization tests (the SQL dialect
  pass is the #1 risk) + Tier 1 domain-layer tests + endpoint contract tests +
  screenshot baseline.
- **Durable artifacts:** URL-level HTTP tests and DB query tests — both are
  re-run unchanged after the platform and database migrations.
- **Deliberately not tested before:** WebForms pages, Tier 3 glue, Tier D
  scaffolding, and most controller internals.

### 5.5 Deployed characterization suite (this repository)

The DB-layer and portability-audit portion of the strategy above has been built,
deployed and is **green** (22 static/DB tests). It runs on Linux against a private
MariaDB instance populated from the checked-in dumps — no Windows/IIS required.

On top of that, the **Tier 1 domain-layer tests** from §5.2 are also deployed: the
real (unmodified) `App_Sources` business classes are compiled directly into a test
assembly via Linux compile-shims for the few `.NET Framework System.Web` symbols
they touch, and run against the same private MariaDB — **8 business-flow tests green
without Windows/IIS** (the "no Windows/IIS at our disposal" scenario).

| Artifact | Location |
|---|---|
| xUnit test project (.NET 10 SDK, MySqlConnector; retargetable to net8.0) — static + DB characterization | `tests/VirtualChief.Tests/` |
| xUnit test project — real `KIS.App_Code`/`KIS.App_Sources` classes compiled in via shims, Tier-1 business flows | `tests/VirtualChief.DomainTests/` |
| DB provisioning script (private MariaDB on `127.0.0.1:3307`, loads the 3 dumps) | `tests/provision-db.sh` |
| Test config via env vars | `VC_DB_HOST`, `VC_DB_PORT`, `VC_DB_USER`, `VC_DB_PASS` (defaults match the script) |

Run with:
```
tests/provision-db.sh     # once, per machine
dotnet test tests/VirtualChief.Tests        # 22 characterization tests
dotnet test tests/VirtualChief.DomainTests  # 8 Tier-1 business-flow tests
```

The three characterization suites in `tests/VirtualChief.Tests/`:

- **`PortabilityAuditTests`** (static, no DB) — drift-detection baselines for the
  MySQL→PostgreSQL surface:
  - 1,614 SQL string literals extracted from 30 `.cs` files, 98 distinct table refs;
  - **76 bare `user` identifiers** (Postgres-reserved) across 10 files, incl.
    `Analysis.cs` `loadTaskEvents` (`registroeventitaskproduzione.user`);
  - MySQL-only constructs: **1 backtick** (`configurazione.cs:420`),
    **5 `LAST_INSERT_ID`**, **1 `NOW()`**;
  - **3,080 `MySql*` concrete-type usages in 28 files**.
- **`SchemaIntegrityTests`** (live DB) — dumps load cleanly; workspaces seeded;
  `configurazione` `Main` rows present; every C# table reference resolves; the
  `masterDB` empty-`database=` tenancy swap is consistent with `data.cs:43`.
- **`QueryCharacterizationTests`** (live DB) — runs the **actual SQL from the
  sources** (delays/warning/ExpiryDate/TimeZone/loadTaskEvents big join, views)
  against the seeded schema; write paths are wrapped in rolled-back transactions.

`tests/VirtualChief.DomainTests/DomainBusinessTests.cs` exercises the compiled
legacy classes end-to-end against the DB: `Dati` tenant connection-string swap,
`KISConfig.ExpiryDate`/`WizLogoCompleted`, `ElencoCommesse.loadCommesse`,
`Reparto` by id / `loadPostazioni` / `loadProcessiVarianti` / `loadOperatori` on the
`vc_dev` tenant (1,301 work orders, 3 departments seeded from the dumps).

How the DomainTests project works ("no Windows/IIS" strategy):

- It links the actual source files from `KisWebApp/App_Sources/` into the test
  assembly (`csproj` `<Compile Include="..\..\KisWebApp\App_Sources\*.cs">`), so the
  code under test is **byte-for-byte the legacy business layer** — the same code the
  migration must port, not a re-implementation.
- `Shims/SystemWeb.cs` supplies compile-only stand-ins for the small `.NET
  Framework System.Web` surface actually used (`HttpContext`/`Session`/
  `GetOwinContext`, `Membership.GeneratePassword`) and empty namespaces for
  `System.Web.Mvc`/`System.Web.Hosting`; `Shims/ResAccountMgm.cs` stubs the
  strongly-typed resource class referenced by `Account.cs`. Only 2 of the 28
  App_Sources files (`data.cs`, `quality.cs`) reference `System.Web` at runtime.
- `App.config` points `masterDB`/`vcmain` at the private MariaDB; `pooling=true`
  mirrors the legacy Web.config (pooling was measured to cut the 1,301-`Commessa`
  load from ~2 min to ~4 s). A `[ModuleInitializer]` (`Support/TestConfig.cs`)
  loads the copied `.dll.config`, since the xunit testhost does not auto-load it.
- The 8 tests pass in ~7 s. This is now the fastest-feedback layer for the porting
  work: each business class can be brought to .NET 8/PostgreSQL and re-verified
  against these same flows.

Characterization findings surfaced by the suite (recorded as baselines/tests):

1. **Schema drift:** `inputpoints`, `inputpoints_departments`,
   `inputpoints_workstations`, `parts`, `parts_suppliers`, `taskspostazioni` are
   referenced by C# SQL but appear in **no** schema dump — the checked-in dumps
   are stale vs. the code.
2. **Alias case-sensitivity:** `Analysis.cs:1790` declares alias `TaskVariant` but
   references `taskvariant`; works on MySQL 8 (case-insensitive), **fails on
   MariaDB and PostgreSQL** — verified by an explicit test.
3. The `loadTaskEvents` big join (10+ tables) executes on the seeded MariaDB once
   the alias-case issue is corrected — a good probe for the Postgres dialect pass.
4. `ExpiryDate` value is stored as `dd/MM/yyyy` (parsed by `KISConfig.ExpiryDate`);
   the Windows timezone id `W. Europe Standard Time` in `configurazione` resolves
   on Linux/.NET 10 via the built-in Windows→IANA timezone mapping (verified live,
   so no timezone shim is needed).
5. The dependency closure of the compiled App_Sources is small: `data`, `clienti`,
   `commesse`, `configurazione`, `eventi`, `processi`, `produzione`, `reparti`,
   `postazioni`, `users`, `Account`, `inputpoints`, `KanbanBox`, `kpi`, `relazioni`,
   `WorkInstructions`, `menu`, `permessi`, `FreeTimeMeasurement`,
   `NoProductiveTasks`, `andon` — plus the `ReadAsAsync` extension from
   `Microsoft.AspNet.WebApi.Client` for the Kanban HTTP helpers.

Deferred (require the app running on Windows/IIS, per §5.1): endpoint contract
tests and the screenshot/visual-regression baseline. The static/DB tests and the
Tier-1 domain tests above are exactly the layers that survive and are re-run
against PostgreSQL after the migration.

### Risks to manage
- **WebForms → Razor conversion** is the riskiest step: keep it as its own,
  visually-verified phase.
- **ASMX → Web API**: preserve the WSDL client contracts the schedulers use, or
  update the clients in the same commit.
- Keep **MySQL 8** and the schema untouched.
- Any NuGet bump must preserve Bootstrap / jQuery UI rendering exactly.

---

## 6. v2.0 Working Plan

> **Versioning:** all work for the new version of VirtualChief happens on the
> **`v2.0` branch**, created off `master` and pushed to `origin`. `master` remains
> the stable product baseline. This session covers **branch setup + Phase 0
> (stabilize) + Phase 1 (same-stack modernization)**; later phases follow the
> roadmap in §4 either on `v2.0` in subsequent sessions or on their own branches.

### 6.1 Branch setup
1. `git checkout -b v2.0` off `master` (clean tree), then `git push -u origin v2.0`.
2. All commits and the eventual PR target `v2.0`.

### 6.2 Phase 0 — Stabilize (no architectural change)
1. **Secrets → config store:** extract hardcoded SMTP/API/DB/SIAV/Auth0
   credentials (e.g. `Controllers/DelaysAlarmController.cs`,
   `App_Sources/Account.cs`, `KanbanBox/*.asmx.cs`, `Eventi/*.asmx.cs`) into
   `Web.config` appSettings / an external secrets source; nothing printed or
   committed.
2. **Parameterize concatenated SQL** in `App_Sources` — remove the string-concat
   injection surface without changing query semantics.
3. **Harden API:** remove `TypeNameHandling.All`
   (`KisWebApp/App_Start/WebApiConfig.cs:23`); restore SSL validation in the SIAV
   tool.
4. **Delete orphans/artifacts:** `ConsoleApp1`, `VCAlarmEvents` (netcoreapp2.0),
   `VCAlarmsEvents` (broken duplicate), `UpgradeLog.*`, `_UpgradeReport_Files/`,
   `Kaizen Indicator System.v11.suo`, `Packages.dgml` — update the solution file
   accordingly.

### 6.3 Phase 1 — Same-stack modernization
1. `packages.config` → **PackageReference**, pin versions compatible with .NET
   Framework 4.8 (preserve Bootstrap/jQuery UI rendering).
2. **Dapper** over the existing SQL in `App_Sources` (same queries, less ADO.NET
   boilerplate).
3. Extract duplicated logic (delays/warnings, SIAV export DTOs, EventsExport)
   into a **shared class library** used by both ASMX and Web API paths.
4. Typed config reader over `Web.config` (same values); **Serilog** structured
   logging in `data.cs`, controllers, console agents.

### 6.4 Verification for this session
- `tests/provision-db.sh` (once per machine), then `dotnet test
  tests/VirtualChief.Tests` (22 green) and `dotnet test
  tests/VirtualChief.DomainTests` (8 green).
- Confirm the DomainTests still compile the unmodified `App_Sources` (the Dapper
  swap must not break the class-level tests) and that no project fails to build.

### 6.5 Guardrails
- No UI or business-function change; MySQL 8 schema untouched.
- Each phase committed separately, tests green before moving on.
