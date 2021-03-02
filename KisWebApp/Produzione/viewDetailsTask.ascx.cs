using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Produzione
{
    public partial class viewDetailsTask : System.Web.UI.UserControl
    {
        public int taskID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ckUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ckUser == true)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    lblNomeTask.InnerText = tsk.Name;
                    tsk.loadEventi();
                    rptDettagli.DataSource = tsk.Eventi;
                    rptDettagli.DataBind();
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptDettagli_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                int evtID = -1;
                HiddenField evt = (HiddenField)e.Item.FindControl("idEvt");
                Label lblNomeUser = (Label)e.Item.FindControl("lblNomeUser");
                try
                {
                    evtID = Int32.Parse(evt.Value.ToString());
                }
                catch
                {
                    evtID = -1;
                }

                if (evtID != -1)
                {
                    EventoTaskProduzione ev = new EventoTaskProduzione(Session["ActiveWorkspace"].ToString(), evtID);
                    if (ev.IdEvento != -1)
                    {
                        User usr = new User(ev.User);
                        lblNomeUser.Text = "(" + usr.name + " " + usr.cognome + ")";
                    }
                }
            }
        }
    }
}