using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class PianoProduzioneModel : PageModel
    {
        private readonly ILogger<PianoProduzioneModel> _logger;

        public PianoProduzioneModel(ILogger<PianoProduzioneModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavPianoProduzione { get; private set; } = "Piano Produzione";
        public int RepId { get; private set; } = -1;
        public string RepName { get; private set; } = "";
        public bool HasReparto { get; private set; } = false;
        public string ErrorMessage { get; private set; } = "";

        public void OnGet(int? id)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            if (id.HasValue && id.Value > 0)
            {
                RepId = id.Value;
                try
                {
                    var rep = new Reparto(Tenant, RepId);
                    if (rep.id != -1)
                    {
                        RepName = rep.name;
                        HasReparto = true;
                    }
                    else
                    {
                        ErrorMessage = "Reparto non trovato.";
                        HasReparto = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "PianoProduzione/OnGet - Reparto load");
                    ErrorMessage = "Errore durante il caricamento del reparto.";
                    HasReparto = false;
                }
            }
            else
            {
                HasReparto = false;
            }
        }
    }
}