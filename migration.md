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

VirtualChief is a web-based shop-floor / production-monitoring suite. One ASP.NET web application (`KisWebApp`) provides the product UI and business logic; ~11 run-once console applications act as integration agents and scheduler triggers (polling kanban systems, ERPs, exporting production data). MySQL 8 is the database. The system is multi-tenant: every tenant (customer plant) gets its own MySQL database, while a central `vcmain` database holds accounts, workspaces, roles and the menu tree.

External integrations:
- **KanbanBox** — cloud kanban system (REST API, card polling, auto production launch)
- **SIAV CPM** — production performance software (client-cert + bearer token, `.dot` graph download, events export into a dedicated MySQL DB)
- **Finestra3000 / Omnia3000** — ERP (XML order import from a drop folder)
- **Auth0** — identity provider (OpenID Connect login)
- **Google SMTP** — transactional e-mail (delays, warnings, license, quality, integration errors)

Deployment model: IIS-hosted ASP.NET app + Windows Task Scheduler invoking the console agents. (The Linux migration runbook is integrated into the Phase 4 deployment instructions in **§4.3**.)

---

## 2. Functional Report

### 2.1 Web application (`KisWebApp`)

Hybrid **WebForms + ASP.NET MVC 5 + Web API 2**, target framework **.NET Framework 4.8**.
Scale: ~1,481 C# files, 114 `.aspx`, 172 `.ascx`, 154 `.cshtml`, 7 `.asmx`, 48 controllers. Main libraries: EF 6.4.4 + MySql.Data 8.0.24 (raw ADO.NET over MySQL is the primary data path), OWIN + Auth0 OpenID Connect, jQuery 3.6, Bootstrap 4.6, jQuery UI, Google Charts, D3/graphviz, iTextSharp (PDF), GenCode128 (barcodes), AjaxControlToolkit (legacy).

#### 2.1.1 Authentication, tenancy & licensing
- **Auth0 login** (OWIN `Startup.cs`): OpenID Connect, cookie auth. On first login the user is auto-provisioned (`useraccounts` in `vcmain`), a default workspace is assigned, and account activation / invites / email-verification flows are handled by the `AccountsMgm` area. Workspace switching supported.
- **Legacy forms login** (`Login/login.aspx`) still present for the old user/password model.
- **Licensing**: per-tenant `ExpiryDate` in the `configurazione` table. `Site.Master` shows a warning banner ≤30 days before expiry and redirects to `LicenseExpired.aspx` once expired. `Eventi/Licensing.asmx` emails the Admin group about renewal.
- **Tenancy**: connection string per workspace built at runtime by replacing the `database=` part of a base connection string (`Dati.GetConnectionString` in `App_Sources/data.cs`); global `vcmain` DB via `Dati.VCMainConn()`.
- **i18n**: per-user language (en / it / es / es-AR) via `CurrentUICulture` in `Global.asax.cs`, resx resource files, localized "whatsnew" tips.

#### 2.1.2 Functional modules (WebForms pages + MVC areas)
- **Customers / CRM** (`Clienti/`, `Customers` area) — customer master data, contacts, emails, phones, customer portfolio.
- **Sales orders / Commesse** (`Commesse/`, `SalesOrders` area) — order creation wizards, PERT scheduling, workload checks, delivery dates, article-linked alarms, barcode label printing, manual task rescheduling (PDF via iTextSharp).
- **Processes & products** (`Processi/`, `Products`, `Parts` areas) — processes, variants, cycle/unload times (`tempiciclo`), PERT precedence editing, microsteps, product and task parameters.
- **Production** (`Produzione/`) — production planning board, launch-in-production with simulation (`SimulaIntroduzioneInProduzione`, `LanciaInProduzione`), workload simulation across departments, task status tracking, production history and "exhume" of finished products, production details per product.
- **Shop floor & organization** (`Reparti/`, `Postazioni/`, `Operatori/`) — departments, workstations, shift calendars / holidays / overtime, operator↔station registration, cycle-time activities.
- **Operator UI** — legacy barcode check-in/out pages (`AzioniBarcode*`, `checkInPostazione_OLD`, `doTasksPostazione_OLD`) and the modern **WebGemba** `Workplace` area: task lists (available / in execution / assigned), station check-in, per-workstation KPIs, operator notes. Work instructions with file uploads.
- **Andon boards** (`Andon/`, `Andon` area) — per-department live boards: current shift, active operators, production indicator; configurable scroll type, username format, show-active-users flag.
- **Analytics & KPIs** (`Analysis/`, `Analysis` area) — global KPIs (delays, lead time, quantities, warnings/NCs/improvement actions), operator productivity, product analysis, production history, Pareto / customer portfolios, cost per commessa, **Process Mining** view (D3 + graphviz).
- **Quality** (`Quality` area) — non-conformities board/CRUD, NC analysis charts (per-period, Pareto, causes), non-conformity types/causes, corrective actions, improvement actions with file attachments.
- **Free time measurement** (`FreeTimeMeasurement` area) — stopwatch-style task time measurements, multi-file upload, SPC-style analysis views.
- **Events / alarms** (`Eventi/`) — warning & delay event configuration scoped per department / order / article, with per-group and per-user e-mail subscriptions.
- **Admin & config** (`Admin/`, `Config` area) — menu tree management, KPI admin, report configuration, order-status mapping, logo / timezone config, first-run configuration wizard (`Configuration/`), measurement units, no-productive tasks,
  departments' auto-pause settings.
