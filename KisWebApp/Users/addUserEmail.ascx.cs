using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Users
{
    public partial class addUserEmail : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti E-mail";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    tblAddEmail.Visible = false;
                }
            }
            else
            {
                tblAddEmail.Visible = false;
                imgShowForm.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgShowForm_Click(object sender, ImageClickEventArgs e)
        {
            if (tblAddEmail.Visible == false)
            {
                tblAddEmail.Visible = true;
            }
            else
            {
                tblAddEmail.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            User usr = new User(userID);
            if (usr.username != "")
            {
                bool rt = usr.addEmail(txtEmail.Text, Server.HtmlEncode(txtNote.Text), chkAlarm.Checked);
                if (rt == false)
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorAdd").ToString();
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtEmail.Text = "";
            txtNote.Text = "";
            chkAlarm.Checked = false;
        }
    }
}