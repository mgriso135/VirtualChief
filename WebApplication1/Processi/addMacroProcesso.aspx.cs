using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using Dati;
using KIS;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class addMacroProcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
            }
            else
            {
                frmAddProc.Visible = false;
                lbl1.Text = "Errore: non hai il permesso di aggiungere macroprocessi.<br/>";
            }
        }

        public void btnAddMacroProc_Click(object sender, EventArgs e)
        {
            macroProcessi proc = new macroProcessi();
            bool vsmV = bool.Parse(vsm.SelectedValue);
            if(proc.Add(Server.HtmlEncode(ProcName.Text), Server.HtmlEncode(ProcDesc.Text), vsmV))
            {
                lbl1.Text = "Processo aggiunto correttamente. <a href=\"/Processi/MacroProcessi.aspx\">Torna all'elenco dei processi</a><br/>";
                ProcName.Enabled = false;
                ProcDesc.Enabled = false;
                vsm.Enabled = false;
                Response.Redirect("/Processi/MacroProcessi.aspx");
            }
            else
            {
                lbl1.Text = "OOOOOPS, si è verificato un errore.<br/>";
            }
        }
    }
}