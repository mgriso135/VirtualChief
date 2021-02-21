using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Menu;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class menuAddMainVoce : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                        List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Menu Voce";
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
                tblFormAdd.Visible = false;
                imgShowFormAddMainMenu.Visible = false;
                imgShowFormAddMainMenu.Enabled = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgShowFormAddMainMenu_Click(object sender, ImageClickEventArgs e)
        {
            if (tblFormAdd.Visible == false)
            {
                tblFormAdd.Visible = true;
            }
            else
            {
                tblFormAdd.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            MainMenu mn = new MainMenu(Session["ActiveWorkspace"].ToString());
            bool rt = mn.Add(Server.HtmlEncode(txtTitolo.Text), Server.HtmlEncode(txtDesc.Text), Server.HtmlEncode(txtURL.Text));
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblAddError").ToString();
                //lbl1.Text = mn.log;
            }
        }

        protected void undo_Click(object sender, ImageClickEventArgs e)
        {
            txtDesc.Text = "";
            txtTitolo.Text = "";
            txtURL.Text = "";
        }
    }
}