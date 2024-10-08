﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class configSplitTasksTurni : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto";
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
                    if (!Page.IsPostBack)
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                        if (rp.splitTasks == true)
                        {
                            splitTasks.SelectedValue = "1";
                        }
                        else
                        {
                            splitTasks.SelectedValue = "0";
                        }
                    }
                }
                else
                {
                    splitTasks.Enabled = false;
                }
            }
            else
            {
                splitTasks.Enabled = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void splitTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (idReparto != -1)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                if (splitTasks.SelectedItem.Value == "0")
                {
                    rp.splitTasks = false;
                }
                else
                {
                    rp.splitTasks = true;
                }
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}