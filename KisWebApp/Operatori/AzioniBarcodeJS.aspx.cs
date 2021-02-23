using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;

namespace KIS.Operatori
{
    public partial class AzioniBarcodeJS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (action.Value == "ManageTasks")
                {
                    imgOK.Visible = false;
                    imgKO.Visible = false;

                    bool rt = elabora();
                    if (rt == true)
                    {
                        log.Text += "<br /><span style='color: green; font-size:20px; font-weight: bold'>"
                            + GetLocalResourceObject("lblOperazioneOK").ToString()
                            +"</span>";
                        box2.Text = "";
                        box1.Text = "";
                        box1.Focus();
                        imgOK.Visible = true;
                    }
                    else
                    {
                        box2.Text = "";
                        box1.Text = "";
                        box1.Focus();
                        imgKO.Visible = true;
                    }
                }
                else if (action.Value == "ChangeQuantity")
                {
                    int newQty = -1;
                    int tskId = -1;
                    try
                    {
                        tskId = Int32.Parse(hFldTaskID.Value);
                        newQty = Int32.Parse(newQtyFld.Value);
                    }
                    catch
                    {
                        tskId = -1;
                        newQty = -1;
                    }

                    if (newQty > 0 && tskId != -1)
                    {
                        TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), tskId);
                        if (tsk.TaskProduzioneID != -1)
                        {
                            tsk.QuantitaProdotta = newQty;
                            Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), tsk.ArticoloID, tsk.ArticoloAnno);
                            if (art.Status == 'F')
                            {
                                art.QuantitaProdotta = newQty;
                            }
                            log.Text += "<br/>"
                                + GetLocalResourceObject("lblQtaEffProdotta").ToString()
                                + ": " + tsk.QuantitaProdotta.ToString();
                            imgChangeQty.Visible = false;
                        }
                    }
                    box1.Focus();
                }

            }
            else
            {
                box1.Focus();
                imgLoading.Style.Value = "visibility: hidden; height: 2px";
                tblLogUtente.Style.Value = "visibility: hidden";
                log.Text = GetLocalResourceObject("lblNowReadBarcodeTaskAction").ToString();
            }
        }

        protected bool elabora()
        {
            KISConfig vcCfg = new KISConfig(Session["ActiveWorkspace"].ToString());
            String origChecksum = vcCfg.basePath;
            imgLoading.Style.Value = "visibility: hidden; height: 2px";
            
            rptStatusCommessa.Visible = false;
            rptPostazioniAttive.Visible = false;

            bool rt = false;
            int usrID = -1;
            int postID = -1;
            String Sutente = box1.Text;
            String Spost = box2.Text;
            String Stask = box2.Text;
            String checksum = "";
            if ((Sutente[0] == 'U' && Spost[0] == 'P') || ((Sutente[0] == 'P' && Spost[0] == 'U')))
            {
                tblLogUtente.Style.Value = "visibility: visible";
                rptPostazioniAttive.Visible = true;
                if (Sutente[0] == 'P' && Spost[0] == 'U')
                {
                    // Li scambio
                    String swap = Sutente;
                    Sutente = Spost;
                    Spost = swap;
                }

                String Sutente1 = Sutente.Substring(1, Sutente.Length - 1);
                var Sutente2 = Sutente1.Split(';');
                Sutente = Sutente2[1];
                checksum = Sutente2[0];
                
                Spost = Spost.Substring(1, Spost.Length - 1);

                try
                {
                    usrID = Int32.Parse(Sutente);
                    postID = Int32.Parse(Spost);
                }
                catch
                {
                    usrID = -1;
                    postID = -1;
                }

                if (usrID != -1 && postID != -1 && checksum.Length > 0 && checksum == origChecksum)
                {
                    User usr = new User(Session["ActiveWorkspace"].ToString(), usrID);
                    Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), postID);
                    if (usr.username.Length > 0 && p.id != -1)
                    {
                        rt = true;

                        // Controllo che l'utente non sia loggato
                        p.loadUtentiLoggati();
                        bool check = false;
                        for (int i = 0; i < p.UtentiLoggati.Count; i++)
                        {
                            if (p.UtentiLoggati[i] == usr.username)
                            {
                                check = true;
                            }
                        }
                        //log.Text = check.ToString();
                        if (check == false)
                        {
                            //log.Text = "Eseguo il check-in";
                            // Entro nella postazione
                            log.Text = usr.name + " "+GetLocalResourceObject("lblAccedeAllaPostazione").ToString() +" " + p.name;
                            rt = usr.DoCheckIn(p);
                        }
                        else
                        {
                            //log.Text = "Eseguo il check-out";
                            // Esco dalla postazione
                            log.Text = usr.name + " " + GetLocalResourceObject("lblEsceDallaPostazione").ToString() + " " + p.name;
                            rt = usr.DoCheckOut(p);
                        }

                        loadTaskAvviati(usr);
                        loadPostazioni(usr);
                    }
                    else
                    {
                        box1.Text = "";
                        box2.Text = "";
                        rt = false;
                    }
                }
                else
                {
                    box1.Text = "";
                    box2.Text = "";
                    rt = false;
                    log.Text = "";
                }
            }
            else if (((Stask[0] == 'I' || Stask[0] == 'A' || Stask[0] == 'F' || Stask[0] == 'W') && Sutente[0] == 'U') || (((Sutente[0] == 'I' || Sutente[0] == 'A' || Sutente[0] == 'F' || Sutente[0] == 'W') && Stask[0] == 'U')))
            {
                tblLogUtente.Style.Value = "visibility: visible";
                rptPostazioniAttive.Visible = true;

                if ((Sutente[0] == 'I' || Sutente[0] == 'A' || Sutente[0] == 'F' || Sutente[0] == 'W') && Stask[0] == 'U')
                {
                    // Li scambio
                    String swap = Sutente;
                    Sutente = Stask;
                    Stask = swap;
                }

                String action = Stask.Substring(0, 1);
                //Sutente = Sutente.Substring(1, Sutente.Length - 1);
                String Sutente1 = Sutente.Substring(1, Sutente.Length - 1);
                var Sutente2 = Sutente1.Split(';');
                Sutente = Sutente2[1];
                checksum = Sutente2[0];

                Stask = Stask.Substring(1, Stask.Length - 1);

                int taskID = -1;
                try
                {
                    usrID = Int32.Parse(Sutente);
                    taskID = Int32.Parse(Stask);
                }
                catch
                {
                    usrID = -1;
                    taskID = -1;
                }

                if (checksum == origChecksum)
                {
                    User usr = new User(Session["ActiveWorkspace"].ToString(), usrID);
                    TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                    //Session["user"] = usr;
                    if (action == "I")
                    {
                        Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                        if (p.barcodeAutoCheckIn)
                        {
                            usr.DoCheckIn(p);
                        }

                        log.Text = usr.name + " "
                            + GetLocalResourceObject("lblFaPartireIlTask").ToString() + " " + tsk.TaskProduzioneID.ToString()
                            + " " + tsk.Name;
                        rt = tsk.Start(usr);
                        if (rt == false)
                        {
                            log.Text += "<br /><br/>" + GetLocalResourceObject("lblErrorReason").ToString()
                                + ":<br/> ";

                            // Controllo che il task non sia già avviato
                            usr.loadTaskAvviati();
                            for (int i = 0; i < usr.TaskAvviati.Count; i++)
                            {
                                if (usr.TaskAvviati[i] == tsk.TaskProduzioneID)
                                {
                                    log.Text += "- " + GetLocalResourceObject("lblErrorReason1").ToString() + " " + tsk.Name + " (ID: " +
                                        tsk.TaskProduzioneID.ToString() + ")<br />";
                                }
                            }

                            // Controllo che il task non sia già completato
                            if (tsk.Status == 'F')
                            {
                                log.Text += "- " + GetLocalResourceObject("lblErrorReason2A").ToString() + " " + tsk.Name + " (ID: " +
                                        tsk.TaskProduzioneID.ToString() + ") "
                                        + GetLocalResourceObject("lblErrorReason2B").ToString()
                                        + "<br />";
                            }

                            // Controllo che l'utente sia loggato in postazione
                            bool foundPost = false;
                            usr.loadPostazioniAttive();
                            for (int i = 0; i < usr.PostazioniAttive.Count; i++)
                            {
                                if (usr.PostazioniAttive[i].id == tsk.PostazioneID)
                                {
                                    foundPost = true;
                                }
                            }

                            if (foundPost == false)
                            {
                                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                                log.Text += "- " + GetLocalResourceObject("lblErrorReason3").ToString() + " " + pst.name + "<br />";
                            }

                            tsk.loadPrecedenti();
                            for (int i = 0; i < tsk.IdPrecedenti.Count; i++)
                            {
                                TaskProduzione prec = new TaskProduzione(Session["ActiveWorkspace"].ToString(), tsk.IdPrecedenti[i]);
                                if (prec.Status != 'F')
                                {
                                    log.Text += "- " + GetLocalResourceObject("lblErrorReason4A").ToString() + " \"" + prec.Name
                                        + "\" (ID: " + prec.TaskProduzioneID.ToString() + ")"
                                        + " "
                                        + GetLocalResourceObject("lblErrorReason4B").ToString() +
                                        ".<br />";
                                }
                            }

                            // Controllo che l'utente non abbia avviato troppi tasks
                            usr.loadTaskAvviati();
                            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), tsk.RepartoID);
                            if (rp.TasksAvviabiliContemporaneamenteDaOperatore > 0 && usr.TaskAvviati.Count >= rp.TasksAvviabiliContemporaneamenteDaOperatore)
                            {
                                log.Text += GetLocalResourceObject("lblMaxTasksReached").ToString() + ":<br /><UL>";
                                for (int i = 0; i < usr.TaskAvviati.Count; i++)
                                {
                                    TaskProduzione tskAttivo = new TaskProduzione(Session["ActiveWorkspace"].ToString(), usr.TaskAvviati[i]);
                                    Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), tskAttivo.ArticoloID, tskAttivo.ArticoloAnno);
                                    Commessa cm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
                                    log.Text += "<li>"
                                        + tskAttivo.TaskProduzioneID.ToString() + " "
                                        + tskAttivo.Name
                                        + " " + GetLocalResourceObject("lblTHPostazione").ToString() + ": " + tskAttivo.PostazioneName
                                        + " " + GetLocalResourceObject("lblTHProdotto").ToString() + ": " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante
                                        + " " + GetLocalResourceObject("lblTHCommessa").ToString() + ": " + cm.ID.ToString() + "/" + cm.Year.ToString()
                                        + " " + GetLocalResourceObject("lblTHCliente").ToString() + ": " + cm.Cliente
                                        + "</li>";

                                }
                                log.Text += "</ul>";
                            }
                        }
                    }
                    else if (action == "A")
                    {
                        tblLogUtente.Style.Value = "visibility: visible";
                        rptPostazioniAttive.Visible = true;
                        log.Text = usr.name + " " + GetLocalResourceObject("lblMetteInPausa")
                            + " " + tsk.TaskProduzioneID.ToString() + " " + tsk.Name;
                        rt = tsk.Pause(usr);
                        if (rt == false)
                        {
                            log.Text += "<br /><br/>" + GetLocalResourceObject("lblErrorReason") + ":<br/>";
                            if (tsk.Status == 'F')
                            {
                                log.Text += "- " + GetLocalResourceObject("lblErrorReason2A")
                                    + " " + tsk.Name + " (ID: " + tsk.TaskProduzioneID.ToString() + ")"
                                    + " "
                                    + GetLocalResourceObject("lblErrorReason2B") + ".<br />";
                            }

                            // Controllo che l'utente stia effettivamente lavorando sul task
                            usr.loadTaskAvviati();
                            bool foundtask = false;
                            for (int i = 0; i < usr.TaskAvviati.Count; i++)
                            {
                                if (usr.TaskAvviati[i] == tsk.TaskProduzioneID)
                                {
                                    foundtask = true;
                                }
                            }
                            if (foundtask == false && tsk.Status != 'F')
                            {
                                log.Text += "- " + GetLocalResourceObject("lblYouAreNotWorking1").ToString()
                                    + " " + tsk.Name + " (ID: " + tsk.TaskProduzioneID.ToString() + ")."
                                    + " " + GetLocalResourceObject("lblYouAreNotWorking2").ToString() + ".<br />";
                            }
                        }
                        Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                        if (p.barcodeAutoCheckIn)
                        {
                            usr.DoCheckOut(p);
                        }
                    }
                    else if (action == "F")
                    {
                        tblLogUtente.Style.Value = "visibility: visible";
                        rptPostazioniAttive.Visible = true;

                        log.Text = usr.name + " " + GetLocalResourceObject("lblCompletaTask").ToString()
                            + " " + tsk.TaskProduzioneID.ToString() + " " + tsk.Name;
                        rt = tsk.Complete(usr);
                        if (rt == false)
                        {
                            log.Text += "<br /><br/>" + GetLocalResourceObject("lblErrorReason").ToString() + ":<br/>";
                            if (tsk.Status == 'F')
                            {
                                log.Text += "- " + GetLocalResourceObject("lblErrorReason2A").ToString()
                                    + " " + tsk.Name + " (ID: " + tsk.TaskProduzioneID.ToString() + ")"
                                    + " " + GetLocalResourceObject("lblErrorReason2B").ToString() + ".<br />";
                            }
                            // Controllo che l'utente stia effettivamente lavorando sul task
                            usr.loadTaskAvviati();
                            bool foundtask = false;
                            for (int i = 0; i < usr.TaskAvviati.Count; i++)
                            {
                                if (usr.TaskAvviati[i] == tsk.TaskProduzioneID)
                                {
                                    foundtask = true;
                                }
                            }
                            if (foundtask == false && tsk.Status != 'F')
                            {
                                log.Text += "- " + GetLocalResourceObject("lblYouAreNotWorking1").ToString()
                                    + " " + tsk.Name + " (ID: " + tsk.TaskProduzioneID.ToString() + ")."
                                    + " " + GetLocalResourceObject("lblYouAreNotWorking3").ToString() + ".<br />";
                            }

                            usr.loadPostazioniAttive();
                            bool checkpost = false;
                            for (int i = 0; i < usr.PostazioniAttive.Count; i++)
                            {
                                if (usr.PostazioniAttive[i].id == tsk.PostazioneID)
                                {
                                    checkpost = true;
                                }
                            }
                            if (checkpost == false)
                            {
                                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                                log.Text += "- " + GetLocalResourceObject("lblNonAcceduto").ToString()
                                    + " " + pst.name + "<br />";
                            }

                            // Check that all the previous tasks had been closed.
                            Boolean checkPreviousTasks = true;
                            tsk.loadPrecedenti();
                            String errPreviousTasks = "";
                            for (int i = 0; i < tsk.PreviousTasks.Count; i++)
                            {
                                TaskProduzione curr = new TaskProduzione(Session["ActiveWorkspace"].ToString(), tsk.PreviousTasks[i].NearTaskID);
                                if(curr.Status!='F')
                                {
                                    checkPreviousTasks = false;
                                    errPreviousTasks += curr.Name + "<br />";
                                }
                                    }

                            if(!checkPreviousTasks)
                            {
                                log.Text += GetLocalResourceObject("lblErrPreviousTasks").ToString()
                                    + "<br />" + errPreviousTasks + "<br />";
                            }
                            
                        }
                        else
                        {
                            imgChangeQty.Visible = true;
                            hFldTaskID.Value = tsk.TaskProduzioneID.ToString();
                        }
                        Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                        if (p.barcodeAutoCheckIn)
                        {
                            usr.DoCheckOut(p);
                        }
                    }
                    else if (action == "W")
                    {
                        tblLogUtente.Style.Value = "visibility: visible";
                        rptPostazioniAttive.Visible = true;

                        log.Text = usr.name + " " + GetLocalResourceObject("lblSegnalaUnProblema").ToString()
                            + " " + tsk.TaskProduzioneID.ToString() + " " + tsk.Name;
                        rt = tsk.generateWarning(usr);
                        if (rt == false)
                        {
                            log.Text += "<br /><br/>" + GetLocalResourceObject("lblErrorReason").ToString() + ":<br/>"
                                    + "- " + GetLocalResourceObject("lblErrorReason5").ToString() + "<br/>";
                        }
                    }
                    loadTaskAvviati(usr);
                    loadPostazioni(usr);
                }
                else
                {
                    box1.Text = "";
                    box2.Text = "";
                    rt = false;
                    log.Text = GetLocalResourceObject("lblChecksumError").ToString();
                }
            }
            else if ((Stask[0] == 'B' && Sutente[0] == 'U') || ((Sutente[0] == 'B' && Stask[0] == 'U')))
            {
                tblLogUtente.Style.Value = "visibility: hidden";
                if (Sutente[0] == 'B' && Stask[0] == 'U')
                {
                    // Li scambio
                    String swap = Sutente;
                    Sutente = Stask;
                    Stask = swap;
                }

                String Sutente1 = Sutente.Substring(1, Sutente.Length - 1);
                var Sutente2 = Sutente1.Split(';');
                Sutente = Sutente2[1];
                checksum = Sutente2[0];
                String idcommessa = Stask.Substring(1, Stask.Length - 1);

                int idComm = -1;
                int yearComm = -1;
                try
                {
                    String[] splitted = new String[2];
                    splitted[0] = "";
                    splitted[1] = "";
                    splitted = idcommessa.Split('.');
                    idComm = Int32.Parse(splitted[0]);
                    yearComm = Int32.Parse(splitted[1]);
                }
                catch
                {
                    idComm = -1;
                    yearComm = -1;
                }

                log.Text = GetLocalResourceObject("lblTHProdotto").ToString() + " " + idComm.ToString() + "/" + yearComm.ToString() + "<br />";
                Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), idComm, yearComm);
                if (art.ID != -1 && checksum == origChecksum)
                {
                    rptStatusCommessa.Visible = true;
                    log.Text = GetLocalResourceObject("lblStatoProdotto").ToString() + " " + art.ID.ToString()
                        + "/"
                        + art.Year.ToString()
                        + " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante
                        + " "+GetLocalResourceObject("lblTHCliente").ToString() + ": " + art.Cliente;
                    art.loadTasksProduzione();
                    rptStatusCommessa.DataSource = art.Tasks;
                    rptStatusCommessa.DataBind();
                }
                else
                {
                    log.Text = GetLocalResourceObject("lblErrorProductNotFound").ToString();
                    rt = false;
                }
                rt = true;

            }
            else
            {
                box1.Text = "";
                box2.Text = "";
                rt = false;
                log.Text = "";
            }
            return rt;
        }

        protected void loadTaskAvviati(User uten)
        {
            // Carico il repeater dei task avviati dall'utente
            uten.loadTaskAvviati();
            List<TaskProduzione> tsk = new List<TaskProduzione>();
            for (int i = 0; i < uten.TaskAvviati.Count; i++)
            {
                tsk.Add(new TaskProduzione(Session["ActiveWorkspace"].ToString(), uten.TaskAvviati[i]));
            }
            if (tsk.Count > 0)
            {
                rptTaskAvviati.Visible = true;
                rptTaskAvviati.DataSource = tsk;
                rptTaskAvviati.DataBind();
            }
            else
            {
                rptTaskAvviati.Visible = false;
            }
        }

        protected void rptTaskAvviati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hID = (HiddenField)e.Item.FindControl("taskID");
                Label lblUtentiAttivi = (Label)e.Item.FindControl("lblUtentiAttivi");
                Label lblCommessa = (Label)e.Item.FindControl("lblCommessa");
                Label lblAnnoCommessa = (Label)e.Item.FindControl("lblAnnoCommessa");
                Label lblCliente = (Label)e.Item.FindControl("lblCliente");
                Label lblProdotto = (Label)e.Item.FindControl("lblProdotto");
                Label lblProcesso = (Label)e.Item.FindControl("lblProcesso");
                Label lblMatricola = (Label)e.Item.FindControl("lblMatricola");

                int id = -1;
                try
                {
                    id = Int32.Parse(hID.Value.ToString());
                }
                catch
                {
                    id = -1;
                }

                if (id != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), id);
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFD700'");
                        if (tsk.LateFinish <= DateTime.Now)
                        {
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                        }
                        else if (tsk.EarlyFinish <= DateTime.Now)
                        {
                            tRow.BgColor = "#FFFF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                        }
                        else
                        {
                            tRow.BgColor = "#00FF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                        }


                    }

                    Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), tsk.ArticoloID, tsk.ArticoloAnno);
                    Commessa comms = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
                    lblAnnoCommessa.Text = comms.Year.ToString();
                    lblCliente.Text = comms.Cliente;
                    lblCommessa.Text = comms.ID.ToString();
                    lblProcesso.Text = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                    lblMatricola.Text = art.Matricola;
                    lblProdotto.Text = art.ID.ToString() + "/" + art.Year.ToString();

                }
            }
        }

        protected void loadPostazioni(User uten)
        {
                uten.loadPostazioniAttive();
                if (uten.PostazioniAttive.Count > 0)
                {
                    rptPostazioniAttive.DataSource = uten.PostazioniAttive;
                    rptPostazioniAttive.DataBind();
                }
                else
                {
                    rptPostazioniAttive.Visible = false;
                }
            }

        protected void rptPostazioniAttive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField HID = (HiddenField)e.Item.FindControl("id");
                int pstID = -1;
                try
                {
                    pstID = Int32.Parse(HID.Value.ToString());
                }
                catch
                {
                    pstID = -1;
                }

                if (pstID != -1)
                {
                    //User curr = (User)Session["user"];

                    // Inserisco la lista degli utenti già loggati
                    Label lblUserLoggati = (Label)e.Item.FindControl("lblUserLogged");
                    Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                    p.loadUtentiLoggati();

                    for (int i = 0; i < p.UtentiLoggati.Count; i++)
                    {
                        
                            lblUserLoggati.Text += p.UtentiLoggati[i];
                            if (i < p.UtentiLoggati.Count - 1)
                            {
                                lblUserLoggati.Text += "<br/>";
                            }
                        
                    }
                }
            }
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

        protected void rptStatusCommessa_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hTaskID = (HiddenField)e.Item.FindControl("hTaskID");
            Label lblOperatori = (Label)e.Item.FindControl("lblOperatori");
            int taskID = -1;
            try
            {
                taskID = Int32.Parse(hTaskID.Value);
            }
            catch
            {
                taskID = -1;
            }

            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                    // Carico lo stato
                    Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                    if (tsk.Status == 'F')
                    {
                        lblStatus.Text = GetLocalResourceObject("lblFinito").ToString();// "Finito";
                        tRow.BgColor = "#ADFF2F";
                    }
                    else if (tsk.Status == 'I')
                    {
                        lblStatus.Text = GetLocalResourceObject("lblInCorso").ToString(); //"In corso";
                        tRow.BgColor = "#ADD8E6";
                    }
                    else if (tsk.Status == 'N')
                    {
                        lblStatus.Text = GetLocalResourceObject("lblNonStarted").ToString(); //"Non ancora iniziato";
                        tRow.BgColor = "#FFFFFF";
                    }
                    else if (tsk.Status == 'P')
                    {
                        lblStatus.Text = "In pausa";
                        tRow.BgColor = "#ffa500";
                    }
                }

            }


            if (taskID != -1)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                String configShowNomi = "0";
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), tsk.RepartoID);
                if (rp.id != -1)
                {
                    configShowNomi = rp.AndonPostazioniFormatoUsername.ToString();
                }

                tsk.loadOperatori();
                for (int i = 0; i < tsk.Operatori.Count; i++)
                {
                    if (configShowNomi == "0")
                    {
                        lblOperatori.Text += tsk.Operatori[i];
                    }
                    else if (configShowNomi == "1")
                    {
                        User usr = new User(tsk.Operatori[i]);
                        lblOperatori.Text += usr.name;
                    }
                    else if (configShowNomi == "2")
                    {
                        User usr = new User(tsk.Operatori[i]);
                        lblOperatori.Text += usr.name + " " + usr.cognome.Substring(0, 1);
                    }
                    else if (configShowNomi == "3")
                    {
                        User usr = new User(tsk.Operatori[i]);
                        lblOperatori.Text += usr.name + " " + usr.cognome;
                    }
                    if (i < tsk.Operatori.Count - 1)
                    {
                        lblOperatori.Text += "<br />";
                    }
                }

                // Utenti attivi -- OLD
                /*
                if (tsk.Status == 'I')
                {
                    
                    tsk.loadUtentiAttivi();
                    for (int i = 0; i < tsk.UtentiAttivi.Count; i++)
                    {
                        if (configShowNomi == "0")
                        {
                            lblOperatori.Text += tsk.UtentiAttivi[i];
                        }
                        else if (configShowNomi == "1")
                        {
                            User usr = new User(tsk.UtentiAttivi[i]);
                            lblOperatori.Text += usr.name;
                        }
                        else if (configShowNomi == "2")
                        {
                            User usr = new User(tsk.UtentiAttivi[i]);
                            lblOperatori.Text += usr.name + " " + usr.cognome.Substring(0, 1);
                        }
                        else if (configShowNomi == "3")
                        {
                            User usr = new User(tsk.UtentiAttivi[i]);
                            lblOperatori.Text += usr.name + " " + usr.cognome;
                        }
                        if (i < tsk.UtentiAttivi.Count - 1)
                        {
                            lblOperatori.Text += "<br />";
                        }
                    }
                }
                else if (tsk.Status == 'P')
                {
                    tsk.loadEventi();
                    List<String> utentiTask = new List<string>();
                    for (int i = 0; i < tsk.Eventi.Count; i++)
                    {
                        if (tsk.Eventi[i].Evento == 'P')
                        {
                            if (configShowNomi == "0")
                            {

                                utentiTask.Add(tsk.Eventi[i].User);
                            }
                            else if (configShowNomi == "1")
                            {
                                User usr = new User(tsk.Eventi[i].User);
                                utentiTask.Add(usr.name);
                            }
                            else if (configShowNomi == "2")
                            {
                                User usr = new User(tsk.Eventi[i].User);
                                utentiTask.Add(usr.name + " " + usr.cognome.Substring(0, 1) + ".");
                            }
                            else if (configShowNomi == "3")
                            {
                                User usr = new User(tsk.Eventi[i].User);
                                utentiTask.Add(usr.name + " " + usr.cognome);
                            }
                            
                        }
                    }

                    List<String> utentiDistinct = utentiTask.Distinct().ToList();
                    for (int i = 0; i < utentiDistinct.Count; i++)
                    {
                        lblOperatori.Text += utentiDistinct[i];
                        if (i < tsk.Eventi.Count - 1)
                        {
                            lblOperatori.Text += "<br />";
                        }
                    }
                }*/
            }
        }

    }
}