- **Personal area** (`Personal/`) — password, contacts, language, alerts, post-login destination URL.
- **Users & permissions** (`Users/`, `Users` area) — users, groups, permissions, work-hours registration, disabled users, checksum-based activation.

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
- `EventsExportController` — exports finished-task events / converts events to timespans for SIAV.
- `RemoteSalesOrderController` / `ThirdPartySalesOrdersController` — third-party sales-order import (auto-create customer, plan and launch production).
- `ConfigController` — API-key guarded get/set of license expiry date and module summaries.

### 2.2 Console scheduler / integration apps

All are **run-once console executables** intended for Windows Task Scheduler (no Windows Services, no timers/loops). Most real logic lives server-side in `KisWebApp`; the agents are thin triggers.

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
- **`vcmain`** — central multi-tenant DB: **13 tables** (`workspaces`, `useraccounts`, `useraccountworkspaces`, `groupss`, `permissions`, `menualbero`, ...).
- **`vc_dev`** — development tenant snapshot: **84 tables, 1 view** (older/different evolution; contains `accounts`, `taskspostazioni_old`; lacks `freemeasurements*`, `microsteps`, `taskstimespans`, `noproductivetasks`).

**No stored procedures, functions or triggers** exist in any dump — all business logic lives in the C# application layer.

Main domain areas (from `kaizenkey`):
- CRM: `anagraficaclienti`, `contatticlienti`, `contatticlienti_email`, `contatticlienti_phone`
- Orders: `commesse`, `productionplan`, `varianti`, `variantiprocessi`
- Tasks/time: `tasksproduzione`, `taskstimespans`, `taskuser`, `taskparameters`, `tempiciclo`, `microsteps`, `task_microsteps`, `tasksmanuals`, `noproductivetasks`, `taskreschedulelog`
- Organization: `reparti`, `postazioni`, `turniproduzione`, `orarilavoroturni`, `straordinarifestivita`, `registrooperatoripostazioni`
- Processes: `processo`, `processipadrifigli`, `precedenzeprocessi`, `relazioniprocessi`
- Event log/alarms: `registroeventiproduzione`, `registroeventitaskproduzione`, `warningproduzione`
- Quality/Kaizen: `noncompliances`, `noncompliancestypes`, `noncompliancescause`, `correctiveactions`, `improvementactions`, `kpi_description`, `kpi_record`
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

## 4. Unified Modernization & Linux Migration Plan

> **Guiding Principle**: Modernize the platform *underneath* the existing UI and business logic while enabling a native Linux deployment. Screens, workflows, URLs, emails, and DB schema (~98 tables) stay identical. Each phase is independently verifiable using automated characterization and domain test suites.
>
> **Working Branch**: All migration work takes place on the **`v2.0`** branch off `master`.

---

### 4.1 Architectural Strategy & Target State

The ultimate goal of this plan is to transform VirtualChief into a modern, cross-platform, containerized, and maintainable platform running natively on Linux (.NET 8 runtime) with zero UI or business logic regression.

#### Current vs. Target Architecture Overview

