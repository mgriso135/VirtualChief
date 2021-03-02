using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                    UserAccount curr = (UserAccount)Session["user"];

                  /*  rptReparti.DataSource = curr.SegnalazioneWarningRepartoCompleto;
                    rptReparti.DataBind();

                    rptCommesse.DataSource = curr.SegnalazioneWarningCommessaCompleto;
                    rptCommesse.DataBind();

                    rptArticolo.DataSource = curr.SegnalazioneWarningArticoloCompleto;
                    rptArticolo.DataBind();*/
                }
            }
        }
    }
}