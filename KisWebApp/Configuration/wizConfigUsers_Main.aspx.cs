using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Configuration
{
    public partial class wizConfigUsers_Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmAddUser.Visible = false;
            frmListUser.Visible = false;
            String permessoRichiesto = "Utenti";
            bool checkUser = false;
            if (Session["User"] != null)
            {
                User curr = (User)Session["user"];
                curr.loadGruppi();
                for (int i = 0; i < curr.Gruppi.Count; i++)
                {
                    for (int j = 0; j < curr.Gruppi[i].Permessi.Elenco.Count; j++)
                    {
                        if (curr.Gruppi[i].Permessi.Elenco[j].NomePermesso == permessoRichiesto && curr.Gruppi[i].Permessi.Elenco[j].W == true)
                        {
                            checkUser = true;
                        }
                    }
                }
            }

            if (checkUser == true)
            {
                frmAddUser.Visible = true;
                frmListUser.Visible = true;
            }
            else
            {
                lbl1.Text = "<a href=\"../Login/login.aspx"
                    + "?red=/Configuration/wizConfigUsers_Main.aspx\">"
                    + GetLocalResourceObject("lblLnkLoginAdmin").ToString()
                    +".</a>";
                frmAddUser.Visible = false;
                frmListUser.Visible = false;
            }
            }
    }
}