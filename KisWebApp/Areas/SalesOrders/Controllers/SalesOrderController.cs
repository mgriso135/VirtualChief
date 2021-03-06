using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KIS.App_Sources;

namespace KIS.Areas.SalesOrders.Controllers
{
    public class SalesOrderController : Controller
    {
        // GET: SalesOrders/SalesOrder
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/SalesOrders/SalesOrder/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/SalesOrders/SalesOrder/Index", "", ipAddr);
            }

            return View();
        }

        public ActionResult AddSalesOrder(int OrderID, int OrderYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/SalesOrders/SalesOrder/AddSalesOrder", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/SalesOrders/SalesOrder/AddSalesOrder", "", ipAddr);
            }

            ViewBag.CustomersList = new List<Cliente>();
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Commesse";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.authAddProductW = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authAddProductW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                PortafoglioClienti cstList = new PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.CustomersList = cstList.Elenco;
                if (OrderID != -1 && OrderYear != -1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), OrderID, OrderYear);
                    if (cm != null)
                    {
                        return View(cm);
                    }
                }
                else
                {

                }
            }

            return View();
        }

        /*returns:
         * [OrderID, OrderYear] if ok
         * [-1, -1] if generic error
         * [-2, -2] if customer not found
         */
        public int AddSalesOrderHeader(String customer, String ExternalID, String Notes)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/AddSalesOrderHeader", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/AddSalesOrderHeader", "", ipAddr);
            }

            int ret = -1;
            /*ret[0] = -1;
            ret[1] = -1;*/
            //ViewBag.CustomersList = new List<Cliente>();
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Commesse";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Cliente curr = new Cliente(Session["ActiveWorkspace_Name"].ToString(), customer);
                if (curr != null && curr.CodiceCliente.Length > 0)
                {
                    ElencoCommesse ordersList = new ElencoCommesse(Session["ActiveWorkspace_Name"].ToString());
                    ret = ordersList.Add(curr.CodiceCliente, Notes, ExternalID);
                    if (ret >= 0)
                    {
                        Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), ret, DateTime.UtcNow.Year);
                        UserAccount currUsr = (UserAccount)Session["user"];
                        cm.Confirmed = true;
                        cm.ConfirmedBy = currUsr;
                        cm.ConfirmationDate = DateTime.UtcNow;
                    }
                }
                else
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /* returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized
         * 3 if order not found
         */
        public int EditSalesOrderHeader(int OrderID, int OrderYear, String ExternalID, String Notes)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/EditSalesOrderHeader", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/EditSalesOrderHeader", "", ipAddr);
            }

            int ret = -1;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Commesse";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), OrderID, OrderYear);
                if (cm != null && cm.ID > -1 && cm.Year > 2000)
                {
                    cm.ExternalID = ExternalID;
                    cm.Note = Notes;
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

        public JsonResult GetProductDepartments(int ProdID, int ProdRev, int variant)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/SalesOrders/SalesOrder/GetProductDepartments", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/SalesOrders/SalesOrder/GetProductDepartments", "", ipAddr);
            }

            List<ProductDepartments> ret = new List<ProductDepartments>();
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW == true)
            {
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variant));
                prcVar.loadReparto();
                if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                {
                    for (int i = 0; i < prcVar.RepartiProduttivi.Count; i++)
                    {
                        ProductDepartments curr = new ProductDepartments();
                        curr.DepartmentID = prcVar.RepartiProduttivi[i].id;
                        curr.DepartmentName = prcVar.RepartiProduttivi[i].name;
                        ret.Add(curr);
                    }
                }
            }

            return Json(ret);
        }

        public struct ProductDepartments
        {
            public int DepartmentID;
            public String DepartmentName;
        }

        /*Returns:
         * 0 if generic error
         * 1 if ok
         * 2 if user is not authorized
         * 3 if product not found
         * 4 if invalid delivery date
         * 5 if sales order not found
         * 6 if error while adding the product to the order
         */
        public int AddProductToSalesOrder(int OrderID, int OrderYear, int ProdID, int ProdRev, int ProdVar, int Quantity, DateTime DeliveryDate)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/AddProductToSalesOrder", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/AddProductToSalesOrder", "", ipAddr);
            }

            int ret = 0;
            ViewBag.authAddProductW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authAddProductW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authAddProductW)
            {
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdRev), new variante(Session["ActiveWorkspace_Name"].ToString(), ProdVar));
                if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                {
                    FusoOrario fuso = new FusoOrario(Session["ActiveWorkspace_Name"].ToString());
                    if (DeliveryDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario))
                    {
                        Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), OrderID, OrderYear);
                        if (cm != null && cm.ID != -1 && cm.Year > 2000)
                        {
                            int[] rt = cm.AddArticoloInt(prcVar, DeliveryDate, Quantity);
                            if (rt[0]!=-1)
                            {
                                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), rt[0], rt[1]);
                                art.DataPrevistaFineProduzione = DeliveryDate;
                                ret = 1;
                            }
                            else
                            {
                                ret = 6;
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
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public ActionResult ListOrderProduct(int OrderID, int OrderYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/SalesOrders/SalesOrder/ListOrderProduct", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/SalesOrders/SalesOrder/ListOrderProduct", "", ipAddr);
            }

            // Authorization to edit product data, delete a product from an order, remove the planification (if product is in status N).
            ViewBag.authEditProductData = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authEditProductData = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            // AUthorization to plan a product in production
            ViewBag.authPlanProduction = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authPlanProduction = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authEditProductData || ViewBag.authPlanProduction)
            {
                Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), OrderID, OrderYear);
                if (cm != null && cm.ID > -1 && cm.Year > 2000)
                {
                    cm.loadArticoli();
                    return View(cm.Articoli);
                }
            }

            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if product correctly delete from sales order
         * 2 if product status is not 'N'
         * 3 if user not authorized
         * 4 if invalid OrderID and OrderYear
         * 5 if Product not found
         */
        public int DeleteProductFromSalesOrder(int OrderID, int OrderYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/DeleteProductFromSalesOrder", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/DeleteProductFromSalesOrder", "", ipAddr);
            }

            int ret = 0;
            ViewBag.authAddProductW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authAddProductW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authAddProductW)
            {
                if (OrderID != -1 && OrderYear != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), OrderID, OrderYear);

                    if (art.ID != -1 && art.Year > 2010)
                    {
                        if (art.Status == 'N')
                        {
                            bool retS = art.Delete();
                            if (retS == true)
                            {
                                ret = 1;
                            }
                            else
                            {
                                ret = 2;
                            }
                        }
                        else
                        {
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
                ret = 3;
            }

            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if product correctly delete from sales order
         * 2 if product status is not 'N'
         * 3 if user not authorized
         * 4 if invalid OrderID and OrderYear
         * 5 if Product not found
         */
        public int UndoProductSchedule(int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/UndoProductSchedule", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/UndoProductSchedule", "", ipAddr);
            }

            int ret = 0;
            ViewBag.authAddProductW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authAddProductW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authAddProductW)
            {
                if (ProductID != -1 && ProductYear != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);

                    if (art.ID != -1 && art.Year > 2010)
                    {
                        if (art.Status == 'P')
                        {
                            bool retS = art.Depianifica();
                            if (retS == true)
                            {
                                ret = 1;
                            }
                            else
                            {
                                ret = 2;
                            }
                        }
                        else
                        {
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
                ret = 3;
            }

            return ret;
        }
        
        /*Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if user is not authorized
         * 3 if product was not found
         * 4 if invalid quantity
         * 5 invalid delivery date
         */
        public int EditProduct(int ProductID, int ProductYear, int Quantity, DateTime DeliveryDate)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/EditProduct", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/EditProduct", "", ipAddr);
            }

            int ret = 0;
            ViewBag.authAddProductW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authAddProductW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authAddProductW)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                if(art!=null && art.ID!=-1 && art.Year > 2010)
                {
                    ret = 1;
                    if(Quantity> 0)
                    {
                        art.Quantita = Quantity;
                    }
                    else
                    {
                        ret = 4;
                    }
                    if(DeliveryDate > DateTime.UtcNow)
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), art.Reparto);
                        art.DataPrevistaConsegna = DeliveryDate;
                    }
                    else
                    {
                        ret = 5;
                    }
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

        /*
          * Lancia il processo in produzione
          * Returns:
          * 0 se errore generico
          * 1 se tutto ok
          * 2 se non sono state impostate correttamente EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate
          * 3 se l'articolo è già stato lanciato in produzione
          * 
          * 5 se il tempo a disposizione è < del CriticalPath, quindi va ritardata la consegna
          * 6 se non sono riuscito a dare un EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate a tutti i task.
          * 7 se la data prevista di fine produzione non è reale
          * 8 if user is not authorized.
          */
        public int PlanProduct(int ProductID, int ProductYear, int DepartmentID, DateTime EndProductionDate)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/PlanProduct", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/PlanProduct", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentID);

                if (art != null && art.ID != -1 && art.Status == 'N' && rp != null && rp.id != -1 && EndProductionDate > DateTime.UtcNow && EndProductionDate<= art.DataPrevistaConsegna)
                {
                    art.Reparto = rp.id;
                    art.DataPrevistaFineProduzione = EndProductionDate;

                    art.Proc.process.loadFigli(art.Proc.variant);
                    for (int i = 0; i < art.Proc.process.subProcessi.Count; i++)
                    {
                        TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), art.Proc.process.subProcessi[i].processID, art.Proc.process.subProcessi[i].revisione), art.Proc.variant);
                        tskVar.loadTempiCiclo();
                        TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), tskVar.Task.processID, tskVar.Task.revisione, art.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                        if (tc.Tempo != null)
                        {
                            lstTasks.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, rp.id, art.Quantita));
                        }
                    }

                    ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), art, lstTasks, rp, art.Quantita);
                    int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                    if (rt1 == 1)
                    {
                        art.Planner = (User)Session["user"];
                        ret = prcCfg.LanciaInProduzione();
                    }
                    else
                    {
                        switch (rt1)
                        {
                            case 2: ret = 5; break;
                            case 3: ret = 6; break;
                            case 4: ret = 7; break;
                            default:
                                break;
                        }

                        ret = rt1;
                    }
                }
                else
                {
                    ret = 5;
                }
            }
            else
            {
                ret = 8;
            }
            return ret;
        }

        /* Returns:
         * String url for the .pdf document
         */
        public String PrintMultipleSheetsBarcodes(int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/PrintMultipleSheetsBarcodes", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/PrintMultipleSheetsBarcodes", "", ipAddr);
            }

            String ret = "";

            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
            Commessa comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
            // Ora creo il pdf!
            String savePath = Server.MapPath(@"~\Data\Produzione\");
            Document cartPDF = new Document(PageSize.A4, 50, 50, 25, 25);

            art.loadTasksProduzione();

            String FileNamePDF = "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + ".pdf";
            // Controllo che il pdf non esista, e se esiste lo cancello.
            if (System.IO.File.Exists(savePath + FileNamePDF))
            {
                String newfilename = savePath + FileNamePDF + DateTime.Now.Ticks.ToString();
                System.IO.File.Move(savePath + FileNamePDF, newfilename);
                System.IO.File.Delete(newfilename);
            }

            System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
            cartPDF.Open();

            for (int i = 0; i < art.Tasks.Count; i++)
            {
                cartPDF.NewPage();
                System.Drawing.Image StartCode = GenCode128.Code128Rendering.MakeBarcodeImage("I" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                System.Drawing.Image EndCode = GenCode128.Code128Rendering.MakeBarcodeImage("F" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                System.Drawing.Image PauseCode = GenCode128.Code128Rendering.MakeBarcodeImage("A" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                System.Drawing.Image WarningCode = GenCode128.Code128Rendering.MakeBarcodeImage("W" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);

                String FileName = "articolo" + art.ID.ToString() + "_" + art.Year.ToString();
                bool check = true;
                try
                {
                    StartCode.Save(savePath + FileName + "I.jpg");
                    EndCode.Save(savePath + FileName + "F.jpg");
                    PauseCode.Save(savePath + FileName + "A.jpg");
                    WarningCode.Save(savePath + FileName + "W.jpg");
                    check = true;
                }
                catch (Exception ex)
                {
                    check = false;
                }

                Logo logoAzienda = new Logo(Session["ActiveWorkspace_Name"].ToString());
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath(logoAzienda.filePath));
                logo.ScaleToFit(50 * logo.Width / logo.Height, 50);
                cartPDF.Add(logo);

                String txtPostazione = ResListOrderProduct.ListOrderProduct.lblBPostazione + " " + art.Tasks[i].PostazioneName;
                iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 40, Font.BOLD));
                posta.Alignment = Element.ALIGN_CENTER;
                cartPDF.Add(posta);
                String txtCommessa = ResListOrderProduct.ListOrderProduct.lblOrdine + " " + comm.ID.ToString() + "/" + comm.Year.ToString() + " - " + Server.HtmlDecode(comm.Cliente);
                iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                commessa.Alignment = Element.ALIGN_CENTER;
                cartPDF.Add(commessa);
                String txtArticolo = art.ID.ToString() + "/" + art.Year.ToString() + " - " + Server.HtmlDecode(art.Proc.process.processName) + " " + Server.HtmlDecode(art.Proc.variant.nomeVariante)
                    + " - " + ResListOrderProduct.ListOrderProduct.lblBQuantita + ": " + art.Quantita.ToString();
                iTextSharp.text.Paragraph articolo = new iTextSharp.text.Paragraph(txtArticolo, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                cartPDF.Add(articolo);

                System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                statusCode.ScaleToFit(20 * statusCode.Width / statusCode.Height, 20);
                iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblProductStatus, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                statusProd.Alignment = Element.ALIGN_LEFT;
                statusCode.Alignment = Element.ALIGN_LEFT;
                cartPDF.Add(statusProd);
                cartPDF.Add(statusCode);

                String txtTask = art.Tasks[i].Name + " (" + art.Tasks[i].TaskProduzioneID.ToString() + ")"
                    + Environment.NewLine
       /* + "Early Start: " + art.Tasks[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss")
        + " Late Start: " + art.Tasks[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss")
        + Environment.NewLine
        + "Early Finish: " + art.Tasks[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss")
        + " Late Finish: " + art.Tasks[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss")
        + Environment.NewLine
        + "Tempo ciclo previsto: " + art.Tasks[i].TempoC.Hours.ToString() + "H:" + art.Tasks[i].TempoC.Minutes.ToString() + "mm:" + art.Tasks[i].TempoC.Seconds.ToString() + ":ss"
    +Environment.NewLine
    */+ ResListOrderProduct.ListOrderProduct.lblBNumOp + ": " + art.Tasks[i].NumOperatori.ToString();
                iTextSharp.text.Paragraph task = new iTextSharp.text.Paragraph(txtTask, new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                cartPDF.Add(task);
                iTextSharp.text.Image ICode = iTextSharp.text.Image.GetInstance(savePath + FileName + "I.jpg");
                cartPDF.Add(ICode);
                iTextSharp.text.Paragraph didascalia = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPInizio + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                cartPDF.Add(didascalia);
                iTextSharp.text.Image ACode = iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg");
                cartPDF.Add(ACode);
                didascalia = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPPausa + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                cartPDF.Add(didascalia);
                iTextSharp.text.Image FCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg");
                cartPDF.Add(FCode);
                didascalia = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPFine + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                cartPDF.Add(didascalia);
                iTextSharp.text.Image WCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "W.jpg");
                cartPDF.Add(WCode);
                didascalia = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPProblema + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                cartPDF.Add(didascalia);

                try
                {
                    System.IO.File.Delete(savePath + FileName + "I.jpg");
                    System.IO.File.Delete(savePath + FileName + "A.jpg");
                    System.IO.File.Delete(savePath + FileName + "F.jpg");
                    System.IO.File.Delete(savePath + FileName + "W.jpg");
                }
                catch
                {
                }
            }


            //bCode.SetAbsolutePosition(100, 100);
            //bCode.Alignment = Element.ALIGN_CENTER;

            cartPDF.Close();

            output.Close();
            output.Dispose();
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('/Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
            //ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
            ret = "/Data/Produzione/" + FileNamePDF;

            return ret;
        }

        public String PrintSingleSheetsBarcodes(int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/PrintSingleSheetsBarcodes", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/PrintSingleSheetsBarcodes", "", ipAddr);
            }

            String ret = "";
            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
            Commessa comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
            // Ora creo il pdf!
            String savePath = Server.MapPath(@"~\Data\Produzione\");
            Document cartPDF = new Document(PageSize.A4, 50, 50, 25, 25);

            art.loadTasksProduzione();

            String FileNamePDF = "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + ".pdf";
            // Controllo che il pdf non esista, e se esiste lo cancello.

            if (System.IO.File.Exists(savePath + FileNamePDF))
            {
                try
                {

                    System.IO.File.Delete(savePath + FileNamePDF);
                }
                catch
                {
                }
            }

            using (System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create))
            {
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
                cartPDF.Open();

                PdfPTable tabIntest = new PdfPTable(3);
                tabIntest.WidthPercentage = 100;

                PdfPCell[] intestazioneFoglio = new PdfPCell[5];
                intestazioneFoglio[0] = new PdfPCell();
                intestazioneFoglio[1] = new PdfPCell();
                intestazioneFoglio[2] = new PdfPCell();
                intestazioneFoglio[3] = new PdfPCell();
                intestazioneFoglio[4] = new PdfPCell();

                Logo logoAzienda = new Logo(Session["ActiveWorkspace_Name"].ToString());
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath(logoAzienda.filePath));
                if (logo.Height > logo.Width)
                {
                    logo.ScaleToFit(150 * logo.Width / logo.Height, 150);
                }
                else
                {
                    logo.ScaleToFit(140, 140 * logo.Height / logo.Width);
                }

                intestazioneFoglio[0].AddElement(logo);
                String idComm = comm.ID.ToString() + "/" + comm.Year.ToString();
                if (comm.ExternalID.Length > 0)
                {
                    idComm = comm.ExternalID;
                }
                Cliente cln = new Cliente(Session["ActiveWorkspace_Name"].ToString(), art.Cliente);
                String txtCommessa = cln.RagioneSociale + Environment.NewLine
                    + ResListOrderProduct.ListOrderProduct.lblOrdine + " " + idComm + Environment.NewLine
                    + Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                    + " - " + ResListOrderProduct.ListOrderProduct.lblBQuantita.ToString() + ": " + art.Quantita.ToString()
                        + Environment.NewLine + ResListOrderProduct.ListOrderProduct.lblDataConsegna.ToString()
                        + ": " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                /*if (art.Planner != null)
                {
                    txtCommessa += "; Disegnatore: " + art.Planner.name + " " + art.Planner.cognome;
                }*/
                iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                intestazioneFoglio[1].Rowspan = 2;
                intestazioneFoglio[1].AddElement(commessa);

                System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");

                iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPStatoProdotto, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                statusProd.Alignment = Element.ALIGN_CENTER;
                statusCode.Alignment = Element.ALIGN_CENTER;
                intestazioneFoglio[2].AddElement(statusProd);
                intestazioneFoglio[2].AddElement(statusCode);



                float[] widths = new float[] { 150, (490 - 300), 150 };
                tabIntest.SetWidths(widths);

                iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[3].AddElement(noteOC);

                iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPInizio.ToString() + ": " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[4].AddElement(inizioProd);

                tabIntest.AddCell(intestazioneFoglio[0]);
                tabIntest.AddCell(intestazioneFoglio[1]);
                tabIntest.AddCell(intestazioneFoglio[2]);
                tabIntest.AddCell(intestazioneFoglio[3]);
                tabIntest.AddCell(intestazioneFoglio[4]);

                commessa.Alignment = Element.ALIGN_LEFT;
                //cartPDF.Add(commessa);
                cartPDF.Add(tabIntest);
                for (int i = 0; i < art.Tasks.Count; i++)
                {
                    System.Drawing.Image StartCode = GenCode128.Code128Rendering.MakeBarcodeImage("I" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image EndCode = GenCode128.Code128Rendering.MakeBarcodeImage("F" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image PauseCode = GenCode128.Code128Rendering.MakeBarcodeImage("A" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image WarningCode = GenCode128.Code128Rendering.MakeBarcodeImage("W" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);

                    String FileName = "articolo" + art.ID.ToString() + "_" + art.Year.ToString();
                    bool check = true;
                    try
                    {
                        StartCode.Save(savePath + FileName + "I.jpg");
                        EndCode.Save(savePath + FileName + "F.jpg");
                        PauseCode.Save(savePath + FileName + "A.jpg");
                        WarningCode.Save(savePath + FileName + "W.jpg");
                        check = true;
                    }
                    catch (Exception ex)
                    {
                        check = false;
                    }


                    PdfPTable tab = new PdfPTable(4);
                    tab.WidthPercentage = 100;

                    PdfPCell[] intestCella = new PdfPCell[4];
                    intestCella[0] = new PdfPCell();
                    intestCella[1] = new PdfPCell();
                    intestCella[2] = new PdfPCell();
                    intestCella[3] = new PdfPCell();

                    String txtPostazione = art.Tasks[i].Name + " (" + art.Tasks[i].TaskProduzioneID.ToString() + ")";
                    iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                    posta.Alignment = Element.ALIGN_LEFT;


                    String txtTask = ResListOrderProduct.ListOrderProduct.lblBNumOp + ": "
                        + art.Tasks[i].NumOperatori.ToString();
                    iTextSharp.text.Paragraph task = new iTextSharp.text.Paragraph(txtPostazione + "; " + txtTask, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                    intestCella[0].AddElement(task);
                    intestCella[0].Colspan = 4;
                    PdfPRow intestazione = new PdfPRow(intestCella);
                    intestazione.MaxHeights = 100;

                    tab.Rows.Add(intestazione);


                    PdfPCell[] celle = new PdfPCell[4];
                    for (int q = 0; q < 4; q++)
                    {
                        celle[q] = new PdfPCell();
                        celle[q].FixedHeight = 45;
                    }

                    celle[0].Padding = 5;
                    celle[0].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "I.jpg"));
                    Paragraph pin = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPInizio, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    celle[0].AddElement(pin);
                    celle[1].Padding = 5;
                    celle[1].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPPausa, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                    celle[2].Padding = 5;
                    celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                    celle[2].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPFine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[3].Padding = 5;
                    celle[3].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPProblema, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[3].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "W.jpg"));

                    PdfPRow riga = new PdfPRow(celle);
                    riga.MaxHeights = 100;
                    tab.Rows.Add(riga);
                    cartPDF.Add(tab);

                    cartPDF.Add(new Paragraph(Environment.NewLine));
                    try
                    {
                        System.IO.File.Delete(savePath + FileName + "I.jpg");
                        System.IO.File.Delete(savePath + FileName + "A.jpg");
                        System.IO.File.Delete(savePath + FileName + "F.jpg");
                        System.IO.File.Delete(savePath + FileName + "W.jpg");
                    }
                    catch
                    {
                    }

                }

                PdfPTable tabNote2 = new PdfPTable(2);
                tabNote2.WidthPercentage = 100;

                PdfPCell cellNote2 = new PdfPCell();
                iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph("" + Environment.NewLine + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);


                parCellNote2 = new iTextSharp.text.Paragraph("" + Environment.NewLine + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2 = new PdfPCell();
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);
                cartPDF.Add(tabNote2);

                String txtNote = ResListOrderProduct.ListOrderProduct.lblPNote + ":";
                iTextSharp.text.Paragraph noteField = new iTextSharp.text.Paragraph(txtNote, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                noteField.Alignment = Element.ALIGN_LEFT;
                cartPDF.Add(noteField);

                cartPDF.Close();
                //ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                ret = "/Data/Produzione/" + FileNamePDF;
                return ret;

            }
        }

        public String PrintSingleA3SheetsBarcodes(int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/PrintSingleA3SheetsBarcodes", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/PrintSingleA3SheetsBarcodes", "", ipAddr);
            }

            String ret = "";
            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
            Commessa comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
            // Ora creo il pdf!
            String savePath = Server.MapPath(@"~\Data\Produzione\");
            Document cartPDF = new Document(PageSize.A3, 630, 30, 20, 20);
            cartPDF.SetPageSize(PageSize.A3.Rotate());

            art.loadTasksProduzione();
            String FileNamePDF = "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + ".pdf";
            // Controllo che il pdf non esista, e se esiste lo cancello.

            if (System.IO.File.Exists(savePath + FileNamePDF))
            {
                try
                {

                    System.IO.File.Delete(savePath + FileNamePDF);
                }
                catch
                {
                }
            }

            using (System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create))
            {

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);

                cartPDF.Open();

                PdfPTable tabIntest = new PdfPTable(3);
                tabIntest.WidthPercentage = 100;

                PdfPCell[] intestazioneFoglio = new PdfPCell[5];
                intestazioneFoglio[0] = new PdfPCell();
                intestazioneFoglio[1] = new PdfPCell();
                intestazioneFoglio[2] = new PdfPCell();
                intestazioneFoglio[3] = new PdfPCell();
                intestazioneFoglio[4] = new PdfPCell();

                Logo logoAzienda = new Logo(Session["ActiveWorkspace_Name"].ToString());
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath(logoAzienda.filePath));
                if (logo.Height > logo.Width)
                {
                    logo.ScaleToFit(150 * logo.Width / logo.Height, 150);
                }
                else
                {
                    logo.ScaleToFit(140, 140 * logo.Height / logo.Width);
                }

                intestazioneFoglio[0].AddElement(logo);

                String idComm = comm.ID.ToString() + "/" + comm.Year.ToString();
                if (comm.ExternalID.Length > 0)
                {
                    idComm = comm.ExternalID;
                }
                Cliente cln = new Cliente(Session["ActiveWorkspace_Name"].ToString(), art.Cliente);
                String txtCommessa = cln.RagioneSociale + Environment.NewLine
                    + ResListOrderProduct.ListOrderProduct.lblOrdine + " " + idComm + Environment.NewLine
                    + Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                    + " - " + ResListOrderProduct.ListOrderProduct.lblQuantity + ": " + art.Quantita.ToString()
                        + Environment.NewLine + ResListOrderProduct.ListOrderProduct.lblDataConsegna + ": " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");

                iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                intestazioneFoglio[1].Rowspan = 2;
                intestazioneFoglio[1].AddElement(commessa);

                System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");

                iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPStatoProdotto, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                statusProd.Alignment = Element.ALIGN_CENTER;
                statusCode.Alignment = Element.ALIGN_CENTER;
                intestazioneFoglio[2].AddElement(statusProd);
                intestazioneFoglio[2].AddElement(statusCode);



                float[] widths = new float[] { 150, (490 - 300), 150 };
                tabIntest.SetWidths(widths);

                iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[3].AddElement(noteOC);

                iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPInizio + ": " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[4].AddElement(inizioProd);

                tabIntest.AddCell(intestazioneFoglio[0]);
                tabIntest.AddCell(intestazioneFoglio[1]);
                tabIntest.AddCell(intestazioneFoglio[2]);
                tabIntest.AddCell(intestazioneFoglio[3]);
                tabIntest.AddCell(intestazioneFoglio[4]);




                commessa.Alignment = Element.ALIGN_LEFT;
                //cartPDF.Add(commessa);
                cartPDF.Add(tabIntest);




                for (int i = 0; i < art.Tasks.Count; i++)
                {
                    System.Drawing.Image StartCode = GenCode128.Code128Rendering.MakeBarcodeImage("I" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image EndCode = GenCode128.Code128Rendering.MakeBarcodeImage("F" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image PauseCode = GenCode128.Code128Rendering.MakeBarcodeImage("A" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);
                    System.Drawing.Image WarningCode = GenCode128.Code128Rendering.MakeBarcodeImage("W" + art.Tasks[i].TaskProduzioneID.ToString(), 2, true);

                    String FileName = "articolo" + art.ID.ToString() + "_" + art.Year.ToString();
                    bool check = true;
                    try
                    {
                        StartCode.Save(savePath + FileName + "I.jpg");
                        EndCode.Save(savePath + FileName + "F.jpg");
                        PauseCode.Save(savePath + FileName + "A.jpg");
                        WarningCode.Save(savePath + FileName + "W.jpg");
                        check = true;
                    }
                    catch (Exception ex)
                    {
                        check = false;
                    }


                    PdfPTable tab = new PdfPTable(4);
                    tab.WidthPercentage = 100;

                    PdfPCell[] intestCella = new PdfPCell[4];
                    intestCella[0] = new PdfPCell();
                    intestCella[1] = new PdfPCell();
                    intestCella[2] = new PdfPCell();
                    intestCella[3] = new PdfPCell();

                    String txtPostazione = art.Tasks[i].Name + " (" + art.Tasks[i].TaskProduzioneID.ToString() + ")";
                    iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                    posta.Alignment = Element.ALIGN_LEFT;


                    String txtTask = ResListOrderProduct.ListOrderProduct.lblBNumOp + ": " + art.Tasks[i].NumOperatori.ToString();
                    iTextSharp.text.Paragraph task = new iTextSharp.text.Paragraph(txtPostazione + "; " + txtTask, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                    intestCella[0].AddElement(task);
                    intestCella[0].Colspan = 4;
                    PdfPRow intestazione = new PdfPRow(intestCella);
                    intestazione.MaxHeights = 100;

                    tab.Rows.Add(intestazione);


                    PdfPCell[] celle = new PdfPCell[4];
                    for (int q = 0; q < 4; q++)
                    {
                        celle[q] = new PdfPCell();
                        celle[q].FixedHeight = 45;
                    }

                    celle[0].Padding = 5;
                    celle[0].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "I.jpg"));
                    Paragraph pin = new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPInizio, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    celle[0].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[0].AddElement(pin);
                    celle[1].Padding = 5;
                    celle[1].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPPausa, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                    celle[1].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[2].Padding = 5;
                    celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                    celle[2].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPFine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[2].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[3].Padding = 5;
                    celle[3].AddElement(new iTextSharp.text.Paragraph(ResListOrderProduct.ListOrderProduct.lblPProblema, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[3].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "W.jpg"));
                    celle[3].HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPRow riga = new PdfPRow(celle);
                    riga.MaxHeights = 100;
                    tab.Rows.Add(riga);
                    cartPDF.Add(tab);

                    cartPDF.Add(new Paragraph(Environment.NewLine));
                    try
                    {
                        System.IO.File.Delete(savePath + FileName + "I.jpg");
                        System.IO.File.Delete(savePath + FileName + "A.jpg");
                        System.IO.File.Delete(savePath + FileName + "F.jpg");
                        System.IO.File.Delete(savePath + FileName + "W.jpg");
                    }
                    catch
                    {
                    }
                }

                PdfPTable tabNote2 = new PdfPTable(2);
                tabNote2.WidthPercentage = 100;

                PdfPCell cellNote2 = new PdfPCell();
                iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph("" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);


                parCellNote2 = new iTextSharp.text.Paragraph("" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2 = new PdfPCell();
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);
                cartPDF.Add(tabNote2);


                String txtNote = ResListOrderProduct.ListOrderProduct.lblPNote + ": ";
                iTextSharp.text.Paragraph noteField = new iTextSharp.text.Paragraph(txtNote, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                noteField.Alignment = Element.ALIGN_LEFT;
                cartPDF.Add(noteField);

                cartPDF.Close();
                ret = "/Data/Produzione/" + FileNamePDF;
                //ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                return ret;
            }
        }

        /*
          * Lancia il processo in produzione
          * Returns:
          * 0 se errore generico
          * 1 se tutto ok
          * 2 se non sono state impostate correttamente EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate
          * 3 se l'articolo è già stato lanciato in produzione
          * 
          * 5 se il tempo a disposizione è < del CriticalPath, quindi va ritardata la consegna
          * 6 se non sono riuscito a dare un EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate a tutti i task.
          * 7 se la data prevista di fine produzione non è reale
          * 8 if user is not authorized.
          */
        public int ReprogramProduct(int ProductID, int ProductYear, int DepartmentID, DateTime EndProductionDate)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/ReprogramProduct", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/ReprogramProduct", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentID);

                if (art != null && art.ID != -1 && (art.Status == 'I' || art.Status =='P') && rp != null && rp.id != -1
                    && EndProductionDate > DateTime.UtcNow && EndProductionDate <= art.DataPrevistaConsegna)
                {
                    art.Reparto = rp.id;
                    art.DataPrevistaFineProduzione = EndProductionDate;
                    ret = art.SpostaPianificazione(art.DataPrevistaFineProduzione, art.DataPrevistaConsegna);
                }
                else
                {
                    ret = 5;
                }
            }
            else
            {
                ret = 8;
            }
            return ret;
        }

        public ActionResult ShowManageOperators(int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/SalesOrders/SalesOrder/AddSalesOrder", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/SalesOrders/SalesOrder/AddSalesOrder", "", ipAddr);
            }
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManageOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ViewBag.ProductID = ProductID;
                ViewBag.ProductYear = ProductYear;

                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                art.loadTasksProduzione();
                return View(art.Tasks);
            }
                return View();
        }

        /* Returns:
         * 0 if error
         * 1 if ok
         */
         public int AssignOperatorToTask(int TaskID, String defOp)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManageOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tsk.TaskProduzioneID!=-1)
                {
                    ret = tsk.addAssignedOperator(defOp);
                }
            }

                return ret;
        }

        public ActionResult ListTasksDefOperators(int ProductID, int ProductYear)
        {
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManageOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                if(art.ID!=-1 && art.Year!=-1)
                {
                    art.loadTasksProduzione();
                    return View(art.Tasks);
                }
            }
                return View();
        }

        /* Returns:
         * 0 if error
         * 1 if ok
         */
        public int DeleteAssignedOperator(int TaskID, String defOp)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManageOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    ret = tsk.deleteAssignedOperator(defOp);
                }
            }

            return ret;
        }

        /*
         * Returns:
         * 1970-1-1 if generic error
         * Foreseen end date if everything is ok
         * 1970-1-2 if user not authorized
         * 1970-1-3 if task not found
         */
        public DateTime RescheduleTaskGetEndDate(int TaskID, DateTime start)
        {
            DateTime ret = new DateTime(1970,1,1);
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/SalesOrders/SalesOrder/RescheduleTaskGetEndDate", "TaskID="+ TaskID+"&start="+start.ToString("yyyy-MM-dd HH:mm:ss"), ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/SalesOrders/SalesOrder/RescheduleTaskGetEndDate", "TaskID=" + TaskID + "&start=" + start.ToString("yyyy-MM-dd HH:mm:ss"), ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tsk!=null && tsk.TaskProduzioneID!=-1)
                {
                    ret = tsk.getEndDate(start);
                }
                else
                {
                    ret = new DateTime(1970, 1, 3);
                }
            }
            else
            {
                ret = new DateTime(19970, 1, 2);
            }
                return ret;
        }

        /*
         */
         public ActionResult RescheduleTasksManually(int ProductID, int ProductYear)
        {
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManuallyReschedule";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                if (art.ID != -1 && art.Year != -1)
                {
                    art.loadTasksProduzione();
                    return View(art.Tasks);
                }
            }
            return View();
        }

        /* This function returns the foreseen end date by using start date (as input) and CycleTime (TempoC)
         * Returns:
         * 1970-1-1 if it is not possible to calculate the end date
         * Foreseen finish date at UTC time otherwise
         */
        public DateTime GetFinishDateByStart(int TaskID, DateTime start)
        {
            DateTime ret = new DateTime(1970, 1, 1);
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManuallyReschedule";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                ret = tsk.getEndDate(start);
            }
                return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if saved changes successfully
         * 2 if user not authorized
         * 3 if error in input dates
         * 4 if task not found
         */
         public int SaveTaskDates(int TaskID, DateTime start, DateTime end)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Tasks ManuallyReschedule";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                if(start <= end && start >= DateTime.UtcNow)
                {
                    TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                    if(tsk.TaskProduzioneID!=-1)
                    { 
                    TimeSpan oldWT = tsk.TempoC;
                        DateTime oldstart = tsk.EarlyStart;
                        DateTime oldend = tsk.LateFinish;
                        User curr1 = (User)Session["user"];
                        tsk.logRescheduledTasks(tsk.TaskProduzioneID, oldstart, oldend, oldWT, curr1.username);
                        tsk.EarlyStart = start;
                    tsk.LateStart = start;
                    tsk.EarlyFinish = end;
                    tsk.LateFinish = end;

                    ret = 1;
                        
                    }
                    else
                    {
                        ret = 4;
                    }
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
}