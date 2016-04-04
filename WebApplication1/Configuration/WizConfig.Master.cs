using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Configuration
{
    public partial class WizConfig : System.Web.UI.MasterPage
    {
        public String section;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (section == "Logo")
            {
                liLogo.Attributes.Add("class", "active");
                liReparto.Attributes.Remove("class");
                liPostazioni.Attributes.Remove("class");
                liUtenti.Attributes.Remove("class");
            }
            else if (section == "Reparti")
            
                {
                    liReparto.Attributes.Add("class", "active");
                    liLogo.Attributes.Remove("class");
                    liPostazioni.Attributes.Remove("class");
                    liUtenti.Attributes.Remove("class");
                
            }
            else if (section == "Postazioni")
            {
                liPostazioni.Attributes.Add("class", "active");
                liLogo.Attributes.Remove("class");
                liReparto.Attributes.Remove("class");
                liUtenti.Attributes.Remove("class");

            }
            else if (section == "Utenti")
            {
                liUtenti.Attributes.Add("class", "active");
                liLogo.Attributes.Remove("class");
                liPostazioni.Attributes.Remove("class");
                liReparto.Attributes.Remove("class");

            }
        }
    }
}