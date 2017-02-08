using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Start;

namespace KIS
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Codice eseguito all'avvio dell'applicazione
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register); // NEW way
            // Default stuff
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Codice eseguito all'arresto dell'applicazione

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Codice eseguito in caso di errore non gestito

        }

        void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            // Code that runs on application startup
            if(HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                User currUser = (User)HttpContext.Current.Session["user"];
                if(currUser!=null && currUser.username.Length>0 && currUser.Language.Length>0)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currUser.Language);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currUser.Language);
                }
            }
            else
            {
                
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it");
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it");
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Codice eseguito all'avvio di una nuova sessione

        }

        void Session_End(object sender, EventArgs e)
        {
            // Codice eseguito al termine di una sessione. 
            // Nota: l'evento Session_End viene generato solo quando la modalità sessionstate
            // è impostata su InProc nel file Web.config. Se la modalità è impostata su StateServer 
            // o SQLServer, l'evento non viene generato.

        }

    }
}
