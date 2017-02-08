using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;

namespace KIS.kpi
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (((User)(Session["user"])).authenticated == true)
                {
                    if (((User)(Session["user"])).typeOfUser == "Admin")
                    {
                        lnkLogin.Visible = false;
                        tblOptions.Visible = true;
                    }
                    else
                    {
                        lnkLogin.Visible = false;
                        tblOptions.Visible = false;
                        lbl1.Text = "Non sei un amministratore quindi non puoi vedere questa pagina.<br/>";
                    }
                }
                else
                {
                    lnkLogin.Visible = true;
                    tblOptions.Visible = false;
                }
            }
            else
            {
                lnkLogin.Visible = true;
                tblOptions.Visible = false;
            }
        }
    }
}