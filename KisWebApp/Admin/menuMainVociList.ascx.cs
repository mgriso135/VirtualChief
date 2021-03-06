using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Menu;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class menuMainVociList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Menu Voce";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    MainMenu lst = new MainMenu(Session["ActiveWorkspace_Name"].ToString());
                    rptMainVoci.DataSource = lst.Elenco;
                    rptMainVoci.DataBind();
                }
            }
            else
            {
                rptMainVoci.Visible = false;
                lbl1.Text = "Non hai il permesso di visualizzare le voci di menu.<br/>";
            }
        }

        protected void rptMainVoci_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void rptMainVoci_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            TextBox txtTitolo = (TextBox)e.Item.FindControl("txtTitolo");
            TextBox txtDesc = (TextBox)e.Item.FindControl("txtDesc");
            TextBox txtURL = (TextBox)e.Item.FindControl("txtURL");
            Label lblTitolo = (Label)e.Item.FindControl("lblTitolo");
            Label lblDesc = (Label)e.Item.FindControl("lblDesc");
            Label lblURL = (Label)e.Item.FindControl("lblURL");
            ImageButton imgSave = (ImageButton)e.Item.FindControl("imgSave");
            ImageButton imgUndo = (ImageButton)e.Item.FindControl("imgUndo");

            int vID = -1;
            try
            {
                vID = Int32.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                vID = -1;
            }
            if (vID != -1)
            {
                if (e.CommandName == "delete")
                {
                    VoceMenu vm = new VoceMenu(vID);
                    bool rt = vm.Delete();
                    
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Errore: cancella i figli del menu oppure i gruppi cui è associato il menu<br/>";
                    }
                }
                else if (e.CommandName == "edit")
                {
                    

                    if (txtTitolo.Visible == false)
                    {
                        txtTitolo.Visible = true;
                        txtDesc.Visible = true;
                        txtURL.Visible = true;
                        lblDesc.Visible = false;
                        lblTitolo.Visible = false;
                        lblURL.Visible = false;
                        imgSave.Visible = true;
                        imgUndo.Visible = true;
                    }
                    else
                    {
                        txtTitolo.Visible = false;
                        txtDesc.Visible = false;
                        txtURL.Visible = false;
                        lblDesc.Visible = true;
                        lblTitolo.Visible = true;
                        lblURL.Visible = true;
                        imgSave.Visible = false;
                        imgUndo.Visible = false;
                    }
                }
                else if (e.CommandName == "save")
                {
                    VoceMenu vm = new VoceMenu(vID);
                    vm.Titolo = Server.HtmlEncode(txtTitolo.Text);
                    vm.Descrizione = Server.HtmlEncode(txtDesc.Text);
                    vm.URL = txtURL.Text;
                    Response.Redirect(Request.RawUrl);
                }
                else if (e.CommandName == "undo")
                {
                    VoceMenu vm = new VoceMenu(vID);
                    txtTitolo.Text = vm.Titolo;
                    txtDesc.Text = vm.Descrizione;
                    txtURL.Text = vm.URL;
                }
            }
            
        }
    }
}