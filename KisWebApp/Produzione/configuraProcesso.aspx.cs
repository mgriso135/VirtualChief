using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Commesse;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class configuraProcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int artID = -1;
            int artYear = -1;
            int proc = -1;
            int var = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]) && !String.IsNullOrEmpty(Request.QueryString["Year"]))
            {
                try
                {
                    artID = Int32.Parse(Request.QueryString["ID"]);
                    artYear = Int32.Parse(Request.QueryString["Year"]);
                }
                catch
                {
                    artID = -1;
                }
            }
            if (artID != -1 && artYear!=-1)
            {
                Articolo art = new Articolo(artID, artYear);
                if (art.ID != -1 && art.Proc != null)
                {
                    /*if (art.Matricola != null)
                    {*/
                        proc = art.Proc.process.processID;
                        var = art.Proc.variant.idVariante;
                    /*}
                    else
                    {
                        lbl1.Text = "Errore: matricola non inserita.&nbsp;<a href='commesseDaProdurre.aspx'>Torna indietro</a>";
                    }*/
                }
            }
                if (proc == -1 && var == -1)
                {
                    frmConfigurazione.Visible = false;
                }
                else
                {
                    frmConfigurazione.Visible = true;
                    frmConfigurazione.artID = artID;
                    frmConfigurazione.artYear = artYear;
                }
        }
    }
}