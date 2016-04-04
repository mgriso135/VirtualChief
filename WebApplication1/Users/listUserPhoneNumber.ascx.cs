using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Users
{
    public partial class listUserPhoneNumber : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti PhoneNumbers";
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
                    curr.loadPhoneNumbers();
                    rptUserPhoneNumbers.DataSource = curr.PhoneNumbers;
                    rptUserPhoneNumbers.DataBind();
                }
            }
            else
            {
                rptUserPhoneNumbers.Visible = false;
                lbl1.Text = "Non hai il permesso di visualizzare i numeri telefonici degli utenti.";
            }
        }

        protected void rptUserPhoneNumbers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptUserPhoneNumbers_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                    UserPhoneNumber usrPhone = new UserPhoneNumber(userID, e.CommandArgument.ToString());
                    bool rt = usrPhone.delete();
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Errore." + usrPhone.log;
                    }
                }
                else if (e.CommandName == "edit")
                {
                    UserPhoneNumber usrPhone = new UserPhoneNumber(userID, e.CommandArgument.ToString());
                    txtNote.Text = usrPhone.Note;
                    chkAlarm.Checked = usrPhone.ForAlarm;
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
                    UserPhoneNumber usrPhone = new UserPhoneNumber(userID, e.CommandArgument.ToString());
                    usrPhone.Note = Server.HtmlEncode(txtNote.Text);
                    usrPhone.ForAlarm = chkAlarm.Checked;
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
                    UserPhoneNumber usrPhone = new UserPhoneNumber(userID, e.CommandArgument.ToString());
                    txtNote.Text = Server.HtmlDecode(usrPhone.Note);
                    chkAlarm.Checked = usrPhone.ForAlarm;
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