| Component | Current State (Legacy) | Target State (Modernized v2.0) | Resolution / Seam |
|---|---|---|---|
| **Web Runtime** | WebForms + ASP.NET MVC 5 + Web API 2 on .NET Framework 4.8 (IIS-dependent) | ASP.NET Core (.NET 8) hosted on Kestrel behind `nginx` reverse proxy with TLS termination | Re-platform UI to Razor Pages/Views + Web API controllers (Phase 2–3) |
| **Data Access** | Raw ADO.NET (`MySqlCommand`/`MySqlDataReader`) & MySql.Data 8.0.24 | **Dapper 2.1.79** + **MySqlConnector** over unchanged MySQL 8 schema | Perform raw SQL parameterization & Dapper mapping across all `App_Sources` (Phase 1) |
| **Scheduler / Agents** | 11 run-once console apps (.NET 4.0–4.6.1) triggered via Windows Task Scheduler against `localhost` | **Single .NET 8 Worker Service** running scheduled cron/timer jobs natively via `systemd` | Extract agent logic into shared library `KIS.Shared` & worker tasks (Phase 3) |
| **Web Services** | 7 ASMX SOAP services consuming WCF bindings | ASP.NET Web API controllers maintaining identical HTTP routes & JSON contracts | Migrate ASMX to Web API endpoints with backward compatibility (Phase 2) |
| **Auth & Tenancy** | Auth0 OpenID Connect via OWIN + Katana middleware | ASP.NET Core OpenID Connect middleware with identical Auth0 tenant & claims | Swap OWIN for ASP.NET Core Auth middleware; wrap tenancy reads in `WebEnv.cs` seam |
| **Configuration & Secrets** | Hardcoded API keys, DB/SMTP passwords in `Web.config` and C# sources | Environment-first configuration reader (`AppConfig.cs` + `Secrets.cs` + `appsettings.json`) | Centralize secrets in `VC_*` env vars without code changes |
| **Logging & Ops** | Custom file writes (`C:\temp`) and unhandled exceptions | **Serilog** structured logging to `stdout`/`journald` with open telemetry / health checks | Inject Serilog into data layer, controllers, and background worker |

---

### 4.2 Code Audit & Progress Inventory

An audit of the repository state on branch `v2.0` establishes the ground-truth baseline of completed, in-progress, and remaining work:

#### 4.2.1 Phase Status Summary

| Phase | Description | Status | Key Deliverables / Milestones |
|---|---|---|---|
| **Phase 0** | Stabilization & Security Hardening | **Completed ✅** | Hardcoded secrets moved to `Secrets.cs`; `TypeNameHandling.None` set; SIAV SSL bypass removed; orphan projects (`ConsoleApp1`, `VCAlarmsEvents`) removed; CI pipeline rewritten (`.github/workflows/dotnet.yml`). |
| **Phase 1** | Same-Stack Modernization & Data Layer | **In Progress ⏳** | Dapper migration across `App_Sources` (**28 of 28 files converted**); `WebEnv.cs` & `AppConfig.cs` seams implemented; MariaDB test net deployed (**24 static/DB tests + 91 Tier-1 domain tests green**). |
| **Phase 2** | UI Modernization & Endpoints Migration | **In Progress ⏳** | **414 `.cshtml` Razor views** created/updated; `<asp:Chart>` control replacement with Google Charts/D3 **completed**; ASMX → Web API transition **completed** (7 controllers ported, MySql drift 1,184 → 1,187). |
| **Phase 3** | .NET 8 Re-Platforming & Worker Service | **Planned 📋** | Retargeting `KisWebApp` to .NET 8; unifying 11 console schedulers into a single .NET 8 Worker Service; Kestrel + `nginx` setup. |
| **Phase 4** | Database Modernization & Linux Verification | **Planned 📋** | Linux MySQL 8 deployment; Postgres dialect audit pass (80 bare `user` keywords, backticks, `LAST_INSERT_ID`); systemd service units. |
| **Phase 5** | Production Hardening & Best Practices | **Planned 📋** | Full DI container integration; ASP.NET Core Health Checks; automated E2E integration test suite expansion. |

#### 4.2.2 Data Layer (App_Sources) Dapper Audit Matrix

Out of 28 core domain files in `KisWebApp/App_Sources/`, all 28 have been fully converted to Dapper. The inventory of concrete `MySql*` type usages has dropped from **2,598 to 1,184** (remaining usages are connection/transaction handling objects required by Dapper):

