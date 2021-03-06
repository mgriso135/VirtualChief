using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Analysis
{
    public partial class CustomerPortfolio1 : System.Web.UI.UserControl
    {
        public static PortafoglioClienti elencoClienti;
        protected void Page_Load(object sender, EventArgs e)
        {
            rptCustomers.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Clienti";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                rptCustomers.Visible = true;
                if (!Page.IsPostBack)
                {
                    elencoClienti = new PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                    var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.RagioneSociale);
                    rptCustomers.DataSource = elencoClientiSorted;
                    rptCustomers.DataBind();
                }
            }
        }

        protected void lnkStatoUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.Stato);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkStatoDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.Stato);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkRagSocUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.RagioneSociale);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkRagSocDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.RagioneSociale);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkProvinciaUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.Provincia);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkProvinciaDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.Provincia);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkPartitaIVAUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.PartitaIVA);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkPartitaIVADown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.PartitaIVA);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCodFiscaleUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.CodiceFiscale);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCodFiscaleDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.CodiceFiscale);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCittaUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.Citta);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCittaDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.Citta);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCAPUp_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderBy(x => x.CAP);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }

        protected void lnkCAPDown_Click(object sender, EventArgs e)
        {
            var elencoClientiSorted = elencoClienti.Elenco.OrderByDescending(x => x.CAP);
            rptCustomers.DataSource = elencoClientiSorted;
            rptCustomers.DataBind();
        }
    }
}