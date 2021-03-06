using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class configModoCalcoloTC : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto ModoCalcoloTC";
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
                if (idReparto != -1)
                {
                    Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    if (rp.id != -1)
                    {
                        if (!Page.IsPostBack && !Page.IsCallback)
                        {
                            if (rp.ModoCalcoloTC == true)
                            {
                                rbList.SelectedValue = "1";
                            }
                            else
                            {
                                rbList.SelectedValue = "0";
                            }
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblRepartoNotFound").ToString();
                        rbList.Visible = false;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrore").ToString();
                    rbList.Visible = false;
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
            lbl1.Text = "";
                bool val;
                if (rbList.SelectedValue == "1")
                {
                    val = true;
                }
                else
                {
                    val = false;
                }
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                if (rp.id != -1)
                {
                    rp.ModoCalcoloTC = val;
                    lbl1.Text += rp.log;
                    if (rp.ModoCalcoloTC == val)
                    {
                    lbl1.Text = GetLocalResourceObject("lblModificheOk").ToString();
                }
                    else
                    {
                    lbl1.Text = GetLocalResourceObject("lblModificheKo").ToString();
                }
                }
                else
                {
                lbl1.Text = GetLocalResourceObject("lblRepartoNotFound").ToString();
            }
            
        }
    }
}