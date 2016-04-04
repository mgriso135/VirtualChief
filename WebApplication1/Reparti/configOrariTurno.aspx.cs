﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class configOrariTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int tID=-1;
                try
                {
                    tID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    tID = -1;
                    orari.Visible = false;
                }
                if (tID != -1)
                {
                    Turno trn = new Turno(tID);
                    if (trn.id != -1)
                    {
                        orari.Visible = true;
                        orari.idTurno = trn.id;
                    }
                }
            }
            else
            {
                orari.Visible = false;
            }
        }
    }
}