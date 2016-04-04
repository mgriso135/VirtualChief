using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

namespace MyUserControls
{
    public partial class newProcessBox : System.Web.UI.UserControl
    {

        public int fatherProcID;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void addSubProc_Click(object sender, EventArgs e)
        {
                processo proc = new processo(fatherProcID);
                proc.createDefaultSubProcess();
                Response.Redirect(Request.RawUrl);            
        }
  
    }
}