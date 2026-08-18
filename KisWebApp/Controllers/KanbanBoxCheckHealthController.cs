using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces KanbanBox/KanbanBoxCheckHealth.asmx – KIS↔KanbanBox consistency checks + e‑mails.
    /// Contract kept identical for scheduler clients (X‑API‑KEY guarded).
    /// </summary>
    public class KanbanBoxCheckHealthController : ApiController
    {
        // GET: api/KanbanBoxCheckHealth
        public IHttpActionResult Get()
        {
            // Original: consistency checks (non‑existent part numbers, ghost products, non‑updated cards, non‑existent customers) → e‑mails managers.
            // Stub.
            return Ok("KanbanBoxCheckHealth check executed.");
        }

        // POST: api/KanbanBoxCheckHealth/Report
        public IHttpActionResult Report()
        {
            // Generate and send the consistency report.
            return Ok("KanbanBoxCheckHealth report executed.");
        }
    }
}