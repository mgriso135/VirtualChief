using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using KIS;

namespace KIS.kpi
{
    public partial class recordValueNow : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSaveRecord_Click(object sender, EventArgs e)
        {
            lblErr.Text = "ENTRO NELLA FUNZIONE!!!";
            int kpiID = -1;
            String kpiIDS = "";
            lblErr.Visible = true;

            try
            {
                kpiIDS = Request.QueryString["kpiID"];
                kpiID = int.Parse(kpiIDS);
            }
            catch
            {
                kpiID = -1;
                lblErr.Text += "QueryString not valid";
            }
            if (kpiID != -1)
            {
                Kpi indic = new Kpi(kpiID);

                if (indic.id != -1)
                {
                    valore.Text = valore.Text.Replace(",", ".");
                    if (indic.recordValueNow(double.Parse(valore.Text, CultureInfo.GetCultureInfo("en-US")), 0))
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lblErr.Text += "Non ho aggiunto niente!";
                    }
                    lblErr.Text += " " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " ";
                }
                else
                {
                    lblErr.Text += "KPI not found";
                }
            }
            else
            {
                lblErr.Text += "<br/>ERRORE!!!";
            }
    }

        protected void btnCancelSaveRecord_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}