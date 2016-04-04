﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;

namespace KIS.Analysis
{
    public partial class ListCommesseChiuse1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Commessa Costo";
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
                ElencoCommesse lstComm = new ElencoCommesse();
                var lstCommChiuse = (from comm in lstComm.Commesse
                                     where comm.Status == 'F'
                                     select comm).ToList();
                var lstCommChiuseOrd = lstCommChiuse.OrderByDescending(x => x.Year).ThenByDescending(y => y.ID);

                rptCommesseChiuse.DataSource = lstCommChiuseOrd;
                rptCommesseChiuse.DataBind();
            }
        }
    }
}