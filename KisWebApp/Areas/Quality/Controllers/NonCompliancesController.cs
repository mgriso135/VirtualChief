using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;
using System.IO;

namespace KIS.Areas.Quality.Controllers
{
    public class NonCompliancesController : Controller
    {
        // GET: Quality/NonCompliances
        [Authorize]
        public ActionResult Index(Char stat, String sortOrder)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/Index", "stat=" + stat+"&sortOrder="+sortOrder, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/Index", "stat=" + stat + "&sortOrder=" + sortOrder, ipAddr);
            }

            ViewBag.authorized = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authorized = true;

                NonCompliances lstNC = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                lstNC.loadNonCompliances();
                for (int i = 0; i < lstNC.NonCompliancesList.Count; i++)
                {
                    ViewBag.Message += lstNC.NonCompliancesList[i].log + "<br />";
                }
                List<NonCompliance> ncList = lstNC.NonCompliancesList;
                ViewBag.filterStat = stat;
                if (stat != '\0' && stat != 'A')
                {
                    ncList = ncList.Where(s => s.Status == stat).ToList();
                }

                ViewBag.sortID = sortOrder == "sortID" ? "sortID_desc" : "sortID";
                ViewBag.sortOpeningDate = sortOrder == "sortOpeningDate" ? "sortOpeningDate_desc" : "sortOpeningDate";
                ViewBag.sortUser = sortOrder == "sortUser" ? "sortUser_desc" : "sortUser";
                ViewBag.sortDescription = sortOrder == "sortDescription" ? "sortDescription_desc" : "sortDescription";
                ViewBag.sortImmAction = sortOrder == "sortImmAction" ? "sortImmAction_desc" : "sortImmAction";
                ViewBag.sortCost = sortOrder == "sortCost" ? "sortCost_desc" : "sortCost";
                ViewBag.sortStatus = sortOrder == "sortStatus" ? "sortStatus_desc" : "sortStatus";
                ViewBag.sortClosureDate = sortOrder == "sortClosureDate" ? "sortClosureDate_desc" : "sortClosureDate";

                switch (sortOrder)
                {
                    case "sortID":
                        ncList = ncList.OrderBy(s => s.Year).ThenBy(x => x.ID).ToList();
                        break;
                    case "sortID_desc":
                        ncList = ncList.OrderByDescending(s => s.Year).ThenByDescending(x => x.ID).ToList();
                        break;
                    case "sortOpeningDate_desc":
                        ncList = ncList.OrderByDescending(s => s.OpeningDate).ToList();
                        break;
                    case "sortOpeningDate":
                        ncList = ncList.OrderBy(s => s.OpeningDate).ToList();
                        break;
                    case "sortUser":
                        ncList = ncList.OrderBy(s => s.user.cognome).ThenBy(z=>z.user.name).ToList();
                        break;
                    case "sortUser_desc":
                        ncList = ncList.OrderByDescending(s => s.user.cognome).ThenByDescending(z => z.user.name).ToList();
                        break;
                    case "sortDescription":
                        ncList = ncList.OrderBy(s => s.Description).ToList();
                        break;
                    case "sortDescription_desc":
                        ncList = ncList.OrderByDescending(s => s.Description).ToList();
                        break;
                    case "sortImmAction":
                        ncList = ncList.OrderBy(s => s.ImmediateAction).ToList();
                        break;
                    case "sortImmAction_desc":
                        ncList = ncList.OrderByDescending(s => s.ImmediateAction).ToList();
                        break;
                    case "sortCost":
                        ncList = ncList.OrderBy(s => s.Cost).ToList();
                        break;
                    case "sortCost_desc":
                        ncList = ncList.OrderByDescending(s => s.Cost).ToList();
                        break;
                    case "sortStatus":
                        ncList = ncList.OrderBy(s => s.Status).ToList();
                        break;
                    case "sortStatus_desc":
                        ncList = ncList.OrderByDescending(s => s.Status).ToList();
                        break;
                    case "sortClosureDate":
                        ncList = ncList.OrderBy(s => s.ClosureDate).ToList();
                        break;
                    case "sortClosureDate_desc":
                        ncList = ncList.OrderByDescending(s => s.ClosureDate).ToList();
                        break;
                    default:
                        ncList = ncList.OrderByDescending(s => s.Year).ThenByDescending(a => a.ID).ToList();
                        break;
                }

                return View(ncList);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/Create", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/Create", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                UserAccount curr = (UserAccount)Session["user"];
                var created = ncList.Add(curr.userId);
                if(created[0]!=-1 && created[1]!=-1)
                {
                    ViewBag.created = true;
                    Response.Redirect("~/Quality/NonCompliances/Update?ID=" + created[0] + "&Year=" + created[1]);
                }
                else
                {
                    ViewBag.created = false;
                    ViewBag.Message = ncList.log;
                }
            }
                        return View();
        }

        [Authorize]
        public ActionResult Update(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/Update", "ID=" + ID+"&Year="+Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/Update", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.catList = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nonC = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                NonComplianceTypes ncCats = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
                ncCats.loadTypeList();
                ViewBag.catList = ncCats.TypeList;

                NonComplianceCauses ncCauses = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                ncCauses.loadCausesList();
                ViewBag.causeList = ncCauses.CausesList;

                return View(nonC);
            }
            else { 
                return View();
            }
        }

        [Authorize]
        public Boolean Delete(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/Delete", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/Delete", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            Boolean ret = false;
            if(ID!=-1 && Year>1970)
            {
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                nc.CategoryLoad();
                for(int i = 0; i < nc.Categories.Count; i++)
                {
                    nc.CategoryDel(nc.Categories[i].ID);
                }
                nc.CauseLoad();
                for(int i = 0; i < nc.Causes.Count; i++)
                {
                    nc.CauseDel(nc.Causes[i].ID);
                }

                nc.FilesLoad();
                for(int i = 0; i < nc.Files.Count; i++)
                {
                    nc.FileDelete(nc.Files[i]);
                }

                nc.ProductsLoad();
                for(int i =0; i < nc.Products.Count; i++)
                {
                    nc.ProductDel(nc.Products[i].ProductID, nc.Products[i].ProductYear);
                }

                ret = ncList.Delete(ID, Year);
                
            }
            return ret;
        }

        [HttpPost]
        public int UpdateNC(int ID, int Year, String desc, String immAction, double cost, char status)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/UpdateNC", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/UpdateNC", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            int ret = 0;
            NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
            if(nc.ID!=-1 && nc.Year > 0)
            {
                
                ViewBag.Message = "status: "+status + " nc.Status: " + nc.Status + " ";
                if (status == 'C' && nc.Status != 'C')
                {
                    nc.ClosureDate = DateTime.UtcNow;
                    ViewBag.Message += nc.log;
                }
                nc.Status = status;
                nc.Cost = cost;
                nc.Description = Server.HtmlEncode(desc);
                nc.ImmediateAction = Server.HtmlEncode(immAction);
                ret = 1;
            }
            return ret;
        }

        [Authorize]
        public ActionResult NCFileList(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/NCFileList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/NCFileList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.NCID = ID;
            ViewBag.NCYear = Year;
            ViewBag.CombinedID = Year + "_" + ID;

            List<String> fileList = new List<string>();
            if(System.IO.Directory.Exists(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString()));
                FileInfo[] Files = d.GetFiles();
                foreach (FileInfo file in Files)
                {
                    fileList.Add(file.Name);
                }
                ViewBag.Message = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
                + " " + ID.ToString() + Year.ToString();
            }
            else
            {
            }
            return PartialView(fileList);
        }

        [HttpPost]
        public int FileUpload()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/FileUpload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/FileUpload", "", ipAddr);
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
            String sNcID = Request.Form["ID"];
            String sNcYear = Request.Form["Year"];
            int ncID = -1; int ncYear = -1;
            try
            {
                ncID = Int32.Parse(sNcID);
                ncYear = Int32.Parse(sNcYear);
            }
            catch(Exception ex)
            {
                ncID = -1;
                ncYear = -1;
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
            ViewBag.fileSize = file.ContentLength;
            if (file != null && ncID != -1 && ncYear != -1 && file.ContentLength>0)
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

                        if (!Directory.Exists(Server.MapPath("~/Data/Quality/NC_" + ncYear.ToString() + "_" + ncID.ToString())))
                        {
                            di = Directory.CreateDirectory(Server.MapPath("~/Data/Quality/NC_" + ncYear.ToString() + "_" + ncID.ToString()));
                        }
                        if (!System.IO.File.Exists(Server.MapPath("~/Data/Quality/NC_" + ncYear.ToString() + "_" + ncID.ToString() + "/" + file.FileName)))
                        {
                            try
                            {
                                string pic = System.IO.Path.GetFileName(file.FileName);
                                string path = System.IO.Path.Combine(Server.MapPath("~/Data/Quality/NC_" + ncYear.ToString() + "_" + ncID.ToString()), pic);
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

        [Authorize]
        public String NCDeleteFile(int ID, int Year, String fileName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCDeleteFile", "ID=" + ID + "&Year="+Year+"&fileName="+fileName, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCDeleteFile", "ID=" + ID + "&Year=" + Year + "&fileName=" + fileName, ipAddr);
            }

            if (Directory.Exists(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString()));
                    if (System.IO.File.Exists(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString() + "/" + fileName)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Data/Quality/NC_" + Year.ToString() + "_" + ID.ToString() + "/" + fileName));
                        Response.Redirect("~/Quality/NonCompliances/Update?ID=" + ID.ToString() + "&Year=" + Year.ToString());
                }
            }
            return "Error while deleting";
        }

        [Authorize]
        public ActionResult NCCategoriesList(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/NCCategoriesList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/NCCategoriesList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.NCID = ID;
            ViewBag.NCYear = Year;
            ViewBag.CombinedID = Year + "_" + ID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nonC = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                nonC.CategoryLoad();
                return PartialView(nonC);
            }
            return PartialView();
        }

        [HttpPost]
        public int NCCategoriesAdd(int ID, int Year, String catName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCCategoriesAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCCategoriesAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            int ret = 0;
            if(ID!=-1 && Year != -1 && catName.Length>0)
            {
                NonComplianceTypes ncCatList = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
                int catID = ncCatList.findIDByName(Server.HtmlEncode(catName));
                Boolean ncAddCheck = false;
                if (catID == -1)
                {
                    ncAddCheck = ncCatList.Add(Server.HtmlEncode(catName), "");
                }

                NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                Boolean ncLinkCheck = nc.CategoryAdd(Server.HtmlEncode(catName));
                ret = ncLinkCheck ? 1 : 4;
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        [HttpPost]
        public int NCCategoriesDel(int ID, int Year, int catID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCCategoriesDel", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCCategoriesDel", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            int ret = 0;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if (ID != -1 && Year != -1 && catID != -1)
                {
                    NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                    Boolean ncLinkCheck = nc.CategoryDel(catID);
                    ret = ncLinkCheck ? 1 : 4;
                }
                else
                {
                    ret = 2;
                }
            }
            return ret;
        }

        [Authorize]
        public ActionResult NCCausesList(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/NCCausesList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/NCCausesList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.NCID = ID;
            ViewBag.NCYear = Year;
            ViewBag.CombinedID = Year + "_" + ID;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nonC = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                nonC.CauseLoad();
                return PartialView(nonC);
            }
            return PartialView();
        }

        [HttpPost]
        public int NCCausesAdd(int ID, int Year, String causeName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCCausesAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCCausesAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            int ret = 0;
            if (ID != -1 && Year != -1 && causeName.Length>0)
            {
                NonComplianceCauses ncCauseList = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                int causeID = ncCauseList.findIDByName(Server.HtmlEncode(causeName));
                Boolean ncAddCheck = false;
                if (causeID == -1)
                {
                    ncAddCheck = ncCauseList.Add(Server.HtmlEncode(causeName), "");
                }

                NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                Boolean ncLinkCheck = nc.CauseAdd(Server.HtmlEncode(causeName));
                ret = ncLinkCheck ? 1 : 4;
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        [HttpPost]
        public int NCCauseDel(int ID, int Year, int causeID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCCauseDel", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCCauseDel", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            int ret = 0;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if (ID != -1 && Year != -1 && causeID != -1)
                {
                    NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                    Boolean ncLinkCheck = nc.CauseDel(causeID);
                    ret = ncLinkCheck ? 1 : 4;
                }
                else
                {
                    ret = 2;
                }
            }
            return ret;
        }
        [Authorize]
        public ActionResult WarningList(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/WarningList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/WarningList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.ncID = ID;
            ViewBag.ncYear = Year;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if(ID!=-1 && Year > 1970)
                { 
                    FreeWarnings fWarnList = new FreeWarnings(Session["ActiveWorkspace_Name"].ToString());
                    return PartialView(fWarnList.FreeWarningList);
                }
            }
            return PartialView();
        }

        [HttpPost]
        public Boolean NCProductWarningAdd(int ID, int Year, int wrnID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCProductWarningAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCProductWarningAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            Boolean ret = true;
            NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
            ret = nc.ProductAdd(new Warning(Session["ActiveWorkspace_Name"].ToString(), wrnID));

            return ret;
        }
        [Authorize]
        public ActionResult NCProductsList(int ID, int Year)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/NCProductsList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/NCProductsList", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            ViewBag.NCID = ID;
            ViewBag.NCYear = Year;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nonC = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                nonC.ProductsLoad();

                ElencoPostazioni elPost = new ElencoPostazioni(Session["ActiveWorkspace_Name"].ToString());
                List <String[]> ddlPst = new List<String[]>();
                for (int i = 0; i < elPost.elenco.Count; i++)
                { 
                    String[] pst = new String[2];
                    pst[0] = elPost.elenco[i].id.ToString();
                    pst[1] = elPost.elenco[i].name.ToString();
                    ddlPst.Add(pst);
                }
                ViewBag.ddlPostazioni = ddlPst;
                return PartialView(nonC.Products);
            }
            return PartialView();
        }

        [HttpPost]
        public Boolean NCProductDel(int ncID, int NCYear, int prdID, int prdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCProductDel", "ID=" + ncID + "&Year=" + NCYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCProductDel", "ID=" + ncID + "&Year=" + NCYear, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ncID, NCYear);
                if (nc.ID != -1 && nc.Year > 1970)
                {
                    ret = nc.ProductDel(prdID, prdYear);
                }
            }
            return ret;
        }

        [HttpPost]
        public Boolean NCProductUpd(int ncID, int NCYear, int prdID, int prdYear, int Qty, char source, int WStation)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCProductUpd", "ncID="+ncID+"&NCYear="+NCYear+"&prdID="+prdID+"&prdYear="+prdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCProductUpd", "ncID=" + ncID + "&NCYear=" + NCYear + "&prdID=" + prdID + "&prdYear=" + prdYear, ipAddr);
            }

            String retS = "";
            Boolean ret = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonComplianceProduct prd = new NonComplianceProduct(Session["ActiveWorkspace_Name"].ToString(), ncID, NCYear, prdID, prdYear);
                if(prd.ProductID!=-1 && prd.ProductYear > 1970)
                {
                    ret = true;
                    // Input control is missing: deploy it!!!
                    if(Qty<= prd.Quantity)
                    { 
                        prd.QuantityInvolved = Qty;
                    }
                    if(source == 'C' || source == 'P')
                    { 
                        prd.Source = source;
                    }

                    if(WStation == -1)
                    {
                        prd.Workstation = -1;
                        retS = prd.log;
                    }
                    else
                    { 
                        Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WStation);
                        if(p.id!=-1)
                        { 
                            prd.Workstation = WStation;
                            retS = prd.log;
                        }
                    }
                }
            }
            return ret;
        }
        [Authorize]
        public ActionResult ProductSearch(int ncID, int ncYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/ProductSearch", "ncID=" + ncID+"&ncYear="+ncYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/ProductSearch", "ncID=" + ncID + "&ncYear=" + ncYear, ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.listClienti = new List<Cliente>();
            ViewBag.ncID = ncID;
            ViewBag.ncYear = ncYear;

            if (checkUser == true)
            {
                ViewBag.ncID = ncID;
                ViewBag.ncYear = ncYear;
                ViewBag.authenticated = true;
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(Session["ActiveWorkspace_Name"].ToString(), true);
                ViewBag.listProdotti = el.elencoFigli.OrderBy(x => x.NomeCombinato);
                List<ProcessoVariante> elP = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                //elP[0].IDCombinato2;
                PortafoglioClienti elCustomers = new PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.listClienti = elCustomers.Elenco;
                //elCustomers.Elenco[0].RagioneSociale            
            }
            return PartialView();
        }
        [Authorize]
        public JsonResult PortafoglioProdotti(String customer)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/PortafoglioProdotti", "customer=" + customer, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/PortafoglioProdotti", "customer=" + customer, ipAddr);
            }

            ElencoProcessiVarianti el = null;
            
            String[] curr3 = new String[2];
            IList<String[]> clienti = new List<String[]>();

            if (customer != "*")
            {
                Cliente cust = new Cliente(Session["ActiveWorkspace_Name"].ToString(), customer);
                if(cust.CodiceCliente.Length>0)
                { 
                    el = new ElencoProcessiVarianti(Session["ActiveWorkspace_Name"].ToString(), true, cust);
                }
            }
            else
            {
                el = new ElencoProcessiVarianti(Session["ActiveWorkspace_Name"].ToString(), true);
            }
            
            for (int i = 0; i < el.elencoFigli.Count; i++)
            {
                String[] curr = new String[2];
                curr[0] = el.elencoFigli[i].IDCombinato2;
                curr[1] = el.elencoFigli[i].NomeCombinato;
                clienti.Add(curr);
            }

            return Json(clienti, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ProductsList(int ncID, int ncYear, String customer, int procID, int rev, int varID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonCompliances/ProductsList", "ncID=" + ncID+"&ncYear="+ncYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliances/ProductsList", "ncID=" + ncID + "&ncYear=" + ncYear, ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            List<FlatProduct> prodList = new List<FlatProduct>();
            ViewBag.ncID = -1;
            ViewBag.ncYear = -1;

            if (checkUser == true)
            {
                ViewBag.authenticated = true;

                ViewBag.ncID = ncID;
                ViewBag.ncYear = ncYear;
                ElencoArticoli elPrd = new ElencoArticoli(Session["ActiveWorkspace_Name"].ToString());
                elPrd.loadProductList();
                prodList = elPrd.ProductList;

                // ALL CUSTOMERS, ALL PRODUCTS --> ProductsList("", -1, -1, -1) --> ElencoArticoli();
                // ALL CUSTOMERS, 1 PRODUCT SELECTED --> ProductsList("", x, y, z) --> ElencoArticoli(ProcessoVariante prcVar, false);
                // 1 CUSTOMER, ALL PRODUCTS --> ProductsList(customerID, -1, -1, -1) --> ElencoArticoli(Cliente customer);
                // 1 CUSTOMER, 1 PRODUCT --> ProductsList(customerID, -1, -1, -1) --> public ElencoArticoli(ProcessoVariante origProc, Cliente customer, DateTime start, DateTime end);
                if (customer.Length == 0 && procID == -1 && rev == -1 && varID == -1)
                {

                }
                else if (customer.Length == 0 && procID != -1 && rev != -1 && varID != -1)
                {
                    /*ProcessoVariante prcVar = new ProcessoVariante(new processo(procID, revID), new variante(varID));
                    if (prcVar != null && prcVar.process != null && prcVar.process.processID != -1 && prcVar.variant != null && prcVar.variant.idVariante != -1)
                    {
                        prodList = (new ElencoArticoli(prcVar, false)).ProductList;
                    }*/
                    prodList = prodList.Where(x => x.processID == procID && x.revID == rev && x.varID == varID).ToList();
                }
                else if (customer.Length > 0 && procID == -1 && rev == -1 && varID == -1)
                {
                    /*Cliente curr = new Cliente(customer);
                    if (curr.CodiceCliente.Length > 0)
                    {
                        prodList = (new ElencoArticoli(curr)).ProductList;
                    }*/
                    prodList = prodList.Where(x => x.CustomerID == customer).ToList();
                }
                else if (customer.Length > 0 && procID != -1 && rev != -1 && varID != -1)
                {
                    /*ProcessoVariante prcVar = new ProcessoVariante(new processo(procID, revID), new variante(varID));
                    Cliente curr = new Cliente(customer);
                    if (curr.CodiceCliente.Length > 0 && prcVar != null && prcVar.process != null && prcVar.process.processID != -1 && prcVar.variant != null && prcVar.variant.idVariante != -1)
                    {
                        prodList = (new ElencoArticoli(prcVar, curr, new DateTime(1970, 1, 1), DateTime.UtcNow.AddYears(10))).ProductList;
                    }*/
                    prodList = prodList.Where(x => x.CustomerID == customer && x.processID == procID && x.revID == rev && x.varID == varID).ToList();
                }
                else
                {
                }
            }
            else
            {
                ViewBag.Message += " not authenticated";
            }
            return PartialView(prodList);
        }
        [Authorize]
        public Boolean NCProductAdd(int ID, int Year, int prodID, int prodYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Quality/NonCompliances/NCProductAdd", "ID=" + ID+"&Year="+Year, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliances/NCProductAdd", "ID=" + ID + "&Year=" + Year, ipAddr);
            }

            Boolean ret = false;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                NonCompliance nc = new NonCompliance(Session["ActiveWorkspace_Name"].ToString(), ID, Year);
                ret = nc.ProductAdd(prodID, prodYear);
            }
            return ret;
        }
    }
}