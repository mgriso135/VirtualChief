using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.Commesse;
using System.Web.UI.HtmlControls;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class listCommesseStatoN : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
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
                    ElencoArticoli elArtAperti = new ElencoArticoli('N');
                    if (elArtAperti.ListArticoli.Count > 0)
                    {
                        rptStatoN.DataSource = elArtAperti.ListArticoli;
                        rptStatoN.DataBind();
                    }
                    else
                    {
                        rptStatoN.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblNothingToPlan").ToString();
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptStatoN.Visible = false;
            }
        }

        protected void rptStatoN_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DropDownList ddlOra = (DropDownList)e.Item.FindControl("ddlOra");
                DropDownList ddlMinuto = (DropDownList)e.Item.FindControl("ddlMinuto");
                DropDownList ddlSecondo = (DropDownList)e.Item.FindControl("ddlSecondo");

                for (int i = 0; i < 24; i++)
                {
                    ddlOra.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                for (int i = 0; i < 60; i++)
                {
                    ddlMinuto.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlSecondo.Items.Add(new ListItem(i.ToString(), i.ToString()));
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
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptStatoN_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgSave = (ImageButton)e.Item.FindControl("btnSaveSN");
            ImageButton imgAssegnaSN = (ImageButton)e.Item.FindControl("lnkAssegnaSN");
            ImageButton imgUndoSN = (ImageButton)e.Item.FindControl("btnUndo");
            TextBox tbMatricola = (TextBox)e.Item.FindControl("txtMatricola");
            Label lblMatricola = (Label)e.Item.FindControl("lblMatricola");
            ImageButton showChangeFP = (ImageButton)e.Item.FindControl("btnChangeDataFP");
            Calendar changeFP = (Calendar)e.Item.FindControl("cal");
            DropDownList ddlOra = (DropDownList)e.Item.FindControl("ddlOra");
            DropDownList ddlMinuto = (DropDownList)e.Item.FindControl("ddlMinuto");
            DropDownList ddlSecondo = (DropDownList)e.Item.FindControl("ddlSecondo");
            Label lblHH = (Label)e.Item.FindControl("lblHH");
            Label lblMM = (Label)e.Item.FindControl("lblMM");
            Label lblSS = (Label)e.Item.FindControl("lblSS");
            ImageButton saveFP = (ImageButton)e.Item.FindControl("saveFP");
            ImageButton undoFP = (ImageButton)e.Item.FindControl("undoFP");
            int artID = -1;
            int artAnno = -1;
            try
            {
                String[] arr = new String[2];
                arr = e.CommandArgument.ToString().Split('/');
                artID = Int32.Parse(arr[0]);
                artAnno = Int32.Parse(arr[1]);
            }
            catch
            {
                artID = -1;
                artAnno = -1;
            }
            if (artID != -1)
            {
                if (e.CommandName == "AssegnaMatricola")
                {
                    imgUndoSN.Visible = true;
                    lblMatricola.Visible = false;
                    tbMatricola.Visible = true;
                    imgSave.Visible = true;
                    imgAssegnaSN.Visible = false;
                }
                else if (e.CommandName == "saveSN")
                {
                    if(tbMatricola.Text.Length > 0)
                    {
                        Articolo art = new Articolo(artID, artAnno);
                        art.Matricola = Server.HtmlEncode(tbMatricola.Text);
                        lbl1.Text = art.log;
                        lblMatricola.Text = Server.HtmlEncode(tbMatricola.Text);
                    }
                    imgUndoSN.Visible = false;
                    lblMatricola.Visible = true;
                    tbMatricola.Visible = false;
                    imgSave.Visible = false;
                    imgAssegnaSN.Visible = true;
                }
                else if (e.CommandName == "resetSN")
                {
                    Articolo art = new Articolo(artID, artAnno);
                    tbMatricola.Text = art.Matricola;
                    lblMatricola.Text = art.Matricola;
                    imgUndoSN.Visible = false;
                    lblMatricola.Visible = true;
                    tbMatricola.Visible = false;
                    imgSave.Visible = false;
                    imgAssegnaSN.Visible = true;
                }
                else if (e.CommandName == "showChangeFP")
                {
                    if (changeFP.Visible == true)
                    {
                        showChangeFP.Visible = true;
                        changeFP.Visible = false;
                        undoFP.Visible = false;
                        saveFP.Visible = false;
                        ddlOra.Visible = false;
                        ddlMinuto.Visible = false;
                        ddlSecondo.Visible = false;
                        lblHH.Visible = false;
                        lblMM.Visible = false;
                        lblSS.Visible = false;
                    }
                    else
                    {
                        showChangeFP.Visible = false;
                        changeFP.Visible = true;
                        undoFP.Visible = true;
                        saveFP.Visible = true;
                        ddlOra.Visible = true;
                        ddlMinuto.Visible = true;
                        ddlSecondo.Visible = true;
                        lblHH.Visible = true;
                        lblMM.Visible = true;
                        lblSS.Visible = true;
                        
                    }
                }
                else if (e.CommandName == "saveFP")
                {
                    int ora, min, sec;
                    ora = min = sec = -1;
                    try
                    {
                        ora = Int32.Parse(ddlOra.SelectedValue);
                        min = Int32.Parse(ddlMinuto.SelectedValue);
                        sec = Int32.Parse(ddlSecondo.SelectedValue);
                    }
                    catch
                    {
                        ora = -1;
                        min = -1;
                        sec = -1;
                    }
                    if (ora != -1 && min != -1 && sec != -1)
                    {
                        lbl1.Text = artID.ToString() + "/" + artAnno.ToString();
                        Articolo art = new Articolo(artID, artAnno);
                        DateTime fineProd = new DateTime(changeFP.SelectedDate.Year, changeFP.SelectedDate.Month, changeFP.SelectedDate.Day, ora, min, sec);
                        if (fineProd > DateTime.Now && fineProd <= art.DataPrevistaConsegna)
                        {
                            art.DataPrevistaFineProduzione = fineProd;
                            lbl1.Text += art.log;
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
                        }
                        showChangeFP.Visible = true;
                        changeFP.Visible = false;
                        undoFP.Visible = false;
                        saveFP.Visible = false;
                        ddlOra.Visible = false;
                        ddlMinuto.Visible = false;
                        ddlSecondo.Visible = false;
                        lblHH.Visible = false;
                        lblMM.Visible = false;
                        lblSS.Visible = false;
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorGeneric").ToString();
                    }
                }
                else if (e.CommandName == "undoFP")
                {
                    undoFP.Visible = false;
                    changeFP.Visible = false;
                    saveFP.Visible = false;
                    showChangeFP.Visible = true;
                    ddlOra.Visible = false;
                    ddlMinuto.Visible = false;
                    ddlSecondo.Visible = false;
                    lblHH.Visible = false;
                    lblMM.Visible = false;
                    lblSS.Visible = false;
                }
            }
        }
    }
}