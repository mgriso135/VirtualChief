using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Processi
{
    public partial class pertManagePrecedenze2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int varID;
            int revTaskID;
            int procID;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["variante"]))
            {
                try
                {
                    procID = Int32.Parse(Request.QueryString["id"]);
                    varID = Int32.Parse(Request.QueryString["variante"]);
                    revTaskID = Int32.Parse(Request.QueryString["revTaskID"]);
                }
                catch
                {
                    procID = -1;
                    varID = -1;
                    revTaskID = 0;
                }
                if (procID != -1 && varID != -1)
                {
                    precedenze.varID = varID;
                    precedenze.taskID = procID;
                    processo proc = new processo(procID, revTaskID);
                    variante var = new variante(varID);
                    if (proc.processID != -1 && var.idVariante != -1)
                    {
                        lblTitle.Text = proc.processName + " - " + var.nomeVariante;
                    }
                }
                else
                {
                    precedenze.Visible = false;
                    lblTitle.Visible = false;
                    lblErr.Text = "Querystring non valide<br/>";
                }
            }
            else
            {
                precedenze.Visible = false;
                lblErr.Text = "Querystring non valide<br/>";
                lblTitle.Visible = false;
            }
        }
    }
}