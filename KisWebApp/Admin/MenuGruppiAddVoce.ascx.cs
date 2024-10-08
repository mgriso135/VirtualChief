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
    public partial class MenuGruppiAddVoce : System.Web.UI.UserControl
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
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idGruppo != -1)
                {
                    Group grp = new Group(idGruppo);
                    grp.loadMenu();
                    if (!Page.IsPostBack)
                    {
                        MainMenu elencoTotale = new MainMenu(Session["ActiveWorkspace_Name"].ToString());
                        List<VoceMenu> NonGruppate = new List<VoceMenu>();
                        for (int i = 0; i < elencoTotale.Elenco.Count; i++)
                        {
                            bool found = false;
                            for (int j = 0; j < grp.VociDiMenu.Count; j++)
                            {
                                if (grp.VociDiMenu[j].ID == elencoTotale.Elenco[i].ID)
                                {
                                    found = true;
                                }
                            }

                            if (found == false)
                            {
                                NonGruppate.Add(elencoTotale.Elenco[i]);
                            }
                        }

                        rptAddVociGruppi.DataSource = NonGruppate;
                        rptAddVociGruppi.DataBind();
                    }
                }
            }
        }

        protected void rptAddVociGruppi_ItemCommand(object source, RepeaterCommandEventArgs e)
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

            if (idGruppo != -1 && voceID != -1)
            {
                Group grp = new Group(idGruppo);
                VoceMenu vm = new VoceMenu(voceID);

                if (e.CommandName == "addVoce")
                {
                    bool rt = grp.AddMenu(vm);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblAddVoiceError").ToString();
                            }
                }
            }
        }
    }
}