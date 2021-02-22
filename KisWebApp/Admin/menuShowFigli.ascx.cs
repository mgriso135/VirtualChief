using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Menu;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class menuShowFigli : System.Web.UI.UserControl
    {
        public int id;
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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (id != -1 && !Page.IsPostBack)
                {
                    VoceMenu voce = new VoceMenu(Session["ActiveWorkspace"].ToString(), id);
                    voce.loadFigli();
                    rptFigli.DataSource = voce.VociFiglie;
                    rptFigli.DataBind();
                }
            }
            else
            {
                rptFigli.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptFigli_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                    VoceMenu vm = new VoceMenu(Session["ActiveWorkspace"].ToString(), vID);
                    bool rt = vm.Delete();
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblDeleteError").ToString();
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
                    VoceMenu vm = new VoceMenu(Session["ActiveWorkspace"].ToString(), vID);
                    vm.Titolo = Server.HtmlEncode(txtTitolo.Text);
                    vm.Descrizione = Server.HtmlEncode(txtDesc.Text);
                    vm.URL = txtURL.Text;
                    Response.Redirect(Request.RawUrl);
                }
                else if (e.CommandName == "undo")
                {
                    VoceMenu vm = new VoceMenu(Session["ActiveWorkspace"].ToString(), vID);
                    txtTitolo.Text = vm.Titolo;
                    txtDesc.Text = vm.Descrizione;
                    txtURL.Text = vm.URL;
                }
                else if (e.CommandName == "MoveUp")
                {
                    VoceMenu padre = new VoceMenu(Session["ActiveWorkspace"].ToString(), id);
                    VoceMenu figlio = new VoceMenu(Session["ActiveWorkspace"].ToString(), vID);
                    padre.SpostaVoceFiglia(figlio, true);
                    padre.loadFigli();
                    rptFigli.DataSource = padre.VociFiglie;
                    rptFigli.DataBind();
                }
                else if (e.CommandName == "MoveDown")
                {
                    VoceMenu padre = new VoceMenu(Session["ActiveWorkspace"].ToString(), id);
                    VoceMenu figlio = new VoceMenu(Session["ActiveWorkspace"].ToString(), vID);
                    padre.SpostaVoceFiglia(figlio, false);
                    padre.loadFigli();
                    rptFigli.DataSource = padre.VociFiglie;
                    rptFigli.DataBind();
                }
            }
        }

        protected void rptFigli_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
    }
}