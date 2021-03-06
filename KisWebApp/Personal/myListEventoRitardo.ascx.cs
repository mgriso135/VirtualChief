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
    public partial class myListEventoRitardo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    
                    /*rptReparti.DataSource = curr.SegnalazioneRitardiRepartoCompleto(Session["ActiveWorkspace_Name"].ToString());
                    rptReparti.DataBind();

                    rptCommesse.DataSource = curr.SegnalazioneRitardiCommessaCompleto;
                    rptCommesse.DataBind();

                    rptArticolo.DataSource = curr.SegnalazioneRitardiArticoloCompleto;
                    rptArticolo.DataBind();*/
                }
            }
        }
    }
}