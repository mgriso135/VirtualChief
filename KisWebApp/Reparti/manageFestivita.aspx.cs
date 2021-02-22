using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class manageFestivita : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int trnID;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    trnID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    trnID = -1;
                }
            }
            else
            {
                trnID = -1;
            }

            if (trnID != -1)
            {
                Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), trnID);
                if (trn.id != -1)
                {
                    addFest.idTurno = trn.id;
                    listFest.idTurno = trn.id;
                }
                else
                {
                    addFest.idTurno = -1;
                    listFest.idTurno = -1;
                    addFest.Visible = false;
                    listFest.Visible = false;
                }
            }
        }
    }
}