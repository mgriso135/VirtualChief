using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KIS.Clienti;
using KIS.Models;
using KIS.App_Code;

namespace KIS.Controllers
{
    public class CustomersController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage GetCustomers(String tenant)
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
                CustomersListModel elencoCli = new CustomersListModel(tenant);
                List<CustomerModel> ret = elencoCli.CustomerPortfolio;
                if (ret != null)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, ret);
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

        // GET api/<controller>/5
        [HttpGet]
        public HttpResponseMessage GetCustomer(String tenant, String id)
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
            CustomerModel cli = new CustomerModel(tenant, id);
            if (cli.CodiceCliente.Length > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cli);
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
        public HttpResponseMessage Post(String tenant, [FromBody]CustomerModelStruct value)
        {
            HttpResponseMessage response;
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
            KIS.App_Code.PortafoglioClienti portfolio = new KIS.App_Code.PortafoglioClienti(tenant);
            bool ret = portfolio.Add(
                value.CodiceCliente,
                value.RagioneSociale,
                value.PartitaIVA,
                value.CodiceFiscale,
                value.Indirizzo,
                value.Citta,
                value.Provincia,
                value.CAP,
                value.Stato,
                value.Telefono,
                value.Email,
                false,
                true,
                true);
            string uri;
            if (ret == true)
            {
                response = Request.CreateResponse(HttpStatusCode.Created, value);
                uri = Url.Link("DefaultApi", new { id = value.CodiceCliente });
                response.Headers.Location = new Uri(uri);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, portfolio.log);
            }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return response;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage PutModificaCliente(String tenant, String id, [FromBody]CustomerModelStruct value)
        {
            KIS.App_Code.Cliente cust = new KIS.App_Code.Cliente(tenant, id);
            HttpResponseMessage response;
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
            if (cust != null && cust.CodiceCliente.Length > 0)
            {
                cust.Indirizzo = value.Indirizzo;
                cust.Citta = value.Citta;
                cust.Provincia = value.Provincia;
                cust.CAP = value.CAP;
                cust.Stato = value.Stato;
                cust.Telefono = value.Telefono;
                cust.Email = value.Email;
                string uri;
                response = Request.CreateResponse(HttpStatusCode.Created, value);
                uri = Url.Link("DefaultApi", new { id = value.CodiceCliente });
                response.Headers.Location = new Uri(uri);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return response;
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(String tenant, String id)
        {
            KIS.App_Code.Cliente cust = new KIS.App_Code.Cliente(tenant, id);
            HttpResponseMessage response;
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
            if (cust != null && cust.CodiceCliente.Length > 0)
            {
                Boolean ret = cust.Delete();
                if (ret == true)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotModified);
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return response;
        }

    }
}