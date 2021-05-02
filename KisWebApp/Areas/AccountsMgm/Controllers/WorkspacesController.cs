using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using System.Net.Mail;

namespace KIS.Areas.AccountsMgm.Controllers
{
    public class WorkspacesController : Controller
    {
        [Authorize]
        // GET: AccountsMgm/Workspaces
        public ActionResult ListWorkspaces()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Workspaces/Workspaces/ViewInvites", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workspaces/Workspaces/ViewInvites", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "User Workspaces";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                UserAccount usr = (UserAccount)Session["user"];
                usr.loadWorkspaces();
                return View(usr.workspaces);
            }

            return View();
        }

        [Authorize]
        public ActionResult WorkspaceDetail()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Workspaces/Workspaces/WorkspaceDetail", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workspaces/Workspaces/WorkspaceDetail", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Workspace Details";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                int wsid = -1;
                try
                {
                    wsid = Int32.Parse(Session["ActiveWorkspace_Id"].ToString());
                }
                catch(Exception ex)
                {
                    wsid = -1;
                }
                if(wsid!=-1)
                { 
/*                    UserAccount curr = (UserAccount)Session["user"];
                    curr.loadWorkspaces();
                    Boolean owned = false;
                    try
                    {
                        var first = curr.workspaces.Find(x => x.id == id);
                        owned = true;
                    }
                    catch(Exception ex)
                    {
                        owned = false;
                    }*/

                    /*if(owned)
                    { */
                        Workspace ws = new Workspace(wsid);
                        if (ws.id != -1)
                        {
                            return View(ws);
                        }
                    // }
                }
            }


            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if invite sent successfully
         * 2 if Workspace not found
         * 3 if email not valid
         */
        [Authorize]
        public int Invite(int workspace, String mail)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Workspaces/Workspaces/Invite", "workspace=" + workspace + "&mail=" + mail, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workspaces/Workspaces/Invite", "workspace=" + workspace + "&mail=" + mail, ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Workspaces Invite";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            if (ViewBag.authW)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Workspace ws = new Workspace(workspace);
                if(ws.id!=-1)
                {
                    MailAddress ml = new MailAddress(mail);
                    ret = ws.InviteUser(ml, curr);
                }
                else
                {
                    ret = 2;
                }
            }
                return ret;
        }
    }
}