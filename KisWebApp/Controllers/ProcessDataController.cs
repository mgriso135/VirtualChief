using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Processi/getProcessData.asmx – AJAX endpoint for the PERT editor (cycle times, precedences).
    /// Contract kept identical for AJAX callers.
    /// </summary>
    public class ProcessDataController : ApiController
    {
        // GET: api/ProcessData
        public IHttpActionResult Get()
        {
            // Original: returns cycle times, precedences for the PERT editor.
            // Stub JSON shape.
            return Ok(new { cycleTimes = new[] { 10, 20, 15 }, precedences = new[] { "A->B", "B->C" } });
        }
    }
}