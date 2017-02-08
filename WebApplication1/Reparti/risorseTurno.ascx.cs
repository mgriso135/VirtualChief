using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class risorseTurno1 : System.Web.UI.UserControl
    {
        public int idTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            rowTitolo.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Turno PostazioneRisorse";
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
                Turno turno = new Turno(idTurno);
                if (turno.id != -1)
                {
                    rowTitolo.Visible = true;
                    lblTitolo.Text = turno.Nome;
                    if (!Page.IsPostBack)
                    {
                        ElencoPostazioni elPostazioni = new ElencoPostazioni();
                        List<Postazione> elPostazioniSorted = elPostazioni.elenco.OrderBy(x => x.name).ToList();
                        rptPostazioni.DataSource = elPostazioniSorted;
                        rptPostazioni.DataBind();
                    }
                }
            }
        }

        protected void txtRisorse_TextChanged(object sender, EventArgs e)
        {
            lbl1.Text = "";
            DropDownList txtNumber = (DropDownList)sender;
            HiddenField hIDPostazione = (HiddenField)txtNumber.Parent.FindControl("hIDPostazione");
            int idPostazione = -1;
            int numRes = -1;
            try
            {
                idPostazione = Int32.Parse(hIDPostazione.Value.ToString());
                numRes = Int32.Parse(txtNumber.SelectedValue);
            }
            catch
            {
                idPostazione = -1;
                numRes = -1;
            }

            if (numRes != -1)
            {
                if (idPostazione != -1)
                {
                    Postazione pst = new Postazione(idPostazione);
                    if (pst.id != -1)
                    {
                        Turno turno = new Turno(idTurno);
                        if (turno.id != -1)
                        {
                            RisorsePostazioneTurno res = new RisorsePostazioneTurno(pst, turno);
                            res.NumRisorse = numRes;
                            lbl1.Text = "Numero di risorse (" + res.NumRisorse.ToString()
                                + ") per postazione " + pst.name.ToString()
                                + " e turno " + turno.Nome.ToString()
                                + " impostato correttamente.";
                            txtNumber.SelectedValue = res.NumRisorse.ToString();
                        }
                        else
                        {
                            lbl1.Text = "Errore: turno non trovato.";
                        }
                    }
                    else
                    {
                        lbl1.Text = "Errore: postazione non trovata.";
                    }
                }
                else
                {
                    lbl1.Text = "Errore: id postazione non valido.";
                }
            }
            else
            {
                lbl1.Text = "Errore: formato numero risorse non valido.";
            }
        }

        protected void rptPostazioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlRisorse = (DropDownList)e.Item.FindControl("txtRisorse");
                HiddenField hPostazioneID = (HiddenField)e.Item.FindControl("hIDPostazione");
                if (ddlRisorse != null && hPostazioneID!=null)
                {
                    int idPostazione = -1;
                    try
                    {
                        idPostazione = Int32.Parse(hPostazioneID.Value.ToString());
                    }
                    catch
                    {
                        idPostazione = -1;
                    }
                    if (idPostazione != -1)
                    {
                        Postazione pst = new Postazione(idPostazione);
                        if (pst!=null && pst.id != -1)
                        {
                            for (int i = 0; i <= 1000; i++)
                            {
                                ddlRisorse.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            }
                            RisorsePostazioneTurno resPost = new RisorsePostazioneTurno(pst, new Turno(idTurno));
                            if (resPost != null && resPost.postazione != null && resPost.postazione.id != -1 && resPost.turno != null && resPost.turno.id != -1)
                            {
                                ddlRisorse.SelectedValue = resPost.NumRisorse.ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}