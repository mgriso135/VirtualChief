using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Configuration
{
    public partial class wizConfigAdminUser1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            tblInputNewUser.Visible = false;
            KISConfig cfg = new KISConfig();
            if (!cfg.WizAdminUserCompleted)
            {
                tblInputNewUser.Visible = true;
            }
            else
            {
                lbl1.Text = "Admin già presente. Se si è dimenticato lo username o la password, utilizzare l'apposita interfaccia.";
                tblInputNewUser.Visible = false;
            }
        }

         protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            User utn = new User();
            int rt = utn.add(Server.HtmlEncode(inputUsername.Text), inputPassword.Text, Server.HtmlEncode(inputNome.Text), Server.HtmlEncode(inputCognome.Text), "Admin");
            if (rt == 2)
            {
                lblEsito.Text = "Error: username già presente<br/>" + utn.log;
            }
            else if (rt == 0)
            {
                lblEsito.Text = "Error: generic<br/>" + utn.log;
            }
            else
            {
                lblEsito.Text = "Utente aggiunto correttamente";
                GroupList grpList = new GroupList();
                int adminGroupID = -1;
                for (int i = 0; i < grpList.Elenco.Count; i++)
                {
                    if (grpList.Elenco[i].Nome == "Admin")
                    {
                        adminGroupID = grpList.Elenco[i].ID;
                    }
                }

                if (adminGroupID != -1)
                {
                    Group adminGroup = new Group(adminGroupID);
                    User adm = new User(Server.HtmlEncode(inputUsername.Text));
                    if (adminGroup.ID != -1 && adm.username.Length>0)
                    {
                        Boolean addRT = adm.addGruppo(adminGroup);
                        if (addRT)
                        {
                            Response.Redirect("~/Configuration/MainWizConfig.aspx");
                        }
                        else
                        {
                            lblEsito.Text = "FATAL ERROR: cannot link user to Admin group. Contact our technical service.";
                        }
                    }
                    else
                    {
                        lblEsito.Text = "FATAL ERROR: Admin group or Admin user not found.";
                    }
                }
                else
                {
                    lblEsito.Text = "FATAL ERROR: il gruppo Admin non esiste!";
                }
                //Response.Redirect(Request.RawUrl);
            }

        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {

        }
    }
}