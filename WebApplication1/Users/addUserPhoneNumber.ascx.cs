using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Users
{
    public partial class addUserPhoneNumber : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti PhoneNumbers";
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
                if (!Page.IsPostBack)
                {
                    tblAddPhoneNumber.Visible = false;
                }
            }
            else
            {
                tblAddPhoneNumber.Visible = false;
                imgShowForm.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgShowForm_Click(object sender, ImageClickEventArgs e)
        {
            if (tblAddPhoneNumber.Visible == false)
            {
                tblAddPhoneNumber.Visible = true;
            }
            else
            {
                tblAddPhoneNumber.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            User usr = new User(userID);
            if (usr.username != "")
            {
                bool rt = usr.addPhoneNumber(txtPhoneNumber.Text, Server.HtmlEncode(txtNote.Text), chkAlarm.Checked);
                if (rt == false)
                {
                    lbl1.Text = GetLocalResourceObject("lblErroreAdd").ToString();
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtPhoneNumber.Text = "";
            txtNote.Text = "";
            chkAlarm.Checked = false;
        }
    }
}