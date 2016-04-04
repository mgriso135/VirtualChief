﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class configKanban : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto ConfigurazioneKanban";
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
                if (!Page.IsPostBack && !Page.IsCallback && idReparto != -1)
                {
                    Reparto rp = new Reparto(idReparto);
                    if (rp.id != -1)
                    {
                        if (rp.KanbanManaged == true)
                        {
                            rb1.SelectedValue = "1";
                        }
                        else
                        {
                            rb1.SelectedValue = "0";
                        }
                    }
                }
            }
            else
            {
                rb1.Visible = false;
                lbl1.Text = "Non hai il permesso di gestire la configurazione del kanban dei reparti";
            }
        }

        protected void rb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reparto rp = new Reparto(idReparto);
            if (rb1.SelectedValue == "0")
            {
                rp.KanbanManaged = false;
            }
            else
            {
                rp.KanbanManaged = true;
            }
            lbl1.Text = "Impostazione aggiornata.";
        }

    }
}