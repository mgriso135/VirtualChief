using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Users
{
    public partial class listUserGruppi : System.Web.UI.UserControl
    {
        public String userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                User usr = new User(userID);
                if (usr.username.Length > 0)
                {
                    lblNome.Text = GetLocalResourceObject("lblTitle1").ToString() + " " + usr.name + " " + usr.cognome;
                    GroupList grpList = new GroupList(Session["ActiveWorkspace"].ToString());
                    rptGruppi.DataSource = grpList.Elenco;
                    rptGruppi.DataBind();
                }
                else
                {
                    lblNome.Visible = false;
                    rptGruppi.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblErrore").ToString();
                }
            }
        }
        protected void rptGruppi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField grpID = (HiddenField)e.Item.FindControl("grp");
                CheckBox ck = (CheckBox)e.Item.FindControl("ck");
                //lbl1.Text += "<br/>" + grpID.Value.ToString() + " " + userID.ToString();
                int gruppo = -1;
                try
                {
                    gruppo = Int32.Parse(grpID.Value);
                }
                catch
                {
                    gruppo = -1;
                    lbl1.Text = GetLocalResourceObject("lblErrore").ToString();
                }

                if (gruppo != -1)
                {
                    User curr = new User(userID);
                    bool found = false;
                    curr.loadGruppi();
                    for (int i = 0; i < curr.Gruppi.Count; i++)
                    {
                        if (curr.Gruppi[i].ID == gruppo)
                        {
                            found = true;
                        }
                    }
                    if (found == true)
                    {
                        ck.Checked = true;
                    }
                    else
                    {
                        ck.Checked = false;
                    }
                }
            }
            
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
                {/*
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void ck_CheckedChanged(object sender, EventArgs e)
        {
            User curr = new User(userID);
            if (curr.username.Length > 0)
            {
                CheckBox ck = (CheckBox)sender;
                HiddenField grp = (HiddenField)ck.Parent.FindControl("grp");
                int grpID = -1;
                try
                {
                    grpID = Int32.Parse(grp.Value);
                }
                catch
                {
                    grpID = -1;
                }
                if (grpID != -1)
                {
                    Group g = new Group(Session["ActiveWorkspace"].ToString(), grpID);
                    
                    if (g.ID != -1)
                    {
                        bool rt;
                        if (ck.Checked == true)
                        {
                            rt = curr.addGruppo(g);
                        }
                        else
                        {
                            rt = curr.deleteGruppo(g);
                        }

                        if (rt == true)
                        {
//                            lbl1.Text = "Gruppo aggiunto correttamente<br/>";
                        }
                        else
                        {
                            lbl1.Text = g.log;
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrore").ToString();
            }
        }
    }
}