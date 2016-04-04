using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.HtmlControls;

namespace WebApplication1.Admin
{
    public partial class managePermessi : System.Web.UI.Page
    {
        public static String userID;
        public static String prID;
        public static int contProc;
        public static macroProcessi el;
        public static processo father;

        protected void Page_Load(object sender, EventArgs e)
        {
                if (Session["user"] != null)
                {
                    if (((User)(Session["user"])).authenticated == true)
                    {
                        if (((User)(Session["user"])).typeOfUser == "Admin")
                        {
                            lnkLogin.Visible = false;
                            userID = Request.QueryString["userID"];
                            User current = new User(userID);
                            nomeUtente.Text = current.username;
                            if (current.username.Length > 0)
                            {
                                contProc = 0;
                                if (!Page.IsPostBack)
                                {
                                    nomeUtente.Text = current.username;
                                    contProc = 0;
                                    String fatherID = Request.QueryString["father"];
                                    if(fatherID != null)
                                    {
                                        father = new processo(Int32.Parse(fatherID));
                                        rptPermessiProcessi.DataSource = father.subProcessi;
                                        rptPermessiProcessi.DataBind();
                                        if (father.processoPadre != -1)
                                        {
                                            lnkGoTop.Visible = true;
                                            lnkGoTop.HRef += "?userID=" + current.username + "&father=" + father.processoPadre.ToString();
                                        }
                                        else
                                        {
                                            lnkGoTop.Visible = true;
                                            lnkGoTop.HRef += "?userID=" + current.username;
                                        }
                                    }
                                    else
                                    {
                                        el = new macroProcessi();
                                        rptPermessiProcessi.DataSource = el.elenco;
                                        rptPermessiProcessi.DataBind();
                                        lnkGoTop.Visible = false;
                                    }
                                    
                                }
                            }
                            else
                            {
                                lbl1.Text = "Errore: utente inesistente.<br/>";
                                lnkLogin.Visible = false;
                                lnkGoTop.Visible = false;
                            }
                        }
                        else
                        {
                            lnkLogin.Visible = false;
                            lnkGoTop.Visible = false;
                            lbl1.Text = "Non sei un amministratore quindi non puoi vedere questa pagina.<br/>";
                        }
                    }
                    else
                    {
                        lnkLogin.Visible = true;
                        lnkLogin.Visible = false;
                        lnkGoTop.Visible = false;
                    }
                }
                else
                {
                    lnkLogin.Visible = true;
                    lnkGoTop.Visible = false;
                }
        }

        protected void imgSavePermessi_Click(object sender, EventArgs e)
        {
            DropDownList MyList = (DropDownList)sender;
            //lbl1.Text = "SELECTED " + MyList.SelectedItem + " " + MyList.SelectedValue[0] + " FROM " + MyList.ID;
            HtmlTableRow prcID = (HtmlTableRow)(MyList.Parent.Parent);
            HiddenField lb = (HiddenField)(prcID.FindControl("procID"));
            //lbl1.Text += " PROCESSO " + lb.Value;
            Permesso perm = new Permesso(userID, new processo(Int32.Parse(lb.Value)));
            perm.idPermesso = MyList.SelectedValue[0];
        }

        protected void rptPermessiProcessi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                User utente = new User(userID);
                int indexPerDef;
                String fatherID = Request.QueryString["father"];
                if(fatherID != null)
                {
                    
                    ((HiddenField)e.Item.FindControl("procID")).Value = father.subProcessi[contProc].processID.ToString();
                    indexPerDef = utente.findPermessoIndex(father.subProcessi[contProc].processID);
                    ((HyperLink)e.Item.FindControl("lnkSons")).NavigateUrl = "/Admin/managePermessi.aspx?userID=" + userID + "&father=" + father.subProcessi[contProc].processID.ToString();
                }
                else
                {
                    ((HiddenField)e.Item.FindControl("procID")).Value = el.elenco[contProc].processID.ToString();
                    indexPerDef = utente.findPermessoIndex(el.elenco[contProc].processID);
                    ((HyperLink)e.Item.FindControl("lnkSons")).NavigateUrl = "/Admin/managePermessi.aspx?userID=" + userID + "&father=" + el.elenco[contProc].processID.ToString();
                }
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlPermessi");
                
                String defValue = utente.permessiProcessi[indexPerDef].idPermesso.ToString();
                ddl.SelectedValue = defValue;

                ((HyperLink)e.Item.FindControl("lnkSons")).ImageUrl = "/img/iconExpand.gif";
                

                contProc++;
            }


            //Colorazione righe!!!
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo verde!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }

        }
    }
}