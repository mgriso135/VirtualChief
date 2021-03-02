using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Personal
{
    public partial class myLanguage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlLanguages.Visible = false;
            imgSave.Visible = false;
            imgUndo.Visible = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                if(curr.id!=-1)
                {
                    ddlLanguages.Visible = true;
                    imgSave.Visible = true;
                    imgUndo.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        if (curr.Language.Length > 0)
                        {
                            ddlLanguages.SelectedValue = curr.Language;
                        }
                        else
                        {
                        }
                    }
                }
            }
            else
            {
            }
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                if (curr.id!=-1)
                {
                    curr.Language = ddlLanguages.SelectedValue.ToString();
                    ddlLanguages.Visible = true;
                    imgSave.Visible = true;
                    imgUndo.Visible = true;
                    lbl1.Text = GetLocalResourceObject("lbl1_ChangeLanguageOK").ToString();
                }
            }
        }

        protected void imgUndo_Click(object sender, ImageClickEventArgs e)
        {
            ddlLanguages.Visible = false;
            imgSave.Visible = false;
            imgUndo.Visible = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                if (curr.id!=-1)
                {
                    ddlLanguages.Visible = true;
                    imgSave.Visible = true;
                    imgUndo.Visible = true;
                    ddlLanguages.SelectedValue = curr.Language;
                }
            }
        }
    }
}