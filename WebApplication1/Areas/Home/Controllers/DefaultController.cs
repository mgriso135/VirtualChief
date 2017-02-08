using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KIS.Areas.Home.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Home/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}