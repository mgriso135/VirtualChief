using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class listWarningAperti : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Warning";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {*/
                lbl1.Text = "";
            if (!Page.IsPostBack)
            {
                WarningAperti Elenco = new WarningAperti();
                if (Elenco.Elenco.Count == 0)
                {
                    rptWarnings.Visible = false;
                    lblTitle.Visible = false;
                    lblData.Visible = false;
                }
                else
                {
                    rptWarnings.Visible = true;
                    lblTitle.Visible = true;
                    lblData.Visible = true;
                    rptWarnings.DataSource = Elenco.Elenco;
                    rptWarnings.DataBind();
                }
            }
            /*}
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare gli warning aperti.<br/>";
                rptWarnings.Visible = false;
            }*/
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            WarningAperti Elenco = new WarningAperti();
            if (Elenco.Elenco.Count == 0)
            {
                rptWarnings.Visible = false;
                lblTitle.Visible = false;
                lblData.Visible = false;
            }
            else
            {
                lblData.Text = "Last update: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                rptWarnings.Visible = true;
                lblTitle.Visible = true;
                lblData.Visible = true;
                rptWarnings.DataSource = Elenco.Elenco;
                rptWarnings.DataBind();
            }
        }

        protected void rptWarnings_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lbl1.Text = e.CommandName + " " + e.CommandArgument.ToString();
        }
    }
}