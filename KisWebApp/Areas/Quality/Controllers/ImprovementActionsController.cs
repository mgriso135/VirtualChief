using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using System.IO;

namespace KIS.Areas.Quality.Controllers
{
    public class ImprovementActionsController : Controller
    {
        // GET: Quality/ImprovementActions
        public ActionResult Index(Char stat, String sortOrder)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/Index", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            List<ImprovementAction> iActsList = new List<ImprovementAction>();
            if (ViewBag.authR || ViewBag.authW)
            {
                ViewBag.authorized = true;
                ImprovementActions impActs = new ImprovementActions(Session["ActiveWorkspace"].ToString());
                impActs.loadImprovementActions();
                iActsList = impActs.ImprovementActionsList;
            }
            else
            {
                if (Session["user"] != null)
                {
                    ViewBag.authR = true;
                    ImprovementActions impActs = new ImprovementActions(Session["ActiveWorkspace"].ToString());
                    impActs.loadImprovementActions((User)Session["user"]);
                    iActsList = impActs.ImprovementActionsList;
                }
            }

            ViewBag.filterStat = stat;
            if (stat != '\0' && stat != 'A')
            {
                iActsList = iActsList.Where(s => s.Status == stat).ToList();
            }

            ViewBag.sortID = sortOrder == "sortID" ? "sortID_desc" : "sortID";
            ViewBag.sortOpeningDate = sortOrder == "sortOpeningDate" ? "sortOpeningDate_desc" : "sortOpeningDate";
            ViewBag.sortCurrentSituation = sortOrder == "sortCurrentSituation" ? "sortCurrentSituation_desc" : "sortCurrentSituation";
            ViewBag.sortExpectedResults = sortOrder == "sortExpectedResults" ? "sortExpectedResults_desc" : "sortExpectedResults";
            ViewBag.sortRootCauses = sortOrder == "sortRootCauses" ? "sortRootCauses_desc" : "sortRootCauses";
            ViewBag.sortEndDateExpected = sortOrder == "sortEndDateExpected" ? "sortEndDateExpected_desc" : "sortEndDateExpected";
            ViewBag.sortStatus = sortOrder == "sortStatus" ? "sortStatus_desc" : "sortStatus";
            ViewBag.sortClosureNotes = sortOrder == "sortClosureNotes" ? "sortClosureNotes_desc" : "sortClosureNotes";
            ViewBag.sortEndDateReal = sortOrder == "sortEndDateReal" ? "sortEndDateReal_desc" : "sortEndDateReal";
            ViewBag.sortCreatedBy = sortOrder == "sortCreatedBy" ? "sortCreatedBy_desc" : "sortCreatedBy";
            ViewBag.sortModifiedBy = sortOrder == "sortModifiedBy" ? "sortModifiedBy_desc" : "sortModifiedBy";
            ViewBag.sortModifiedDate = sortOrder == "sortModifiedDate" ? "sortModifiedDate_desc" : "sortModifiedDate";

