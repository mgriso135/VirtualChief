using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class risorseTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idTurno;
            frmRisorsePostazioni.Visible = false;
            lnkReparto.Visible = false;
            lnkRisorseTurno.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["idTurno"]))
            {
                try
                {
                    idTurno = Int32.Parse(Request.QueryString["idTurno"]);
                }
                catch
                {
                    idTurno = -1;
                }
            }
            else
            {
                idTurno = -1;
            }

            if (idTurno != -1)
            {
                Turno turno = new Turno(idTurno);
                Reparto rp = new Reparto(turno.idReparto);
                if (turno.id != -1 && rp.id != -1)
                {
                    frmRisorsePostazioni.Visible = true;
                    frmRisorsePostazioni.idTurno = turno.id;
                    lnkRisorseTurno.Visible = true;
                    lnkReparto.Visible = true;
                    lnkReparto.NavigateUrl = "~/Reparti/configReparto.aspx?id=" + rp.id.ToString();
                    lnkRisorseTurno.NavigateUrl = "~/Reparti/risorseTurno.aspx?idTurno=" + turno.id.ToString();
                    if (!Page.IsPostBack)
                    {
                    }
                }
            }
        }
    }
}