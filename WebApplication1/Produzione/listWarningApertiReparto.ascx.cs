using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Produzione
{
    public partial class listWarningApertiReparto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
/*            List<String[]> elencoPermessi = new List<String[]>();
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
                bool check = false;
                int repID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        repID = Int32.Parse(Request.QueryString["id"]);
                        check = true;
                    }
                    catch
                    {
                        repID = -1;
                        check = false;
                    }
                }
                if (repID != -1 && check == true)
                {
                    Reparto rp = new Reparto(repID);
                    if (!Page.IsPostBack)
                    {
                        WarningAperti Elenco = new WarningAperti(rp);
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
                bool check = false;
                int repID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        repID = Int32.Parse(Request.QueryString["id"]);
                        check = true;
                    }
                    catch
                    {
                        repID = -1;
                        check = false;
                    }
                }
                if (repID != -1 && check == true)
                {
                    WarningAperti Elenco = new WarningAperti(new Reparto(repID));
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
            /*}
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare gli warning aperti.<br/>";
                rptWarnings.Visible = false;
            }*/
            
        }

        protected void rptWarnings_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lbl1.Text = e.CommandName + " " + e.CommandArgument.ToString();
        }
    }
}