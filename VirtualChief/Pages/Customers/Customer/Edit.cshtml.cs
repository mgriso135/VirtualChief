using KIS.App_Code;
using KIS.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace VirtualChief.Pages.Customers.Customer
{
    /// <summary>
    /// Migrated from WebForms EditCliente.aspx (KIS.Clienti.EditCliente):
    /// edit-customer host page reached as /Customers/Customer/Edit?idCliente=...
    /// Legacy behaviour kept: with an empty idCliente every form stays hidden;
    /// otherwise the page shows the base-info editor (Clienti/EditCliente.ascx,
    /// "Anagrafica Clienti" W) and the contacts panels (ContattiClienti.ascx +
    /// addContattoCliente.ascx, "Anagrafica Clienti Contatti" R/W), which are
    /// Blazor components here. User-action logging replicated like ListModel.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;

        public EditModel(ILogger<EditModel> logger)
        {
            _logger = logger;
        }

        /// <summary>Legacy QueryString["idCliente"].</summary>
        public string CustomerCode { get; private set; } = "";

        public bool CanWrite { get; private set; }

        public bool CanReadContacts { get; private set; }

        public string Tenant { get; private set; } = "";

        /// <summary>vcmain numeric user id (legacy Session["user"].id); 0 when unknown.</summary>
        public int Uid { get; private set; }

        public void OnGet(string idCliente)
        {
            // Legacy pages log through Dati.Utilities.LogAction.
            LogPageVisit();

            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            Uid = int.TryParse(User.FindFirst("uid")?.Value, out var uidParsed) ? uidParsed : 0;
            CustomerCode = idCliente ?? "";
            if (CustomerCode.Length == 0)
            {
                return; // legacy EditCliente.Page_Load hides all the forms
            }

            Tenant = tenant;
            CanWrite = HasPermission(tenant, "Anagrafica Clienti", "W");
            CanReadContacts = HasPermission(tenant, "Anagrafica Clienti Contatti", "R");
        }

        private void LogPageVisit()
        {
            try
            {
                var uidStr = User.FindFirst("uid")?.Value;
                var user = int.TryParse(uidStr, out var uid) ? uid.ToString() : User.Identity?.Name ?? "";
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                Dati.Utilities.LogAction(user, "Action", "/Customers/Customer/Edit", "", ip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/Edit: LogAction");
            }
        }

        private bool HasPermission(string tenant, string permission, string perm)
        {
            if (!int.TryParse(User.FindFirst("uid")?.Value, out var uid))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { permission, perm } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/Edit: verifica permesso {Perm} {Mode}", permission, perm);
                return false;
            }
        }
    }
}