            switch (sortOrder)
            {
                case "sortID":
                    iActsList = iActsList.OrderBy(s => s.Year).ThenBy(s => s.ID).ToList();
                    break;
                case "sortID_desc":
                    iActsList = iActsList.OrderByDescending(s => s.Year).ThenByDescending(s => s.ID).ToList();
                    break;
                case "sortOpeningDate":
                    iActsList = iActsList.OrderBy(s => s.OpeningDate).ToList();
                    break;
                case "sortOpeningDate_desc":
                    iActsList = iActsList.OrderByDescending(s => s.OpeningDate).ToList();
                    break;
                case "sortCurrentSituation_desc":
                    iActsList = iActsList.OrderByDescending(s => s.CurrentSituation).ToList();
                    break;
                case "sortCurrentSituation":
                    iActsList = iActsList.OrderBy(s => s.CurrentSituation).ToList();
                    break;
                case "sortExpectedResults":
                    iActsList = iActsList.OrderBy(s => s.ExpectedResults).ToList();
                    break;
                case "sortExpectedResults_desc":
                    iActsList = iActsList.OrderByDescending(s => s.ExpectedResults).ToList();
                    break;
                case "sortRootCauses":
                    iActsList = iActsList.OrderBy(s => s.RootCauses).ToList();
                    break;
                case "sortRootCauses_desc":
                    iActsList = iActsList.OrderByDescending(s => s.RootCauses).ToList();
                    break;
                case "sortEndDateExpected":
                    iActsList = iActsList.OrderBy(s => s.EndDateExpected).ToList();
                    break;
                case "sortEndDateExpected_desc":
                    iActsList = iActsList.OrderByDescending(s => s.EndDateExpected).ToList();
                    break;
                case "sortStatus":
                    iActsList = iActsList.OrderBy(s => s.Status).ToList();
                    break;
                case "sortStatus_desc":
                    iActsList = iActsList.OrderByDescending(s => s.Status).ToList();
                    break;
                case "sortClosureNotes":
                    iActsList = iActsList.OrderBy(s => s.ClosureNotes).ToList();
                    break;
                case "sortClosureNotes_desc":
                    iActsList = iActsList.OrderByDescending(s => s.ClosureNotes).ToList();
                    break;
                case "sortEndDateReal":
                    iActsList = iActsList.OrderBy(s => s.EndDateReal).ToList();
                    break;
                case "sortEndDateReal_desc":
                    iActsList = iActsList.OrderByDescending(s => s.EndDateReal).ToList();
                    break;
                case "sortCreatedBy":
                    iActsList = iActsList.OrderBy(s => s.CreatedBy).ToList();
                    break;
                case "sortCreatedBy_desc":
                    iActsList = iActsList.OrderByDescending(s => s.CreatedBy).ToList();
                    break;
                case "sortModifiedBy":
                    iActsList = iActsList.OrderBy(s => s.ModifiedBy).ToList();
                    break;
                case "sortModifiedBy_desc":
                    iActsList = iActsList.OrderByDescending(s => s.ModifiedBy).ToList();
                    break;
                case "sortModifiedDate":
                    iActsList = iActsList.OrderBy(s => s.ModifiedDate).ToList();
                    break;
                case "sortModifiedDate_desc":
                    iActsList = iActsList.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
                default:
                    iActsList = iActsList.OrderByDescending(s => s.Year).ThenByDescending(a => a.ID).ToList();
                    break;
            }

            return View(iActsList);
        }

        public ActionResult Create()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/Create", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/Create", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.result = false;

