using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Controllers
{
    public class NonComplianceTypesController : Controller
    {
        // GET: NonComplianceTypes
        public ActionResult Index()
        {
            NonComplianceTypes ncTypeList = new NonComplianceTypes(Session["ActiveWorkspace"].ToString());
            ncTypeList.loadTypeList();
            return View(ncTypeList.TypeList.ToList());
        }
    }
}