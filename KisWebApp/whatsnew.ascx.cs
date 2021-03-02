using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;
using KIS.App_Sources;

namespace KIS
{
    public partial class whatsnew : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UserAccount curr = (UserAccount)Session["user"];
                string sWhatsNewPath = "~/whatsnew";
                if(curr!=null && curr.Language.Length>0)
                { 
                switch (curr.Language)
                {
                    case "it":
                        sWhatsNewPath += "_it.xml";
                        break;
                    case "es":
                        sWhatsNewPath += "_es.xml";
                            break;
                        case "es-AR":
                            sWhatsNewPath += "_es.xml";
                            break;
                    case "en":
                        sWhatsNewPath += "_en.xml";
                        break;
                        case "en-US":
                            sWhatsNewPath += "_en.xml";
                            break;
                        default:
                        sWhatsNewPath += ".xml";
                        break;
                }
                }
                else
                {
                    sWhatsNewPath += ".xml";
                }
                XmlDataSource1.DataFile = sWhatsNewPath;
                
            }
        }
    }
}