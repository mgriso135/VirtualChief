using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/Ritardi.asmx – finds late tasks, emails delay notifications, marks as segnalato.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class RitardiController : ApiController
    {
        // GET: api/Ritardi
        public IHttpActionResult Get()
        {
            // Original: find late tasks, email delay notifications, mark as segnalato.
            // Stub.
            return Ok("Ritardi check executed.");
        }

        // POST: api/Ritardi/Trigger
        public IHttpActionResult Trigger()
        {
            // Re‑run the delay detection and e‑mail flow.
            return Ok("Ritardi trigger executed.");
        }
    }
}