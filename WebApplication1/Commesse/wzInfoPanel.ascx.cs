using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzInfoPanel : System.Web.UI.UserControl
    {
        public int idCommessa, annoCommessa, idProc, revProc, idVariante, idProdotto, annoProdotto, idReparto, quantita;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblIDCommessa.Visible = false;
            lblCliente.Visible = false;
            lblNoteCommessa.Visible = false;
            lblProcesso.Visible = false;
            lblReparto.Visible = false;
            lblDataConsegna.Visible = false;
            lblDataFineProduzione.Visible = false;
            lblQuantita.Visible = false;

            Commessa cm = new Commessa(idCommessa, annoCommessa);
            if (cm.ID!=-1 && cm.Year!=-1)
            {
                lblIDCommessa.Visible = true;
                lblCliente.Visible = true;
                lblNoteCommessa.Visible = true;
                lblCliente.Text = "Cliente: " + cm.Cliente + "<br/>";
                lblIDCommessa.Text = "Commessa: " + cm.ID.ToString() + "/" + cm.Year.ToString();
                lblNoteCommessa.Text = cm.Note + "<br/>";
            }

            if (idProc != -1 && revProc != -1 && idVariante != -1)
            {
                lblQuantita.Visible = true;
                lblQuantita.Text = "Quantità: " + quantita.ToString() + "<br />";
                ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                if (prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                {
                    lblProcesso.Visible = true;
                    lblProcesso.Text = "Prodotto: " + prcVar.variant.nomeVariante + " - " + prcVar.variant.descrizioneVariante + "<br />";
                }
            }

            if (idReparto != -1)
            {
                Reparto rp = new Reparto(idReparto);
                if (rp.id != -1)
                {
                    lblReparto.Visible = true;
                    lblReparto.Text = "Reparto " + rp.name + "<br />";
                }
            }

            if (idProdotto != -1 && annoProdotto != -1)
            {
                Articolo art = new Articolo(idProdotto, annoProdotto);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (art.DataPrevistaConsegna > DateTime.Now)
                    {
                        lblDataConsegna.Visible = true;
                        lblDataConsegna.Text = "Data consegna richiesta: " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy") + "<br />";
                    }

                    if (art.DataPrevistaFineProduzione > DateTime.Now)
                    {
                        lblDataFineProduzione.Visible = true;
                        lblDataFineProduzione.Text = "Data fine produzione: " + art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                    }
                    lblQuantita.Visible = true;
                    lblQuantita.Text = "Quantità: " + art.Quantita.ToString() + "<br />";
                }
            }
        }
    }
}