using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class updatePERT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["act"]) && !String.IsNullOrEmpty(Request.QueryString["variante"])
                && Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                int varID = Int32.Parse(Request.QueryString["variante"]);
                // Action: update position
                if (Request.QueryString["act"] == "updatepos")
                {
                    if(!String.IsNullOrEmpty(Request.QueryString["posx"]) && !String.IsNullOrEmpty(Request.QueryString["posy"]))
                    {
                    processo tobeupdated = new processo(Session["ActiveWorkspace_Name"].ToString(), Int32.Parse(Request.QueryString["id"]));
                    int posX = Int32.Parse(Request.QueryString["posx"]);
                    int posY = Int32.Parse(Request.QueryString["posy"]);
                    //tobeupdated.posX = posX;
                    //tobeupdated.posY = posY;
                    tobeupdated.setPosX(posX, new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    tobeupdated.setPosY(posY, new variante(Session["ActiveWorkspace_Name"].ToString(), varID));

                    Response.Write("<script language='javascript'> { window.close();}</script>");
                    }
                }
                else if (Request.QueryString["act"] == "delprecedenze") // action: cancella precedenze
                {
                    lbl.Text = Request.QueryString["act"];
                    if (!String.IsNullOrEmpty(Request.QueryString["verso"]) && !String.IsNullOrEmpty(Request.QueryString["delID"]))
                    {
                        processo current = new processo(Session["ActiveWorkspace_Name"].ToString(), Int32.Parse(Request.QueryString["id"]));
                        
                        
                        if (Request.QueryString["verso"] == "prec")
                        {
                            current.loadPrecedenti();
                            current.deleteProcessoPrecedente(new processo(Session["ActiveWorkspace_Name"].ToString(), Int32.Parse(Request.QueryString["delID"])), new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>");
                        }
                        else if(Request.QueryString["verso"] == "succ")
                        {
                            current.loadSuccessivi();
                            current.deleteProcessoSuccessivo(new processo(Session["ActiveWorkspace_Name"].ToString(), Int32.Parse(Request.QueryString["delID"])), new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>");
                        }

                    }
                }
            }
        }
    }
}