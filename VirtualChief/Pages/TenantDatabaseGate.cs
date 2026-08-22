using System.Collections.Concurrent;
using KIS.App_Code;
using MySql.Data.MySqlClient;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Detects workspaces whose tenant database does not exist on the MySQL
    /// master server. A workspace row in vcmain.workspaces does NOT imply the
    /// schema exists (UserAccount.addWorkspace never creates it): every legacy
    /// query would die with a MySQL "Unknown database" error. Results are
    /// cached per workspace — positives longer than negatives so that a
    /// database created/fixed while the app runs is picked up quickly.
    /// </summary>
    public sealed class TenantDatabaseChecker
    {
        private static readonly TimeSpan PositiveTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan NegativeTtl = TimeSpan.FromSeconds(30);

        private sealed class CacheEntry
        {
            public bool Exists;
            public DateTimeOffset ExpiresAt;
        }

        private readonly ILogger<TenantDatabaseChecker> _logger;
        private readonly ConcurrentDictionary<string, CacheEntry> _cache =
            new(StringComparer.Ordinal);

        public TenantDatabaseChecker(ILogger<TenantDatabaseChecker> logger)
        {
            _logger = logger;
        }

        public async Task<bool> DatabaseExistsAsync(
            string tenant, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                return false;
            }

            var now = DateTimeOffset.UtcNow;
            if (_cache.TryGetValue(tenant, out var entry) && entry.ExpiresAt > now)
            {
                return entry.Exists;
            }

            bool exists;
            try
            {
                exists = await ProbeAsync(tenant, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Master server unreachable or connection string misconfigured:
                // no tenant page could work either way, treat as "not available".
                _logger.LogError(ex,
                    "TenantDatabaseChecker: verifica del database del workspace {Tenant} fallita", tenant);
                exists = false;
            }

            _cache[tenant] = new CacheEntry
            {
                Exists = exists,
                ExpiresAt = now + (exists ? PositiveTtl : NegativeTtl),
            };
            return exists;
        }

        /// <summary>
        /// Existence probe against the master server, same connection source as
        /// Dati.Dati.GetConnectionString (env VC_MASTERDB_CONN / config
        /// "masterDB") but WITHOUT selecting any schema.
        /// </summary>
        internal static async Task<bool> ProbeAsync(
            string tenant, CancellationToken cancellationToken)
        {
            var csb = new MySqlConnectionStringBuilder(
                Secrets.GetConnectionString("VC_MASTERDB_CONN", "masterDB"))
            {
                // Probe server-wide: connecting with the missing schema selected
                // is exactly what throws "Unknown database".
                Database = "",
            };

            using var con = new MySqlConnection(csb.ConnectionString);
            await con.OpenAsync(cancellationToken);

            using var cmd = con.CreateCommand();
            cmd.CommandText =
                "SELECT COUNT(*) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @db";
            cmd.Parameters.AddWithValue("@db", tenant);

            var result = await cmd.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result) > 0;
        }
    }

    /// <summary>
    /// Gate: when the active workspace has no database, NO page may work.
    /// Every authenticated request carrying a "tenant" claim is short-circuited
    /// to /Error instead of rendering half-working pages with empty lists (the
    /// previous behaviour swallowed the MySQL "Unknown database" error page by
    /// page).
    /// Escape hatches stay reachable: home / workspace creation (/), workspace
    /// selector (/Login/*), error page (/Error).
    /// </summary>
    public sealed class TenantDatabaseGateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantDatabaseGateMiddleware> _logger;

        public TenantDatabaseGateMiddleware(
            RequestDelegate next, ILogger<TenantDatabaseGateMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, TenantDatabaseChecker checker)
        {
            if (context.User.Identity?.IsAuthenticated == true
                && !IsAllowed(context.Request.Path))
            {
                var tenant = CurrentWorkspace.Of(context.User);
                if (!string.IsNullOrEmpty(tenant))
                {
                    var exists = await checker.DatabaseExistsAsync(
                        tenant, context.RequestAborted);
                    if (!exists)
                    {
                        _logger.LogWarning(
                            "TenantDatabaseGate: database del workspace {Tenant} assente o non raggiungibile, richiesta a {Path} bloccata",
                            tenant, context.Request.Path);
                        context.Response.Redirect("/Error?workspace="
                            + Uri.EscapeDataString(tenant));
                        return;
                    }
                }
            }

            await _next(context);
        }

        /// <summary>Paths that must keep working without a tenant database.</summary>
        internal static bool IsAllowed(PathString path)
        {
            var p = path.Value ?? "";
            if (p.Length == 0 || p == "/")
            {
                return true;
            }
            return p.StartsWith("/Login", StringComparison.OrdinalIgnoreCase)
                || p.StartsWith("/Error", StringComparison.OrdinalIgnoreCase);
        }
    }
}
