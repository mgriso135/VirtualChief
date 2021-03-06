using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;


namespace KIS.Processi
{
    public partial class listTempiCiclo : System.Web.UI.UserControl
    {
        public TaskVariante prc;
        protected void Page_Load(object sender, EventArgs e)
        {
            int tskID = -1;
            int vrID = -1;
            if (prc != null && prc.Task != null && prc.variant != null)
            {
                tskID = prc.Task.processID;
                vrID = prc.variant.idVariante;
            }
            else
            {
                try
                {
                    tskID = Int32.Parse(taskID.Value);
                    vrID = Int32.Parse(taskID.Value);
                }
                catch
                {
                    tskID = -1;
                    vrID = -1;
                }
            }
            lbl1.Text = "";
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            String[] prmUser1 = new String[2];
            prmUser1[0] = "Processo TempiCiclo";
            prmUser1[1] = "W";
            elencoPermessi.Add(prmUser1);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            
            if (checkUser == true)
            {
                    
                
                    
                if(tskID!=-1 && vrID!=-1)
                {
                    TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), tskID), new variante(Session["ActiveWorkspace_Name"].ToString(), vrID));
                    taskID.Value = tsk.Task.processID.ToString();
                    varID.Value = tsk.variant.idVariante.ToString();
                    tsk.loadTempiCiclo();
                    rptTempi.DataSource = tsk.Tempi.Tempi;
                    rptTempi.DataBind();
                }
                else
                {
                    lbl1.Text = "Some kind of error<br/>";
                }
                
            }
            else
            {
                lbl1.Text = "<br/>Non hai il permesso di visualizzare e rimuovere i tempi ciclo.<br/>";
                rptTempi.Visible = false;
            }
        }

        protected void rptTempi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    num_ops = -1;
                }
                if (num_ops != -1 && prc != null && prc.Task != null && prc.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), prc.Task.processID, prc.Task.revisione, prc.variant.idVariante, num_ops);
                    bool rt = tc.Delete();
                    lbl1.Text = rt.ToString();
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = tc.log;
                    }
                }
            }

        }
    }
}