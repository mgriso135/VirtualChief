using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Personal
{
    public partial class myPhone : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                imgShowFormAddPhone.Visible = true;
                if (!Page.IsPostBack)
                {
                    frmAddPhone.Visible = false;
                    User curr = (User)Session["user"];
                    curr.loadPhoneNumbers();
                    rptListPhone.DataSource = curr.PhoneNumbers;
                    rptListPhone.DataBind();
                }
            }
            else
            {
                frmAddPhone.Visible = false;
                imgShowFormAddPhone.Visible = false;
            }
        }

        protected void imgSavePhone_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                if (curr.username != "")
                {
                    bool rt = curr.addPhoneNumber(Server.HtmlEncode(txtPhone.Text), Server.HtmlEncode(txtAmbito.Text), chkForAlarm.Checked);
                    if (rt == false)
                    {
                        lbl1.Text =GetLocalResourceObject("lblErroreDuplicato").ToString();// "Errore: non è stato possibile inserire il numero di telefono.<br/>Verificare che non sia già stato inserito e che sia un numero di telefono valido.";
                    }
                    else
                    {
                        frmAddPhone.Visible = false;
                        curr.loadPhoneNumbers();
                        rptListPhone.DataSource = curr.PhoneNumbers;
                        rptListPhone.DataBind();
                    }
                }
            }
        }

        protected void imgUndoPhone_Click(object sender, ImageClickEventArgs e)
        {
            txtPhone.Text = "";
            txtAmbito.Text = "";
            chkForAlarm.Checked = false;
        }

        protected void imgShowFormAddPhone_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                if (frmAddPhone.Visible == false)
                {
                    frmAddPhone.Visible = true;
                }
                else
                {
                    frmAddPhone.Visible = false;
                }
            }
        }

        protected void rptListPhone_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgEdit = (ImageButton)e.Item.FindControl("btnEdit");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("btnDelete");
            ImageButton imgSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton imgUndo = (ImageButton)e.Item.FindControl("btnUndo");
            CheckBox chkAlarm = (CheckBox)e.Item.FindControl("chkAlarm");
            if (Session["user"] != null)
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
                    UserPhoneNumber usrPhone = new UserPhoneNumber(curr.username, e.CommandArgument.ToString());
                    if (usrPhone != null && usrPhone.UserID != "")
                    {
                        chkAlarm.Checked = usrPhone.ForAlarm;
                    }
                }
                else if (e.CommandName == "save")
                {
                    UserPhoneNumber usrPhone = new UserPhoneNumber(curr.username, e.CommandArgument.ToString());
                    if (usrPhone != null && usrPhone.UserID != "")
                    {
                        usrPhone.ForAlarm = chkAlarm.Checked;
                    }
                    imgEdit.Visible = true;
                    imgDelete.Visible = false;
                    imgSave.Visible = false;
                    imgUndo.Visible = false;
                    chkAlarm.Enabled = false;
                }
                else if (e.CommandName == "delete")
                {
                    UserPhoneNumber usrPhone = new UserPhoneNumber(curr.username, e.CommandArgument.ToString());
                    if (usrPhone != null && usrPhone.UserID != "")
                    {
                        usrPhone.delete();
                    }

                    curr.loadPhoneNumbers();
                    rptListPhone.DataSource = curr.PhoneNumbers;
                    rptListPhone.DataBind();
                }
            }
        }
    }
}