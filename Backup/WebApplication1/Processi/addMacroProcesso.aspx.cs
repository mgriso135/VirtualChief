using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using Dati;

namespace WebApplication1.Processi
{
    public partial class addMacroProcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnAddMacroProc_Click(object sender, EventArgs e)
        {
            macroProcessi proc = new macroProcessi();
            bool vsmV = bool.Parse(vsm.SelectedValue);
            if(proc.Add(Server.HtmlEncode(ProcName.Text), Server.HtmlEncode(ProcDesc.Text), vsmV))
            {
                lbl1.Text = "Processo aggiunto correttamente.<br/>";
            }
            else
            {
                lbl1.Text = "OOOOOPS, si è verificato un errore.<br/>";
            }
        }
    }
}