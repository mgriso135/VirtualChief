using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Users
{
    public partial class PermessiGruppi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
            int grpID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                    try
                    {
                        grpID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        grpID = -1;
                        lbl1.Text = GetLocalResourceObject("lblError").ToString();
                        lstPermGruppi.Visible = false;
                    }
            }
            else
            {
                grpID = -1;
            }

            
                if (grpID != -1)
                {

                    //lstPermGruppi.GroupID = grpID;

                }
                else
                {
                    //lstPermGruppi.GroupID = -1;
                }
            }
        }
    }
}