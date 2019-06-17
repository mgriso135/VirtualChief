using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Users
{
    public partial class listUserEmails : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti E-mail";
            prmUser[1] = "R";
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
                    User curr = new User(userID);
                    curr.loadEmails();
                    rptUserEmails.DataSource = curr.Email;
                    rptUserEmails.DataBind();
                }
            }
            else
            {
                rptUserEmails.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptUserEmails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptUserEmails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (userID != "")
            {
                TextBox txtNote = (TextBox)e.Item.FindControl("txtNote");
                CheckBox chkAlarm = (CheckBox)e.Item.FindControl("chkAlarm");
                Label lblNote = (Label)e.Item.FindControl("lblNote");
                ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
                ImageButton btnUndo = (ImageButton)e.Item.FindControl("btnUndo");
                ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");

                if (e.CommandName == "delete")
                {
                    UserEmail usrMail = new UserEmail(userID, e.CommandArgument.ToString());
                    bool rt = usrMail.delete();
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Error. " + usrMail.log;
                    }
                }
                else if (e.CommandName == "edit")
                {
                    UserEmail usrMail = new UserEmail(userID, e.CommandArgument.ToString());
                    txtNote.Text = usrMail.Note;
                    chkAlarm.Checked = usrMail.ForAlarm;
                    if (txtNote.Visible == false)
                    {
                        txtNote.Visible = true;
                        chkAlarm.Enabled = true;
                        lblNote.Visible = false;
                        btnSave.Visible = true;
                        btnUndo.Visible = true;
                        btnEdit.Visible = false;
                    }
                    else
                    {
                        txtNote.Visible = false;
                        chkAlarm.Enabled = false;
                        lblNote.Visible = true;
                        btnSave.Visible = false;
                        btnEdit.Visible = true;
                        btnUndo.Visible = false;
                    }
                }
                else if (e.CommandName == "save")
                {
                    UserEmail usrMail = new UserEmail(userID, e.CommandArgument.ToString());
                    usrMail.Note = Server.HtmlEncode(txtNote.Text);
                    usrMail.ForAlarm = chkAlarm.Checked;
                    lblNote.Text = Server.HtmlEncode(txtNote.Text);
                    txtNote.Visible = false;
                    chkAlarm.Enabled = false;
                    lblNote.Visible = true;
                    
                    btnEdit.Visible = true;
                    btnSave.Visible = false;
                    btnUndo.Visible = false;
                }
                else if (e.CommandName == "undo")
                {
                    UserEmail usrMail = new UserEmail(userID, e.CommandArgument.ToString());
                    txtNote.Text = Server.HtmlDecode(usrMail.Note);
                    chkAlarm.Checked = usrMail.ForAlarm;
                    txtNote.Visible = false;
                    chkAlarm.Enabled = false;
                    lblNote.Visible = true;
                    btnEdit.Visible = true;
                    btnSave.Visible = false;
                    btnUndo.Visible = false;
                }
            }
        }
    }
}