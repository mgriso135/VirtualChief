using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Personal
{
    public partial class myEmail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                imgShowFormAddEmail.Visible = true;
                if (!Page.IsPostBack)
                {
                    frmAddEmail.Visible = false;
                    User curr = (User)Session["user"];
                    curr.loadEmails();
                    rptListMail.DataSource = curr.Email;
                    rptListMail.DataBind();
                }
            }
            else
            {
                frmAddEmail.Visible = false;
                imgShowFormAddEmail.Visible = false;
            }
        }

        protected void imgShowFormAddEmail_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                if (frmAddEmail.Visible == false)
                {
                    frmAddEmail.Visible = true;
                }
                else
                {
                    frmAddEmail.Visible = false;
                }
            }
        }

        protected void imgSaveMail_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                if (curr.username != "")
                {
                    bool rt = curr.addEmail(txtEmail.Text, Server.HtmlEncode(txtAmbito.Text), chkForAlarm.Checked);
                    if (rt == false)
                    {
                        lbl1.Text = "Errore: non è stato possibile inserire l'indirizzo e-mail.<br/>Verificare che non sia già stato inserito e che sia un indirizzo e-mail valido.";
                    }
                    else
                    {
                        frmAddEmail.Visible = false;
                        curr.loadEmails();
                        rptListMail.DataSource = curr.Email;
                        rptListMail.DataBind();
                    }
                }
            }
        }

        protected void imgUndoMail_Click(object sender, ImageClickEventArgs e)
        {
            txtEmail.Text = "";
            txtAmbito.Text = "";
            chkForAlarm.Checked = false;
        }

        protected void rptListMail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgEdit = (ImageButton)e.Item.FindControl("btnEdit");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("btnDelete");
            ImageButton imgSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton imgUndo = (ImageButton)e.Item.FindControl("btnUndo");
            CheckBox chkAlarm = (CheckBox)e.Item.FindControl("chkAlarm");
            if(Session["user"]!=null)
            {
            User curr = (User)Session["user"];

            if (e.CommandName == "edit")
            {
                imgEdit.Visible = false;
                imgDelete.Visible = true;
                imgSave.Visible = true;
                imgUndo.Visible = true;
                chkAlarm.Enabled = true;
            }
            else if (e.CommandName == "undo")
            {
                
                imgEdit.Visible = true;
                imgDelete.Visible = false;
                imgSave.Visible = false;
                imgUndo.Visible = false;
                chkAlarm.Enabled = false;
                UserEmail usrMail = new UserEmail(curr.username, e.CommandArgument.ToString());
                if (usrMail != null && usrMail.UserID != "")
                {
                    chkAlarm.Checked = usrMail.ForAlarm;
                }
            }
            else if (e.CommandName == "save")
            {
                UserEmail usrMail = new UserEmail(curr.username, e.CommandArgument.ToString());
                if (usrMail != null && usrMail.UserID != "")
                {
                    usrMail.ForAlarm = chkAlarm.Checked;
                }
                imgEdit.Visible = true;
                imgDelete.Visible = false;
                imgSave.Visible = false;
                imgUndo.Visible = false;
                chkAlarm.Enabled = false;
            }
            else if (e.CommandName == "delete")
            {
                UserEmail usrMail = new UserEmail(curr.username, e.CommandArgument.ToString());
                if (usrMail != null && usrMail.UserID != "")
                {
                    usrMail.delete();
                }

                curr.loadEmails();
                rptListMail.DataSource = curr.Email;
                rptListMail.DataBind();
            }
            }
        }
    }
}