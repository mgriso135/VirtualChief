using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.Reparti
{
    public partial class configSplitTasksTurni : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto";
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
                if (idReparto != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Reparto rp = new Reparto(idReparto);
                        if (rp.splitTasks == true)
                        {
                            splitTasks.SelectedValue = "1";
                        }
                        else
                        {
                            splitTasks.SelectedValue = "0";
                        }
                    }
                }
                else
                {
                    splitTasks.Enabled = false;
                }
            }
            else
            {
                splitTasks.Enabled = false;
                lbl1.Text = "Non hai il permesso di gestire il reparto.<br />";
            }
        }

        protected void splitTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (idReparto != -1)
            {
                Reparto rp = new Reparto(idReparto);
                if (splitTasks.SelectedItem.Value == "0")
                {
                    rp.splitTasks = false;
                }
                else
                {
                    rp.splitTasks = true;
                }
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}