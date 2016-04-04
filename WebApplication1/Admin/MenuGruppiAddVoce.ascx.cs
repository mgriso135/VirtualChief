using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idGruppo != -1)
                {
                    Group grp = new Group(idGruppo);
                    grp.loadMenu();
                    if (!Page.IsPostBack)
                    {
                        Menu.MainMenu elencoTotale = new Menu.MainMenu();
                        List<Menu.VoceMenu> NonGruppate = new List<Menu.VoceMenu>();
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
                Menu.VoceMenu vm = new Menu.VoceMenu(voceID);

                if (e.CommandName == "addVoce")
                {
                    bool rt = grp.AddMenu(vm);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text += "E' avvenuto un errore durante l'aggiunta della voce di menu.<br />";
                    }
                }
            }
        }
    }
}