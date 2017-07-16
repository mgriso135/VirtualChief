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

namespace KIS.Commesse
{
    public partial class wzPrintBarcordes : System.Web.UI.UserControl
    {
        public int idArticolo, annoArticolo;
        protected void Page_Load(object sender, EventArgs e)
        {
            frmPrintBarcodes.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                frmPrintBarcodes.Visible = true;
                if (!Page.IsPostBack)
                {
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgPrintOrdini_Click(object sender, ImageClickEventArgs e)
        {
                Articolo art = new Articolo(idArticolo, annoArticolo);
                Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
                comm.loadArticoli();
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

                    Logo logoAzienda = new Logo();
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath(logoAzienda.filePath));
                    logo.ScaleToFit(50 * logo.Width / logo.Height, 50);
                    cartPDF.Add(logo);

                    String txtPostazione = GetLocalResourceObject("lblPostazione").ToString() + " " + art.Tasks[i].PostazioneName;
                    iTextSharp.text.Paragraph posta = new iTextSharp.text.Paragraph(txtPostazione, new Font(Font.FontFamily.TIMES_ROMAN, 40, Font.BOLD));
                    posta.Alignment = Element.ALIGN_CENTER;
                    cartPDF.Add(posta);
                    String txtCommessa = comm.ID.ToString() + "/" + comm.Year.ToString() + " - " + Server.HtmlDecode(comm.Cliente);
                    iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                    commessa.Alignment = Element.ALIGN_CENTER;
                    cartPDF.Add(commessa);
                    String txtArticolo = art.ID.ToString() + "/" + art.Year.ToString() + " - " + Server.HtmlDecode(art.Proc.process.processName) + " " + Server.HtmlDecode(art.Proc.variant.nomeVariante)
                        + " - "+ GetLocalResourceObject("lblQuantita").ToString() + ": " + art.Quantita.ToString();
                    iTextSharp.text.Paragraph articolo = new iTextSharp.text.Paragraph(txtArticolo, new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL));
                    cartPDF.Add(articolo);

                    System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                    StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                    iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                    statusCode.ScaleToFit(20 * statusCode.Width / statusCode.Height, 20);
                    iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblStatus").ToString()+":", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    statusProd.Alignment = Element.ALIGN_LEFT;
                    statusCode.Alignment = Element.ALIGN_LEFT;
                    cartPDF.Add(statusProd);
                    cartPDF.Add(statusCode);

                    String txtTask = art.Tasks[i].TaskProduzioneID.ToString() + " " + art.Tasks[i].Name
                        + Environment.NewLine
                        + GetLocalResourceObject("lblNumeroOp").ToString()
                        + ": " + art.Tasks[i].NumOperatori.ToString();
                    iTextSharp.text.Paragraph task = new iTextSharp.text.Paragraph(txtTask, new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                    cartPDF.Add(task);
                    iTextSharp.text.Image ICode = iTextSharp.text.Image.GetInstance(savePath + FileName + "I.jpg");
                    cartPDF.Add(ICode);
                    iTextSharp.text.Paragraph didascalia = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblINIZIO").ToString() + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    cartPDF.Add(didascalia);
                    iTextSharp.text.Image ACode = iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg");
                    cartPDF.Add(ACode);
                    didascalia = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblPAUSA").ToString()+ Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    cartPDF.Add(didascalia);
                    iTextSharp.text.Image FCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg");
                    cartPDF.Add(FCode);
                    didascalia = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblFINE").ToString() + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    cartPDF.Add(didascalia);
                    iTextSharp.text.Image WCode = iTextSharp.text.Image.GetInstance(savePath + FileName + "W.jpg");
                    cartPDF.Add(WCode);
                    didascalia = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblSEGNALAWARNING").ToString() + Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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
                cartPDF.Close();

                output.Close();
                output.Dispose();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
        }

        protected void imgPrintOrdiniSingolo_Click(object sender, ImageClickEventArgs e)
        {
            Articolo art = new Articolo(idArticolo, annoArticolo);
            Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
            comm.loadArticoli();
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
                String txtCommessa = cln.RagioneSociale+Environment.NewLine +Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                    + " - " + GetLocalResourceObject("lblQuantita").ToString() + ": " + art.Quantita.ToString()
                        + Environment.NewLine + GetLocalResourceObject("lblConsegnaPrevista").ToString()+ ": " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
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

                iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblStatus").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                statusProd.Alignment = Element.ALIGN_CENTER;
                statusCode.Alignment = Element.ALIGN_CENTER;
                intestazioneFoglio[2].AddElement(statusProd);
                intestazioneFoglio[2].AddElement(statusCode);



                float[] widths = new float[] { 150, (490 - 300), 150 };
                tabIntest.SetWidths(widths);

                iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("OC: ", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[3].AddElement(noteOC);

                iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblINIZIO2").ToString()+": " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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


                    String txtTask = tabIntest.TotalWidth.ToString() + GetLocalResourceObject("lblNumeroOp").ToString() + ": " + art.Tasks[i].NumOperatori.ToString();
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
                    Paragraph pin = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblINIZIO").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    celle[0].AddElement(pin);
                    celle[1].Padding = 5;
                    celle[1].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblPAUSA").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                    celle[2].Padding = 5;
                    celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                    celle[2].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblFINE").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[3].Padding = 5;
                    celle[3].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblWARNING").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
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
                iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph(Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);


                parCellNote2 = new iTextSharp.text.Paragraph(Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
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
                Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
            }
        }

        protected void imgPrintOrdiniSingoloA3_Click(object sender, ImageClickEventArgs e)
        {
            Articolo art = new Articolo(idArticolo, annoArticolo);
            Commessa comm = new Commessa(art.Commessa, art.AnnoCommessa);
            comm.loadArticoli();
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
                String txtCommessa = cln.RagioneSociale+Environment.NewLine+Server.HtmlDecode(art.Proc.process.processName + " " + art.Proc.variant.nomeVariante)
                    + " - "+ GetLocalResourceObject("lblQuantita").ToString() + ": " + art.Quantita.ToString()
                        + Environment.NewLine + GetLocalResourceObject("lblConsegnaPrevista").ToString()+": " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy");

                iTextSharp.text.Paragraph commessa = new iTextSharp.text.Paragraph(txtCommessa, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                intestazioneFoglio[1].Rowspan = 2;
                intestazioneFoglio[1].AddElement(commessa);

                System.Drawing.Image StatusCode = GenCode128.Code128Rendering.MakeBarcodeImage("B" + art.ID.ToString() + "." + art.Year.ToString(), 2, true);
                StatusCode.Save(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");
                iTextSharp.text.Image statusCode = iTextSharp.text.Image.GetInstance(savePath + "articolo" + art.ID.ToString() + "_" + art.Year.ToString() + "STATUS.jpg");

                iTextSharp.text.Paragraph statusProd = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblStatoProdotto").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                statusProd.Alignment = Element.ALIGN_CENTER;
                statusCode.Alignment = Element.ALIGN_CENTER;
                intestazioneFoglio[2].AddElement(statusProd);
                intestazioneFoglio[2].AddElement(statusCode);



                float[] widths = new float[] { 150, (490 - 300), 150 };
                tabIntest.SetWidths(widths);

                iTextSharp.text.Paragraph noteOC = new iTextSharp.text.Paragraph("OC: ", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                intestazioneFoglio[3].AddElement(noteOC);

                iTextSharp.text.Paragraph inizioProd = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblINIZIO2").ToString() + ": " + art.EarlyStart.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
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


                    String txtTask = GetLocalResourceObject("lblNumeroOp").ToString()+": " + art.Tasks[i].NumOperatori.ToString();
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
                    Paragraph pin = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblINIZIO").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
                    celle[0].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[0].AddElement(pin);
                    celle[1].Padding = 5;
                    celle[1].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblPAUSA").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[1].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "A.jpg"));
                    celle[1].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[2].Padding = 5;
                    celle[2].AddElement(iTextSharp.text.Image.GetInstance(savePath + FileName + "F.jpg"));
                    celle[2].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblFINE").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                    celle[2].HorizontalAlignment = Element.ALIGN_CENTER;
                    celle[3].Padding = 5;
                    celle[3].AddElement(new iTextSharp.text.Paragraph(GetLocalResourceObject("lblWARNING").ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
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
                iTextSharp.text.Paragraph parCellNote2 = new iTextSharp.text.Paragraph(Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
                parCellNote2.Alignment = Element.ALIGN_LEFT;
                cellNote2.AddElement(parCellNote2);
                tabNote2.AddCell(cellNote2);


                parCellNote2 = new iTextSharp.text.Paragraph(Environment.NewLine + Environment.NewLine, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL));
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
                Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('../Data/Produzione/" + FileNamePDF + "', '_newtab')", true);
            }
        }

    }
}