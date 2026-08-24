using KIS.App_Code;
using KIS.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace VirtualChief.Pages.Customers.Customer
{
    /// <summary>
    /// Migrated from WebForms EditContattoDetails.aspx (KIS.Clienti.EditContattoDetails):
    /// detail editor of a customer contact, reached as
    /// /Customers/Customer/ContactDetail?idContatto=... The editor itself is the
    /// ContactDetail Blazor component ("Anagrafica Clienti Contatti" W). The page
    /// loads the contact once to build the breadcrumb link back to the customer,
    /// exactly like legacy lnkModCustomer.NavigateUrl.
    /// </summary>
    public class ContactDetailModel : PageModel
    {
        private readonly ILogger<ContactDetailModel> _logger;

        public ContactDetailModel(ILogger<ContactDetailModel> logger)
        {
            _logger = logger;
        }

        /// <summary>Legacy QueryString["idContatto"].</summary>
        public int ContactId { get; private set; } = -1;

        /// <summary>Customer code owning the contact, for the breadcrumb link.</summary>
        public string CustomerCode { get; private set; } = "";

        public bool CanWrite { get; private set; }

        public string Tenant { get; private set; } = "";

        /// <summary>vcmain numeric user id (legacy Session["user"].id); 0 when unknown.</summary>
        public int Uid { get; private set; }

        public void OnGet(string idContatto)
        {
            LogPageVisit();

            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            Uid = int.TryParse(User.FindFirst("uid")?.Value, out var uidParsed) ? uidParsed : 0;
            if (!int.TryParse(idContatto, out var id) || id <= 0)
            {
                return; // invalid/missing id: view shows "Contatto non trovato"
            }
            ContactId = id;
            Tenant = tenant;
            CanWrite = HasPermission(tenant, "Anagrafica Clienti Contatti", "W");

            try
            {
                // Legacy EditContattoDetails.aspx.cs: lnkModCustomer.NavigateUrl += "?idCliente=" + contCln.Cliente
                var contCln = new Contatto(tenant, ContactId);
                CustomerCode = contCln.ID != -1 ? contCln.Cliente ?? "" : "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/ContactDetail: caricamento contatto {Id}", ContactId);
                CustomerCode = "";
            }
        }

        private void LogPageVisit()
        {
            try
            {
                var uidStr = User.FindFirst("uid")?.Value;
                var user = int.TryParse(uidStr, out var uid) ? uid.ToString() : User.Identity?.Name ?? "";
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                Dati.Utilities.LogAction(user, "Action", "/Customers/Customer/ContactDetail", "", ip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Customers/Customer/ContactDetail: LogAction");
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
                _logger.LogError(ex, "Customers/Customer/ContactDetail: verifica permesso {Perm} {Mode}", permission, perm);
                return false;
            }
        }
    }
}