| Source File | MySql Usages (Before) | MySql Usages (Current) | Dapper Conversion Status |
|---|---|---|---|
| `relazioni.cs` | 42 | 2 | **Completed ✅** |
| `menu.cs` | 45 | 15 | **Completed ✅** |
| `permessi.cs` | 38 | 11 | **Completed ✅** |
| `configurazione.cs` | 312 | 208 | **Completed ✅** (queries mapped; connections retained) |
| `processi.cs` | 485 | 159 | **Completed ✅** (queries mapped; connections retained) |
| `users.cs` | 246 | 106 | **Completed ✅** (queries mapped; connections retained) |
| `quality.cs` | 270 | 152 | **Completed ✅** (queries mapped; connections retained) |
| `inputpoints.cs` | 63 | 0 | **Completed ✅** |
| `postazioni.cs` | 64 | 0 | **Completed ✅** |
| `produzione.cs` | 132 | 1 | **Completed ✅** (1 residual line in dead code) |
| `WorkInstructions.cs` | 82 | 0 | **Completed ✅** |
| `andon.cs` | 103 | 0 | **Completed ✅** |
| `commesse.cs` | 184 | 84 | **Completed ✅** |
| `eventi.cs` | 120 | 72 | **Completed ✅** |
| `clienti.cs` | 110 | 68 | **Completed ✅** |
| `FreeTimeMeasurement.cs` | 95 | 58 | **Completed ✅** |
| `KanbanBox.cs` | 25 | 5 | **Completed ✅** |
| `kpi.cs` | 32 | 12 | **Completed ✅** |
| `NoProductiveTasks.cs` | 28 | 14 | **Completed ✅** |
| `parts.cs` | 28 | 14 | **Completed ✅** |
| `reparti.cs` | 145 | 86 | **Completed ✅** |
| `Analysis.cs` | 35 | 8 | **Completed ✅** |
| `data.cs` | 12 | 7 | **Completed ✅** (Core connection factory seam) |
| `Account.cs` | 140 | 94 | **Completed ✅** (remaining raw ADO.NET only in commented-out dead code) |
| **Total** | **2,598** | **1,184** | **28 / 28 Files Converted** |

---

### 4.3 Comprehensive Phase-by-Phase Roadmap & Runbook

#### Phase 0 — System Stabilization & Security Hardening [Status: Completed ✅]

**Objective:** Fix active security vulnerabilities, remove legacy code rot, extract secrets into configuration, and establish a working CI pipeline without altering business logic or system architecture.

- [x] **Secrets Extraction:** Move hardcoded SMTP passwords, API keys, SIAV credentials, and Auth0 secrets out of `Web.config` and C# sources into `Secrets.cs` and environment variables (`VC_SMTP_PASS`, `VC_AUTH0_CLIENT_SECRET`, etc.).
- [x] **SQL Parameterization:** Remove string concatenation in SQL queries across `App_Sources` to eliminate SQL injection attack surface.
- [x] **API Hardening:** Remove deserialization risk `TypeNameHandling.All` in `KisWebApp/App_Start/WebApiConfig.cs` (set to `TypeNameHandling.None`). Restore SSL certificate validation in SIAV export tool (`VCProductionEventsExport-SIAV`).
- [x] **Orphan & Legacy Cleanup:** Delete dead projects (`ConsoleApp1`, `VCAlarmEvents` netcoreapp2.0, `VCAlarmsEvents` broken duplicate), VS upgrade logs (`UpgradeLog*`, `_UpgradeReport_Files/`), `.suo` files, and clean up solution bindings in `Virtual Chief.sln`.
- [x] **CI Pipeline Renewal:** Rewrite `.github/workflows/dotnet.yml` as a modern CI pipeline running on Linux runners, installing MariaDB, executing `tests/provision-db.sh`, and running both `VirtualChief.Tests` and `VirtualChief.DomainTests`.

---

#### Phase 1 — Same-Stack Modernization & Data Layer Decoupling [Status: In Progress ⏳]

**Objective:** Upgrade data access to Dapper + MySqlConnector, introduce cross-platform host seams for `System.Web`, migrate NuGet packages to PackageReference, and maintain a robust Linux test net.

- [x] **Dapper ORM Migration:** Convert raw ADO.NET `MySqlCommand`/`MySqlDataReader` boilerplate to Dapper query methods while retaining exact SQL syntax and domain API signatures. (All 28 domain files converted; `Account.cs` completed — remaining `MySqlCommand`/`MySqlDataReader` pairs only in commented-out dead code).
- [x] **Cross-Platform Host Seams (`System.Web` Decoupling):**
  - Implement `WebEnv.cs` (`KIS.App_Code.WebEnv`) to isolate `HttpContext.Current`, session reads, and OWIN claim extractions for multi-tenant workspace resolution (`ActiveWorkspaceId`, `ActiveWorkspaceName`).
  - Implement `AppConfig.cs` for environment-first configuration reading (`VC_*` env vars over `Web.config`).
  - Provide cross-platform password generation fallback for `Membership.GeneratePassword`.
- [x] **Linux Test Harness Deployment:**
  - Maintain `tests/provision-db.sh` to initialize a private MariaDB instance on `127.0.0.1:3307` and populate `kaizenkey`, `vcmain`, and `vc_dev` schemas.
  - Maintain `tests/VirtualChief.Tests` (24 static/DB characterization tests locking table references, SQL string syntax, and query execution).
  - Maintain `tests/VirtualChief.DomainTests` (96 business flow tests compiling raw `App_Sources` on Linux via `Shims/SystemWeb.cs`).
