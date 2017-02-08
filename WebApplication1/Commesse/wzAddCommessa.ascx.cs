using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzAddCommessa1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Commesse";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    KIS.App_Code.PortafoglioClienti elencoClienti = new App_Code.PortafoglioClienti();
                    ddlCliente.DataSource = elencoClienti.Elenco;
                    ddlCliente.DataTextField = "RagioneSociale";
                    ddlCliente.DataValueField = "CodiceCliente";
                    ddlCliente.DataBind();
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di aggiungere commesse.<br/>";
                frmAddCommessa.Visible = false;
            }
        }

        protected void btnShowFrmAddCommessa_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddCommessa.Visible == true)
            {
                frmAddCommessa.Visible = false;
            }
            else
            {
                frmAddCommessa.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            ElencoCommesse el = new ElencoCommesse();
            String client = Server.HtmlEncode(ddlCliente.SelectedValue);
            String note = Server.HtmlEncode(txtNote.Text);
            int rt = el.Add(client, note);
            if (rt != -1)
            {
                Response.Redirect("wzAddPERT.aspx?idCommessa=" + rt.ToString() + "&annoCommessa=" + DateTime.Now.Year.ToString());
            }
            else
            {
                lbl1.Text = el.log;
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            ddlCliente.SelectedValue = "";
            txtNote.Text = "";
        }
    }
}