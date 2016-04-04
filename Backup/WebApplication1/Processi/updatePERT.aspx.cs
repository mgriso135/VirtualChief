using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Processi
{
    public partial class updatePERT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["act"]))
            {
                // Action: update position
                if (Request.QueryString["act"] == "updatepos")
                {
                    if(!String.IsNullOrEmpty(Request.QueryString["posx"]) && !String.IsNullOrEmpty(Request.QueryString["posy"]))
                    {
                    processo tobeupdated = new processo(Int32.Parse(Request.QueryString["id"]));
                    int posX = Int32.Parse(Request.QueryString["posx"]);
                    int posY = Int32.Parse(Request.QueryString["posy"]);
                    tobeupdated.posX = posX;
                    tobeupdated.posY = posY;
                    Response.Write("<script language='javascript'> { window.close();}</script>");
                    }
                }
                else if (Request.QueryString["act"] == "delprecedenze") // action: cancella precedenze
                {
                    lbl.Text = Request.QueryString["act"];
                    if (!String.IsNullOrEmpty(Request.QueryString["verso"]) && !String.IsNullOrEmpty(Request.QueryString["delID"]))
                    {
                        processo current = new processo(Int32.Parse(Request.QueryString["id"]));
                        
                        
                        if (Request.QueryString["verso"] == "prec")
                        {
                            current.loadPrecedenti();
                            current.deleteProcessoPrecedente(new processo(Int32.Parse(Request.QueryString["delID"])));
                            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>");
                        }
                        else if(Request.QueryString["verso"] == "succ")
                        {
                            current.loadSuccessivi();
                            current.deleteProcessoSuccessivo(new processo(Int32.Parse(Request.QueryString["delID"])));
                            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>");
                        }

                    }
                }
            }
        }
    }
}