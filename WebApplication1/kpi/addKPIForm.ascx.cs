using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using Dati;
using KIS;

namespace KIS.kpi
{
    public partial class addKPIForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string procID = Request.QueryString[0];
                processo padre = new processo(int.Parse(procID));
                if (padre.processID == -1)
                {
                    imgAddKPI.Visible = false;
                    kpi_title.Visible = false;
                    imgCancel.Visible = false;
                }
            }
            catch
            {
                imgAddKPI.Visible = false;
                kpi_title.Visible = false;
                imgCancel.Visible = false;
            }
            if(!Page.IsPostBack)
            {
                nomeKPI.Visible = false;
                descrKPI.Visible = false;
                lblInputDescrKPI.Visible = false;
                lblInputNameKPI.Visible = false;
                btnADDKPI.Visible = false;
                imgCancel.Visible = false;
                valNome.Visible = false;
                valDesc.Visible = false;
            }

        }

        protected void imgAddKPI_Click(object sender, EventArgs e)
        {
            imgAddKPI.Visible = false;
            lblInputDescrKPI.Visible = true;
            lblInputNameKPI.Visible = true;
            nomeKPI.Visible = true;
            descrKPI.Visible = true;
            btnADDKPI.Visible = true;
            imgCancel.Visible = true;
            valNome.Visible = true;
            valDesc.Visible = true;
        }

        protected void btnADDKPI_Click(object sender, EventArgs e)
        {

            String kpiName = nomeKPI.Text;
            String kpiDescr = descrKPI.Text;
            if (kpiName.Length > 0 && kpiDescr.Length > 0)
            {
                string procID = Request.QueryString[0];
                if (procID.Length > 0)
                {
                    processo proc = new processo(int.Parse(procID));
                    bool ret = proc.addKPI(Server.HtmlEncode(kpiName), Server.HtmlEncode(kpiDescr));
                    if (ret == true)
                    {
                        lblKPI.Text = "Kpi added<br/>";
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lblKPI.Text = "Error: cannot add kpi<br/>";
                        imgAddKPI.Visible = true;
                        lblInputDescrKPI.Visible = false;
                        lblInputNameKPI.Visible = false;
                        nomeKPI.Visible = false;
                        descrKPI.Visible = false;
                        btnADDKPI.Visible = false;
                        imgCancel.Visible = false;

                    }
                }
            }
            else
            {
                lblKPI.Text = "Error: name and description can't be empty<br/>";
            }
        }

        protected void imgCancel_Click(object sender, EventArgs e)
        {
            imgCancel.Visible = false;
            lblInputDescrKPI.Visible = false;
            lblInputNameKPI.Visible = false;
            btnADDKPI.Visible = false;
            nomeKPI.Visible = false;
            descrKPI.Visible = false;
            imgAddKPI.Visible = true;
            valDesc.Visible = false;
            valNome.Visible = false;
        }
    }
}