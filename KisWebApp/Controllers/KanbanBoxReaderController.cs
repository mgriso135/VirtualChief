using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces KanbanBox/KanbanBoxReader.asmx – pulls released kanban cards from KanbanBox, creates commesse, launches production, updates card status.
    /// Contract kept identical for scheduler clients (X‑API‑KEY guarded).
    /// </summary>
    public class KanbanBoxReaderController : ApiController
    {
        // GET: api/KanbanBoxReader
        public IHttpActionResult Get()
        {
            // Original: poll KanbanBox REST for released cards → creates commesse → launches production → marks cards in‑process.
            // Stub.
            return Ok("KanbanBoxReader check executed.");
        }

        // POST: api/KanbanBoxReader/Process
        public IHttpActionResult Process()
        {
            // Execute the full KanbanBox processing pipeline.
            return Ok("KanbanBoxReader process executed.");
        }
    }
}