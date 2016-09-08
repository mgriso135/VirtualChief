using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class listArticoliINP : System.Web.UI.UserControl
    {
        public static List<Articolo> artINP;
        public static String ordine;
        protected void Page_Load(object sender, EventArgs e)
        {            
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Articolo Depianifica";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    ElencoArticoliInProduzione el = new ElencoArticoliInProduzione();
                    ElencoArticoli elN = new ElencoArticoli('N');
                    artINP = el.ElencoArticoli.Concat(elN.ListArticoli).ToList();
                    List<Articolo> artINPOrdered = artINP.OrderBy(x => x.Status).ThenBy(x => x.DataPrevistaFineProduzione).ThenBy(x=>x.DataPrevistaConsegna).ToList();
                    //rptArticoli.DataSource = el.ElencoArticoli;
                    rptArticoli.DataSource = artINPOrdered;
                    rptArticoli.DataBind();
                }
            }
            else
            {
                rptArticoli.Visible = false;
                lbl1.Text = "Non hai il permesso di visualizzare il piano di produzione.<br />";
            }
        }

        protected void rptArticoli_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                    HiddenField hID = (HiddenField)e.Item.FindControl("lblIDArticolo");
                    HiddenField hYear = (HiddenField)e.Item.FindControl("lblAnnoArticolo");
                    System.Web.UI.WebControls.Image imgPrintKbCard = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPrintKanbanCard");
                    HyperLink lnkPrintKbCard = (HyperLink)e.Item.FindControl("lnkPrintKanbanCard");

                    ImageButton imgPrintOrdini = (ImageButton)e.Item.FindControl("imgPrintOrdini");
                    ImageButton imgPrintOrdini2 = (ImageButton)e.Item.FindControl("imgPrintOrdini2");
                    ImageButton imgPrintOrdiniSingolo = (ImageButton)e.Item.FindControl("imgPrintOrdiniSingolo");
                    ImageButton imgPrintOrdiniSingoloA3 = (ImageButton)e.Item.FindControl("imgPrintOrdiniSingoloA3");

                    HyperLink lnkRipianifica = (HyperLink)e.Item.FindControl("lnkRipianifica");
                    System.Web.UI.WebControls.Image imgRipianifica = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgRipianifica");

                    int idArt = -1;
                    int yearArt = -1;
                    try
                    {
                        idArt = Int32.Parse(hID.Value.ToString());
                        yearArt = Int32.Parse(hYear.Value.ToString());
                    }
                    catch
                    {
                        idArt = -1;
                        yearArt = -1;
                    }

                    if (idArt != -1 && yearArt != -1)
                    {
                        Articolo art = new Articolo(idArt, yearArt);
                        if (art.ID != -1)
                        {
                            if (art.KanbanCardID.Length == 0)
                            {
                                lnkPrintKbCard.Visible = false;
                                imgPrintKbCard.Visible = false;
                            }

                            if (art.Status == 'N')
                            {
                                imgPrintOrdini.Visible = false;
                                imgPrintOrdini2.Visible = false;
                                imgPrintOrdiniSingolo.Visible = false;
                                imgPrintOrdiniSingoloA3.Visible = false;

                                imgRipianifica.Visible = true;
                                lnkRipianifica.Visible = true;
                                lnkRipianifica.Enabled = true;
                                lnkRipianifica.NavigateUrl = "~/Commesse/wzAssociaPERTReparto.aspx?idCommessa=" + art.Commessa.ToString()
                                    + "&annoCommessa=" + art.AnnoCommessa.ToString()
                                    + "&idProc=" + art.Proc.process.processID.ToString()
                                    + "&revProc=" + art.Proc.process.revisione.ToString()
                                    + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                                    + "&idProdotto=" + art.ID.ToString()
                                    + "&annoProdotto=" + art.Year.ToString()
                                    + "&quantita=" + art.Quantita.ToString();
                            }
                            else if(art.Status == 'I' || art.Status == 'P')
                            {
                                imgPrintOrdini.Visible = true;
                                imgPrintOrdini2.Visible = true;
                                imgPrintOrdiniSingolo.Visible = true;
                                imgPrintOrdiniSingoloA3.Visible = true;
                                imgRipianifica.Visible = true;
                                lnkRipianifica.Visible = true;
                                lnkRipianifica.Enabled = true;

                                lnkRipianifica.NavigateUrl = "~/Commesse/wzInserisciDataConsegna.aspx?"
                                    + "idCommessa=" + art.Commessa.ToString()
                                    + "&annoCommessa=" + art.AnnoCommessa.ToString()
                                    + "&idProc=" + art.Proc.process.processID.ToString()
                                    + "&revProc=" + art.Proc.process.revisione.ToString()
                                    + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                                    + "&idReparto=" + art.Reparto.ToString()
                                    + "&idProdotto=" + art.ID.ToString()
                                    + "&annoProdotto=" + art.Year.ToString()
                                    + "&quantita=" + art.Quantita.ToString();
                            }
                        }
                    }
                    
                    
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");

                    if (lblStatus.Text == "P")
                    {
                        tRow.BgColor = "#C0C0C0";
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                    }
                    else if (lblStatus.Text == "I")
                    {
                        tRow.BgColor = "#00FF00";
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                    }
                    else if (lblStatus.Text == "N")
                    {
                        tRow.BgColor = "#FFFFFF";
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                    }
                }
            }

        }

        protected void rptArticoli_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String[] sArtID = e.CommandArgument.ToString().Split(';');
            int artID = -1;
            int artYear = -1;
            if (sArtID.Length == 2)
            {
                try
                {
                    artID = Int32.Parse(sArtID[0]);
                    artYear = Int32.Parse(sArtID[1]);
                }
                catch
                {
                    artID = -1;
                    artYear = -1;
                }
            }

            if (artID != -1 && artYear != -1)
            {
                if (e.CommandName == "printOrdini")
                {
                    Articolo art = new Articolo(artID, artYear);
                    Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
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

                    for(int i = 0; i < art.Tasks.Count; i++)
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

                        Logo logoAzienda = new Logo();
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath(logoAzienda.filePath));
                        logo.ScaleToFit(50 * logo.Width / logo.Height, 50);
                        cartPDF.Add(logo);

                        String txtPostazione = "Postazione " + art.Tasks[i].PostazioneName;
                        iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 40, Font.BOLD));
                        posta.Alignment = Element.ALIGN_CENTER;
                        cartPDF.Add(posta);
                        String txtCommessa = comm.ID.ToString() + "/" + comm.Year.ToString() + " - " + Server.HtmlDecode(comm.Cliente);
                        iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                        commessa.Alignment = Element.ALIGN_CENTER;
                        cartPDF.Add(commessa);
                        String txtArticolo = art.ID.ToString() + "/" + art.Year.ToString() + " - " + Server.HtmlDecode(art.Proc.process.processName) + " " + Server.HtmlDecode(art.Proc.variant.nomeVariante)
                            + " - Quantità: " + art.Quantita.ToString();
                        iTextSharp.text.Paragraph articolo = new iTextSharp.text.Paragraph(txtArticolo, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                        cartPDF.Add(articolo);

                        System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                        StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                        iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                        statusCode.ScaleToFit(20 * statusCode.Width / statusCode.Height, 20); 
                        iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph("STATO AVANZAMENTO PRODOTTO:", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        statusProd.Alignment = Element.ALIGN_LEFT;
                        statusCode.Alignment = Element.ALIGN_LEFT;
                        cartPDF.Add(statusProd);
                        cartPDF.Add(statusCode);

                        String txtTask = art.Tasks[i].TaskProduzioneID.ToString() + " " + art.Tasks[i].Name
                            + Environment.NewLine
                           /* + "Early Start: " + art.Tasks[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss")
                            + " Late Start: " + art.Tasks[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss")
                            + Environment.NewLine
                            + "Early Finish: " + art.Tasks[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss")
                            + " Late Finish: " + art.Tasks[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss")
                            + Environment.NewLine
                            + "Tempo ciclo previsto: " + art.Tasks[i].TempoC.Hours.ToString() + "H:" + art.Tasks[i].TempoC.Minutes.ToString() + "mm:" + art.Tasks[i].TempoC.Seconds.ToString() + ":ss"
                        +Environment.NewLine
                        */+ "Numero operatore previsti: " + art.Tasks[i].NumOperatori.ToString();
                        iTextSharp.text.Paragraph task = new iTextSharp.text.Paragraph(txtTask, new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                        cartPDF.Add(task);
                        iTextSharp.text.Image ICode = iTextSharp.text.Image.GetInstance(savePath + FileName + "I.jpg");
                        cartPDF.Add(ICode);
                        iTextSharp.text.Paragraph didascalia = new iTextSharp.text.Paragraph("INIZIO" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        cartPDF.Add(didascalia);
                        iTextSharp.text.Image ACode = iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg");
                        cartPDF.Add(ACode);
                        didascalia = new iTextSharp.text.Paragraph("PAUSA" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        cartPDF.Add(didascalia);
                        iTextSharp.text.Image FCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg");
                        cartPDF.Add(FCode);
                        didascalia = new iTextSharp.text.Paragraph("FINE" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        cartPDF.Add(didascalia);
                        iTextSharp.text.Image WCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "W.jpg");
                        cartPDF.Add(WCode);
                        didascalia = new iTextSharp.text.Paragraph("SEGNALA UN PROBLEMA" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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
                    ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('/Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                }
                else if (e.CommandName == "printOrdiniSingolo")
                {
                    Articolo art = new Articolo(artID, artYear);
                    Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
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

                        Logo logoAzienda = new Logo();
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

                        Cliente cln = new Cliente(art.Cliente);
                        String txtCommessa =cln.RagioneSociale + Environment.NewLine+ Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                            + " - Quantità: " + art.Quantita.ToString()
                                + Environment.NewLine + "Consegna prevista: " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
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

                        iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph("STATO PRODOTTO", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        statusProd.Alignment = Element.ALIGN_CENTER;
                        statusCode.Alignment = Element.ALIGN_CENTER;
                        intestazioneFoglio[2].AddElement(statusProd);
                        intestazioneFoglio[2].AddElement(statusCode);



                        float[] widths = new float[] { 150, (490 - 300), 150 };
                        tabIntest.SetWidths(widths);

                        iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("OC: ", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        intestazioneFoglio[3].AddElement(noteOC);

                        iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph("Inizio: " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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

                            String txtPostazione = art.Tasks[i].TaskProduzioneID.ToString() + " " + art.Tasks[i].Name;
                            iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                            posta.Alignment = Element.ALIGN_LEFT;


                            String txtTask = tabIntest.TotalWidth.ToString() + "Numero operatori previsti: " + art.Tasks[i].NumOperatori.ToString();
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
                            Paragraph pin = new iTextSharp.text.Paragraph("INIZIO", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                            celle[0].AddElement(pin);
                            celle[1].Padding = 5;
                            celle[1].AddElement(new iTextSharp.text.Paragraph("PAUSA", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                            celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                            celle[2].Padding = 5;
                            celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                            celle[2].AddElement(new iTextSharp.text.Paragraph("FINE", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                            celle[3].Padding = 5;
                            celle[3].AddElement(new iTextSharp.text.Paragraph("PROBLEMA", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
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
                        iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph("Codice FOM:" + Environment.NewLine + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                        parCellNote2.Alignment = Element.ALIGN_LEFT;
                        cellNote2.AddElement(parCellNote2);
                        tabNote2.AddCell(cellNote2);


                        parCellNote2 = new iTextSharp.text.Paragraph("Info lastre:" + Environment.NewLine + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                        parCellNote2.Alignment = Element.ALIGN_LEFT;
                        cellNote2 = new PdfPCell();
                        cellNote2.AddElement(parCellNote2);
                        tabNote2.AddCell(cellNote2);
                        cartPDF.Add(tabNote2);

                        String txtNote = "Note: ";
                        iTextSharp.text.Paragraph noteField = new iTextSharp.text.Paragraph(txtNote, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        noteField.Alignment = Element.ALIGN_LEFT;
                        cartPDF.Add(noteField);

                        cartPDF.Close();
                        ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('/Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('/Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                    }
                }
                else if (e.CommandName == "printOrdiniSingoloA3")
                {
                    Articolo art = new Articolo(artID, artYear);
                    Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
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

                        Logo logoAzienda = new Logo();
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

                        Cliente cln = new Cliente(art.Cliente);
                        String txtCommessa = cln.RagioneSociale + Environment.NewLine+ Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                            + " - Quantità: " + art.Quantita.ToString()
                                + Environment.NewLine + "Consegna prevista: " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                                /*if(art.Planner!=null)
                                {
                                    txtCommessa += "; Disegnatore: " + art.Planner.name + " " + art.Planner.cognome;
                                }*/
                        iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                        intestazioneFoglio[1].Rowspan = 2;
                        intestazioneFoglio[1].AddElement(commessa);

                        System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                        StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                        iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");

                        iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph("STATO PRODOTTO", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        statusProd.Alignment = Element.ALIGN_CENTER;
                        statusCode.Alignment = Element.ALIGN_CENTER;
                        intestazioneFoglio[2].AddElement(statusProd);
                        intestazioneFoglio[2].AddElement(statusCode);



                        float[] widths = new float[] { 150, (490 - 300), 150 };
                        tabIntest.SetWidths(widths);

                        iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("OC: ", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                        intestazioneFoglio[3].AddElement(noteOC);

                        iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph("Inizio: " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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

                            String txtPostazione = art.Tasks[i].TaskProduzioneID.ToString() + " " + art.Tasks[i].Name;
                            iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                            posta.Alignment = Element.ALIGN_LEFT;


                            String txtTask = "Numero operatori previsti: " + art.Tasks[i].NumOperatori.ToString();
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
                            Paragraph pin = new iTextSharp.text.Paragraph("INIZIO", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                            celle[0].HorizontalAlignment = Element.ALIGN_CENTER;
                            celle[0].AddElement(pin);
                            celle[1].Padding = 5;
                            celle[1].AddElement(new iTextSharp.text.Paragraph("PAUSA", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                            celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                            celle[1].HorizontalAlignment = Element.ALIGN_CENTER;
                            celle[2].Padding = 5;
                            celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                            celle[2].AddElement(new iTextSharp.text.Paragraph("FINE", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                            celle[2].HorizontalAlignment = Element.ALIGN_CENTER;
                            celle[3].Padding = 5;
                            celle[3].AddElement(new iTextSharp.text.Paragraph("PROBLEMA", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
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
                        iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph("Codice FOM:" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                        parCellNote2.Alignment = Element.ALIGN_LEFT;
                        cellNote2.AddElement(parCellNote2);
                        tabNote2.AddCell(cellNote2);


                        parCellNote2 = new iTextSharp.text.Paragraph("Info lastre:" + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                        parCellNote2.Alignment = Element.ALIGN_LEFT;
                        cellNote2 = new PdfPCell();
                        cellNote2.AddElement(parCellNote2);
                        tabNote2.AddCell(cellNote2);
                        cartPDF.Add(tabNote2);


                        String txtNote = "Note: ";
                        iTextSharp.text.Paragraph noteField = new iTextSharp.text.Paragraph(txtNote, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        noteField.Alignment = Element.ALIGN_LEFT;
                        cartPDF.Add(noteField);

                        cartPDF.Close();
                        ScriptManager.RegisterStartupScript(updProduction, updProduction.GetType(), null, "window.open('/Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
                    }
                }
                /*else if (e.CommandName == "depianifica")
                {
                    Articolo art = new Articolo(artID, artYear);
                    bool rt = art.Depianifica();
                    if (rt == true)
                    {
                        ElencoArticoliInProduzione el = new ElencoArticoliInProduzione();
                        ElencoArticoli elN = new ElencoArticoli('N');
                        List<Articolo> artINP = el.ElencoArticoli.Concat(elN.ListArticoli).ToList();
                        //rptArticoli.DataSource = el.ElencoArticoli;
                        List<Articolo> artINPOrdered = artINP.OrderBy(x => x.DataPrevistaConsegna).ToList();
                        rptArticoli.DataSource = artINPOrdered;
                        rptArticoli.DataBind();
                        lbl1.Text = "La pianificazione del prodotto " + art.ID.ToString() 
                        + "/" + art.Year.ToString() + " è stata rimossa con successo.<br />";
                    }
                    else
                    {
                        lbl1.Text += "E' avvenuto un errore.<br/>" + art.log;
                    }
                }*/
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            ElencoArticoliInProduzione el = new ElencoArticoliInProduzione();
            ElencoArticoli elN = new ElencoArticoli('N');
            List<Articolo> artINP = el.ElencoArticoli.Concat(elN.ListArticoli).ToList();
            
            List<Articolo> artINPOrdered;
            switch (ordine)
            {
                case "DataPrevistaFineProduzioneUp":
                    artINPOrdered = artINP.OrderBy(x => x.DataPrevistaFineProduzione).ToList();
                    break;
                case "DataPrevistaFineProduzioneDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.DataPrevistaFineProduzione).ToList();
                    break;
                case "DataPrevistaConsegnaUp":
                    artINPOrdered = artINP.OrderBy(x => x.DataPrevistaConsegna).ToList();
                    break;
                case "DataPrevistaConsegnaDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.DataPrevistaConsegna).ToList();
                    break;
                case "KanbanCardUp":
                    artINPOrdered = artINP.OrderBy(x => x.KanbanCardID).ToList();
                    break;
                case "KanbanCardDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.KanbanCardID).ToList();
                    break;
                case "ArticoloUp":
                    artINPOrdered = artINP.OrderBy(x => x.Year).ThenBy(y => y.ID).ToList();
                    break;
                case "ArticoloDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Year).ThenByDescending(y => y.ID).ToList();
                    break;
                case "ClienteUp":
                    artINPOrdered = artINP.OrderBy(x => x.Cliente).ToList();
                    break;
                case "ClienteDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Cliente).ToList();
                    break;
                case "CommessaUp":
                    artINPOrdered = artINP.OrderBy(x => x.AnnoCommessa).ThenBy(y => y.Commessa).ToList();
                    break;
                case "CommessaDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.AnnoCommessa).ThenByDescending(y => y.Commessa).ToList();
                    break;
                case "ProcessoUp":
                    artINPOrdered = artINP.OrderBy(x => x.Proc.NomeCombinato).ToList();
                    break;
                case "ProcessoDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Proc.NomeCombinato).ToList();
                    break;
                case "QuantitaUp":
                    artINPOrdered = artINP.OrderBy(x => x.Quantita).ToList();
                    break;
                case "QuantitaDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Quantita).ToList();
                    break;
                case "MatricolaUp":
                    artINPOrdered = artINP.OrderBy(x => x.Matricola).ToList();
                    break;
                case "MatricolaDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Matricola).ToList();
                    break;
                case "StatusUp":
                    artINPOrdered = artINP.OrderBy(x => x.Status).ToList();
                    break;
                case "StatusDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.Status).ToList();
                    break;
                case "RepartoUp":
                    artINPOrdered = artINP.OrderBy(x => x.RepartoNome).ToList();
                    break;
                case "RepartoDown":
                    artINPOrdered = artINP.OrderByDescending(x => x.RepartoNome).ToList();
                    break;
                default:
                    artINPOrdered = artINP.OrderBy(x => x.Status).ThenBy(x => x.DataPrevistaFineProduzione).ThenBy(x => x.DataPrevistaConsegna).ToList();
                    break;
            }
            rptArticoli.DataSource = artINPOrdered;
            rptArticoli.DataBind();
            lblDataUpdate.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        protected void lnkDataFineProdUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.DataPrevistaFineProduzione);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "DataPrevistaFineProduzioneUp";
        }

        protected void lnkDataFineProdDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.DataPrevistaFineProduzione);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "DataPrevistaFineProduzioneDown";
        }

        protected void lnkDataConsegnaUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.DataPrevistaConsegna);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "DataPrevistaConsegnaUp";
        }

        protected void lnkDataConsegnaDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.DataPrevistaConsegna);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "DataPrevistaConsegnaDown";
        }

        protected void lnkKanbanCardUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.KanbanCardID);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "KanbanCardUp";
        }

        protected void lnkKanbanCardDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.KanbanCardID);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "KanbanCardDown";
        }

        protected void lnkArticoloUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Year).ThenBy(y => y.ID);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ArticoloUp";
        }

        protected void lnkArticoloDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Year).ThenByDescending(y=>y.ID);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ArticoloDown";
        }

        protected void lnkClienteDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Cliente);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ClienteUp";
        }

        protected void lnkClienteUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Cliente);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ClienteDown";
        }

        protected void lnkCommessaUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.AnnoCommessa).ThenBy(y => y.Commessa);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "CommessaUp";
        }

        protected void lnkCommessaDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.AnnoCommessa).ThenByDescending(y => y.Commessa);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "CommessaDown";
        }

        protected void lnkProcessoUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Proc.NomeCombinato);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ProcessoUp";
        }

        protected void lnkProcessoDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Proc.NomeCombinato);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "ProcessoDown";
        }

        protected void lnkQuantitaUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Quantita);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "QuantitaUp";
        }

        protected void lnkQuantitaDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Quantita);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "QuantitaDown";
        }

        protected void lnkMatricolaUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Matricola);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "MatricolaUp";
        }

        protected void lnkMatricolaDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Matricola);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "MatricolaDown";
        }

        protected void lnkStatusUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.Status);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "StatusUp";
        }

        protected void lnkStatusDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.Status);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "StatusDown";
        }

        protected void lnkRepartoUp_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderBy(x => x.RepartoNome);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "RepartoUp";
        }

        protected void lnkRepartoDown_Click(object sender, EventArgs e)
        {
            var artINPSorted = artINP.OrderByDescending(x => x.RepartoNome);
            rptArticoli.DataSource = artINPSorted;
            rptArticoli.DataBind();
            ordine = "RepartoDown";
        }
    }
}