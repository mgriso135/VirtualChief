using KIS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization;
using KIS.App_Sources;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics;

namespace KIS.Areas.Quality.Controllers
{
    public class NonCompliancesAnalysisController : Controller
    {
        // GET: Quality/NonCompliancesAnalysis
        [Authorize]
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/Index", "", ipAddr);
            }

            return View();
        }

        /* Start = Analysis start date
         * End = Aalysis end date
         * format = { 'W' --> weekly view, 'M' --> monthly view, 'D' --> daily view }
         */
        [Authorize]
        public ActionResult Number()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/Number", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/Number", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
            }
                return View();
        }
        [Authorize]
        public ActionResult NCPerPeriod(DateTime start, DateTime end, Char format)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCPerPeriod", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCPerPeriod", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> lista = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.format = format;

                return View(lista);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCPerPeriodList(DateTime start, DateTime end, Char format)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCPerPeriodList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction( Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCPerPeriodList", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> lista = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.format = format;

                return View(lista);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCCausesPerPeriodGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCCausesPerPeriodGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCCausesPerPeriodGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NCAnalysis nc = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                nc.loadNcCauses();
                List<AnalysisNCCause> lista = nc.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                return View(lista);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCCausesPerPeriodList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCCausesPerPeriodList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCCausesPerPeriodList", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NCAnalysis nc = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                nc.loadNcCauses();
                List<AnalysisNCCause> lista = nc.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                return View(lista);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCParetoCategoriesChart(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCParetoCategoriesChart", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCParetoCategoriesChart", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NCAnalysis nc = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                nc.loadNcCategories();
                List<AnalysisNCCategory> lista = nc.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                return View(lista);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCParetoCategoriesList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCParetoCategoriesList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction( Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCParetoCategoriesList", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
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
                NCAnalysis nc = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                nc.loadNcCategories();
                List<AnalysisNCCategory> lista = nc.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                return View(lista);
            }
            return View();
        }

        public struct CategoryCost
        {
            public int CategoryID;
            public string CategoryName;
            public int frequency;
            public double cost;
            public double risk { get { return frequency * cost; } }
            public double risk2;
        }

        [Authorize]
        protected List<CategoryCost> CategoriesCostRegression(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Action", "/Quality/NonCompliancesAnalysis/CategoriesCostRegression", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliancesAnalysis/CategoriesCostRegression", "", ipAddr);
            }

            List<CategoryCost> retCost = new List<CategoryCost>();
            NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
            ncList.loadNonCompliances();
            List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
            NonComplianceTypes ncCategories = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
            ncCategories.loadTypeList();

            double[][] xArr = new double[ncList2.Count][];
            double[] yCost = new double[ncList2.Count];
            List<double[]> resList = new List<double[]>();
            List<String> cats = new List<string>();
            List<int> catIDs = new List<int>();

            for (int j = 0; j < ncCategories.TypeList.Count; j++)
            {
                cats.Add(ncCategories.TypeList[j].Name);
                catIDs.Add(ncCategories.TypeList[j].ID);
            }

            for (int i = 0; i < ncList2.Count; i++)
            {
                yCost[i] = ncList2[i].Cost;
                xArr[i] = new double[ncCategories.TypeList.Count];
                ncList2[i].CategoryLoad();
                for (int j = 0; j < ncCategories.TypeList.Count; j++)
                {
                    try
                    {
                        NonComplianceType result = ncList2[i].Categories.First(s => s.ID == ncCategories.TypeList[j].ID);
                        xArr[i][j] = 1.0;
                    }
                    catch
                    {
                        xArr[i][j] = 0.0;
                    }
                }
            }


            double[] r = null;

            try
            {
                r = MultipleRegression.QR(xArr, yCost, intercept: false);
                resList.Add(r);
            }
            catch
            {

            }

            Dictionary<string, double> costCategories = new Dictionary<string, double>();
            if (r != null)
            {
                for (int i = 0; i < r.Length; i++)
                {
                    CategoryCost curr = new CategoryCost();
                    curr.CategoryName = cats[i];
                    curr.CategoryID = catIDs[i];

                    if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                    {
                        costCategories.Add(cats[i], r[i]);
                        curr.cost = r[i];
                    }
                    else
                    {
                        costCategories.Add(cats[i], 0.0);
                        curr.cost = 0.0;
                    }
                    retCost.Add(curr);
                }
            }
            costCategories = costCategories.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return retCost;
        }
        [Authorize]
        public ActionResult CategoriesCostList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction( usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesCostList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesCostList", "", ipAddr);
            }

            // Code here: https://numerics.mathdotnet.com/regression.html#Multiple-Regression

            ViewBag.startD = start;
            ViewBag.endD = end;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                NonComplianceTypes ncCategories = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
                ncCategories.loadTypeList();

                double[][] xArr = new double[ncList2.Count][];
                double[] yCost = new double[ncList2.Count];
                List<double[]> resList = new List<double[]>();
                List<String> cats = new List<string>();

                for (int j = 0; j < ncCategories.TypeList.Count; j++)
                {
                    cats.Add(ncCategories.TypeList[j].Name);
                }

                for (int i = 0; i < ncList2.Count; i++)
                {
                    yCost[i] = ncList.NonCompliancesList[i].Cost;
                    xArr[i] = new double[ncCategories.TypeList.Count];
                    ncList2[i].CategoryLoad();
                    for (int j = 0; j < ncCategories.TypeList.Count; j++)
                    {
                        try
                        {
                            NonComplianceType result = ncList2[i].Categories.First(s => s.ID == ncCategories.TypeList[j].ID);
                            xArr[i][j] = 1.0;
                        }
                        catch
                        {
                            xArr[i][j] = 0.0;
                        }
                    }
                }


                double[] r = null;

                try
                {
                    r = MultipleRegression.QR(xArr, yCost, intercept: false);
                    resList.Add(r);
                }
                catch
                {

                }

                ViewBag.resList = resList;
                ViewBag.categories = cats;

                Dictionary<string, double> costCategories = new Dictionary<string, double>();
                if (r != null)
                {
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                        {
                            costCategories.Add(cats[i], r[i]);
                        }
                        else
                        {
                            costCategories.Add(cats[i], 0.0);
                        }
                    }
                }
                costCategories = costCategories.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                return View(costCategories);
            }
            return View();
        }
        [Authorize]
        public ActionResult CategoriesCostGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesCostGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesCostGraph", "", ipAddr);
            }

            // Code here: https://numerics.mathdotnet.com/regression.html#Multiple-Regression

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                NonComplianceTypes ncCategories = new NonComplianceTypes(Session["ActiveWorkspace_Name"].ToString());
                ncCategories.loadTypeList();

                double[][] xArr = new double[ncList2.Count][];
                double[] yCost = new double[ncList2.Count];
                List<double[]> resList = new List<double[]>();
                List<String> cats = new List<string>();

                for (int j = 0; j < ncCategories.TypeList.Count; j++)
                {
                    cats.Add(ncCategories.TypeList[j].Name);
                }

                for (int i = 0; i < ncList2.Count; i++)
                {
                    yCost[i] = ncList.NonCompliancesList[i].Cost;
                    xArr[i] = new double[ncCategories.TypeList.Count];
                    ncList2[i].CategoryLoad();
                    for (int j = 0; j < ncCategories.TypeList.Count; j++)
                    {
                        try
                        {
                            NonComplianceType result = ncList2[i].Categories.First(s => s.ID == ncCategories.TypeList[j].ID);
                            xArr[i][j] = 1.0;
                        }
                        catch
                        {
                            xArr[i][j] = 0.0;
                        }
                    }
                }


                double[] r = null;

                try
                {
                    r = MultipleRegression.QR(xArr, yCost, intercept: false);
                    resList.Add(r);
                }
                catch
                {

                }

                ViewBag.resList = resList;
                ViewBag.categories = cats;

                Dictionary<string, double> costCategories = new Dictionary<string, double>();
                if (r != null)
                {
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                        {
                            costCategories.Add(cats[i], r[i]);
                        }
                        else
                        {
                            costCategories.Add(cats[i], 0.0);
                        }
                    }
                }
                costCategories = costCategories.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                return View(costCategories);
            }
            return View();
        }
        [Authorize]
        public ActionResult CategoriesProbabilityList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesProbabilityList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesProbabilityList", "", ipAddr);
            }


            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCategories();
                var ncAnalys2 = ncAnalys.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                var ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.NcNumber = ncList2.Count;

                return View(ncAnalys2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CategoriesProbabilityGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesProbabilityGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesProbabilityGraph", "", ipAddr);
            }


            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCategories();
                var ncAnalys2 = ncAnalys.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                var ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.NcNumber = ncList2.Count;

                return View(ncAnalys2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CategoriesRiskList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesRiskList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesRiskList", "", ipAddr);
            }


            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCategories();
                var catFreq = ncAnalys.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                var catFreq2 = catFreq.GroupBy(x => new { x.CategoryID, x.CategoryName })
                .Select(group => new { CategoryID = group.Key.CategoryID, CategoryName = group.Key.CategoryName, Frequency = group.Count() })
                .OrderByDescending(x => x.Frequency)
                .ToList();

                List<CategoryCost> catCost = CategoriesCostRegression(start, end);

                var resCost = (from item1 in catFreq2
                                           join item2 in catCost
                                           on item1.CategoryID equals item2.CategoryID // join on some property
                                           select new { item1.CategoryID, item1.CategoryName, item1.Frequency, item2.cost, item2.risk, item2.risk2 })
                                           .OrderByDescending(t => t.risk)
                                           .ToList();

                //var ncAnalysis2 = catFreq.Join
                /* NonCompliances ncList = new NonCompliances();
                 ncList.loadNonCompliances();
                 var ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                 ViewBag.NcNumber = ncList2.Count;*/
                ViewBag.resCost = resCost;
                List<CategoryCost> resCost2 = new List<CategoryCost>();
                for(int i = 0; i < resCost.Count; i++)
                {
                    CategoryCost curr = new CategoryCost();
                    curr.CategoryID = resCost[i].CategoryID;
                    curr.CategoryName = resCost[i].CategoryName;
                    curr.cost = resCost[i].cost;
                    curr.risk2 = resCost[i].cost * resCost[i].Frequency;
                    curr.frequency = resCost[i].Frequency;
                    resCost2.Add(curr);
                }
                resCost2 = resCost2.OrderByDescending(x => x.risk2).ToList();

                return View(resCost2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CategoriesRiskGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CategoriesRiskGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CategoriesRiskGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCategories();
                var catFreq = ncAnalys.ncCategory.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                var catFreq2 = catFreq.GroupBy(x => new { x.CategoryID, x.CategoryName })
                .Select(group => new { CategoryID = group.Key.CategoryID, CategoryName = group.Key.CategoryName, Frequency = group.Count() })
                .OrderByDescending(x => x.Frequency)
                .ToList();

                List<CategoryCost> catCost = CategoriesCostRegression(start, end);

                var resCost = (from item1 in catFreq2
                               join item2 in catCost
                               on item1.CategoryID equals item2.CategoryID // join on some property
                               select new { item1.CategoryID, item1.CategoryName, item1.Frequency, item2.cost, item2.risk, item2.risk2 })
                                           .OrderByDescending(t => t.risk)
                                           .ToList();

                ViewBag.resCost = resCost;
                List<CategoryCost> resCost2 = new List<CategoryCost>();
                for (int i = 0; i < resCost.Count; i++)
                {
                    CategoryCost curr = new CategoryCost();
                    curr.CategoryID = resCost[i].CategoryID;
                    curr.CategoryName = resCost[i].CategoryName;
                    curr.cost = resCost[i].cost;
                    curr.risk2 = resCost[i].cost * resCost[i].Frequency;
                    curr.frequency = resCost[i].Frequency;
                    resCost2.Add(curr);
                }
                resCost2 = resCost2.OrderByDescending(x => x.risk2).ToList();

                return View(resCost2);
            }
            return View();
        }

        public struct CauseCost
        {
            public int CauseID;
            public string CauseName;
            public int frequency;
            public double cost;
            public double risk { get { return frequency * cost; } }
            public double risk2;
        }

        protected List<CauseCost> CausesCostRegression(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Action", "/Quality/NonCompliancesAnalysis/CausesCostRegression", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Quality/NonCompliancesAnalysis/CausesCostRegression", "", ipAddr);
            }

            List<CauseCost> retCost = new List<CauseCost>();
            NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
            ncList.loadNonCompliances();
            List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
            NonComplianceCauses ncCauses = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
            ncCauses.loadCausesList();

            double[][] xArr = new double[ncList2.Count][];
            double[] yCost = new double[ncList2.Count];
            List<double[]> resList = new List<double[]>();
            List<String> cats = new List<string>();
            List<int> catIDs = new List<int>();

            for (int j = 0; j < ncCauses.CausesList.Count; j++)
            {
                cats.Add(ncCauses.CausesList[j].Name);
                catIDs.Add(ncCauses.CausesList[j].ID);
            }

            for (int i = 0; i < ncList2.Count; i++)
            {
                yCost[i] = ncList2[i].Cost;
                xArr[i] = new double[ncCauses.CausesList.Count];
                ncList2[i].CauseLoad();
                for (int j = 0; j < ncCauses.CausesList.Count; j++)
                {
                    try
                    {
                        NonComplianceCause result = ncList2[i].Causes.First(s => s.ID == ncCauses.CausesList[j].ID);
                        xArr[i][j] = 1.0;
                    }
                    catch
                    {
                        xArr[i][j] = 0.0;
                    }
                }
            }


            double[] r = null;

            try
            {
                r = MultipleRegression.QR(xArr, yCost, intercept: false);
                resList.Add(r);
            }
            catch
            {

            }

            Dictionary<string, double> costCategories = new Dictionary<string, double>();
            if (r != null)
            {
                for (int i = 0; i < r.Length; i++)
                {
                    CauseCost curr = new CauseCost();
                    curr.CauseName = cats[i];
                    curr.CauseID = catIDs[i];

                    if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                    {
                        costCategories.Add(cats[i], r[i]);
                        curr.cost = r[i];
                    }
                    else
                    {
                        costCategories.Add(cats[i], 0.0);
                        curr.cost = 0.0;
                    }
                    retCost.Add(curr);
                }
            }
            costCategories = costCategories.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return retCost;
        }

        [Authorize]
        public ActionResult CausesCostList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesCostList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesCostList", "", ipAddr);
            }

            // Code here: https://numerics.mathdotnet.com/regression.html#Multiple-Regression

            ViewBag.startD = start;
            ViewBag.endD = end;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                NonComplianceCauses ncCauses = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                ncCauses.loadCausesList();
                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                double[][] xArr = new double[ncList2.Count][];
                double[] yCost = new double[ncList2.Count];
                List<double[]> resList = new List<double[]>();
                List<String> causes = new List<string>();

                for (int j = 0; j < ncCauses.CausesList.Count; j++)
                {
                    causes.Add(ncCauses.CausesList[j].Name);
                }

                for (int i = 0; i < ncList2.Count; i++)
                {
                    yCost[i] = ncList2[i].Cost;
                    xArr[i] = new double[ncCauses.CausesList.Count];
                    ncList2[i].CauseLoad();
                    for (int j = 0; j < ncCauses.CausesList.Count; j++)
                    {
                        try
                        {
                            NonComplianceCause result = ncList2[i].Causes.First(s => s.ID == ncCauses.CausesList[j].ID);
                            xArr[i][j] = 1.0;
                        }
                        catch
                        {
                            xArr[i][j] = 0.0;
                        }
                    }
                }


                double[] r = null;

                try
                {
                    r = MultipleRegression.QR(xArr, yCost, intercept: false);
                    resList.Add(r);
                }
                catch
                {

                }

                ViewBag.resList = resList;
                ViewBag.causes = causes;

                Dictionary<string, double> costCauses = new Dictionary<string, double>();
                if (r != null)
                {
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                        {
                            costCauses.Add(causes[i], r[i]);
                        }
                        else
                        {
                            costCauses.Add(causes[i], 0.0);
                        }
                    }
                }
                costCauses = costCauses.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                return View(costCauses);
            }
            return View();
        }
        [Authorize]
        public ActionResult CausesCostGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesCostGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesCostGraph", "", ipAddr);
            }

            // Code here: https://numerics.mathdotnet.com/regression.html#Multiple-Regression

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                NonComplianceCauses ncCauses = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                ncCauses.loadCausesList();
                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                double[][] xArr = new double[ncList2.Count][];
                double[] yCost = new double[ncList2.Count];
                List<double[]> resList = new List<double[]>();
                List<String> causes = new List<string>();

                for (int j = 0; j < ncCauses.CausesList.Count; j++)
                {
                    causes.Add(ncCauses.CausesList[j].Name);
                }

                for (int i = 0; i < ncList2.Count; i++)
                {
                    yCost[i] = ncList2[i].Cost;
                    xArr[i] = new double[ncCauses.CausesList.Count];
                    ncList2[i].CauseLoad();
                    for (int j = 0; j < ncCauses.CausesList.Count; j++)
                    {
                        try
                        {
                            NonComplianceCause result = ncList2[i].Causes.First(s => s.ID == ncCauses.CausesList[j].ID);
                            xArr[i][j] = 1.0;
                        }
                        catch
                        {
                            xArr[i][j] = 0.0;
                        }
                    }
                }


                double[] r = null;

                //try
                {
                    r = MultipleRegression.QR(xArr, yCost, intercept: false);
                    resList.Add(r);
                }
                //catch
                {

                }

                ViewBag.resList = resList;
                ViewBag.causes = causes;

                Dictionary<string, double> costCauses = new Dictionary<string, double>();
                if (r != null)
                {
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (r[i] >= 0 && r[i] < 10 * Math.Exp(10))
                        {
                            costCauses.Add(causes[i], r[i]);
                        }
                        else
                        {
                            costCauses.Add(causes[i], 0.0);
                        }
                    }
                }
                costCauses = costCauses.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                return View(costCauses);
            }
            return View();
        }
        [Authorize]
        public ActionResult CausesProbabilityList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesProbabilityList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesProbabilityList", "", ipAddr);
            }


            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCauses();
                var ncAnalys2 = ncAnalys.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                var ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.NcNumber = ncList2.Count;

                return View(ncAnalys2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CausesProbabilityGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesProbabilityGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesProbabilityGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCauses();
                var ncAnalys2 = ncAnalys.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                var ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                ViewBag.NcNumber = ncList2.Count;

                return View(ncAnalys2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CausesRiskList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesRiskList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesRiskList", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCauses();
                var causeFreq = ncAnalys.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                var causeFreq2 = causeFreq.GroupBy(x => new { x.CauseID, x.CauseName })
                .Select(group => new { CauseID = group.Key.CauseID, CauseName = group.Key.CauseName, Frequency = group.Count() })
                .OrderByDescending(x => x.Frequency)
                .ToList();

                List<CauseCost> causeCost = CausesCostRegression(start, end);

                var resCost = (from item1 in causeFreq2
                               join item2 in causeCost
                               on item1.CauseID equals item2.CauseID // join on some property
                               select new { item1.CauseID, item1.CauseName, item1.Frequency, item2.cost, item2.risk, item2.risk2 })
                                           .OrderByDescending(t => t.risk)
                                           .ToList();

                ViewBag.resCost = resCost;
                List<CauseCost> resCost2 = new List<CauseCost>();
                for (int i = 0; i < resCost.Count; i++)
                {
                    CauseCost curr = new CauseCost();
                    curr.CauseID = resCost[i].CauseID;
                    curr.CauseName = resCost[i].CauseName;
                    curr.cost = resCost[i].cost;
                    curr.risk2 = resCost[i].cost * resCost[i].Frequency;
                    curr.frequency = resCost[i].Frequency;
                    resCost2.Add(curr);
                }
                resCost2 = resCost2.OrderByDescending(x => x.risk2).ToList();

                return View(resCost2);
            }
            return View();
        }
        [Authorize]
        public ActionResult CausesRiskGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/CausesRiskGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/CausesRiskGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                NCAnalysis ncAnalys = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncAnalys.loadNcCauses();
                var causeFreq = ncAnalys.ncCauses.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                var causeFreq2 = causeFreq.GroupBy(x => new { x.CauseID, x.CauseName })
                .Select(group => new { CauseID = group.Key.CauseID, CauseName = group.Key.CauseName, Frequency = group.Count() })
                .OrderByDescending(x => x.Frequency)
                .ToList();

                List<CauseCost> causeCost = CausesCostRegression(start, end);

                var resCost = (from item1 in causeFreq2
                               join item2 in causeCost
                               on item1.CauseID equals item2.CauseID // join on some property
                               select new { item1.CauseID, item1.CauseName, item1.Frequency, item2.cost, item2.risk, item2.risk2 })
                                           .OrderByDescending(t => t.risk)
                                           .ToList();

                ViewBag.resCost = resCost;
                List<CauseCost> resCost2 = new List<CauseCost>();
                for (int i = 0; i < resCost.Count; i++)
                {
                    CauseCost curr = new CauseCost();
                    curr.CauseID = resCost[i].CauseID;
                    curr.CauseName = resCost[i].CauseName;
                    curr.cost = resCost[i].cost;
                    curr.risk2 = resCost[i].cost * resCost[i].Frequency;
                    curr.frequency = resCost[i].Frequency;
                    resCost2.Add(curr);
                }
                resCost2 = resCost2.OrderByDescending(x => x.risk2).ToList();

                return View(resCost2);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCCostList(DateTime start, DateTime end, char format)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCCostList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCCostList", "", ipAddr);
            }

            ViewBag.startD = start;
            ViewBag.endD = end;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.format = format;
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();
                

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                return View(ncList2);
            }
            return View();
        }
        [Authorize]
        public ActionResult NCCostGraph(DateTime start, DateTime end, char format)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/NCCostGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/NCCostGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
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

                ViewBag.format = format;
                NonCompliances ncList = new NonCompliances(Session["ActiveWorkspace_Name"].ToString());
                ncList.loadNonCompliances();
                List<NonCompliance> ncList2 = ncList.NonCompliancesList.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                ViewBag.start = start.Year + "-";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "-";
                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.end = end.Year + "-";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "-";
                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();

                return View(ncList2);
            }
            return View();
        }
        [Authorize]
        public ActionResult ProductsNumberList(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/ProductsNumberList", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/ProductsNumberList", "", ipAddr);
            }

            ViewBag.startD = start;
            ViewBag.endD = end;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Product";
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
                NCAnalysis ncProdList = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncProdList.loadNcProducts();
                List<AnalysisNCProduct> ncProdList2 = ncProdList.ncProduct.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.start += "/";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "/";
                ViewBag.start += start.Year;

                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();
                ViewBag.end += "/";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "/";
                ViewBag.end += end.Year;

                return View(ncProdList2);
            }
            return View();
        }
        [Authorize]
        public ActionResult ProductsNumberGraph(DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonCompliancesAnalysis/ProductsNumberGraph", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonCompliancesAnalysis/ProductsNumberGraph", "", ipAddr);
            }

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Product";
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
                NCAnalysis ncProdList = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
                ncProdList.loadNcProducts();
                List<AnalysisNCProduct> ncProdList2 = ncProdList.ncProduct.Where(x => x.OpeningDate >= start && x.OpeningDate <= end).ToList();

                ViewBag.start += start.Day < 10 ? "0" + start.Day.ToString() : start.Day.ToString();
                ViewBag.start += "/";
                ViewBag.start += start.Month < 10 ? "0" + start.Month.ToString() : start.Month.ToString();
                ViewBag.start += "/";
                ViewBag.start += start.Year;

                ViewBag.end += end.Day < 10 ? "0" + end.Day.ToString() : end.Day.ToString();
                ViewBag.end += "/";
                ViewBag.end += end.Month < 10 ? "0" + end.Month.ToString() : end.Month.ToString();
                ViewBag.end += "/";
                ViewBag.end += end.Year;

                return View(ncProdList2);
            }
            return View();
        }

    }
}