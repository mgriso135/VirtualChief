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
            if (Session["User"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                UserAccount usr = (UserAccount)Session["User"];
                usr.loadWorkspaces();
                return View(usr.workspaces);
            }

            return View();
        }

        [Authorize]
        public ActionResult WorkspaceDetail(int id)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["User"] != null)
            {
                UserAccount curr = (UserAccount)Session["User"];
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
                if(id != -1)
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
                        Workspace ws1 = new Workspace(id);
                        if (ws1.id != -1)
                        {
                            return View(ws1);
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

        /*
         * Returns: 
         * 0 if generic error
         * 1 if user not authorized
         */
        [Authorize]
        public int DeleteUserGroup(int workspace, int userid, int groupid)
        {
            int ret = 0;
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Workspaces/Workspaces/DeleteUserGroup", "workspaceid=" + workspace + "&userid=" + userid + "&groupid=" + groupid, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workspaces/Workspaces/DeleteUserGroup", "workspaceid=" + workspace + "&userid=" + userid + "&groupid=" + groupid, ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Workspaces UserAccountsGroups";
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
                UserAccount usr = new UserAccount(userid);
                ret = usr.DeleteWorkspaceGroup(workspace, groupid);
            }
            return ret;
        }
    }
}