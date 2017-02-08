using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS
{
    public partial class menuBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            List<VoceMenu> lista = new List<VoceMenu>();
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack)
                {
                    User curr = (User)Session["user"];
                    curr.loadGruppi();
                    for (int i = 0; i < curr.Gruppi.Count; i++)
                    {
                        curr.Gruppi[i].loadMenu();
                        for (int j = 0; j < curr.Gruppi[i].VociDiMenu.Count; j++)
                        {
                            bool controllo = false;
                            for (int k = 0; k < lista.Count; k++)
                            {
                                if (lista[k].ID == curr.Gruppi[i].VociDiMenu[j].ID)
                                {
                                    controllo = true;
                                }
                            }
                            if (controllo == false)
                            {
                                    lista.Add(curr.Gruppi[i].VociDiMenu[j]);
                            }
                        }
                    }
                    frmMenu.Orientation = Orientation.Horizontal;
                    for (int i = 0; i < lista.Count; i++)
                    {
                        frmMenu.Items.Add(new MenuItem(lista[i].Titolo, lista[i].Descrizione, "", lista[i].URL));
                        lista[i].loadFigli();
                        for (int j = 0; j < lista[i].VociFiglie.Count; j++)
                        {
                            frmMenu.Items[i].ChildItems.Add(new MenuItem(lista[i].VociFiglie[j].Titolo, lista[i].VociFiglie[j].Descrizione, "", lista[i].VociFiglie[j].URL));
                        }
                    }


                    frmMenu.DataBind();
                }
            }
            else
            {
                //Response.Redirect("/Login/login.aspx");

                if (!Page.IsPostBack)
                {
                    frmMenu.Items.Add(new MenuItem("Login", "Login", "", "/Login/login.aspx"));
                }
            }
        }
    }
}