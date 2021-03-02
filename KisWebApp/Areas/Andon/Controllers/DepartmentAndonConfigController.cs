using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Andon.Controllers
{
    public class DepartmentAndonConfigController : Controller
    {
        // GET: Andon/DepartmentAndonConfig
        public ActionResult GetDepartmentAndonConfig(int DepartmentID)
        {
            ViewBag.DepartmentID = -1;

            // Check read permissions
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonReparto Config";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if(ViewBag.authW)
            {
                ViewBag.ShowActiveUsers = false;
                ViewBag.ShowProdIndicator = false;
                ViewBag.UsernameFormat = '0';
                AndonReparto andonCfg = new AndonReparto(Session["ActiveWorkspace"].ToString(), DepartmentID);
            if(andonCfg.RepartoID!=-1)
            {
                    ViewBag.DepartmentID = andonCfg.RepartoID;
                    andonCfg.loadShowActiveUsers();
                ViewBag.ShowActiveUsers = andonCfg.ShowActiveUsers;
                andonCfg.loadShowProductionIndicator();
                ViewBag.ShowProdIndicator = andonCfg.ShowProductionIndicator;
                ViewBag.UsernameFormat = andonCfg.UsernameFormat;
            }
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user is not authorized
         * 3 if Department not found
         */
        public int setShowProductivityIndicators(int DepartmentID, Boolean flag)
        {
            ViewBag.authW = false;
            int ret = 0;
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonReparto Config";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                AndonReparto aRp = new AndonReparto(Session["ActiveWorkspace"].ToString(), DepartmentID);
                if(aRp.RepartoID!=-1)
                {
                    aRp.ShowProductionIndicator = flag;
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
                return ret;
        }

        /* Returns:
 * 0 if generic error
 * 1 if all is ok
 * 2 if user is not authorized
 * 3 if Department not found
 */
        public int setShowActiveUsers(int DepartmentID, Boolean flag)
        {
            int ret = 0;
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonReparto Config";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                AndonReparto aRp = new AndonReparto(Session["ActiveWorkspace"].ToString(), DepartmentID);
                if (aRp.RepartoID != -1)
                {
                    aRp.ShowActiveUsers = flag;
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /* Returns:
 * 0 if generic error
 * 1 if all is ok
 * 2 if user is not authorized
 * 3 if Department not found
 */
        public int setUsernameFormat(int DepartmentID, char flag)
        {
            int ret = 0;
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonReparto Config";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null && Session["ActiveWorkspace"]!=null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                AndonReparto aRp = new AndonReparto(Session["ActiveWorkspace"].ToString(), DepartmentID);
                if (aRp.RepartoID != -1)
                {
                    aRp.UsernameFormat = flag;
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
    }
}