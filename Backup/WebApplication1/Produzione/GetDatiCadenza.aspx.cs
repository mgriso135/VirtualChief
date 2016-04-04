using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class GetDatiCadenza : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Now;

            Response.ContentType = "text/event-stream";

            while (startDate.AddMinutes(1) > DateTime.Now)
            {

                Response.Write(string.Format("data: {0}\n\n", DateTime.Now.ToString()));
                Response.Flush();

                System.Threading.Thread.Sleep(1000);
            }

            Response.Close();
        }
    }
}