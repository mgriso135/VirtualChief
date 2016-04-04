using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Personal
{
    public partial class myListEventoWarning : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack)
                {
                    User curr = (User)Session["user"];

                    rptReparti.DataSource = curr.SegnalazioneWarningRepartoCompleto;
                    rptReparti.DataBind();

                    rptCommesse.DataSource = curr.SegnalazioneWarningCommessaCompleto;
                    rptCommesse.DataBind();

                    rptArticolo.DataSource = curr.SegnalazioneWarningArticoloCompleto;
                    rptArticolo.DataBind();
                }
            }
        }
    }
}