using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
namespace MyUserControls
{
    public partial class newProcessBox : System.Web.UI.UserControl
    {

        public int fatherProcID;
        public int variante;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void addSubProc_Click(object sender, EventArgs e)
        {
            if (fatherProcID != -1 && variante != -1)
            {
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), variante);
                if (var.idVariante != -1)
                {
                    processo proc = new processo(Session["ActiveWorkspace_Name"].ToString(), fatherProcID);
                    int res = proc.createDefaultSubProcess(var);

                    if (res != -1)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = proc.log;
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblNotFound").ToString();
            }
        }
  
    }
}