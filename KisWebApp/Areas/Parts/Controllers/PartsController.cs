using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Parts.Controllers
{
    public class PartsController : Controller
    {
        // GET: Parts/Parts
        public JsonResult GetPartsListJson()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Parts/Parts/GetPartsListJson", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Parts/Parts/GetPartsListJson", "", ipAddr);
            }

            JsonResult ret = Json("");
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Parts GetParts";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                KIS.App_Sources.Parts prlist = new KIS.App_Sources.Parts(Session["ActiveWorkspace_Name"].ToString());
                prlist.loadParts(true);
                ret = Json(prlist.List);
            }

            return ret;
        }

        /* Returns:
         * partId if everything is ok or if partnumber
         * -1 if generic error
         * -2 if part name is not correct
         * -3 if user not authorized
         */
        public int addPart(String PartNumber, String Name, String Description)
        {
            int ret = -4;

            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Parts/Parts/addPart", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Parts/Parts/GetPartsListJson", "", ipAddr);
            }

            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Parts Add";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                User usr = (User)Session["user"];
                KIS.App_Sources.Parts prlist = new KIS.App_Sources.Parts(Session["ActiveWorkspace_Name"].ToString());
                ret = prlist.add(PartNumber, Name, Description, usr.username, true);
            }
            else
            {
                ret = -3;
            }

            return ret;
        }
    }
}