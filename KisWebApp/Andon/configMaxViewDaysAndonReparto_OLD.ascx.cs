using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Andon
{
    public partial class configMaxViewDaysAndonReparto : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Andon Reparto VisualizzazioneGiorni";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    KIS.App_Code.AndonReparto andrp = new KIS.App_Code.AndonReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    if (andrp.RepartoID != -1)
                    {
                        for (int i = 1; i <= 500; i++)
                        {
                            ddlNumDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        ddlNumDays.SelectedValue = andrp.MaxViewDays.ToString();
                    }
                    else
                    {
                        ddlNumDays.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblErrorDeptNotFound").ToString();
                    }
                }
            }
            else
            {
                ddlNumDays.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void ddlNumDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl1.Text += idReparto.ToString() + " " + ddlNumDays.SelectedValue.ToString() + "<br />";
            KIS.App_Code.AndonReparto andRep = new App_Code.AndonReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            int maxDays = -1;
            try
            {
                maxDays = Int32.Parse(ddlNumDays.SelectedValue.ToString());
            }
            catch
            {
                maxDays = -1;
            }

            if (maxDays != -1)
            {
                andRep.MaxViewDays = maxDays;
                lbl1.Text = GetLocalResourceObject("lblModificaOk").ToString();
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblModificaKo").ToString();
            }
        }
    }
}