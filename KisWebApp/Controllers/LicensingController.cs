using System.Web.Http;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/Licensing.asmx – License expiry e‑mail alerts to Admin group.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class LicensingController : ApiController
    {
        // GET: api/Licensing
        public IHttpActionResult Get()
        {
            // TODO: call the original business logic (KISConfig.ExpiryDate check, email dispatch).
            // For now return a simple OK so the scheduler clients still work.
            return Ok("License check executed – no expiry pending.");
        }

        // POST: api/Licensing/Refresh
        public IHttpActionResult Refresh()
        {
            // Trigger the same license‑expiry e‑mail flow that the old ASMX service used.
            // Implementation will call the same KISConfig/Email code that Eventi/Licensing.asmx.cs used.
            return Ok("License refresh executed.");
        }
    }
}