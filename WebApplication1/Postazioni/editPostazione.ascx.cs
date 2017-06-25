using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
namespace KIS.Postazioni
{
    public partial class editPostazione_ascx : System.Web.UI.UserControl
    {
        public int pstID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione";
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
                if (pstID != -1)
                {
                    Postazione pst = new Postazione(pstID);
                    if (pst.id != -1)
                    {
                        if (!Page.IsPostBack)
                        {
                            postName.Text = pst.name;
                            postDesc.Text = pst.desc;
                        }
                    }
                    else
                    {
                        postDesc.Visible = false;
                        postName.Visible = false;
                        save.Visible = false;
                        reset.Visible = false;
                    }
                }
                else
                {
                    postDesc.Visible = false;
                    postName.Visible = false;
                    save.Visible = false;
                    reset.Visible = false;
                }
            }
            else
            {
                err.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                postDesc.Visible = false;
                postName.Visible = false;
                save.Visible = false;
                reset.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            if (pstID != -1)
            {
                Postazione pst = new Postazione(pstID);
                if (pst.id != -1)
                {
                    pst.name = Server.HtmlEncode(postName.Text);
                    pst.desc = Server.HtmlEncode(postDesc.Text);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script language=\"javascript\">window.opener.location.reload();window.close();</script>");
                }
                else
                {
                    err.Text = "Error!<br/>";
                }
            }
            else
            {
                err.Text = "Error!<br/>";
            }
        }

        protected void reset_Click(object sender, ImageClickEventArgs e)
        {
            if (pstID != -1)
            {
                Postazione pst = new Postazione(pstID);
                if (pst.id != -1)
                {
                    postName.Text = pst.name;
                    postDesc.Text = pst.desc;
                }
                else
                {
                    err.Text = "Error!<br/>";
                }
            }
            else
            {
                err.Text = "Error!<br/>";
            }
        }
    }
}