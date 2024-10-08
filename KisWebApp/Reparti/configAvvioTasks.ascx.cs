﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class configAvvioTasks : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto AvvioTasksOperatori";
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
                if (!Page.IsPostBack && !Page.IsCallback && idReparto!=-1)
                {
                    Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    if (rp.id != -1)
                    {
                        for (int i = 0; i <= 100; i++)
                        {
                            ddlLimiteTask.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        if (rp.TasksAvviabiliContemporaneamenteDaOperatore == 0)
                        {
                            ddlLimiteTask.Visible = false;
                            rb1.SelectedValue = "0";
                            ddlLimiteTask.SelectedValue = "0";
                        }
                        else
                        {
                            rb1.SelectedValue = "1";
                            ddlLimiteTask.Visible = true;
                            ddlLimiteTask.SelectedValue = rp.TasksAvviabiliContemporaneamenteDaOperatore.ToString();
                        }
                    }
                }
            }
            else
            {
                rb1.Visible = false;
                ddlLimiteTask.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            if (rb1.SelectedValue == "0")
            {
                rp.TasksAvviabiliContemporaneamenteDaOperatore = 0;
                ddlLimiteTask.Visible = false;
                ddlLimiteTask.SelectedValue = "0";
                lbl1.Text = GetLocalResourceObject("lblModificaOk").ToString();
            }
            else
            {
                ddlLimiteTask.SelectedValue = "0";
                ddlLimiteTask.Visible = true;
            }
        }

        protected void ddlLimiteTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            int numTasks = 0;
            Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            if (rp.id != -1)
            {
                try
                {
                    numTasks = Int32.Parse(ddlLimiteTask.SelectedValue.ToString());
                }
                catch
                {
                    numTasks = 0;
                }

                if (numTasks > 0)
                {
                    rp.TasksAvviabiliContemporaneamenteDaOperatore = numTasks;
                    lbl1.Text = GetLocalResourceObject("lblModificaOk").ToString();
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorZeroInvalid").ToString();
                }

            }
        }
    }
}