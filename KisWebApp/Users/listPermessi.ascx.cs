using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Users
{
    public partial class listPermessi : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Permessi";
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    ElencoPermessi el = new ElencoPermessi();
                    rptPermessi.DataSource = el.Elenco;
                    rptPermessi.DataBind();
                }
            }
            else
            {
                rptPermessi.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        public void rptListPermessi_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void rptPermessi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                TextBox tbNome = (TextBox)e.Item.FindControl("txtNomeP");
                Label lbNome = (Label)e.Item.FindControl("lblNomeP");
                TextBox tbDesc = (TextBox)e.Item.FindControl("txtDescP");
                Label lbDesc = (Label)e.Item.FindControl("lblDescP");
                ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
                ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDel");

                if (tbNome.Visible == false)
                {
                    lbNome.Visible = false;
                    tbNome.Visible = true;
                    tbDesc.Visible = true;
                    lbDesc.Visible = false;
                    btnSave.Visible = true;
                    btnDel.Visible = true;
                }
                else
                {
                    lbNome.Visible = true;
                    tbNome.Visible = false;
                    tbDesc.Visible = false;
                    lbDesc.Visible = true;
                    btnSave.Visible = false;
                    btnDel.Visible = false;
                }
            }
            else if(e.CommandName=="save")
            {
                TextBox tbNome = (TextBox)e.Item.FindControl("txtNomeP");
                TextBox tbDesc = (TextBox)e.Item.FindControl("txtDescP");
                String nomeP = Server.HtmlEncode(tbNome.Text);
                String descP = Server.HtmlEncode(tbDesc.Text);
                if (nomeP.Length > 0 && descP.Length > 0)
                {
                    int idPerm = -1;
                    try
                    {
                        idPerm = Int32.Parse(e.CommandArgument.ToString());
                    }
                    catch
                    {
                        idPerm = -1;
                    }
                    if (idPerm != -1)
                    {
                        Permesso prm = new Permesso(idPerm);
                        if (prm.ID != -1)
                        {
                            prm.Nome = nomeP;
                            prm.Descrizione = descP;
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblPermNotFound").ToString();
                        }

                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblPermNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrNotNull").ToString();
                }
                
            }
            else if (e.CommandName == "delete")
            {
                int idPerm = -1;
                try
                {
                    idPerm = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    idPerm = -1;
                }
                if (idPerm != -1)
                {
                    Permesso prm = new Permesso(idPerm);
                    if (prm.ID != -1)
                    {
                        bool rt = prm.Delete();
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = prm.log + "<br/>";
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblPermNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblPermNotFound").ToString();
                }
            }
        }
    }
}