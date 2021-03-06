using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class addCommessa : System.Web.UI.UserControl
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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    frmAddCommessa.Visible = false;
                    KIS.App_Code.PortafoglioClienti elencoClienti = new App_Code.PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                    ddlCliente.DataSource = elencoClienti.Elenco;
                    ddlCliente.DataTextField = "RagioneSociale";
                    ddlCliente.DataValueField = "CodiceCliente";
                    ddlCliente.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmAddCommessa.Visible = false;
                btnShowFrmAddCommessa.Visible = false;
            }
        }

        /*protected void btnShowFrmAddCommessa_Click(object sender, ImageClickEventArgs e)
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
        */
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            ElencoCommesse el = new ElencoCommesse(Session["ActiveWorkspace_Name"].ToString());
            String client = Server.HtmlEncode(ddlCliente.SelectedValue);
            String note = Server.HtmlEncode(txtNote.Text);
            String externalID = Server.HtmlEncode(txtExternalID.Text);
            int rt = el.Add(client, note, externalID);
            if (rt !=-1)
            {
                Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), rt, DateTime.UtcNow.Year);
                if (cm != null && cm.ID != -1 && cm.Year > 2000)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    cm.Confirmed = true;
                    cm.ConfirmedBy = curr;
                    cm.ConfirmationDate = DateTime.UtcNow;
                    Response.Redirect("linkArticoliToCommessa.aspx?id=" + rt.ToString() + "&anno=" + DateTime.UtcNow.Year.ToString());
                }
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