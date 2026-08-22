using KIS.App_Code;
using KIS.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VirtualChief.Components.Customer;

namespace VirtualChief.Pages.Customers.Customer
{
    /// <summary>
    /// Migrated from MVC5 area Customers/Customer/List (CustomerController.List +
    /// Views/Customer/List.cshtml): customer master-data register.
    /// The code-behind keeps the legacy behaviour: user-action logging
    /// (Dati.Utilities.LogAction), permission checks "Anagrafica Clienti" R/W via
    /// UserAccount.ValidatePermissions, tenant from the active-workspace claim and
    /// data loading through PortafoglioClienti (App_Sources). Rendering of the
    /// grid is delegated to the Blazor component CustomerList.
    /// </summary>
    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> _logger;

        public ListModel(ILogger<ListModel> logger)
        {
            _logger = logger;
        }

        public IReadOnlyList<CustomerRow> Customers { get; private set; } =
            Array.Empty<CustomerRow>();

        /// <summary>R or W permission on "Anagrafica Clienti" (legacy ViewBag.authR || ViewBag.authW).</summary>
        public bool Authorized { get; private set; }

        public bool CanWrite { get; private set; }

        public string Tenant { get; private set; } = "";

        /// <summary>vcmain numeric user id (legacy Session["user"].id); 0 when unknown.</summary>
        public int Uid { get; private set; }

        public void OnGet()
        {
            // Legacy controller: Dati.Utilities.LogAction(..., "/Customers/Customer/List", "", ip)
            LogPageVisit();

            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            Uid = int.TryParse(User.FindFirst("uid")?.Value, out var uidParsed) ? uidParsed : 0;
            CanWrite = HasPermission(tenant, "W");
            var canRead = HasPermission(tenant, "R");
            Authorized = canRead || CanWrite;
            if (!Authorized)
            {
                return; // view shows lblUserNotAuthorized, like the legacy page
            }

            Tenant = tenant;
            try
            {
                Customers = new PortafoglioClienti(tenant).Elenco
                    .Select(c => new CustomerRow(
                        c.CodiceCliente ?? "",
                        c.RagioneSociale ?? "",
                        c.PartitaIVA ?? "",
                        c.CodiceFiscale ?? "",
                        c.Citta ?? "",
                        c.Provincia ?? "",
                        c.Stato ?? "",
                        c.Telefono ?? "",
                        c.Email ?? ""))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/List: caricamento elenco clienti");
                Customers = Array.Empty<CustomerRow>();
            }
        }

        private void LogPageVisit()
        {
            try
            {
                var uidStr = User.FindFirst("uid")?.Value;
                var user = int.TryParse(uidStr, out var uid) ? uid.ToString() : User.Identity?.Name ?? "";
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                Dati.Utilities.LogAction(user, "Action", "/Customers/Customer/List", "", ip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/List: LogAction");
            }
        }

        private bool HasPermission(string tenant, string perm)
        {
            var uidStr = User.FindFirst("uid")?.Value;
            if (!int.TryParse(uidStr, out var uid))
            {
                return false;
            }
            try
            {
                // Legacy: prmUser[0] = "Anagrafica Clienti"; prmUser[1] = "R"/"W"
                var prm = new List<string[]> { new[] { "Anagrafica Clienti", perm } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/List: verifica permesso {Perm}", perm);
                return false;
            }
        }
    }
}
