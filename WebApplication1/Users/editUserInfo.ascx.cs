using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Users
{
    public partial class editUserInfo : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                User usr = new User(userID);
                if (usr.username.Length > 0)
                {
                    lblUsername.Text = usr.username;
                    tbNome.Text = usr.name;
                    tbCognome.Text = usr.cognome;
                }
                else
                {
                    lblUsername.Visible = false;
                    tbEdit.Visible = false;
                }
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            User usr = new User(userID);
            if (usr.username.Length > 0)
            {
                usr.name = Server.HtmlEncode(tbNome.Text);
                lbl1.Text = usr.log;
                usr.cognome = Server.HtmlEncode(tbCognome.Text);
                lbl1.Text += "<br />" + usr.log;
                lblRes.Text = "<br />Modifiche eseguite";
            }
            else
            {
                lbl1.Text = "Error.<br/>";
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            User usr = new User(userID);
            if (usr.username.Length > 0)
            {
                tbNome.Text = usr.name;
                tbCognome.Text = usr.cognome;
            }
            else
            {
            }
        }
    }
}