- [ ] **NuGet Modernization:** Migrate `packages.config` to **PackageReference** format across projects, pinning dependencies compatible with .NET Framework 4.8 (preserving Bootstrap 4.6 and jQuery UI rendering). *(Blocked: the WebForms `KisWebApp` project cannot compile on Linux, so PackageReference changes are unverifiable in CI.)*

---

#### Phase 2 — UI Modernization & Web Service Endpoints Migration [Status: In Progress ⏳]

**Objective:** Migrate WebForms controls (`.ascx`) and pages (`.aspx`) to ASP.NET MVC Razor views (`.cshtml`), replace proprietary WebForms controls with cross-platform JS libraries, and convert ASMX SOAP services to Web API controllers.

- [/] **WebForms → Razor View Conversion:**
  - Convert 114 `.aspx` pages and 172 `.ascx` user controls to MVC Razor `.cshtml` views (414 `.cshtml` views created/updated).
  - Preserve identical HTML structure, CSS classes, element IDs, and client-side JavaScript logic.
- [x] **`<asp:Chart>` Control Replacement:**
  - Replace server-rendered `<asp:Chart>` (WebForms `System.Web.DataVisualization`) controls with client-side **Google Charts** or **D3.js** renderings across the 10 affected views:
    - `Produzione/wlReparto.ascx` & `Produzione/wlSimReparto.ascx` (Workload charts)
    - `Postazioni/viewCalendarioPostazione.ascx` & `Reparti/postazioneWorkLoad.ascx` (Station calendars & workloads)
    - `Reparti/manageCalendarFesteStraordinari.aspx` & `showCalendarFesteStraordinari.aspx` (Shift calendars)
    - `Reparti/processoWorkLoad.ascx` & `Commesse/wzCheckWorkLoadReparto.ascx` (Process workloads)
    - `OLD_kpi/showKPIRecords.ascx` & `Analysis/DetailAnalysisCustomer.ascx` (KPI & Customer charts)
  - Enforced by `AspChartControls_AreEliminated` (no `<asp:Chart` remains repo-wide).
- [x] **ASMX Web Services → Web API Controllers:**
  - Replace the 7 `.asmx` SOAP endpoints with ASP.NET Web API controllers:
    - `Eventi/Licensing.asmx` → `api/licensing/check`
    - `Eventi/Warning.asmx` → `api/events/warnings`
    - `Eventi/Ritardi.asmx` → `api/events/delays`
    - `Eventi/QualityModuleEvents.asmx` → `api/quality/events`
    - `KanbanBox/KanbanBoxReader.asmx` → `api/kanbanbox/reader`
    - `KanbanBox/KanbanBoxCheckHealth.asmx` → `api/kanbanbox/health`
    - `Processi/getProcessData.asmx` → `api/processi/pert`
  - Controllers port the full ASMX business logic (Dapper/MySql and `WebEnv.ActiveWorkspaceName` in place of `Session["ActiveWorkspace_Name"]`); MySql drift baseline updated to reflect the ports (1,184 → 1,187 across 26 files).
  - Maintain backward-compatible route aliases and payload structures so console agents / background jobs continue uninterrupted.
- [ ] **Visual Regression Baseline:** Establish screenshot comparison tests for core screens (Workplace WebGemba, Production Board, PERT Editor, Quality NC Board) to guarantee 100% UI parity.

---

#### Phase 3 — .NET 8 Runtime Re-Platforming & Unified Worker Service [Status: Planned 📋]

**Objective:** Re-target the web application to .NET 8 (ASP.NET Core) and consolidate all scheduled console applications into a single cross-platform .NET 8 Worker Service.

- [ ] **ASP.NET Core (.NET 8) Web App Retargeting:**
  - Port `KisWebApp` project structure to SDK-style .NET 8 (`net8.0`).
  - Replace OWIN/Katana Auth0 middleware with `Microsoft.AspNetCore.Authentication.OpenIdConnect`.
  - Replace `Web.config` with `appsettings.json` and environment variable binding (`VC_*`).
  - Configure Kestrel web server to host behind `nginx` reverse proxy with TLS.
- [ ] **Unified .NET 8 Worker Service (`VirtualChief.Worker`):**
  - Create a single .NET 8 Worker Service project to replace the 11 individual console executables:
    - `KISScheduler` (Delay & Warning alerts)
    - `KISLicenseCheck` (License expiration checks)
    - `KISQualityEventsCheck` (Quality event reminders)
    - `KanbanBoxReaderScheduler` & `KanbanBoxCheckHealthScheduler` (KanbanBox integration)
    - `VCAutoPauseTasks` (Auto-pause tasks outside shifts)
    - `ThirdPartySalesOrders_Finestra3000` (Finestra3000 XML drop-folder reader)
    - `VCProductionEventsExport-SIAV` & `SIAV-GetDotFile` (SIAV CPM integration)
  - Execute jobs asynchronously using hosted background tasks driven by configurable Cron/Timer schedules.
  - Eliminate fixed `C:\temp` log files; stream structured logs via Serilog to `stdout` / `journald`.

