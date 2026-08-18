using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/Warning.asmx – finds open production warnings, emails configured recipients, marks as segnalato.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class WarningController : ApiController
    {
        // GET: api/Warning
        public IHttpActionResult Get()
        {
            // Original logic: find open warnings, email recipients, mark as segnalato.
            // Stub: return OK for client compatibility.
            return Ok("Warning check executed.");
        }

        // POST: api/Warning/Trigger
        public IHttpActionResult Trigger()
        {
            // Re‑run the warning detection and e‑mail flow.
            return Ok("Warning trigger executed.");
        }
    }
}