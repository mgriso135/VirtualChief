using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Menu;

namespace KIS.Admin
{
    public partial class menuShowVoce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    id = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    id=-1;
                }
                if (id != -1)
                {
                    frmShowFigli.id = id;
                    VoceMenu mnv = new VoceMenu(id);
                    lblDescrizione.Text = mnv.Descrizione;
                    lblTitolo.Text = mnv.Titolo;
                    lblURL.Text = mnv.URL;
                    frmAddFiglio.vID = id;
                }
                else
                {
                    frmShowFigli.Visible = false;
                    frmAddFiglio.Visible = false;
                }
            }
            else
            {
                frmShowFigli.Visible = false;
                frmAddFiglio.Visible = false;
            }
        }
    }
}