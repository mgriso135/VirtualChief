using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Users
{
    public partial class addPermesso : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Permessi";
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
                    tblFormAddPermesso.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                tblFormAddPermesso.Visible = false;
                showAddPermesso.Visible = false;
            }
        }

        protected void showAddPermesso_Click(object sender, ImageClickEventArgs e)
        {
            if (tblFormAddPermesso.Visible == true)
            {
                tblFormAddPermesso.Visible = false;
            }
            else
            {
                tblFormAddPermesso.Visible = true;
            }
        }

        protected void imgAddPermesso_Click(object sender, ImageClickEventArgs e)
        {
            ElencoPermessi el = new ElencoPermessi(Session["ActiveWorkspace"].ToString());
            String nome = Server.HtmlEncode(nomeP.Text);
            String dsc = Server.HtmlEncode(descP.Text);
            bool rt = el.Add(nome, dsc);
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                err.Text = el.log;
            }
        }

        protected void imgReset_Click(object sender, ImageClickEventArgs e)
        {
            nomeP.Text = "";
            descP.Text = "";
        }
    }
}