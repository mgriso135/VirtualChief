using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
            if (Session["User"] != null && Session["ActiveWorkspace"]!=null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Workspace ws = new Workspace(Session["ActiveWorkspace"].ToString());
                if(ws.id!=-1)
                { 
                    curr.loadGroups(ws.id);
                    for (int i = 0; i < curr.groups.Count; i++)
                    {
                        for (int j = 0; j < curr.groups[i].Permissions.Elenco.Count; j++)
                        {
                            if (curr.groups[i].Permissions.Elenco[j].NomePermesso == permessoRichiesto && curr.groups[i].Permissions.Elenco[j].W == true)
                            {
                                checkUser = true;
                            }
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