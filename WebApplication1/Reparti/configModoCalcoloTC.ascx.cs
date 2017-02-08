using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class configModoCalcoloTC : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto ModoCalcoloTC";
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
                    Reparto rp = new Reparto(idReparto);
                    if (rp.id != -1)
                    {
                        if (!Page.IsPostBack && !Page.IsCallback)
                        {
                            if (rp.ModoCalcoloTC == true)
                            {
                                rbList.SelectedValue = "1";
                            }
                            else
                            {
                                rbList.SelectedValue = "0";
                            }
                        }
                    }
                    else
                    {
                        lbl1.Text = "Reparto non trovato.";
                        rbList.Visible = false;
                    }
                }
                else
                {
                    lbl1.Text = "Errore.";
                    rbList.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di modificare il modo di calcolo del tempo ciclo.<br />";
                rbList.Visible = false;
            }
        }

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl1.Text = "";
                bool val;
                if (rbList.SelectedValue == "1")
                {
                    val = true;
                }
                else
                {
                    val = false;
                }
                Reparto rp = new Reparto(idReparto);
                if (rp.id != -1)
                {
                    rp.ModoCalcoloTC = val;
                    lbl1.Text += rp.log;
                    if (rp.ModoCalcoloTC == val)
                    {
                        lbl1.Text = "Modifiche salvate correttamente.";
                    }
                    else
                    {
                        lbl1.Text = "E' avvenuto un errore nel cambio di opzione.";
                    }
                }
                else
                {
                    lbl1.Text = "Reparto non trovato.";
                }
            
        }
    }
}