            if (checkUser == true)
            {
                ViewBag.authenticated = true;

                ImprovementActions impActList = new ImprovementActions(Session["ActiveWorkspace"].ToString());
                UserAccount curr = (UserAccount)Session["user"];
                int[] ret = impActList.Add(curr);
                ViewBag.Message = impActList.log;
                if(ret[0] != -1 && ret[1] != -1)
                {
                    ViewBag.result = true;
                    Response.Redirect("~/Quality/ImprovementActions/Update?ID=" + ret[0] + "&Year=" + ret[1]);
                }
                else
                {
                    ViewBag.result = false;
                }
            }
            return View();
        }

        public Boolean Delete(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/Delete", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/Delete", "", ipAddr);
            }

            Boolean ret = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if (ID != -1 && Year > 1970)
                {
                    ImprovementActions impActsList = new ImprovementActions(Session["ActiveWorkspace"].ToString());
                    ImprovementAction curr = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
                    curr.loadTeamMembers();
                    for (int i = 0; i < curr.TeamMembers.Count; i++)
                    {
                        curr.MemberRemove(curr.TeamMembers[i].User);
                    }

                    ret = impActsList.Delete(ID, Year);
                }
            }
            return ret;
        }

        public ActionResult Update(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/Update", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/Update", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for(int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if(!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                
                if(currImpAct.ID!=-1 && currImpAct.Year > 1970)
                { 
                    return View(currImpAct);
                }
            }
                return View();
        }

        public ActionResult ImprovementActionTeamMembers(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/ImprovementActionTeamMembers", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/ImprovementActionTeamMembers", "", ipAddr);
            }

            ViewBag.iActID = ID;
            ViewBag.iActYear = Year;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                ImprovementAction iAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
                if(iAct.ID!=-1 && iAct.Year!=-1)
                {
                    iAct.loadTeamMembers();
                    return View(iAct.TeamMembers);
                }
            }
                return View();
        }

        public Boolean TeamMemberRemove(int ID, int Year, int user)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/TeamMemberRemove", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/TeamMemberRemove", "", ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                ImprovementAction iAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
                if (iAct.ID != -1 && iAct.Year > 1970)
                {
                    User usrToDelete = new User(Session["ActiveWorkspace"].ToString(), user);
                    if(usrToDelete.username.Length > 0)
                    { 
                        ret = iAct.MemberRemove(usrToDelete.username);
                    }
                }
            }
            return ret;
        }

        public Boolean TeamMemberAdd(int ID, int Year, String user, Char role)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/TeamMemberAdd", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/TeamMemberAdd", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                ret = currImpAct.MemberAdd(user, role);
            }
            return ret;
        }

        /* Returns:
         * 0 if user does not have authorization
         * 1 if all goes right
         * 2 if tried to close improvement actions, but there are some opened corrective actions.
         */
        public int UpdateImprovementAction(int ID, int Year, String CurrentSituation, String ExpectedResults, String RootCauses, DateTime ClosurePlannedDate, String ClosureNotes, Char Status)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/UpdateImprovementAction", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/UpdateImprovementAction", "", ipAddr);
            }

            int ret = 0;
            ViewBag.authR = false;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                ImprovementAction iAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ID, Year);
                if(iAct.ID!=-1 && iAct.Year > 1970)
                {
                    ret = 1;
                    iAct.CurrentSituation = Server.HtmlEncode(CurrentSituation);
                    iAct.ExpectedResults = Server.HtmlEncode(ExpectedResults);
                    iAct.RootCauses = Server.HtmlEncode(RootCauses);

                    FusoOrario fuso = new FusoOrario(Session["ActiveWorkspace"].ToString());
                    if (TimeZoneInfo.ConvertTimeToUtc(ClosurePlannedDate, fuso.tzFusoOrario) > DateTime.UtcNow)
                    { 
                        iAct.EndDateExpected = ClosurePlannedDate;
                    }

                    iAct.ClosureNotes = Server.HtmlEncode(ClosureNotes);

                    Boolean checkClosure = true;
                    if (Status == 'C')
                    {
                        iAct.loadCorrectiveActions();
                        for (int i = 0; i < iAct.CorrectiveActions.Count; i++)
                        {
                            if (iAct.CorrectiveActions[i].Status != 'C')
                            {
                                checkClosure = false;
                            }
                        }
                    }
                    if(checkClosure == false) { ret = 2; }

                    if (iAct.Status == 'O' && Status == 'C')
                    {
                        iAct.Status = Status;
                        iAct.EndDateReal = DateTime.UtcNow;
                    }
                    else if(iAct.Status == 'C' && Status == 'O')
                    {
                        iAct.Status = Status;
                        iAct.EndDateReal = new DateTime(1970, 1, 1);
                    }

                    UserAccount curr = (UserAccount)Session["user"];
                    iAct.ModifiedBy = curr.userId;
                    iAct.ModifiedDate = DateTime.UtcNow;

                }
            }
                return ret;
        }

        public ActionResult CorrectiveActionsList(int ImprovementActionID, int ImprovementActionYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/CorrectiveActionsList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/CorrectiveActionsList", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            ViewBag.ImprovementActionID = ImprovementActionID;
            ViewBag.ImprovementActionYear = ImprovementActionYear;

            if (ViewBag.authR || ViewBag.authW)
            {
                ImprovementAction iAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
                if (iAct.ID != -1 && iAct.Year > 1970)
                {
                    iAct.loadCorrectiveActions();
                    return View(iAct.CorrectiveActions);
                }
            }
            return View();
        }

        public Boolean CorrectiveActionAdd(int ImprovementActionID, int ImprovementActionYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionAdd", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionAdd", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            int retAdd = -1;
            if (ViewBag.authW)
            {
                retAdd = currImpAct.CorrectiveActionAdd();
            }

            if(retAdd!=-1)
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public Boolean CorrectiveActionDelete(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionDelete", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionDelete", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                ImprovementAction iAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
                if (iAct.ID > -1 && iAct.Year > 1970 && CorrectiveActionID > -1)
                {
                    ret = iAct.CorrectiveActionRemove(CorrectiveActionID);
                }
            }
            return ret;
        }

        public ActionResult CorrectiveActionEdit(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/CorrectiveActionEdit", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/CorrectiveActionEdit", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            ViewBag.authX = false;

            if (ViewBag.authR || ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                cAct.loadTeamMembers();
                UserAccount curr = (UserAccount)Session["user"];
                for(int i =0; i < cAct.TeamMembers.Count; i++)
                {
                    if(cAct.TeamMembers[i].User == curr.userId && cAct.TeamMembers[i].Role == 'E')
                    {
                        ViewBag.authX = true;
                    }
                }
                if (cAct.ImprovementActionID!=-1 && cAct.ImprovementActionYear > 1970 && cAct.CorrectiveActionID!=-1)
                {
                    return View(cAct);
                }
            }
            return View();
        }

        public Boolean CorrectiveActionEditUpdate(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, 
            String Description, Double LeadTimeExpected, DateTime EndDateRequired, Char Status)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionEditUpdate", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionEditUpdate", "", ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                if (cAct.CorrectiveActionID != -1 && cAct.ImprovementActionID != -1 && cAct.ImprovementActionYear > 2000 && LeadTimeExpected >=0)
                {
                    cAct.Description = Server.HtmlEncode(Description);
                    cAct.LeadTimeExpected = LeadTimeExpected;
                    int intLeadTimeExpected = (Int32)Math.Round(LeadTimeExpected, 0);
                    DateTime startdate = Dati.Utilities.SubtractBusinessDays(EndDateRequired, intLeadTimeExpected);
                    cAct.EarlyStart = startdate;
                    cAct.LateStart = startdate;
                    cAct.EarlyFinish = EndDateRequired;
                    cAct.LateFinish = EndDateRequired;

                    if (Status == 'C' && (cAct.Status == 'I' || cAct.Status == 'O'))
                    {
                        cAct.Status = Status;
                        cAct.EndDateReal = DateTime.UtcNow;
                    }
                    else if ((Status == 'O' || Status == 'I') && cAct.Status == 'C')
                    {
                        cAct.Status = Status;
                        cAct.EndDateReal = new DateTime(1970, 1, 1);
                    }
                    cAct.Status = Status;

                    User currUsr = (User)Session["user"];
                    currImpAct.ModifiedBy = currUsr.username;
                    currImpAct.ModifiedDate = DateTime.UtcNow;
                    ret = true;
                }
            }
                return ret;
        }

        public ActionResult CorrectiveActionTeamMembers(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/CorrectiveActionTeamMembers", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/CorrectiveActionTeamMembers", "", ipAddr);
            }

            ViewBag.iActID = ImprovementActionID;
            ViewBag.iActYear = ImprovementActionYear;
            ViewBag.cActID = CorrectiveActionID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                if (cAct.ImprovementActionID != -1 && cAct.ImprovementActionYear != -1 && cAct.CorrectiveActionID!=-1)
                {
                    cAct.loadTeamMembers();
                    return View(cAct.TeamMembers);
                }
            }
            return View();
        }

        public Boolean CorrectiveActionTeamMemberAdd(int ImprovementActionID, int ImprovementActionYear, 
            int CorrectiveActionID, String user, Char role)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionTeamMemberAdd", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionTeamMemberAdd", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                CorrectiveAction curr = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                if(curr.ImprovementActionID!=-1 && curr.ImprovementActionYear > 2000 && curr.CorrectiveActionID!=-1)
                { 
                    ret = curr.MemberAdd(user, role);
                }
            }
            return ret;
        }

        public Boolean CorrectiveActionTeamMemberRemove(int ImprovementActionID, int ImprovementActionYear,
            int CorrectiveActionID, int user)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionTeamMemberRemove", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionTeamMemberRemove", "", ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                if (cAct.ImprovementActionID != -1 && cAct.ImprovementActionYear > 1970 && cAct.CorrectiveActionID != -1)
                {
                    User usrToDelete = new App_Code.User(Session["ActiveWorkspace"].ToString(), user);
                    if (usrToDelete.username.Length > 0) { 
                        ret = cAct.MemberRemove(usrToDelete.username);
                    }
                }
            }
            return ret;
        }

        public ActionResult ImprovementActionsProgress()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/ImprovementActionsProgress", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/ImprovementActionsProgress", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                ViewBag.authorized = true;
            }
            else
            {
            }

            return View();
        }

        public ActionResult ImprovementActionsProgressAndon()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/ImprovementActionsProgressAndon", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/ImprovementActionsProgressAndon", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementActions iActscActsList = new ImprovementActions(Session["ActiveWorkspace"].ToString());
            if (ViewBag.authR || ViewBag.authW)
            {
                ViewBag.authorized = true;
                iActscActsList.loadImprovementActions('O');
            }
            else
            {
                if (Session["user"] != null)
                {
                    ViewBag.authR = true;
                    iActscActsList.loadImprovementActions((User)Session["user"], 'O');
                }
            }
            var lstiActs = iActscActsList.ImprovementActionsList.OrderBy(x => x.EndDateExpected).ToList();
            return View(lstiActs);
        }

        public ActionResult CorrectiveActionTasksList(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/CorrectiveActionTasksList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/CorrectiveActionTasksList", "", ipAddr);
            }

            ViewBag.currUser = "";
            ViewBag.iActID = ImprovementActionID;
            ViewBag.iActYear = ImprovementActionYear;
            ViewBag.cActID = CorrectiveActionID;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
                ViewBag.currUser = curr.userId;
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
                ViewBag.currUser = curr.userId;
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.currUser = curr.userId;
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                cAct.loadTasks();
                return View(cAct.Tasks);
            }
                return View();
        }

        public Boolean CorrectiveActionTaskAdd(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, String Description, Char cActStatus)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionTaskAdd", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionTaskAdd", "", ipAddr);
            }

            Boolean ret = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            ViewBag.authX = false;

            if (ViewBag.authR || ViewBag.authW)
            {
                CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                cAct.loadTeamMembers();
                UserAccount curr = (UserAccount)Session["user"];
                for (int i = 0; i < cAct.TeamMembers.Count; i++)
                {
                    if (cAct.TeamMembers[i].User == curr.userId && cAct.TeamMembers[i].Role == 'E')
                    {
                        ViewBag.authX = true;
                    }
                }

                if(ViewBag.authX || ViewBag.authW)
                {
                    ret = cAct.TaskAdd(Server.HtmlEncode(Description), curr, DateTime.UtcNow);
                    if(cActStatus == 'O' || cActStatus == 'I'|| cActStatus == 'C')
                    { 
                        cAct.Status = cActStatus;
                    }
                }
            }
            return ret;
            }

        public Boolean CorrectiveActionTaskRemove(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CorrectiveActionTaskRemove", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CorrectiveActionTaskRemove", "", ipAddr);
            }

            Boolean ret = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                CorrectiveActionTask tsk = new CorrectiveActionTask(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID, TaskID);
                if(tsk.ImprovementActionID!=-1 && tsk.ImprovementActionYear > 2000 && tsk.TaskID!=-1)
                {
                    if(tsk.User == curr.userId)
                    {
                        CorrectiveAction cAct = new CorrectiveAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear, CorrectiveActionID);
                        ret = cAct.TaskRemove(TaskID);
                    }
                }
            }
            return ret;
        }

        public ActionResult IAFileList(int ImprovementActionID, int ImprovementActionYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/IAFileList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/IAFileList", "", ipAddr);
            }

            ViewBag.ImprovementActionID = ImprovementActionID;
            ViewBag.ImprovementActionYear = ImprovementActionYear;
            ViewBag.CombinedID = ImprovementActionYear + "_" + ImprovementActionID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            List<String> fileList = new List<string>();
            if (ViewBag.authR || ViewBag.authW)
            {
                if (System.IO.Directory.Exists(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString())))
                {
                    DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString()));
                    FileInfo[] Files = d.GetFiles();
                    foreach (FileInfo file in Files)
                    {
                        fileList.Add(file.Name);
                    }
                    ViewBag.Message = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
                    + " " + ImprovementActionID.ToString() + ImprovementActionYear.ToString();
                }
                else
                {
                }
            }
            return View(fileList);
        }

        [HttpPost]
        public int IAFileUpload()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/IAFileUpload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/IAFileUpload", "", ipAddr);
            }

            ViewBag.Message = "";

            /* 1 --> tutto ok
             * 0 --> file or non compliance not found
             * 2 --> filename already exists
             * 3 --> generic error 
             * 4 --> file size exceeded or not allowed extension
             * 5 --> file type not allowed
             */
            int ret = 0;
            String sNcID = Request.Form["ImprovementActionID"];
            String sNcYear = Request.Form["ImprovementActionYear"];
            int ncID = -1; int ncYear = -1;
            try
            {
                ncID = Int32.Parse(sNcID);
                ncYear = Int32.Parse(sNcYear);
            }
            catch (Exception ex)
            {
                ncID = -1;
                ncYear = -1;
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                ViewBag.fileSize = file.ContentLength;
                if (file != null && ncID != -1 && ncYear != -1 && file.ContentLength > 0)
                {
                    // Allowed extensions
                    var allowedExtensions = new[] { ".doc", ".docx", ".log", ".msg","odt.",".pages",".rtf",".tex",".txt", ".pdf",
                    ".csv",".dat",".key",".ppt",".pptx",".pps",".xml",
                    ".3gp",".avi",".mp4",".mpg",".mov",".wmv",
                    ".bmp",".gif",".jpg",".jpeg",".png",".tif",".tiff",
                    ".xls",".xlsx",
                    ".dbf",".mdb",".sql",
                    ".dwg",".dxf",
                    ".htm",".html"
                };
                    var extension = Path.GetExtension(file.FileName);

                    if (file.ContentLength < (20480 * 1024))
                    {
                        if (allowedExtensions.Contains(extension.ToLower()))
                        {

                            DirectoryInfo di = null;
                            if (!Directory.Exists(Server.MapPath("~/Data/Quality")))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality"));
                            }

                            if (!Directory.Exists(Server.MapPath("~/Data/Quality/IA_" + ncYear.ToString() + "_" + ncID.ToString())))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality/IA_" + ncYear.ToString() + "_" + ncID.ToString()));
                            }
                            if (!System.IO.File.Exists(Server.MapPath("~/Data/Quality/IA_" + ncYear.ToString() + "_" + ncID.ToString() + "/" + file.FileName)))
                            {
                                try
                                {
                                    string pic = System.IO.Path.GetFileName(file.FileName);
                                    string path = System.IO.Path.Combine(Server.MapPath("~/Data/Quality/IA_" + ncYear.ToString() + "_" + ncID.ToString()), pic);
                                    // file is uploaded
                                    file.SaveAs(path);
                                    ret = 1;
                                }
                                catch (Exception ex)
                                {
                                    // Generic error
                                    ret = 3;
                                    ViewBag.Message = ex.Message;
                                }
                            }
                            else
                            {
                                // Filename already exists
                                ret = 2;
                            }

                        }
                        else
                        {
                            ret = 5;
                        }
                    }
                    else
                    {
                        ret = 4;
                    }
                }
                else
                {
                    // file or non compliance not found
                    ret = 0;
                }

            }
            return ret;
        }

        public Boolean IADeleteFile(int ImprovementActionID, int ImprovementActionYear, String fileName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/IADeleteFile", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/IADeleteFile", "", ipAddr);
            }

            Boolean ret = false;
            if (Directory.Exists(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString()));
                if (System.IO.File.Exists(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "/" + fileName)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Data/Quality/IA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "/" + fileName));
                    ret = true;
                }
            }
            return ret;
        }

        public ActionResult CAFileList(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/CAFileList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/CAFileList", "", ipAddr);
            }

            ViewBag.ImprovementActionID = ImprovementActionID;
            ViewBag.ImprovementActionYear = ImprovementActionYear;
            ViewBag.CorrectiveActionID = CorrectiveActionID;
            ViewBag.CombinedID = ImprovementActionYear + "_" + ImprovementActionID + "_" + CorrectiveActionID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            List<String> fileList = new List<string>();
            if (ViewBag.authR || ViewBag.authW)
            {
                if (System.IO.Directory.Exists(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString())))
                {
                    DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString()));
                    FileInfo[] Files = d.GetFiles();
                    foreach (FileInfo file in Files)
                    {
                        fileList.Add(file.Name);
                    }
                    ViewBag.Message = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
                    + " " + ImprovementActionID.ToString() + ImprovementActionYear.ToString() + " " + CorrectiveActionID.ToString();
                }
                else
                {
                }
            }
            return View(fileList);
        }

        [HttpPost]
        public int CAFileUpload()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CAFileUpload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CAFileUpload", "", ipAddr);
            }

            ViewBag.Message = "";

            /* 1 --> tutto ok
             * 0 --> file or non compliance not found
             * 2 --> filename already exists
             * 3 --> generic error 
             * 4 --> file size exceeded or not allowed extension
             * 5 --> file type not allowed
             */
            int ret = 0;
            String sNcID = Request.Form["ImprovementActionID"];
            String sNcYear = Request.Form["ImprovementActionYear"];
            String sCAID = Request.Form["CorrectiveActionID"];
            int ncID = -1; int ncYear = -1; int caID = -1;
            try
            {
                ncID = Int32.Parse(sNcID);
                ncYear = Int32.Parse(sNcYear);
                caID = Int32.Parse(sCAID);
            }
            catch (Exception ex)
            {
                ncID = -1;
                ncYear = -1;
                caID = 1;
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                ViewBag.fileSize = file.ContentLength;
                if (file != null && ncID != -1 && ncYear > 2000 && caID!=-1 && file.ContentLength > 0)
                {
                    // Allowed extensions
                    var allowedExtensions = new[] { ".doc", ".docx", ".log", ".msg","odt.",".pages",".rtf",".tex",".txt", ".pdf",
                    ".csv",".dat",".key",".ppt",".pptx",".pps",".xml",
                    ".3gp",".avi",".mp4",".mpg",".mov",".wmv",
                    ".bmp",".gif",".jpg",".jpeg",".png",".tif",".tiff",
                    ".xls",".xlsx",
                    ".dbf",".mdb",".sql",
                    ".dwg",".dxf",
                    ".htm",".html"
                };
                    var extension = Path.GetExtension(file.FileName);

                    if (file.ContentLength < (20480 * 1024))
                    {
                        if (allowedExtensions.Contains(extension.ToLower()))
                        {

                            DirectoryInfo di = null;
                            if (!Directory.Exists(Server.MapPath("~/Data/Quality")))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality"));
                            }

                            if (!Directory.Exists(Server.MapPath("~/Data/Quality/CA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString())))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality/CA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString()));
                            }
                            if (!System.IO.File.Exists(Server.MapPath("~/Data/Quality/CA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString() + "/" + file.FileName)))
                            {
                                try
                                {
                                    string pic = System.IO.Path.GetFileName(file.FileName);
                                    string path = System.IO.Path.Combine(Server.MapPath("~/Data/Quality/CA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString()), pic);
                                    // file is uploaded
                                    file.SaveAs(path);
                                    ret = 1;
                                }
                                catch (Exception ex)
                                {
                                    // Generic error
                                    ret = 3;
                                    ViewBag.Message = ex.Message;
                                }
                            }
                            else
                            {
                                // Filename already exists
                                ret = 2;
                            }

                        }
                        else
                        {
                            ret = 5;
                        }
                    }
                    else
                    {
                        ret = 4;
                    }
                }
                else
                {
                    // file or non compliance not found
                    ret = 0;
                }

            }
            return ret;
        }

        public Boolean CADeleteFile(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, String fileName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/CADeleteFile", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/CADeleteFile", "", ipAddr);
            }

            Boolean ret = false;
            if (Directory.Exists(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString()));
                if (System.IO.File.Exists(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString() + "/" + fileName)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Data/Quality/CA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString() + "/" + fileName));
                    ret = true;
                }
            }
            return ret;
        }

        public ActionResult TaskCAFileList(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/ImprovementActions/TaskCAFileList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/ImprovementActions/TaskCAFileList", "", ipAddr);
            }

            ViewBag.ImprovementActionID = ImprovementActionID;
            ViewBag.ImprovementActionYear = ImprovementActionYear;
            ViewBag.CorrectiveActionID = CorrectiveActionID;
            ViewBag.CombinedID = ImprovementActionYear + "_" + ImprovementActionID + "_" + CorrectiveActionID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi = new List<String[]>();
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ImprovementAction currImpAct = new ImprovementAction(Session["ActiveWorkspace"].ToString(), ImprovementActionID, ImprovementActionYear);
            if ((!ViewBag.authR || !ViewBag.authW) && Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                currImpAct.loadTeamMembers();
                for (int i = 0; i < currImpAct.TeamMembers.Count; i++)
                {
                    if (currImpAct.TeamMembers[i].User == curr.userId)
                    {
                        if (!ViewBag.authR && currImpAct.TeamMembers[i].Role == 'T')
                        {
                            ViewBag.authR = true;
                        }

                        if (!ViewBag.authW && currImpAct.TeamMembers[i].Role == 'M')
                        {
                            ViewBag.authW = true;
                        }
                    }
                }
            }

            List<String> fileList = new List<string>();
            if (ViewBag.authR || ViewBag.authW)
            {
                if (System.IO.Directory.Exists(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString())))
                {
                    DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString()));
                    FileInfo[] Files = d.GetFiles();
                    foreach (FileInfo file in Files)
                    {
                        fileList.Add(file.Name);
                    }
                    ViewBag.Message = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
                    + " " + ImprovementActionID.ToString() + ImprovementActionYear.ToString() + " " + CorrectiveActionID.ToString();
                }
                else
                {
                }
            }
            return View(fileList);
        }

        public Boolean TaskCADeleteFile(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, String fileName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/TaskCADeleteFile", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/TaskCADeleteFile", "", ipAddr);
            }

            Boolean ret = false;
            if (Directory.Exists(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString()));
                if (System.IO.File.Exists(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString() + "/" + fileName)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Data/Quality/TCA_" + ImprovementActionYear.ToString() + "_" + ImprovementActionID.ToString() + "_" + CorrectiveActionID.ToString() + "/" + fileName));
                    ret = true;
                }
            }
            return ret;
        }

        [HttpPost]
        public int TaskCAFileUpload()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/ImprovementActions/TaskCAFileUpload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/ImprovementActions/TaskCAFileUpload", "", ipAddr);
            }

            ViewBag.Message = "";

            /* 1 --> tutto ok
             * 0 --> file or non compliance not found
             * 2 --> filename already exists
             * 3 --> generic error 
             * 4 --> file size exceeded or not allowed extension
             * 5 --> file type not allowed
             */
            int ret = 0;
            String sNcID = Request.Form["ImprovementActionID"];
            String sNcYear = Request.Form["ImprovementActionYear"];
            String sCAID = Request.Form["CorrectiveActionID"];
            int ncID = -1; int ncYear = -1; int caID = -1;
            try
            {
                ncID = Int32.Parse(sNcID);
                ncYear = Int32.Parse(sNcYear);
                caID = Int32.Parse(sCAID);
            }
            catch (Exception ex)
            {
                ncID = -1;
                ncYear = -1;
                caID = 1;
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                ViewBag.fileSize = file.ContentLength;
                if (file != null && ncID != -1 && ncYear > 2000 && caID != -1 && file.ContentLength > 0)
                {
                    // Allowed extensions
                    var allowedExtensions = new[] { ".doc", ".docx", ".log", ".msg","odt.",".pages",".rtf",".tex",".txt", ".pdf",
                    ".csv",".dat",".key",".ppt",".pptx",".pps",".xml",
                    ".3gp",".avi",".mp4",".mpg",".mov",".wmv",
                    ".bmp",".gif",".jpg",".jpeg",".png",".tif",".tiff",
                    ".xls",".xlsx",
                    ".dbf",".mdb",".sql",
                    ".dwg",".dxf",
                    ".htm",".html"
                };
                    var extension = Path.GetExtension(file.FileName);

                    if (file.ContentLength < (20480 * 1024))
                    {
                        if (allowedExtensions.Contains(extension.ToLower()))
                        {

                            DirectoryInfo di = null;
                            if (!Directory.Exists(Server.MapPath("~/Data/Quality")))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality"));
                            }

                            if (!Directory.Exists(Server.MapPath("~/Data/Quality/TCA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString())))
                            {
                                di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality/TCA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString()));
                            }
                            if (!System.IO.File.Exists(Server.MapPath("~/Data/Quality/TCA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString() + "/" + file.FileName)))
                            {
                                try
                                {
                                    string pic = System.IO.Path.GetFileName(file.FileName);
                                    string path = System.IO.Path.Combine(Server.MapPath("~/Data/Quality/TCA_" + ncYear.ToString() + "_" + ncID.ToString() + "_" + caID.ToString()), pic);
                                    // file is uploaded
                                    file.SaveAs(path);
                                    ret = 1;
                                }
                                catch (Exception ex)
                                {
                                    // Generic error
                                    ret = 3;
                                    ViewBag.Message = ex.Message;
                                }
                            }
                            else
                            {
                                // Filename already exists
                                ret = 2;
                            }

                        }
                        else
                        {
                            ret = 5;
                        }
                    }
                    else
                    {
                        ret = 4;
                    }
                }
                else
                {
                    // file or non compliance not found
                    ret = 0;
                }

            }
            return ret;
        }
    }
}