using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Andon
{
    public partial class configFormatoUsername : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto VisualizzazioneNomiUtente";
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    AndonCompleto andone = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    rbList.SelectedValue = andone.PostazioniFormatoUsername.ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rbList.Visible = false;
            }
        }

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AndonCompleto andone = new AndonCompleto(Session["ActiveWorkspace"].ToString());
            lbl1.Text = "";
            char valore = rbList.SelectedValue[0];
            andone.PostazioniFormatoUsername = valore;
        }
    }
}