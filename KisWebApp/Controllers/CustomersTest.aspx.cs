using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using KIS.App_Code;

namespace KIS.Controllers
{
    public partial class CustomersTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptCustomerList.Visible = false;
                tblCustomerData.Visible = false;
            }
        }

        protected void btnCustomerList_Click(object sender, EventArgs e)
        {
            tblCustomerData.Visible = false;
            rptCustomerList.Visible = true;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
            CustomersControllerConfig cControllerCfg = (CustomersControllerConfig)System.Configuration.ConfigurationManager.GetSection("controllersConfig");
            client.BaseAddress = new Uri(cControllerCfg.BaseUrl + "customers");
            HttpResponseMessage response = null;
            String ret = "";
            Boolean recCusts = false;
            try
            {
                response = client.GetAsync("").Result;
                recCusts = true;
            }
            catch (Exception ex)
            {
                recCusts = false;
                ret = ex.Message;
            }

            if (response!=null && response.IsSuccessStatusCode && recCusts == true)
            {
                lbl1.Text = "Tutto ok...<br/>";
                var customerList = response.Content.ReadAsAsync<customerStruct[]>().Result;
                rptCustomerList.Visible = true;
                rptCustomerList.DataSource = customerList;
                rptCustomerList.DataBind();
            }
            else
            {
                // Errore
                lbl1.Text = "ERRORE: "+ret + " "
                    + recCusts.ToString() + " "
                    + response.StatusCode.ToString();
            }
        }

        public struct customerStruct
        {
            public String type { get; set; }
            public String codiceCliente { get; set; }
            public String ragioneSociale { get; set; }
            public String partitaIVA { get; set; }
            public String codiceFiscale { get; set; }
            public String indirizzo { get; set; }
            public String citta { get; set; }
            public String provincia { get; set; }
            public String cap { get; set; }
            public String stato { get; set; }
            public String telefono { get; set; }
            public String email { get; set; }
        }

        protected void btnGetCustomer_Click(object sender, EventArgs e)
        {
            String custID = Server.HtmlEncode(txtCustomerID.Text);
            tblCustomerData.Visible = true;
            rptCustomerList.Visible = false;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
            CustomersControllerConfig cControllerCfg = (CustomersControllerConfig)System.Configuration.ConfigurationManager.GetSection("controllersConfig");
            client.BaseAddress = new Uri(cControllerCfg.BaseUrl + "customers");
            String urlParameters = "?id="+custID;
            HttpResponseMessage response = null;
            String ret = "";
            Boolean recCusts = false;
            try
            {
                response = client.GetAsync(urlParameters).Result;
                recCusts = true;
            }
            catch (Exception ex)
            {
                recCusts = false;
                ret = ex.Message;
            }

            if (response != null && response.IsSuccessStatusCode && recCusts == true)
            {
                tblCustomerData.Visible = true;
                lbl1.Text = "Tutto ok...<br/>";
                var customer = response.Content.ReadAsAsync<customerStruct>().Result;
                lblcodiceCliente.Text = customer.codiceCliente;
                lblragioneSociale.Text = customer.ragioneSociale;
                lblpartitaiva.Text = customer.partitaIVA;
                lblcodiceFiscale.Text = customer.codiceFiscale;
                lblindirizzo.Text = customer.indirizzo;
                lblcitta.Text = customer.citta;
                lblprovincia.Text = customer.provincia;
                lblCAP.Text = customer.cap;
                lblstato.Text = customer.stato;
                lbltelefono.Text = customer.telefono;
                lblemail.Text = customer.email;
            }
            else
            {
                lbl1.Text = "ERRORE: " + response.StatusCode.ToString();
                tblCustomerData.Visible = false;
            }
        }

        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            customerStruct nCli = new customerStruct();
            nCli.codiceCliente = txtcodiceCliente.Text;
            nCli.ragioneSociale = txtragioneSociale.Text;
            nCli.partitaIVA = txtpartitaiva.Text;
            nCli.codiceFiscale = txtcodicefiscale.Text;
            nCli.indirizzo = txtindirizzo.Text;
            nCli.citta = txtcitta.Text;
            nCli.provincia = txtprovincia.Text;
            nCli.cap = txtcap.Text;
            nCli.stato = txtstato.Text;
            nCli.telefono = txttelefono.Text;
            nCli.email = txtemail.Text;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
            CustomersControllerConfig cControllerCfg = (CustomersControllerConfig)System.Configuration.ConfigurationManager.GetSection("controllersConfig");
            client.BaseAddress = new Uri(cControllerCfg.BaseUrl + "customers");

            var response = client.PostAsJsonAsync(client.BaseAddress, nCli);

            if (response != null && response.Result != null && response.Result.IsSuccessStatusCode)
            {
                lbl1.Text = "Nuovo cliente aggiunto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nell'aggiunta di un nuovo cliente. Verificare che CodiceCliente, RagioneSociale, PartitaIVA o CodiceFiscale non siano duplicati."
                    +"<br />" + response.Result.StatusCode.ToString();
            }
        }

        protected void btnModifyCustomer_Click(object sender, EventArgs e)
        {
            KIS.App_Code.Cliente customer = new App_Code.Cliente(Session["ActiveWorkspace_Name"].ToString(), txtcodiceCliente.Text);
            customerStruct nCli = new customerStruct();
            nCli.codiceCliente = customer.CodiceCliente;
            nCli.ragioneSociale = customer.RagioneSociale;
            nCli.partitaIVA = customer.PartitaIVA;
            nCli.codiceFiscale = customer.CodiceFiscale;
            nCli.indirizzo = txtindirizzo.Text;
            nCli.citta = txtcitta.Text;
            nCli.provincia = txtprovincia.Text;
            nCli.cap = txtcap.Text;
            nCli.stato = txtstato.Text;
            nCli.telefono = txttelefono.Text;
            nCli.email = txtemail.Text;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
            CustomersControllerConfig cControllerCfg = (CustomersControllerConfig)System.Configuration.ConfigurationManager.GetSection("controllersConfig");
            client.BaseAddress = new Uri(cControllerCfg.BaseUrl + "customers");

            var response = client.PutAsJsonAsync(client.BaseAddress + "?id=" + txtcodiceCliente.Text, nCli);
            

            if (response != null && response.Result != null && response.Result.IsSuccessStatusCode)
            {
                lbl1.Text = "Cliente modificato correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nella modifica del cliente. Verificare che il cliente esista e che CodiceCliente, RagioneSociale, PartitaIVA o CodiceFiscale non siano duplicati.<br />" + response.Result.StatusCode.ToString(); ;
            }
        }

        protected void btnDeleteCliente_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            CustomersControllerConfig cControllerCfg = (CustomersControllerConfig)System.Configuration.ConfigurationManager.GetSection("controllersConfig");
            client.BaseAddress = new Uri(cControllerCfg.BaseUrl + "customers");
            client.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
            var response = client.DeleteAsync(client.BaseAddress + "?id=" + txtCodClienteDelete.Text);

            if (response.Result.IsSuccessStatusCode)
            {
                lbl1.Text = "Customer successfully deleted.";
            }
            else
            {
                lbl1.Text = "Error while deleting customer. Be sure customer exists. " + response.Result.StatusCode.ToString();
            }
        }
    }
}