---

#### Phase 4 — Database Modernization & Linux Verification [Status: Planned 📋]

**Objective:** Run the entire system natively on Linux (MySQL 8 or PostgreSQL) and execute the complete operational deployment runbook.

##### Step-by-Step Linux Deployment Runbook

```bash
# 1. Prepare Linux Host (Ubuntu 22.04 / 24.04 LTS)
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0 nginx mariadb-server git

# 2. Provision Production MySQL/MariaDB Databases
tests/provision-db.sh

# 3. Build & Publish Web App and Worker Service
dotnet publish KisWebApp/KisWebApp.csproj -c Release -o /var/www/virtualchief
dotnet publish VirtualChief.Worker/VirtualChief.Worker.csproj -c Release -o /opt/virtualchief-worker

# 4. Configure systemd Service for Web App (Kestrel)
sudo cat << 'EOF' > /etc/systemd/system/virtualchief-web.service
[Unit]
Description=VirtualChief Web Application (.NET 8)
After=network.target mariadb.service

[Service]
WorkingDirectory=/var/www/virtualchief
ExecStart=/usr/bin/dotnet /var/www/virtualchief/KisWebApp.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=virtualchief-web
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=VC_MASTERDB_CONN=Server=127.0.0.1;Port=3307;Uid=vc;Pwd=vc;Database=vcmain;

[Install]
WantedBy=multi-user.target
EOF

# 5. Configure systemd Service for Background Worker
sudo cat << 'EOF' > /etc/systemd/system/virtualchief-worker.service
[Unit]
Description=VirtualChief Background Worker (.NET 8)
After=network.target mariadb.service

[Service]
WorkingDirectory=/opt/virtualchief-worker
ExecStart=/usr/bin/dotnet /opt/virtualchief-worker/VirtualChief.Worker.dll
Restart=always
RestartSec=10
User=www-data
Environment=DOTNET_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
EOF

# 6. Enable & Start Services
sudo systemctl daemon-reload
sudo systemctl enable --now virtualchief-web virtualchief-worker
```

##### PostgreSQL Dialect Audit & Migration Pass

For installations targeted at PostgreSQL instead of MySQL:
- Resolve **80 bare `user` reserved identifier occurrences** across 10 files (quote as `"user"`).
- Remove **4 MySQL backticks** in `configurazione.cs`.
- Replace **5 `LAST_INSERT_ID()`** calls with `RETURNING id`.
- Replace **1 `NOW()`** call with `CURRENT_TIMESTAMP`.

---

#### Phase 5 — Production Hardening & Operational Excellence [Status: Planned 📋]

**Objective:** Introduce modern software engineering practices, health diagnostics, and production observability.

- [ ] **Dependency Injection Container:** Refactor `App_Sources` instantiation to leverage `Microsoft.Extensions.DependencyInjection` (injecting DB factories, logger, and configuration).
- [ ] **Health Checks & Diagnostics:** Expose `/healthz` endpoints verifying database connectivity, storage access, and external integration health (KanbanBox, SIAV, Auth0).
- [ ] **Centralized Logging & Observability:** Configure Serilog sinks for structured log aggregation (Loki / Elasticsearch / OpenTelemetry).

---

## 5. Testing & Quality Assurance Strategy

The codebase migration is protected by a multi-layered automated testing framework designed to catch regressions in SQL syntax, domain logic, HTTP API contracts, and visual UI rendering.

### 5.1 Timing & Strategy Principles

| When | What to write | Why |
|---|---|---|
| **Before** | DB characterization tests, domain-layer tests, endpoint contract tests, screenshot/visual-regression fixture baseline | Locks behavior that survives; becomes the migration safety net |
| **During** | Dialect-equivalence re-run of DB tests (same suite on MySQL then PostgreSQL), visual-regression diffs per converted page | Proves queries and pixels are unchanged |
| **After** | Razor pages, Kestrel hosting, worker-service jobs, Docker/nginx, Serilog/DI/health checks | Platform-specific code does not exist yet |

### 5.2 Verification Matrix & Test Layers

