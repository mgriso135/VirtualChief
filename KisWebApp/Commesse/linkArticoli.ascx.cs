using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class linkArticoli : System.Web.UI.UserControl
    {
        public int idComm, annoComm;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
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
                if (idComm != -1 && annoComm != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Commessa comm = new Commessa(Session["ActiveWorkspace"].ToString(), idComm, annoComm);
                        ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                        ddlArticoli.DataSource = el.elencoFigli;
                        ddlArticoli.DataValueField = "IDCombinato";
                        ddlArticoli.DataTextField = "NomeCombinato";
                        ddlArticoli.DataBind();
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                ddlArticoli.Visible = false;
                frmLinkArticolo.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            bool check = true;
            if (consegnaprevista.SelectedDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, (new FusoOrario(Session["ActiveWorkspace"].ToString())).tzFusoOrario))
            {
                check = false;
                lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
            }
            int procID = -1;
            int varID = -1;
            int rev = -1;
            int qty = -1;
            try
            {
                qty = Int32.Parse(txtQuantita.Text);
            }
            catch
            {
                qty = -1;
                check = false;
                lbl1.Text = GetLocalResourceObject("lblErrorFormatQty").ToString();
            }
            ProcessoVariante artic = null;
            if (ddlArticoli.SelectedValue == "-1")
            {
                check = false;
                lbl1.Text = GetLocalResourceObject("lblErrorProdNotSel").ToString();
            }
            else
            {
                String[] strProcVar = ddlArticoli.SelectedValue.Split(',');
                try
                {
                    procID = Int32.Parse(strProcVar[0]);
                    varID = Int32.Parse(strProcVar[1]);
                }
                catch
                {
                    procID = -1;
                    varID = -1;
                    rev = -1;
                    check = false;
                }
                if (check == true)
                {
                    artic = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), procID), new variante(Session["ActiveWorkspace"].ToString(), varID));
                    artic.loadReparto();
                    artic.process.loadFigli(artic.variant);
                    if (artic.process == null || artic.variant == null)
                    {
                        check = false;
                    }
                    else
                    {
                        rev = artic.process.revisione;
                    }
                }

                //Se tutto è ok inserisco!
                if (check == true && qty!=-1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace"].ToString(), idComm, annoComm);
                    cm.loadArticoli();
                    if (cm.ID != -1)
                    {
                        bool ret = cm.AddArticolo(artic, consegnaprevista.SelectedDate, qty);
                        if (ret == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = cm.log;
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblError").ToString();
                    }
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            ddlArticoli.SelectedValue = "-1";
            consegnaprevista.SelectedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, (new FusoOrario(Session["ActiveWorkspace"].ToString())).tzFusoOrario);
        }
    }
}