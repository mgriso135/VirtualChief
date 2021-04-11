using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Areas.AccountsMgm.Controllers
{
    public class WorkspacesController : Controller
    {
        [Authorize]
        // GET: AccountsMgm/Workspaces
        public ActionResult ListWorkspaces()
        {
            ViewBag.authR = false;
            if (Session["user"]!=null)
            {
                ViewBag.authR = true;
                UserAccount usr = (UserAccount)Session["user"];
                usr.loadWorkspaces();
                return View(usr.workspaces);
            }
            return View();
        }

        [Authorize]
        public ActionResult WorkspaceDetail(int id)
        {
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                ViewBag.authR = true;
                Workspace ws = new Workspace(id);
                if(ws.id!=-1)
                {
                    return View(ws);
                }
                
            }
            return View();
        }
    }
}