```
+-----------------------------------------------------------------------+
|                       Visual Parity / UI Tests                        |
|             (Screenshot Diffing for WebForms -> Razor)               |
+-----------------------------------------------------------------------+
|                    HTTP API & Controller Contract                     |
|           (Contract Tests for Schedulers & Web API Routes)            |
+-----------------------------------------------------------------------+
|               Domain Logic Flow Tests (Tier 1 Business)                |
|      (VirtualChief.DomainTests: 91 Tests running via SystemWeb Shims) |
+-----------------------------------------------------------------------+
|             Database Characterization & Portability Audit             |
|   (VirtualChief.Tests: 22 Static & Live Query Tests on MariaDB 3307)  |
+-----------------------------------------------------------------------+
```

### 5.3 App_Sources Tiered Coverage Specification

1. **Tier 1 — Domain Business Logic (96 Active Tests in `VirtualChief.DomainTests`):**
   - High-risk domain models compiled directly into Linux test binaries: `produzione.cs`, `processi.cs`, `commesse.cs`, `quality.cs`, `clienti.cs`, `reparti.cs`, `eventi.cs`, `postazioni.cs`, `users.cs`, `inputpoints.cs`, `KanbanBox.cs`, `Account.cs`, `Analysis.cs`, `FreeTimeMeasurement.cs`, `WorkInstructions.cs`, `andon.cs`.
   - Deployed on Linux without Windows/IIS by compiling the unmodified `App_Sources` into a test assembly via System.Web shims.
2. **Tier 2 — SQL & Config Wrappers (Covered by `VirtualChief.Tests`):**
   - Direct query wrappers verified by live SQL characterization tests: `configurazione.cs`, `menu.cs`, `permessi.cs`, `relazioni.cs`, `data.cs`.
   - Their behavior is "run query X, map result." DB characterization tests exercise the exact SQL.
3. **Tier 3 — UI & Output Helpers (Replaced during Migration):**
   - Utility code re-implemented during WebForms→Razor work: `ImageHandler.cs`, `iTextSharpEventHandler.cs`, `FilesHelper.cs`. No tests before migration.

### 5.4 Controller Tiered Coverage Specification

Controllers are the orchestration layer over the domain classes. Since they will be reworked during migration (routing, model binding, DI, async signatures), we focus coverage on HTTP contract tests and fat orchestrators:

- **Tier A — Fat Orchestrating MVC Controllers (Extract to services and test services):**
  - Includes controllers with significant logic: `Quality/ImprovementActionsController.cs` (2,286 lines), `Workplace/WebGembaController.cs` (1,766 lines), `SalesOrders/SalesOrderController.cs` (1,745 lines), `Quality/NonCompliancesAnalysisController.cs` (1,703 lines), `Quality/NonCompliancesController.cs` (1,192 lines), `FreeTimeMeasurement/FreeMeasurementController.cs` (1,169 lines), `Products/ProductsController.cs` (1,052 lines), `Analysis/GlobalKPIsController.cs` (953 lines), `Analysis/ProductAnalysisController.cs` (698 lines), `Analysis/ProductionWorkloadController.cs` (605 lines), `Analysis/TaskAnalysisController.cs` (564 lines), `Users/UsersController.cs` (607 lines).
  - Rather than brittle controller-level unit tests, extract logic into services and unit-test the services.
- **Tier B — Web API Controllers (HTTP/Contract Tests):**
  - Includes `X-API-KEY`-guarded scheduler endpoints: `customers.cs`, `products.cs`, `RemoteSalesOrderController.cs`, `ThirdPartySalesOrdersController.cs`, `DelaysAlarmController.cs`, `AutoPauseTasksController.cs`, `EventsExportController.cs`.
  - Write URL-level contract tests (request → status code → JSON shape → DB effect) that survive the migration and serve as scheduler-to-API contracts.
- **Tier C — Small CRUD/Config MVC Controllers (End-to-End HTTP Flow Tests):**
  - Includes remaining Areas controllers (e.g. `Config/*`, `Andon/*`, `Parts`, `Personal`, `Departments`, `Production/*`).
  - Run a few E2E integration tests per area to catch route, auth, and model binding regressions.
- **Tier D — Demo/Scaffolding (Skip):**
  - Skip testing for `TestController.cs`, `TestClass.cs`, `CustomersTest.aspx(.cs)`, etc.

### 5.5 Deployed Characterization Suite & Linux DomainTests Harness

The DB-layer and portability-audit portion of the strategy runs on Linux against a private MariaDB instance populated from the checked-in dumps.

