using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Menu;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class menuAddVoceFiglia : System.Web.UI.UserControl
    {
        public int vID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Menu Voce";
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
                if (vID != -1 && !Page.IsPostBack)
                {
                    VoceMenu vm = new VoceMenu(vID);
                    if (vm.ID != -1)
                    {
                    }
                    else
                    {
                        imgShowFormAddMainMenu.Visible = false;
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di aggiungere una voce di menu.<br/>";
                imgShowFormAddMainMenu.Visible = false;
                imgShowFormAddMainMenu.Enabled = false;
                tblFormAdd.Visible = false;
            }
        }

        protected void imgShowFormAddMainMenu_Click(object sender, ImageClickEventArgs e)
        {
            if (tblFormAdd.Visible == false)
            {
                tblFormAdd.Visible = true;
            }
            else
            {
                tblFormAdd.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            VoceMenu mn = new VoceMenu(vID);
            bool rt = mn.AddFiglio(Server.HtmlEncode(txtTitolo.Text), Server.HtmlEncode(txtDesc.Text), Server.HtmlEncode(txtURL.Text));
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text = "Attenzione: si è verificato un errore nell'inserimento della voce di menu.<br />";
                //lbl1.Text = mn.log;
            }
        }

        protected void undo_Click(object sender, ImageClickEventArgs e)
        {
            txtDesc.Text = "";
            txtTitolo.Text = "";
            txtURL.Text = "";
        }

    }
}