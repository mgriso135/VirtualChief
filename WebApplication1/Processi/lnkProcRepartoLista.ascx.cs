using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace KIS.Processi
{
    public partial class lnkProcRepartoLista : System.Web.UI.UserControl
    {
        public int idProc, revProc, idVar;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto ProcessoVariante";
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
                if (idProc != -1 && revProc != -1 && idVar != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVar));
                        prcVar.loadReparto();
                        ElencoReparti elRep = new ElencoReparti();
                        rptReparti.DataSource = elRep.elenco;
                        rptReparti.DataBind();
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di associare processivarianti al reparto.<br />";
            }
        }

        protected void rptReparti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tRow = (HtmlTableRow)e.Item.FindControl("tr1");
                CheckBox chk = (CheckBox)e.Item.FindControl("chk");
                HiddenField hRepID = (HiddenField)e.Item.FindControl("idRep");
                HyperLink lnkPostazioni = (HyperLink)e.Item.FindControl("lnkProcPostazione");
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
                    Reparto rp = new Reparto(repID);
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVar));
                    lnkPostazioni.NavigateUrl += prcVar.process.processID.ToString() + "&variante=" + prcVar.variant.idVariante.ToString() + "&repID=" + rp.id.ToString();
                    prcVar.loadReparto();
                    bool trovato = false;
                    for (int i = 0; i < prcVar.RepartiProduttivi.Count; i++)
                    {
                        if (prcVar.RepartiProduttivi[i].id == rp.id)
                        {
                            trovato = true;
                        }
                    }

                    if (trovato == true)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        lnkPostazioni.Visible = false;
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

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            HtmlTableRow tRow = (HtmlTableRow)chk.Parent.Parent;
            HiddenField hRepID = (HiddenField)tRow.FindControl("idRep");
            int repID = -1;
            try
            {
                repID = Int32.Parse(hRepID.Value);
            }
            catch
            {
                lbl1.Text = "Error.<br/>";
            }

            if (repID != -1)
            {
                Reparto rp = new Reparto(repID);
                if (rp.id != -1)
                {
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVar));
                    bool rt;
                    if (chk.Checked == true)
                    {
                        rt = prcVar.AddReparto(rp);
                    }
                    else
                    {
                        rt = prcVar.DeleteReparto(rp);
                    }
                    if (rt == true)
                    {
                        lbl1.Text = "Operazione andata a buon fine.<br/>";
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "E' avvenuto un errore.<br/>";
                    }
                    lbl1.Text += prcVar.log;
                }
            }
        }
    }
}