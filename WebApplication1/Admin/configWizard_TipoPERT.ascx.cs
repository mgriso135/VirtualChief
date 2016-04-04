using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class configWizard_TipoPERT : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Wizard TipoPERT";
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
                lbl1.Visible = true;
                rb1.Visible = true;
                if (!Page.IsPostBack && !Page.IsCallback)
                {                    
                    WizardConfig wizCfg = new WizardConfig();
                    rb1.SelectedValue = wizCfg.interfacciaPERT;
                }
            }

        }

        protected void rb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WizardConfig wizCfg = new WizardConfig();
            wizCfg.interfacciaPERT = rb1.SelectedValue;
        }
    }
}