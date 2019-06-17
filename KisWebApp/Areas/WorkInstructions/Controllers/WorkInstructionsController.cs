using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using jQuery_File_Upload.MVC5.Helpers;
using jQuery_File_Upload.MVC5.Models;
using KIS.App_Sources;
using KIS.App_Code;
using Newtonsoft.Json;

namespace KIS.Areas.WorkInstructions.Controllers
{
    public class WorkInstructionsController : Controller
    {
        // GET: WorkInstructions/WorkInstructions
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/WorkInstructions/WorkInstructions/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/WorkInstructions/WorkInstructions/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authR)
            {
                if (ViewBag.authW)
                {
                    // Show upload panel
                }

                // Show list
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList();
                wiList.loadWorkInstructionList();
                return View(wiList.List);
            }

            return View();
        }

        public ActionResult TestUpload()
        {
            return View();
        }

        /* Results:
         * -2 if user is not authorized
         * 0 if generic error
         * ManualID if all is ok
         */
        public int AddWorkInstruction(String name, String description)
        {
            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }
            if (ViewBag.authW)
            {
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList();
                int[] retWI = wiList.Add(name, description, "", ((User)Session["user"]).username);
                if (retWI[0] != -1)
                {
                    ret = retWI[0];
                }
                else
                {
                    ret = 0;
                }
            }
            else
            {
                ret = -2;
            }
            return ret;
        }

        public JsonResult getJsonWorkInstructions(String param)
        {
            List<JsonManual> manualLst = new List<JsonManual>();
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }
            if (ViewBag.authR)
            {
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList();
                wiList.loadWorkInstructionList();
                for (int i = 0; i < wiList.List.Count; i++)
                {
                    JsonManual curr = new JsonManual();
                    curr.ID = wiList.List[i].ID;
                    curr.Version = wiList.List[i].Version;
                    curr.Name = wiList.List[i].Name;
                    curr.Description = wiList.List[i].Description;
                    curr.UploadDate = wiList.List[i].UploadDate;
                    curr.ExpiryDate = wiList.List[i].ExpiryDate;

                    manualLst.Add(curr);
                }
            }
            return Json(JsonConvert.SerializeObject(manualLst), JsonRequestBehavior.AllowGet);
        }

        public ActionResult WiEdit(int manualID)
        {
            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = null;
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/WorkInstructions/WorkInstructions/WiEdit", "manualID=" + manualID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/WorkInstructions/WorkInstructions/Index", "manualID=" + manualID, ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                currWI = new App_Sources.WorkInstructions.WorkInstruction(manualID);
            }

            return View(currWI);
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized
         * 3 if WorkInstruction not found
         */
        public int EditWorkInstruction(int ID, int Version, String Name, String Description, DateTime ExpiryDate, Boolean IsActive)
        {
            int ret = 0;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "WorkInstructions Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(ID, Version);
                if(currWI.ID!=-1)
                {
                    currWI.Name = Name;
                    currWI.Description = Description;
                    currWI.ExpiryDate = ExpiryDate;
                    currWI.IsActive = IsActive;
                    ret = 1;
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
        
    //Comment
    public class FileUploadController : Controller
    {
        FilesHelper filesHelper;
        String tempPath = "~/Data/WorkInstructions/tmp";
        String serverMapPath = "~/Data/WorkInstructions";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/Data/WorkInstructions/";
        String DeleteURL = "/FileUpload/DeleteFile/?file=";
        String DeleteType = "GET";
        public FileUploadController()
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }


        [HttpPost]
        public JsonResult Upload(String action)
        {
            var resultList = new List<ViewDataUploadFilesResult>();

            var CurrentContext = HttpContext;

            filesHelper.UploadAndShowResults(CurrentContext, resultList);
            JsonFiles files = new JsonFiles(resultList);

            for(int i=0; i < resultList.Count; i++)
            {
                if(action == "add")
                {

                }
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList();
                int[] res = wiList.Add(resultList[i].name, "", resultList[i].name, ((KIS.App_Code.User)Session["user"]).username);
                if(res[0]!=-1 && res[1]!=-1)
                {
                    System.IO.File.Move(HostingEnvironment.MapPath(serverMapPath) +"/"+ resultList[i].name, 
                        HostingEnvironment.MapPath(serverMapPath) + "/" + res[0]+"_"+res[1]+".pdf");
                    App_Sources.WorkInstructions.WorkInstruction curr = new App_Sources.WorkInstructions.WorkInstruction(res[0], res[1]);
                    curr.Path = res[0] + "_" + res[1] + ".pdf";

                }
            }

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
            }
        }
        public JsonResult GetFileList()
        {
            var list = filesHelper.GetFileList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }




    }


    public class JsonManual
    {
        public int ID;
        public int Version;
        public String Name;
        public String Description;
        public DateTime UploadDate;
        public DateTime ExpiryDate;
    }
}