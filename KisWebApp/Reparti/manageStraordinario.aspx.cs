using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class manageStraordinario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idTurno;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    idTurno = Int32.Parse(Request.QueryString["id"]);
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
                Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), trn.idReparto);
                if (trn.id != -1)
                {
                    lbl1.Text ="<h3>"+ rp.name + " - " + trn.Nome + "</h3>";
                    addStraord.idTurno = trn.id;
                    listStraord.idTurno = trn.id;
                }
                else
                {
                    addStraord.idTurno = -1;
                    listStraord.idTurno = -1;
                    addStraord.Visible = false;
                    listStraord.Visible = false;
                }
            }
            else
            {
                addStraord.idTurno = -1;
                listStraord.idTurno = -1;
                addStraord.Visible = false;
                listStraord.Visible = false;
            }
        }
    }
}