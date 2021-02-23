using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KIS.App_Code;

namespace KIS.Controllers
{
    public class ProductsController : ApiController
    {
        // GET api/<controller>
        /*public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        // GET api/<controller>/5
        /*public string Get(int id)
        {
            return "value";
        }
        */
        [HttpGet]
        public HttpResponseMessage FindNomeDuplicato(String tenant, String nomeVariante)
        {
            String cKey = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                cKey = arrCKey.FirstOrDefault();
            }
            catch
            {
                cKey = "";
            }
            CustomersControllerConfig cfg = new CustomersControllerConfig();
            String xKey = cfg.x_api_key;
            if (xKey == cKey && xKey.Length > 0)
            {
                elencoVarianti elVar = new elencoVarianti(tenant);
                bool result = elVar.elenco.Any(v => v.nomeVariante == nomeVariante);
                
                if(result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}