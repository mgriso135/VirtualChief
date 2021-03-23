using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using System.Xml.Serialization;
using System.Data;
using System.Xml;
using System.IO;

namespace KIS.Areas.Home.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Home/Default
        [Authorize]
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Home/Default/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Home/Default/Index", "", ipAddr);
            }

            return View();
        }

        [Authorize]
        public ActionResult HomeRandomTips()
        {
            ViewBag.log = "";
            UserAccount curr = (UserAccount)Session["user"];
            string sWhatsNewPath = "~/whatsnew";
            if (curr != null && curr.Language.Length > 0)
            {
                switch (curr.Language)
                {
                    case "it":
                        sWhatsNewPath += "_it.xml";
                        break;
                    case "es":
                        sWhatsNewPath += "_es.xml";
                        break;
                    case "es-AR":
                        sWhatsNewPath += "_es.xml";
                        break;
                    case "en":
                        sWhatsNewPath += "_en.xml";
                        break;
                    case "en-US":
                        sWhatsNewPath += "_en.xml";
                        break;
                    default:
                        sWhatsNewPath += ".xml";
                        break;
                }
            }
            else
            {
                sWhatsNewPath += ".xml";
            }

            ViewBag.log = sWhatsNewPath;

            string xmlData = Server.MapPath(sWhatsNewPath);//Path of the xml script  

                 XmlDocument xmldoc = new XmlDocument();
            xmldoc.PreserveWhitespace = true;
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream(xmlData, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("Story");
            List<TipStruct> tips = new List<TipStruct>();
                for (i = 0; i <= xmlnode.Count - 1; i++)
                {
                TipStruct currTip = new TipStruct();
                currTip.id = i + 1;
                currTip.Title = xmlnode[i].ChildNodes.Item(3).InnerText.Trim();
                currTip.Description = xmlnode[i].ChildNodes.Item(5).InnerText.Trim();

                tips.Add(currTip);
                }
            ViewBag.log += "xmlnode.Count: " + xmlnode.Count + " ";
            TipStruct rnd = new TipStruct();
            Random rndNum = new Random();
            int next = rndNum.Next(0, xmlnode.Count);
            rnd = tips[next];
            ViewBag.log += next;
            return PartialView(rnd);
        }

        public struct TipStruct
        {
            public int id;
            public String Title;
            public String Description;
        }
    }
}