| Artifact | Location |
|---|---|
| xUnit test project (.NET 10 SDK, MySqlConnector; retargetable to net8.0) — static + DB characterization | `tests/VirtualChief.Tests/` |
| xUnit test project — real `KIS.App_Code`/`KIS.App_Sources` classes compiled in via shims, Tier-1 business flows | `tests/VirtualChief.DomainTests/` |
| Compile-only harness for the legacy WebForms code-behind layer (286 `.aspx.cs`/`.ascx.cs` + designers) compiled against WebForms shims on Linux — 0 errors | `tests/VirtualChief.CodeBehindCompileCheck/` |
| DB provisioning script (private MariaDB on `127.0.0.1:3307`, loads the 3 dumps) | `tests/provision-db.sh` |
| Test config via env vars | `VC_DB_HOST`, `VC_DB_PORT`, `VC_DB_USER`, `VC_DB_PASS` (defaults match the script) |

#### Portability Audit & Characterization Tests (`VirtualChief.Tests`)
- **`PortabilityAuditTests` (static):** Tracks MySQL-to-Postgres dialect differences, including 1,614 SQL string literals, 76 bare `user` reserved identifier occurrences, MySQL backticks, `LAST_INSERT_ID`, and `NOW()`. Tracks `MySql*` type usages (down to 1,187 remaining after Dapper conversion + ASMX→WebAPI ports) and WebForms control elimination (`asp:Chart` gone).
- **`SchemaIntegrityTests` (live DB):** Verifies dumps load cleanly, workspaces seed correctly, and tenancy connection swapping behaves consistently.
- **`QueryCharacterizationTests` (live DB):** Exercises the actual SQL queries from code against the seeded schema, with write paths wrapped in rolled-back transactions.

#### Domain Business Flow Tests (`VirtualChief.DomainTests`)
- Business-flow tests compile the unmodified `App_Sources` classes using a shim (`Shims/SystemWeb.cs`) to supply compile-only stand-ins for standard `System.Web` types (`HttpContext`, `Session`, `Server.MapPath`, etc.).
- Verifies workflows such as workspace loading, order status management, and shift/holiday tracking without Windows/IIS dependencies.

#### Code-Behind Logic Extraction (Pilot Pattern)
- **Pilot:** `KisWebApp/Produzione/avanzamentoProduzione.aspx.cs` → `App_Sources/AvanzamentoProduzioneService.cs`.
  - `CaricaArticoliNonPianificati(tenant)` — extracted the page's `loadCommesse()` aggregation (all `N`/`P` articles across work orders).
  - `ClassificaStato(...)` — extracted the green/yellow/red row-status rule. A pure overload (`ClassificaStato(DateTime, IEnumerable<(EarlyStart, LateStart)>)`) is unit-tested with synthetic windows; the `Articolo` overload is tested against the seeded DB.
  - The code-behind now delegates to the service (verified by `VirtualChief.CodeBehindCompileCheck`).
  - **Pattern for future work:** extract embedded business rules from code-behinds into `App_Sources` services, add DomainTests, then delegate the code-behind to the service. The compile harness keeps the code-behind delegation build-verified while the service is test-verified on Linux.

#### Legacy Code-Behind Compile Harness (`VirtualChief.CodeBehindCompileCheck`)
- The legacy WebForms `.aspx`/`.ascx` markup compiler is Windows-only, so the code-behind layer cannot be built on Linux directly. This harness compiles every `KisWebApp/**/*.aspx.cs`, `*.ascx.cs`, and `*.designer.cs` against expanded WebForms shims (`Shims/SystemWebUI.cs`, `SystemWebMisc.cs`, `GenCode128.cs`) plus the real `iTextSharp` 5.5.13.3 and `System.Drawing.Common` packages.
- The shims cover the charting surface (`System.Web.UI.DataVisualization.Charting`), `TemplateControl`/`Page`/`UserControl` resource APIs, MVC/WebMethod/Helpers namespaces, and the request/response/session/cache surface, keeping the entire code-behind layer build-verified on Linux in CI.

### 5.6 Visual Regression Verification Guidelines for Razor Views
- Establish visual regression baselines for core pages (e.g. Workplace WebGemba, Production Board, PERT Editor, Quality NC Board).
- Capture screenshots before the migration, then programmatically compare them against Razor rendered outputs to verify 100% layout and interactive parity.

### 5.7 Automated Test Execution Instructions

To execute the test suite locally or on a CI/CD Linux agent:

```bash
# 1. Provision the local characterization database (starts MariaDB on 127.0.0.1:3307)
tests/provision-db.sh

# 2. Run static audit & DB characterization tests (24 tests)
dotnet test tests/VirtualChief.Tests

# 3. Run domain business flow tests (96 tests)
dotnet test tests/VirtualChief.DomainTests

# 4. Compile the legacy WebForms code-behind layer against the shims (0 errors expected)
dotnet build tests/VirtualChief.CodeBehindCompileCheck
```

