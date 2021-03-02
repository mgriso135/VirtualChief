using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Produzione
{
    public partial class configCadenza : System.Web.UI.UserControl
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
                    if (!Page.IsPostBack)
                    {
                        ore.Enabled = false;
                        minuti.Enabled = false;
                        secondi.Enabled = false;
                        save.Enabled = false;
                        save.Visible = false;
                        valButtons.Visible = false;
                        Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                        if (rep.id != -1)
                        {
                            if (rep.Cadenza != null)
                            {
                                ore.Text = rep.Cadenza.Hours.ToString();
                                minuti.Text = rep.Cadenza.Minutes.ToString();
                                secondi.Text = rep.Cadenza.Seconds.ToString();
                            }
                            else
                            {
                                ore.Text = "00";
                                minuti.Text = "00";
                                secondi.Text = "00";
                            }
                        }
                        else
                        {
                            ore.Enabled = false;
                            minuti.Enabled = false;
                            secondi.Enabled = false;
                            save.Enabled = false;
                            save.Visible = false;
                            valButtons.Visible = false;
                        }

                    }
                }
            }
            else
            {
                ore.Enabled = false;
                minuti.Enabled = false;
                secondi.Enabled = false;
                save.Enabled = false;
                save.Visible = false;
                valButtons.Visible = false;
                editCadenza.Visible = false;
                editCadenza.Enabled = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            if (repID != -1)
            {
                Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                rep.Cadenza = new TimeSpan(Int32.Parse(ore.Text), Int32.Parse(minuti.Text), Int32.Parse(secondi.Text));
                ore.Enabled = false;
                minuti.Enabled = false;
                secondi.Enabled = false;
                save.Enabled = false;
                save.Visible = false;
                valButtons.Visible = false;
            }
        }

        protected void reset_Click(object sender, ImageClickEventArgs e)
        {
            if (repID != -1)
            {
                Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                ore.Text = rep.Cadenza.Hours.ToString();
                minuti.Text = rep.Cadenza.Minutes.ToString();
                secondi.Text = rep.Cadenza.Seconds.ToString();
            }
        }

        protected void editCadenza_Click(object sender, ImageClickEventArgs e)
        {
            if (valButtons.Visible == false)
            {
                ore.Enabled = true;
                minuti.Enabled = true;
                secondi.Enabled = true;
                save.Enabled = true;
                save.Visible = true;
                valButtons.Visible = true;
            }
            else
            {
                ore.Enabled = false;
                minuti.Enabled = false;
                secondi.Enabled = false;
                save.Enabled = false;
                save.Visible = false;
                valButtons.Visible = false;
            }
        }
    }
}