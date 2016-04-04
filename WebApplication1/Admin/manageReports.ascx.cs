using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Admin
{
    public partial class manageReports1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            tblOptions.Visible = false;
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = true;
            }

            if (checkUser == true)
            {
                tblOptions.Visible = true;
                Boolean checkAnalisi;
                List<String[]> perm = new List<String[]>();
                String[] prm = new String[2];
                User curr = (User)Session["user"];

                boxOrderStatusReport.Visible = false;
                checkAnalisi = false;
                prm[0] = "Configurazione Report Stato Ordini Clienti";
                prm[1] = "W";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                
                boxOrderStatusReport.Visible = checkAnalisi;
                /*if (checkAnalisi == true)
                {
                    boxOrderStatusReport.Visible = true;
                }
                else
                {
                    boxOrderStatusReport.Visible = false;
                }*/
                perm.Clear();
            }
            else
            {
                tblOptions.Visible = false;
                lbl1.Text = "Utente non riconosciuto.";
            }
        }
    }
}