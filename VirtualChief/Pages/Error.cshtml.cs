using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Global error page: target of UseExceptionHandler ("/Error") in
    /// production and of the TenantDatabaseGateMiddleware redirect for
    /// workspaces whose database does not exist or is unreachable.
    /// Touches no database, so it always renders.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public string Title { get; private set; } = "Errore";
        public string Message { get; private set; } =
            "Si \u00e8 verificato un errore durante l'elaborazione della richiesta.";

        /// <summary>True when the active workspace has no usable database.</summary>
        public bool WorkspaceUnavailable { get; private set; }

        public void OnGet(string workspace)
        {
            if (!string.IsNullOrEmpty(workspace))
            {
                // Redirected by TenantDatabaseGate: no page of this workspace
                // can work until its database exists again.
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                Title = "Workspace non disponibile";
                Message = $"Il database del workspace '{workspace}' non esiste o non "
                    + "\u00e8 raggiungibile. Nessuna pagina di questo workspace pu\u00f2 funzionare: "
                    + "chiedi all'amministratore di crearlo oppure passa a un altro workspace.";
                WorkspaceUnavailable = true;
                return;
            }

            if (HttpContext.Features.Get<IExceptionHandlerFeature>() != null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Title = "Errore inatteso";
                Message = "Si \u00e8 verificato un errore inatteso. Riprova pi\u00f9 tardi.";
            }
        }
    }
}
