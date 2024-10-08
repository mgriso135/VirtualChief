﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class MenuGruppiList : System.Web.UI.UserControl
    {
        public int idGruppo;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Gruppi Menu";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Gruppi Menu";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idGruppo != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Group grp = new Group(idGruppo);
                        grp.loadMenu();
                        rptVociGruppi.DataSource = grp.VociDiMenu;
                        rptVociGruppi.DataBind();
                    }
                }
            }
        }

        protected void rptVociGruppi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int voceID = -1;
            try
            {
                voceID = Int32.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                voceID = -1;
            }

            if (voceID != -1 && idGruppo!=-1)
            {
                VoceMenu vm = new VoceMenu(voceID);
                Group grp = new Group(idGruppo);
                if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown")
                {
                    bool verso = false;
                    if (e.CommandName == "MoveUp")
                    {
                        //lbl1.Text += "in su";
                        verso = true;
                    }
                    else
                    {
                        verso = false;
                    }
                    grp.loadMenu();
                    bool rt = grp.SpostaVoce(vm, verso);

                    grp.loadMenu();
                    rptVociGruppi.DataSource = grp.VociDiMenu;
                    rptVociGruppi.DataBind();
                }
                else if(e.CommandName == "Delete")
                {
                    bool rt = grp.DeleteMenu(vm);
                    if (rt == false)
                    {
                        lbl1.Text = GetLocalResourceObject("lblDeleteVoiceError").ToString();
                    }
                    else
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }
    }
}