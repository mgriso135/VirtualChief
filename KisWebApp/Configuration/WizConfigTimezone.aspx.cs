using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Configuration
{
    public partial class WizConfigTimezone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WizConfig s = (WizConfig)this.Master;
            s.section = "TimeZone";
        }
    }
}