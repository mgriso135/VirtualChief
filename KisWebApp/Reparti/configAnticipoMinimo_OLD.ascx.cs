using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class configAnticipoMinimo : System.Web.UI.UserControl
    {
        public int repID;
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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (repID != -1)
                {
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                    if (rp.id != -1)
                    {
                        if (!Page.IsPostBack)
                        {
                            for (int i = 0; i < 60; i++)
                            {
                                minuti.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                secondi.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            }
                            ore.Text = rp.anticipoMinimoTasks.Hours.ToString();
                            minuti.SelectedValue = rp.anticipoMinimoTasks.Minutes.ToString();
                            secondi.SelectedValue = rp.anticipoMinimoTasks.Seconds.ToString();
                            ore.Enabled = false;
                            minuti.Enabled = false;
                            secondi.Enabled = false;
                            save.Visible = false;
                            undo.Visible = false;
                        }
                    }

                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire l'anticipo per i task non appartenenti al percorso critico.<br/>";
                ore.Enabled = false;
                minuti.Enabled = false;
                secondi.Enabled = false;
                save.Visible = false;
                undo.Visible = false;
            }
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (ore.Enabled == false && minuti.Enabled == false && secondi.Enabled == false)
            {
                ore.Enabled = true;
                minuti.Enabled = true;
                secondi.Enabled = true;
                save.Visible = true;
                undo.Visible = true;
            }
            else
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                ore.Text = rp.anticipoMinimoTasks.Hours.ToString();
                minuti.SelectedValue = rp.anticipoMinimoTasks.Minutes.ToString();
                secondi.SelectedValue = rp.anticipoMinimoTasks.Seconds.ToString();
                ore.Enabled = false;
                minuti.Enabled = false;
                secondi.Enabled = false;
                save.Visible = false;
                undo.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            if (repID != -1)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                rp.anticipoMinimoTasks = new TimeSpan(Int32.Parse(ore.Text), Int32.Parse(minuti.SelectedValue), Int32.Parse(secondi.SelectedValue));
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void undo_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
            ore.Text = rp.anticipoMinimoTasks.Hours.ToString();
            minuti.SelectedValue = rp.anticipoMinimoTasks.Minutes.ToString();
            secondi.SelectedValue = rp.anticipoMinimoTasks.Seconds.ToString();
        }
    }
}