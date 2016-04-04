using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;

namespace KIS.Produzione
{
    public partial class StatoTasksArticolo : System.Web.UI.UserControl
    {
        public int artID, artYear;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (artID != -1 && artYear != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Articolo art = new Articolo(artID, artYear);
                        // if (!Page.IsCallback)
                        {
                            art.loadTasksProduzione();
                            rptTasks.DataSource = art.Tasks;
                            rptTasks.DataBind();
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare lo stato di avanzamento dell'articolo.<br />";
                rptTasks.Visible = false;
            }
        }

        protected void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField hTaskID = (HiddenField)e.Item.FindControl("hTaskID");
                Label postazione = (Label)e.Item.FindControl("lblPostazione");
                int taskID = -1;
                try
                {
                    taskID = Int32.Parse(hTaskID.Value.ToString());
                }
                catch
                {
                    taskID = -1;
                }

                TaskProduzione tsk = new TaskProduzione(taskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    Postazione pst = new Postazione(tsk.PostazioneID);
                    postazione.Text = pst.name;
                }
                else
                {
                    postazione.Text = "#ERROR!";
                }

                // COLORE!!!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    if (tsk.Status == 'F')
                    {
                        tRow.BgColor = "#00FF00";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                    }
                    else if (tsk.Status == 'N')
                    {
                        tRow.BgColor = "#FFFFFF";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                    }
                    else
                    {
                        tRow.BgColor = "#0000FF";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#0000FF'");
                    }
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            Articolo art = new Articolo(artID, artYear);
            lblDataUpdate.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            art.loadTasksProduzione();
            rptTasks.DataSource = art.Tasks;
            rptTasks.DataBind();
        }
    
    }
}