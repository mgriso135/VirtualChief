using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class listArticoliCommessa : System.Web.UI.UserControl
    {
        public int commID, commYear;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
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
                if (!Page.IsPostBack)
                {
                    if (commID != -1 && commYear != -1)
                    {
                        Commessa comm = new Commessa(commID, commYear);
                        comm.loadArticoli();
                        rptArticoliCommessa.DataSource = comm.Articoli;
                        rptArticoliCommessa.DataBind();
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare gli articoli.<br/>";
                rptArticoliCommessa.Visible = false;
            }
        }

        protected void rptArticoliCommessa_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
                ImageButton imgDepianificazione = (ImageButton)e.Item.FindControl("imgDepianificazione");
                ImageButton imgEdit = (ImageButton)e.Item.FindControl("imgEdit");
                HiddenField hID = (HiddenField)e.Item.FindControl("hID");
                HiddenField hYear = (HiddenField)e.Item.FindControl("hYear");
                int artID = -1;
                int artAnno = -1;
                try
                {
                    artID = Int32.Parse(hID.Value);
                    artAnno = Int32.Parse(hYear.Value);
                }
                catch
                {
                    artAnno = -1;
                    artID = -1;
                }

                if (artID != -1 && artAnno !=-1)
                {
                    Articolo art = new Articolo(artID, artAnno);
                    
                    if (art.Status == 'P')
                    {
                        imgEdit.Visible = false;
                        imgDelete.Visible = false;
                        imgDepianificazione.Visible = true;
                    }
                    else if(art.Status=='N')
                    {
                        imgEdit.Visible = true;
                        imgDelete.Visible = true;
                        imgDepianificazione.Visible = false;
                    }
                    else
                    {
                        imgDelete.Visible = false;
                        imgEdit.Visible = false;
                        imgDepianificazione.Visible = false;
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
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptArticoliCommessa_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgEdit = (ImageButton)e.Item.FindControl("imgEdit");
            Calendar calDataPC = (Calendar)e.Item.FindControl("calEditDataPC");
            ImageButton imgSavePC = (ImageButton)e.Item.FindControl("imgSavePC");
            ImageButton imgResetPC = (ImageButton)e.Item.FindControl("imgResetPC");
            Label lblDataPC = (Label)e.Item.FindControl("lblDataPC");

            int idArt = -1;
            int artAnno = -1;
            try
            {
                String[] artic = e.CommandArgument.ToString().Split('/');
                idArt = Int32.Parse(artic[0]);
                artAnno = Int32.Parse(artic[1]);
            }
            catch
            {
                idArt = -1;
                artAnno = -1;
            }
            lbl1.Text = e.CommandArgument.ToString() + " " + idArt.ToString() + " " + artAnno.ToString();
            if (e.CommandName == "delete")
            {
                if (idArt != -1 && artAnno!=-1)
                {
                    Commessa comm = new Commessa(commID, commYear);
                    comm.loadArticoli();
                    for (int i = 0; i < comm.Articoli.Count; i++)
                    {
                        if (comm.Articoli[i].ID == idArt && comm.Articoli[i].Year == artAnno)
                        {
                            if (comm.Articoli[i].Status == 'N')
                            {
                                bool ret = comm.Articoli[i].Delete();
                                if (ret == true)
                                {
                                    Response.Redirect(Request.RawUrl);
                                }
                                else
                                {
                                    lbl1.Text = comm.Articoli[i].log;
                                }
                            }
                            else
                            {
                                lbl1.Text = "Non posso cancellarlo perché lo status non è 'N', quindi il processo produttivo è già iniziato.<br/>";
                            }
                        }
                    }
                }
                else
                {
                    lbl1.Text += "Error<br/>";
                }
            }
            else if (e.CommandName == "editData")
            {
                if (idArt != -1 && artAnno != -1)
                {
                    Articolo art = new Articolo(idArt, artAnno);
                    if (art.ID != -1)
                    {
                        if (art.Status == 'N')
                        {
                            calDataPC.Visible = true;
                            imgEdit.Visible = false;
                            imgResetPC.Visible = true;
                            imgSavePC.Visible = true;
                            calDataPC.SelectedDate = art.DataPrevistaConsegna;
                        }
                        else
                        {
                            lbl1.Text = "Non puoi più cambiare la data di consegna.<br/>";
                        }
                    }
                }
            }
            else if (e.CommandName == "savePC")
            {
                if (idArt != -1 && artAnno!=-1)
                {
                    Articolo art = new Articolo(idArt, artAnno);
                    if (art.ID != -1)
                    {
                        if (calDataPC.SelectedDate > DateTime.Now)
                        {
                            art.DataPrevistaConsegna = calDataPC.SelectedDate;
                            lblDataPC.Text = calDataPC.SelectedDate.ToString("dd/MM/yyyy");
                            imgEdit.Visible = true;
                            imgResetPC.Visible = false;
                            imgSavePC.Visible = false;
                            calDataPC.Visible = false;

                        }
                        else
                        {
                            lbl1.Text = "Errore: la data di consegna non può essere antecedente ad oggi.<br />";
                        }
                    }
                }
            }
            else if (e.CommandName == "undoPC")
            {
                if (idArt != -1 && artAnno!=-1)
                {
                    Articolo art = new Articolo(idArt, artAnno);
                    if (art.ID != -1)
                    {
                        calDataPC.SelectedDate = art.DataPrevistaConsegna;
                        imgEdit.Visible = true;
                        imgSavePC.Visible = false;
                        imgResetPC.Visible = false;
                        calDataPC.Visible = false;
                    }
                }
            }
            else if (e.CommandName == "depianifica")
            {
                Articolo art = new Articolo(idArt, artAnno);
                bool rt = art.Depianifica();
                if (rt == true)
                {
                    Commessa comm = new Commessa(commID, commYear);
                    comm.loadArticoli();
                    rptArticoliCommessa.DataSource = comm.Articoli;
                    rptArticoliCommessa.DataBind();
                    lbl1.Text = "La pianificazione del prodotto " + art.ID.ToString()
                    + "/" + art.Year.ToString() + " è stata rimossa con successo.<br />";
                }
                else
                {
                    lbl1.Text += "E' avvenuto un errore.<br/>" + art.log;
                }
            }
        }
    }
}