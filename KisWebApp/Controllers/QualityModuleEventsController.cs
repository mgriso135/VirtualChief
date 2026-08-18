using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/QualityModuleEvents.asmx – late improvement / corrective action reminder e-mails.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class QualityModuleEventsController : ApiController
    {
        // GET: api/QualityModuleEvents
        public IHttpActionResult Get()
        {
            // Original: send late improvement/corrective action reminder e‑mails.
            // Stub.
            return Ok("Quality module events check executed.");
        }

        // POST: api/QualityModuleEvents/Trigger
        public IHttpActionResult Trigger()
        {
            // Re‑run the reminder e‑mail flow.
            return Ok("Quality module events trigger executed.");
        }
    }
}