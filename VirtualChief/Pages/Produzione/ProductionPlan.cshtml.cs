using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class ProductionPlanModel : PageModel
    {
        private readonly ILogger<ProductionPlanModel> _logger;

        public ProductionPlanModel(ILogger<ProductionPlanModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavProductionPlan { get; private set; } = "Piano di Produzione";
        public bool HasPermission { get; private set; } = false;
        public int RepId { get; private set; }
        public string RepartoName { get; private set; } = "";
        public string ErrorMessage { get; private set; } = "";
        public bool ShowAddForm { get; private set; } = false;
        public string AddNewItemLabel { get; private set; } = "Aggiungi nuovo elemento";
        public string AddFormNotImplementedMessage { get; private set; } = "Form per aggiungere elementi non ancora implementato";

        public void OnGet(int id, bool? toggleForm)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            RepId = id;

            if (toggleForm == true)
            {
                ShowAddForm = true;
            }

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Reparto", "R" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (HasPermission && RepId != 0)
                    {
                        var reparto = new Reparto(Tenant, RepId);
                        if (reparto.id != -1)
                        {
                            RepartoName = reparto.name;
                        }
                        else
                        {
                            HasPermission = false;
                            ErrorMessage = "Reparto non trovato";
                        }
                    }
                    else
                    {
                        HasPermission = false;
                        ErrorMessage = "Reparto non specificato";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductionPlan/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}