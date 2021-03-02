using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (pstID != -1)
                {
                    Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                    if (pst.id != -1)
                    {
                        if (!Page.IsPostBack)
                        {
                            postName.Text = Server.HtmlDecode(pst.name);
                            postDesc.Text = Server.HtmlDecode(pst.desc);
                            ddlAutoCheckIn.SelectedValue = pst.barcodeAutoCheckIn == true ? "1" : "0";
                        }
                    }
                    else
                    {
                        postDesc.Visible = false;
                        postName.Visible = false;
                        save.Visible = false;
                        reset.Visible = false;
                        ddlAutoCheckIn.Visible = false;
                    }
                }
                else
                {
                    postDesc.Visible = false;
                    postName.Visible = false;
                    save.Visible = false;
                    reset.Visible = false;
                    ddlAutoCheckIn.Visible = false;
                }
            }
            else
            {
                err.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                postDesc.Visible = false;
                postName.Visible = false;
                save.Visible = false;
                reset.Visible = false;
                ddlAutoCheckIn.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            if (pstID != -1)
            {
                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                if (pst.id != -1)
                {
                    pst.name = Server.HtmlEncode(postName.Text);
                    pst.desc = Server.HtmlEncode(postDesc.Text);
                    pst.barcodeAutoCheckIn = ddlAutoCheckIn.SelectedValue == "1" ? true : false;
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
                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                if (pst.id != -1)
                {
                    postName.Text = pst.name;
                    postDesc.Text = pst.desc;
                    ddlAutoCheckIn.SelectedValue = pst.barcodeAutoCheckIn == true ? "1" : "0";
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