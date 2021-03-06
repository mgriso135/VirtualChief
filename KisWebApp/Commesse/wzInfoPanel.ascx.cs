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

            Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
            if (cm.ID!=-1 && cm.Year!=-1)
            {
                lblIDCommessa.Visible = true;
                lblCliente.Visible = true;
                lblNoteCommessa.Visible = true;
                lblCliente.Text = GetLocalResourceObject("lblCliente").ToString() + ": " + cm.Cliente + "<br/>";
                lblIDCommessa.Text = GetLocalResourceObject("lblCommessa").ToString() + ": " + cm.ID.ToString() + "/" + cm.Year.ToString();
                lblNoteCommessa.Text = cm.Note + "<br/>";
            }

            if (idProc != -1 && revProc != -1 && idVariante != -1)
            {
                lblQuantita.Visible = true;
                lblQuantita.Text = GetLocalResourceObject("lblQuantita").ToString() + ": " + quantita.ToString() + "<br />";
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), idProc, revProc), new variante(Session["ActiveWorkspace_Name"].ToString(), idVariante));
                prcVar.loadReparto();
                prcVar.process.loadFigli(prcVar.variant);
                if (prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                {
                    lblProcesso.Visible = true;
                    lblProcesso.Text = GetLocalResourceObject("lblProdotto").ToString() + ": " + prcVar.variant.nomeVariante + " - " + prcVar.variant.descrizioneVariante + "<br />";
                }
            }

            if (idReparto != -1)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                if (rp.id != -1)
                {
                    lblReparto.Visible = true;
                    lblReparto.Text = GetLocalResourceObject("lblReparto").ToString() + ": " + rp.name + "<br />";
                }
            }

            if (idProdotto != -1 && annoProdotto != -1)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), idProdotto, annoProdotto);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (art.DataPrevistaConsegna > DateTime.Now)
                    {
                        lblDataConsegna.Visible = true;
                        lblDataConsegna.Text = GetLocalResourceObject("lblDataConsegna").ToString() + ": " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy") + "<br />";
                    }

                    if (art.DataPrevistaFineProduzione > DateTime.Now)
                    {
                        lblDataFineProduzione.Visible = true;
                        lblDataFineProduzione.Text = GetLocalResourceObject("lblDataFineProd").ToString() + ": " + art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                    }
                    lblQuantita.Visible = true;
                    lblQuantita.Text = GetLocalResourceObject("lblQuantita").ToString()+": " + art.Quantita.ToString() + " <br />";
                }
            }
        }
    }
}