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
    public partial class editUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti";
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
                String usrID = Server.HtmlEncode(Request.QueryString["id"]);
                User usr = new User(usrID);
                if (usr.username.Length > 0)
                {
                    frmEditUserInfo.userID = usr.username;
                    frmManageUserGruppi.userID = usr.username;
                    frmEditUserEmails.userID = usr.username;
                    frmAddEmail.userID = usr.username;
                    frmListPhoneNumbers.userID = usr.username;
                    frmAddPhoneNumbers.userID = usr.username;
                }
                else
                {
                    frmAddEmail.Visible = false;
                    frmEditUserEmails.Visible = false;
                    frmEditUserInfo.Visible = false;
                    frmManageUserGruppi.Visible = false;
                    frmListPhoneNumbers.Visible = false;
                    frmAddPhoneNumbers.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmAddEmail.Visible = false;
                frmEditUserEmails.Visible = false;
                frmEditUserInfo.Visible = false;
                frmManageUserGruppi.Visible = false;
                frmListPhoneNumbers.Visible = false;
                frmAddPhoneNumbers.Visible = false;
            }
        }
    }
}