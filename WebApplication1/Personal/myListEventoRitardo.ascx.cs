using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
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
                    User curr = (User)Session["user"];
                    
                    rptReparti.DataSource = curr.SegnalazioneRitardiRepartoCompleto;
                    rptReparti.DataBind();

                    rptCommesse.DataSource = curr.SegnalazioneRitardiCommessaCompleto;
                    rptCommesse.DataBind();

                    rptArticolo.DataSource = curr.SegnalazioneRitardiArticoloCompleto;
                    rptArticolo.DataBind();
                }
            }
        }
    }
}