using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Controllers
{
    public class UtentiController : Controller
    {
        // GET: Utenti
        public ActionResult Index()
        {
            UserList usrList = new UserList(Session["ActiveWorkspace"].ToString());
            return View(usrList.listUsers.ToList());
        }
    }
}