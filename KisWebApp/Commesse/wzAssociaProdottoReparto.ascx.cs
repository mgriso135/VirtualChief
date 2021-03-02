using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class wzAssociaProdottoReparto : System.Web.UI.UserControl
    {
        public int idProc, revProc, idVar, idCommessa, annoCommessa, idProdotto, annoProdotto, quantita;
        public string matricola;
        protected void Page_Load(object sender, EventArgs e)
        {
            rptReparti.Visible = false;
            lnkGoBack.Visible = false;
            imgGoBack.Visible = false;
            imgGoFwd.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto ProcessoVariante";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                lnkGoBack.Visible = true;

                matricola = (String.IsNullOrEmpty(matricola) || matricola.Length == 0) ? "" : matricola;

                String page = "wzEditPERT.aspx";
                WizardConfig wizCfg = new WizardConfig();
                if (wizCfg.interfacciaPERT == "Table")
                {
                    page = "wzEditPERT_updtable.aspx";
                }
                else
                {
                    page = "wzEditPERT.aspx";
                }
                lnkGoBack.NavigateUrl = page + "?idCommessa=" + idCommessa.ToString()
                + "&annoCommessa=" + annoCommessa.ToString()
                + "&idProc=" + idProc.ToString()
                + "&revProc=" + revProc.ToString()
                + "&idVariante=" + idVar.ToString()
                + "&idProdotto=" + idProdotto.ToString()
                + "&annoProdotto=" + annoProdotto.ToString()
                + "&quantita=" + quantita.ToString()
                + "&matricola=" + matricola.ToString();

                if (idProc != -1 && revProc != -1 && idVar != -1)
                {
                    rptReparti.Visible = true;
                    lnkGoBack.Visible = true;
                    imgGoBack.Visible = true;
                    imgGoFwd.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        bool checkCoerenza = false;
                        if (idProdotto == -1 && annoProdotto == -1)
                        {
                            checkCoerenza = true;
                        }
                        else
                        {
                            Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), idProdotto, annoProdotto);
                            if (art.ID!=-1 && art.Proc.process.processID == idProc && art.Proc.process.revisione == revProc && art.Proc.variant.idVariante == idVar && art.Commessa == idCommessa && art.AnnoCommessa == annoCommessa)
                            {
                                checkCoerenza = true;
                            }
                        }

                        if (checkCoerenza == true)
                        {
                            ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProc, revProc), new variante(Session["ActiveWorkspace"].ToString(), idVar));
                            prcVar.loadReparto();
                            prcVar.process.loadFigli(prcVar.variant);
                            ElencoReparti elRep = new ElencoReparti(Session["ActiveWorkspace"].ToString());
                            rptReparti.DataSource = elRep.elenco;
                            rptReparti.DataBind();
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblDataKo").ToString();
                            rptReparti.Visible = false;
                            lnkGoBack.Visible = false;
                            imgGoBack.Visible = false;
                            imgGoFwd.Visible = false;
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptReparti.Visible = false;
                lnkGoBack.Visible = false;
                imgGoBack.Visible = false;
                imgGoFwd.Visible = false;
            }
        }


        protected void rptReparti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tRow = (HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    HtmlInputRadioButton rb = (HtmlInputRadioButton)e.Item.FindControl("rbReparto");
                    //RadioButton rb = (RadioButton)e.Item.FindControl("rbReparto");
                    HiddenField hRepID = (HiddenField)e.Item.FindControl("idRep");
                    //HyperLink lnkPostazioni = (HyperLink)e.Item.FindControl("lnkProcPostazione");
                    int repID = -1;
                    try
                    {
                        repID = Int32.Parse(hRepID.Value);
                    }
                    catch
                    {
                        repID = -1;
                    }
                    if (repID != -1)
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                        ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProc, revProc), new variante(Session["ActiveWorkspace"].ToString(), idVar));
                        prcVar.loadReparto();
                        prcVar.process.loadFigli(prcVar.variant);
                        bool trovato = false;
                        for (int i = 0; i < prcVar.RepartiProduttivi.Count; i++)
                        {
                            if (prcVar.RepartiProduttivi[i].id == rp.id)
                            {
                                trovato = true;
                            }
                        }

                        /*if (rp.id == prcVar.UltimoRepartoUtilizzato.id && prcVar.UltimoRepartoUtilizzato.id != -1 && rb!=null)
                        {
                            rb.Checked = true;
                        }*/

                        if (trovato == true)
                        {
                            tRow.Style.Add("border", "2px solid #adff2f");
                            //tRow.BorderColor = "#adff2f";
                        }
                    }
                }

                // solo se è il pager
                if (e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    // lo rendo rosso!
                    //System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        /*tRow.BgColor = "#00FF00";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                    }
                }
                else
                {
                    //System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        /*tRow.BgColor = "#C0C0C0";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                    }
                }
            }
        }

        protected void imgGoFwd_Click(object sender, ImageClickEventArgs e)
        {
            int idReparto = -1;
            try
            {
                idReparto = Int32.Parse(Request.Form["rbReparto"]);
            }
            catch
            {
                idReparto = -1;
                lbl1.Text = GetLocalResourceObject("lblRepartoNotFound").ToString();
            }

            if (idReparto != -1)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), idReparto);
                if (rp.id != -1)
                {
                    ProcessoVariante procVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProc, revProc), new variante(Session["ActiveWorkspace"].ToString(), idVar));
                    procVar.loadReparto();
                    procVar.process.loadFigli(procVar.variant);
                    procVar.AddReparto(rp);
                    Response.Redirect("wzAssociaTaskPostazioni.aspx?idCommessa=" + idCommessa.ToString()
                + "&annoCommessa=" + annoCommessa.ToString()
                + "&idProc=" + idProc.ToString()
                + "&revProc=" + revProc.ToString()
                + "&idVariante=" + idVar.ToString()
                +"&idReparto=" + rp.id.ToString()
                + "&idProdotto=" + idProdotto.ToString()
                + "&annoProdotto=" + annoProdotto.ToString()
                + "&quantita=" + quantita.ToString()
                + "&matricola=" + matricola.ToString());
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblRepartoNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblRepartoNotFound").ToString();
            }
        }

    }
}