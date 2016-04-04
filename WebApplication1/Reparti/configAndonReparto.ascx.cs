﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class configAndonReparto : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Andon VisualizzazioneNomiUtente";
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    Reparto rp = new Reparto(idReparto);
                    if (rp.id != -1)
                    {
                        rbList.SelectedValue = rp.AndonPostazioniFormatoUsername.ToString();
                    }
                    else
                    {
                        rbList.Visible = false;
                        lbl1.Text = "Errore: reparto non trovato.<br />";
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai i permessi necessari per modificare la configurazione dell'andon di reparto.<br />";
                rbList.Visible = false;
            }
        }

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl1.Text = "";
            char valore = rbList.SelectedValue[0];
            if (idReparto != -1)
            {
                Reparto rp = new Reparto(idReparto);
                if (rp.id != -1)
                {
                    rp.AndonPostazioniFormatoUsername = valore;
                }
            }
        }
    }
}