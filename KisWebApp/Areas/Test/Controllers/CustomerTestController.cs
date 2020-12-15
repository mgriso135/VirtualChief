using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_DB;

namespace KIS.Areas.Test.Controllers
{
    public class CustomerTestController : Controller
    {
        private VCContext db = new VCContext(ConfigurationManager.ConnectionStrings["masterDB"].ConnectionString);
        // GET: Test/CustomerTest
        public ActionResult Index()
        {
            ViewBag.anagcli = db.Anagraficaclienti.ToList();
            return View();
        }
    }
}