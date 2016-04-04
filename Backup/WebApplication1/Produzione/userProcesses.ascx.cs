using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class userProcesses : System.Web.UI.UserControl
    {
        public string user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (user != "")
            {
                User current = new User(user);
                current.loadMainOwnedProcesses();
                elencoProc.Text = user + "<br/>";
                processo[] elenco = new processo[current.numMainOwnedProcesses];
                for(int i = 0; i < current.numMainOwnedProcesses; i++)
                {
                    //elencoProc.Text += current.ownedProcesses[i] + "<br/>";
                    elenco[i] = new processo(current.mainOwnedProcesses[i]);
                }
                rptOwnedProcesses.DataSource = elenco;
                rptOwnedProcesses.DataBind();

            }
        }
    }
}