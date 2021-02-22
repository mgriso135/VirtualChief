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
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList(Session["ActiveWorkspace"].ToString());
                wiList.loadWorkInstructionList();
                return View(wiList.List);
            }

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
                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList(Session["ActiveWorkspace"].ToString());
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
                List<int> ids = new List<int>();
                var strArr = param.Split(';');
                foreach(var m in strArr)
                {
                    try
                    {
                        ids.Add(Int32.Parse(m));
                    }
                    catch
                    { }
                }

                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList(Session["ActiveWorkspace"].ToString(), ids);
                //wiList.loadWorkInstructionList();
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

        public ActionResult WiEdit(int manualID, int manualRev)
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
                currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), manualID, manualRev);
                currWI.loadLabels();
                currWI.loadTaskProducts();
                currWI.loadOlderVersions();
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
                KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), ID, Version);
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

        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         */
        public int AddLabelById(int ID, int Version, int LabelID)
        {
            int ret = 0;
            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), ID, Version);
            if(currWI.ID!=-1 && currWI.Version!=-1)
            {
                bool bret = currWI.addLabel(LabelID);
                if (bret) { ret = 1; }
            }
            return ret;
        }

        /* Returns:
 * 0 if generic error
 * -2 if label not added correctly
 * -3 if label not correctly linked to WorkInstruction
 * -4 if WorkInstruction not found
 * LabelID if all is ok
 */
        public int AddLabelByName(int ID, int Version, String LabelName)
        {
            int ret = 0;
            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), ID, Version);
            if (currWI.ID != -1 && currWI.Version != -1)
            {
                KIS.App_Sources.WorkInstructions.WILabelList lblList = new App_Sources.WorkInstructions.WILabelList(Session["ActiveWorkspace"].ToString());
                lblList.loadLabelsList();
                int LabelID = -1;
                try
                { 
                    var found = lblList.List.First(x => x.WILabelName == LabelName);
                    LabelID = found.WILabelID;
                }
                catch
                {
                    LabelID = -1;
                }

                if(LabelID==-1)
                {
                    LabelID = lblList.addLabel(LabelName);
                }

                if(LabelID!=-1)
                { 
                bool bret = currWI.addLabel(LabelID);
                    ret = bret ? LabelID : -3;
                }
                else
                {
                    ret = -2;
                }
            }
            else
            {
                ret = -4;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized
         * 3 if error while deleting
         */
        public int DeleteLabel(int ID, int Version, int LabelID)
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
                KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), ID, Version);
                bool bret = currWI.deleteLabel(LabelID);
                ret = bret ? 1 : 3;
            }
            else
            {
                ret = 2;
            }
                return ret;
        }

        public JsonResult getAllLabels()
        {
            List<WILabels> retW = new List<WILabels>();
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

            if (Session["user"] != null)
            {
                if(ViewBag.authR)
                {
                    KIS.App_Sources.WorkInstructions.WILabelList currList = new App_Sources.WorkInstructions.WILabelList(Session["ActiveWorkspace"].ToString());
                    currList.loadLabelsList();
                    for(int i =0; i < currList.List.Count; i++)
                    {
                        WILabels curr = new WILabels();
                        curr.LabelID = currList.List[i].WILabelID;
                        curr.LabelName = currList.List[i].WILabelName;
                        retW.Add(curr);
                    }
                }
            }

            return Json(retW) ;

            }

    }
        

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
        public JsonResult Upload(String action, String NewVerInitialDate, String NewVerExpiryDate, int origManualID)
        {
            var resultList = new List<ViewDataUploadFilesResult>();

            var CurrentContext = HttpContext;

            String[] aExpDate = NewVerExpiryDate.Split('/');
            String[] aInitDate = NewVerInitialDate.Split('/');
            DateTime initial = DateTime.UtcNow;
            DateTime expiry = new DateTime(2099, 12, 31);

            bool checkdates = true;
            try
            {
                initial = new DateTime(Int32.Parse(aInitDate[2]), Int32.Parse(aInitDate[1]), Int32.Parse(aInitDate[0]));
                expiry = new DateTime(Int32.Parse(aExpDate[2]), Int32.Parse(aExpDate[1]), Int32.Parse(aExpDate[0]));
                checkdates = true;
            }
        
            catch
            {
                initial = DateTime.UtcNow;
                expiry = new DateTime(2099, 12, 31);
                checkdates = false;
            }

            if(action == "add")
            {
                initial = DateTime.UtcNow.AddDays(1);
            }

            if (checkdates && initial >= DateTime.UtcNow.AddDays(-1) && initial < expiry)
            {
                checkdates = true;
            }
            else
            {
                checkdates = false;
            }

            if(checkdates && action== "NewVersion")
            {
                KIS.App_Sources.WorkInstructions.WorkInstruction wi = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), origManualID);
                wi.loadTaskProducts();
                for(int i = 0; i < wi.listTasksProducts.Count && checkdates; i++)
                {
                    if(wi.listTasksProducts[i].ExpiryDate.AddDays(1) >= expiry)
                    {
                        checkdates = false;
                    }
                    if(initial < wi.listTasksProducts[i].ExpiryDate.AddDays(1))
                    {
                        initial = wi.listTasksProducts[i].ExpiryDate.AddDays(1);
                    }
                }
            }


            if (checkdates)
            {
                filesHelper.UploadAndShowResults(CurrentContext, resultList);
                for (int i=0; i < resultList.Count; i++)
                {
                    if(checkdates)
                    {
                        if (resultList[i].name.Substring(resultList[i].name.Length - 4, 4) == ".pdf")
                        {
                            if (action == "add")
                            {
                                KIS.App_Sources.WorkInstructions.WorkInstructionsList wiList = new App_Sources.WorkInstructions.WorkInstructionsList(Session["ActiveWorkspace"].ToString());
                                int[] res = wiList.Add(resultList[i].name, "", resultList[i].name, ((KIS.App_Code.User)Session["user"]).username);
                                if (res[0] != -1 && res[1] != -1)
                                {
                                    System.IO.File.Move(HostingEnvironment.MapPath(serverMapPath) + "/" + resultList[i].name,
                                    HostingEnvironment.MapPath(serverMapPath) + "/" + res[0] + "_" + res[1] + ".pdf");
                                    App_Sources.WorkInstructions.WorkInstruction curr = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), res[0], res[1]);
                                    curr.Path = res[0] + "_" + res[1] + ".pdf";
                                    curr.ExpiryDate = expiry;
                                }
                            }
                            else if(action == "NewVersion")
                            {
                                KIS.App_Sources.WorkInstructions.WorkInstruction wi = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), origManualID);
                                if(wi!=null && wi.ID!=-1 && wi.Version>=0)
                                { 

                                int[] res = wi.ReviewManual(resultList[i].name, initial, expiry, ((KIS.App_Code.User)Session["user"]).username);
                                if (res[0] >= 0 && res[1] >= 0)
                                {
                                        
                                    System.IO.File.Move(HostingEnvironment.MapPath(serverMapPath) + "/" + resultList[i].name,
                                    HostingEnvironment.MapPath(serverMapPath) + "/" + res[0] + "_" + res[1] + ".pdf");
                                    App_Sources.WorkInstructions.WorkInstruction curr = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace"].ToString(), res[0], res[1]);
                                    curr.Path = res[0] + "_" + res[1] + ".pdf";
                                    curr.ExpiryDate = expiry;
                                }
                                }
                                else
                                {
                                    resultList[i].error = ResWorkInstructions.WiEdit.lblErrOrigNotFound;
                                }
                            }
               
                        }
                        else
                        {
                            // If it's not a pdf, delete it.
                            System.IO.File.Delete(HostingEnvironment.MapPath(serverMapPath) + "/" + resultList[i].name);
                            resultList[i].error = ResWorkInstructions.Index.lblErrorFileType;
                        }
                    }
                    else
                    {
                        resultList[i].error = ResWorkInstructions.Index.lblErrorExpiryDate;
                    }
                }
            }
            else
            {
                resultList = new List<ViewDataUploadFilesResult>();
                ViewDataUploadFilesResult curr = new ViewDataUploadFilesResult();
                curr.error = ResWorkInstructions.Index.lblErrorExpiryDate;
                resultList.Add(curr);
            }

            // Delete all thumbs
            try
            { 
            System.IO.DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath(serverMapPath) + "/thumbs/");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            }
            catch { }
            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error isEmpty");
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

    public struct WILabels
    {
        public int LabelID;
        public String LabelName;
    }
}