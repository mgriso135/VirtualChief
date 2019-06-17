using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using KIS.App_Sources;

namespace KIS.Areas.Config.Controllers
{
    public class ConfigController:Controller
    {

        public JsonResult getExpiryDate()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = HttpContext.Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if(apikeyGiven == apikey && apikey.Length > 10)
            {
                String expDate = cfg.ExpiryDate.ToString("dd/MM/yyyy");
                if(expDate.Length > 0)
                {
                    Response.StatusCode = 200;
                    return Json(expDate);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json("");
                }
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult setExpiryDate(DateTime NewExpiryDate)
        {
            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;
            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                if(NewExpiryDate > DateTime.UtcNow)
                { 
                    cfg.ExpiryDate = NewExpiryDate;
                    Response.StatusCode = 200;
                    return Json(cfg.ExpiryDate.ToString("dd/MM/yyyy"));
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json("");
                }
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult getOrdersResume()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                    ElencoArticoli elArt = new ElencoArticoli(new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1));
    var elArtMonth = elArt.ListArticoli
        .OrderBy(z => z.DataInserimento)
        .GroupBy(x => new
        {
            Year = x.DataInserimento.Year,
            Month = x.DataInserimento.Month
        }).Select(x => new
        {
            Value = x.Count(),
            Year = x.Key.Year,
            Month = x.Key.Month
        });
                Response.StatusCode = 200;
                    return Json(elArtMonth);
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult getTasksResume()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                ElencoTaskProduzione elTasks = new ElencoTaskProduzione(new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1), 'F');
                var elTasksMonth = elTasks.Tasks.Where(y => y.DataFineTask >= DateTime.UtcNow.AddMonths(-13))
                    .OrderBy(z => z.DataFineTask)
                    .GroupBy(x => new
                    {
                        Year = x.DataFineTask.Year,
                        Month = x.DataFineTask.Month
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                        Month = x.Key.Month
                    });
                Response.StatusCode = 200;
                return Json(elTasksMonth);
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult getNonCompliancesResume()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                NonCompliances elNCs = new NonCompliances();
                elNCs.loadNonCompliances();
                var elNCsMonth = elNCs.NonCompliancesList
                    .OrderBy(z => z.OpeningDate)
                    .GroupBy(x => new
                    {
                        Year = x.OpeningDate.Year,
                        Month = x.OpeningDate.Month
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                        Month = x.Key.Month
                    });

                Response.StatusCode = 200;
                return Json(elNCsMonth);
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult getImprovementActionsResume()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                ImprovementActions elIActs = new ImprovementActions();
                elIActs.loadImprovementActions();
                var elIActsMonth = elIActs.ImprovementActionsList
                    .OrderBy(z => z.OpeningDate)
                    .GroupBy(x => new
                    {
                        Year = x.OpeningDate.Year,
                        Month = x.OpeningDate.Month
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                        Month = x.Key.Month
                    });

                Response.StatusCode = 200;
                return Json(elIActsMonth);
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public JsonResult getCorrectiveActionsResume()
        {
            KISConfig cfg = new KISConfig();
            String apikey = cfg.ConfigController_X_API_KEY;

            String apikeyGiven = "";
            try
            {
                var arrCKey = Request.Headers.GetValues("X-API-KEY");
                apikeyGiven = arrCKey.FirstOrDefault();
            }
            catch
            {
                apikeyGiven = "";
            }

            if (apikeyGiven == apikey && apikey.Length > 10)
            {
                List<CorrectiveAction> elCActs = new List<CorrectiveAction>();
                ImprovementActions elIActs = new ImprovementActions();
                elIActs.loadImprovementActions();
                for (int i = 0; i < elIActs.ImprovementActionsList.Count; i++)
                {
                    elIActs.ImprovementActionsList[i].loadCorrectiveActions();
                    for(int j =0; j < elIActs.ImprovementActionsList[i].CorrectiveActions.Count; j++)
                    {
                        elCActs.Add(elIActs.ImprovementActionsList[i].CorrectiveActions[j]);
                    }
                }

                var elCActsMonth = elCActs
                    .OrderBy(z => z.EarlyStart)
                    .GroupBy(x => new
                    {
                        Year = x.EarlyStart.Year,
                        Month = x.EarlyStart.Month
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                        Month = x.Key.Month
                    });

                Response.StatusCode = 200;
                return Json(elCActsMonth);
            }
            else
            {
                Response.StatusCode = 401;
                return Json("");
            }
        }

        public ActionResult SalesOrderImport3PartySystemActive()
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config SalesOrdersImport3rdPartySystem";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }


            ViewBag.ThirdPartyActive = false;
            if (ViewBag.authW)
            {
                KISConfig cfg = new KISConfig();
                ViewBag.ThirdPartyActive = cfg.SalesOrderImportFrom3PartySystem;
            }
            return View();
        }

        public Boolean SetSalesOrderImport3PartySystem(Boolean ThirdPartyActive)
        {
            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config SalesOrdersImport3rdPartySystem";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }


            ViewBag.ThirdPartyActive = false;
            if (ViewBag.authW)
            {
                KISConfig cfg = new KISConfig();
                cfg.SalesOrderImportFrom3PartySystem = ThirdPartyActive;
                ret = true;
            }
            return ret;
        }
    }
}