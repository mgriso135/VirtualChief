using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.Commesse;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KIS;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_summary1 : System.Web.UI.UserControl
    {
        public String customerID;
        protected void Page_Load(object sender, EventArgs e)
        {
            rpt1.Visible = false;
            //lnkGoFwd.Visible = false;
            imgGoFwd.Visible = false;
            lnkGoBack.Visible = false;
            imgGoBack.Visible = false;
            frmReport.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Report Stato Ordini Clienti";
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
                Cliente customer = new Cliente(customerID);
                if (customer.CodiceCliente.Length > 0)
                {
                    rpt1.Visible = true;
                    frmReport.Visible = true;
                    //lnkGoFwd.Visible = true;
                    imgGoFwd.Visible = true;
                    lnkGoBack.Visible = true;
                    imgGoBack.Visible = true;
                    lnkGoBack.NavigateUrl = "~/Analysis/ReportCustomerProdProgress_chooseFProducts.aspx?customerID=" + customer.CodiceCliente;
                    if (Session["prodI"] != null && Session["prodN"] != null && Session["prodF"] != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            //lnkGoFwd.NavigateUrl = "~/Analysis/ReportCustomerProdProgress_summary.aspx?customerID=" + customer.CodiceCliente;
                            List<Articolo> elencoTot = new List<Articolo>();
                            List<Articolo> tmp = (List<Articolo>)Session["prodI"];
                            var tmpO = tmp.OrderBy(x => x.DataPrevistaConsegna).ToList();
                            for (int i = 0; i < tmpO.Count; i++)
                            {
                                elencoTot.Add(tmpO[i]);
                            }
                            tmp = (List<Articolo>)Session["prodN"];
                            tmpO = tmp.OrderBy(x => x.DataPrevistaConsegna).ToList();
                            for (int i = 0; i < tmpO.Count; i++)
                            {
                                elencoTot.Add(tmpO[i]);
                            }
                            tmp = (List<Articolo>)Session["prodP"];
                            tmpO = tmp.OrderBy(x => x.DataPrevistaConsegna).ToList();
                            for (int i = 0; i < tmpO.Count; i++)
                            {
                                elencoTot.Add(tmpO[i]);
                            }
                            tmp = (List<Articolo>)Session["prodF"];
                            tmpO = tmp.OrderBy(x => x.DataPrevistaConsegna).ToList();
                            for (int i = 0; i < tmpO.Count; i++)
                            {
                                elencoTot.Add(tmpO[i]);
                            }
                            rpt1.DataSource = elencoTot;
                            rpt1.DataBind();
                        }
                    }
                    else
                    {
                        frmReport.Visible = false;
                        lbl1.Text = "Elenco di prodotti non impostato.";
                    }
                }
                else
                {
                    lbl1.Text = "Cliente non trovato.";
                    frmReport.Visible = false;
                    rpt1.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "Non hai i permessi per eseguire il report.";
                frmReport.Visible = false;
                rpt1.Visible = false;
            }
        }

        protected void imgGoFwd_Click(object sender, ImageClickEventArgs e)
        {
            switch (ddlFormato.SelectedValue)
            {
                case "pdf":
                    printReportPDF();
                    break;
                default:
                    lbl1.Text = "Nessun formato selezionato.";
                    break;
            }
        }

        protected void printReportPDF()
        {
            Cliente customer = new Cliente(customerID);
            if (customer.CodiceCliente.Length > 0)
            {
                String savePath = Server.MapPath(@"~\Data\Reports\");
                Document cartPDF = new Document(PageSize.A4, 50, 50, 25, 25);

                String FileNamePDF = "Report" + customer.CodiceCliente + ".pdf";
                // Controllo che il pdf non esista, e se esiste lo cancello.
                if (System.IO.File.Exists(savePath + FileNamePDF))
                {
                    String newfilename = savePath + FileNamePDF;
                    System.IO.File.Move(savePath + FileNamePDF, newfilename);
                    System.IO.File.Delete(newfilename);
                }

                System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
                // Our custom Header and Footer is done using Event Handler
                TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
                writer.PageEvent = PageEventHandler;
                // Define the page header
                PageEventHandler.Title = "";
                PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.NORMAL);
                PageEventHandler.HeaderLeft = "";
                PageEventHandler.HeaderRight = "1";

                cartPDF.Open();

                PdfPTable tabIntest = new PdfPTable(2);
                tabIntest.WidthPercentage = 100;
                PdfPCell[] intestazioneFoglio = new PdfPCell[2];
                intestazioneFoglio[0] = new PdfPCell();
                intestazioneFoglio[1] = new PdfPCell();
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
                intestazioneFoglio[0].VerticalAlignment = Element.ALIGN_MIDDLE;
                iTextSharp.text.Paragraph titolo = new iTextSharp.text.Paragraph("Report avanzamento produzione\n" + customer.RagioneSociale, new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                titolo.Alignment = Element.ALIGN_LEFT;
                intestazioneFoglio[1].AddElement(titolo);

                float[] widths = new float[] { 150, (490 - 150) };
                tabIntest.SetWidths(widths);

                tabIntest.AddCell(intestazioneFoglio[0]);
                tabIntest.AddCell(intestazioneFoglio[1]);
                cartPDF.Add(tabIntest);

                // Stampa tasks in stato I
                if (Session["prodI"] != null)
                {
                    List<Articolo> lstProd = (List<Articolo>)Session["prodI"];
                    if (lstProd.Count > 0)
                    {
                        configCustomerOrderStatusReport cfgCustomer = new configCustomerOrderStatusReport(customer.CodiceCliente);
                        iTextSharp.text.Paragraph par = new Paragraph("Prodotti in corso di realizzazione", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        par.SpacingAfter = 20;
                        cartPDF.Add(par);

                        for (int i = 0; i < lstProd.Count; i++)
                        {
                            PdfPTable tbl = new PdfPTable(2);
                            tbl.WidthPercentage = 100;

                            // Commessa_IDCommessa
                            if (cfgCustomer.IDCommessa)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Commessa.ToString() + "/" + lstProd[i].AnnoCommessa.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Cliente
                            if (cfgCustomer.Cliente)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Cliente", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(customer.RagioneSociale, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Commessa_DataInserimentoOrdine
                            if (cfgCustomer.DataInserimentoOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data ricevimento ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.DataInserimento.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Note
                            if (cfgCustomer.NoteOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Note ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.Note, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IDProdotto
                            if (cfgCustomer.IDProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("ID prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].ID.ToString() + "/" + lstProd[i].Year.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeProdotto
                            if (cfgCustomer.NomeProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Linea prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.process.processName, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeVariante
                            if (cfgCustomer.NomeVariante)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Nome prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.variant.nomeVariante, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Matricola
                            if (cfgCustomer.Matricola)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Matricola", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Matricola, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Status
                            if (cfgCustomer.Status)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph("In produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Reparto
                            if (cfgCustomer.Reparto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Reparto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].RepartoNome, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaConsegna
                            if (cfgCustomer.DataPrevistaConsegna)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista consegna", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaConsegna.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaFineProduzione
                            if (cfgCustomer.DataPrevistaFineProduzione)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista fine produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaFineProduzione.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyStart
                            if (cfgCustomer.EarlyStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyFinish
                            if (cfgCustomer.EarlyFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateStart
                            if (cfgCustomer.LateStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateFinish
                            if (cfgCustomer.LateFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaPrevista
                            if (cfgCustomer.Quantita)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi richiesti", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Quantita.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaProdotta
                            /*if (cfgCustomer.QuantitaProdotta)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi realizzati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_Ritardo
                            if (cfgCustomer.Ritardo)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Giorni di ritardo", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Ritardo.TotalDays,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_TempoDiLavoroTotale
                            /*if (cfgCustomer.TempoDiLavoroTotale)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro impiegate (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].TempoDiLavoroTotale.TotalHours, 2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_LeadTime
                            /*if (cfgCustomer.LeadTime)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Lead Time (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].LeadTime.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_TempoDiLavoroPrevisto
                            if (cfgCustomer.TempoDiLavoroPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].TempoDiLavoroPrevisto.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IndicatoreCompletamentoTasks
                            if (cfgCustomer.IndicatoreCompletamentoTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks (n° tasks terminati / n° tasks totali)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTasks, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IndicatoreCompletamentoTasks
                            if (cfgCustomer.IndicatoreCompletamentoTempoPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks su tempo di lavoro previsto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTempoPrevisto, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_GANTT
                            if (cfgCustomer.ViewGanttTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Timeline avanzamento\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                cl[0].Colspan = 2;
                                                                

                                lstProd[i].loadTasksProduzione();
                                PdfPTable gantt = new PdfPTable(lstProd[i].Tasks.Count);
                                gantt.WidthPercentage = 100;
                                for (int k = 0; k < lstProd[i].Tasks.Count; k++)
                                {
                                    PdfPCell tsk = new PdfPCell();
                                    iTextSharp.text.Paragraph pTask = new Paragraph(lstProd[i].Tasks[k].Name, new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL));
                                    if (lstProd[i].Tasks[k].Status == 'I')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Blue);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.White);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'N')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.White);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'F')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Green);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'P')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Orange);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    tsk.AddElement(pTask);
                                    gantt.AddCell(tsk);
                                }
                                cl[0].AddElement(gantt);
                                tbl.AddCell(cl[0]);
                            }

                            // Prodotto_ViewElencoTasks
                            if (cfgCustomer.ViewElencoTasks)
                            {
                                PdfPCell[] clT = new PdfPCell[1];
                                clT[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt = new Paragraph("Elenco tasks", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD));
                                clT[0].AddElement(txt);
                                clT[0].Colspan = 2;
                                tbl.AddCell(clT[0]);

                                lstProd[i].loadTasksProduzione();
                                for (int j = 0; j < lstProd[i].Tasks.Count; j++)
                                {
                                    System.Drawing.Color bgColor = (j % 2 == 0) ? System.Drawing.Color.FromArgb(240, 240, 240) : System.Drawing.Color.White;
                                    // Tasks_ID
                                    if (cfgCustomer.Task_ID)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Task ID", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TaskProduzioneID.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Nome
                                    if (cfgCustomer.Task_Nome)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Nome task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Name.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Descrizione
                                    if (cfgCustomer.Task_Descrizione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Descrizione task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Description.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Postazione
                                    if (cfgCustomer.Task_Postazione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Postazione di lavoro", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].PostazioneName.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_EarlyStart
                                    if (cfgCustomer.Task_EarlyStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }
                                    
                                    // Task_EarlyFinish
                                    if (cfgCustomer.Task_EarlyFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_LateStart
                                    if (cfgCustomer.Task_LateStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_LateFinish
                                    if (cfgCustomer.Task_LateFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_NumeroOperatori
                                    if (cfgCustomer.Task_NOperatori)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Numero operatori assegnati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].NumOperatori.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoCiclo
                                    if (cfgCustomer.Task_TempoCiclo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Tempo ciclo previsto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoC.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroPrevisto
                                    if (cfgCustomer.Task_TempoDiLavoroPrevisto)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoDiLavoroPrevisto.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroEffettivo
                                    /*if (cfgCustomer.Task_TempoDiLavoroEffettivo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro effettive", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoDiLavoroEffettivo.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }*/

                                    // Task_Status
                                    if (cfgCustomer.Task_Status)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        String status = "";
                                        switch (lstProd[i].Tasks[j].Status)
                                            {
                                            case 'I': 
                                                status = "In produzione"; break;
                                            case 'N':
                                                status = "Non ancora iniziato";break;
                                            case 'P':
                                                status = "In pausa";break;
                                            case 'F':
                                                status = "Terminato";
                                                break;
                                            default:
                                                status = "#ND";
                                                    break;
                                            }
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(status, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_QuantitaProdotta
                                    if (cfgCustomer.Task_QuantitaProdotta)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }
                                }
                            }

                            tbl.SpacingAfter = 10;
                            cartPDF.Add(tbl);
                            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
                            p.SpacingAfter = 20;
                            cartPDF.Add(p);
                        }
                    }
                }

                // Stampa tasks in stato P
                if (Session["prodP"] != null)
                {
                    List<Articolo> lstProd = (List<Articolo>)Session["prodP"];
                    if (lstProd.Count > 0)
                    {
                        configCustomerOrderStatusReport cfgCustomer = new configCustomerOrderStatusReport(customer.CodiceCliente);
                        iTextSharp.text.Paragraph par = new Paragraph("Prodotti pianificati in produzione", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        par.SpacingAfter = 20;
                        cartPDF.Add(par);

                        for (int i = 0; i < lstProd.Count; i++)
                        {
                            PdfPTable tbl = new PdfPTable(2);
                            tbl.WidthPercentage = 100;

                            // Commessa_IDCommessa
                            if (cfgCustomer.IDCommessa)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Commessa.ToString() + "/" + lstProd[i].AnnoCommessa.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Cliente
                            if (cfgCustomer.Cliente)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Cliente", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(customer.RagioneSociale, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Commessa_DataInserimentoOrdine
                            if (cfgCustomer.DataInserimentoOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data ricevimento ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.DataInserimento.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Note
                            if (cfgCustomer.NoteOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Note ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.Note, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IDProdotto
                            if (cfgCustomer.IDProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("ID prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].ID.ToString() + "/" + lstProd[i].Year.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeProdotto
                            if (cfgCustomer.NomeProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Linea prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.process.processName, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeVariante
                            if (cfgCustomer.NomeVariante)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Nome prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.variant.nomeVariante, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Matricola
                            if (cfgCustomer.Matricola)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Matricola", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Matricola, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Status
                            if (cfgCustomer.Status)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph("Pianificato in produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Reparto
                            if (cfgCustomer.Reparto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Reparto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].RepartoNome, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaConsegna
                            if (cfgCustomer.DataPrevistaConsegna)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista consegna", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaConsegna.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaFineProduzione
                            if (cfgCustomer.DataPrevistaFineProduzione)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista fine produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaFineProduzione.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyStart
                            if (cfgCustomer.EarlyStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyFinish
                            if (cfgCustomer.EarlyFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateStart
                            if (cfgCustomer.LateStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateFinish
                            if (cfgCustomer.LateFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaPrevista
                            if (cfgCustomer.Quantita)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi richiesti", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Quantita.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaProdotta
                            /*if (cfgCustomer.QuantitaProdotta)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi realizzati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_Ritardo
                            if (cfgCustomer.Ritardo)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Giorni di ritardo", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Ritardo.TotalDays,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_TempoDiLavoroTotale
                            /*if (cfgCustomer.TempoDiLavoroTotale)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro impiegate (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].TempoDiLavoroTotale.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_LeadTime
                            /*if (cfgCustomer.LeadTime)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Lead Time (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].LeadTime.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_TempoDiLavoroPrevisto
                            if (cfgCustomer.TempoDiLavoroPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].TempoDiLavoroPrevisto.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks (n° tasks terminati / n° tasks totali)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTasks, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTempoPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks su tempo di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTempoPrevisto, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_GANTT
                            if (cfgCustomer.ViewGanttTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Timeline avanzamento\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                cl[0].Colspan = 2;


                                lstProd[i].loadTasksProduzione();
                                PdfPTable gantt = new PdfPTable(lstProd[i].Tasks.Count);
                                gantt.WidthPercentage = 100;
                                for (int k = 0; k < lstProd[i].Tasks.Count; k++)
                                {
                                    PdfPCell tsk = new PdfPCell();
                                    iTextSharp.text.Paragraph pTask = new Paragraph(lstProd[i].Tasks[k].Name, new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL));
                                    if (lstProd[i].Tasks[k].Status == 'I')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Blue);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.White);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'N')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.White);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'F')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Green);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'P')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Orange);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    tsk.AddElement(pTask);
                                    gantt.AddCell(tsk);
                                }
                                cl[0].AddElement(gantt);
                                tbl.AddCell(cl[0]);
                            }

                            // Prodotto_ViewElencoTasks
                            if (cfgCustomer.ViewElencoTasks)
                            {
                                PdfPCell[] clT = new PdfPCell[1];
                                clT[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt = new Paragraph("Elenco tasks", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD));
                                clT[0].AddElement(txt);
                                clT[0].Colspan = 2;
                                tbl.AddCell(clT[0]);

                                lstProd[i].loadTasksProduzione();

                                for (int j = 0; j < lstProd[i].Tasks.Count; j++)
                                {
                                    System.Drawing.Color bgColor = (j % 2 == 0) ? System.Drawing.Color.FromArgb(240, 240, 240) : System.Drawing.Color.White;
                                    // Tasks_ID
                                    if (cfgCustomer.Task_ID)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Task ID", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TaskProduzioneID.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Nome
                                    if (cfgCustomer.Task_Nome)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Nome task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Name.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Descrizione
                                    if (cfgCustomer.Task_Descrizione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Descrizione task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Description.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Postazione
                                    if (cfgCustomer.Task_Postazione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Postazione di lavoro", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].PostazioneName.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_EarlyStart
                                    if (cfgCustomer.Task_EarlyStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_EarlyFinish
                                    if (cfgCustomer.Task_EarlyFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_LateStart
                                    if (cfgCustomer.Task_LateStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_LateFinish
                                    if (cfgCustomer.Task_LateFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_NumeroOperatori
                                    if (cfgCustomer.Task_NOperatori)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Numero operatori assegnati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].NumOperatori.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoCiclo
                                    if (cfgCustomer.Task_TempoCiclo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Tempo ciclo previsto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoC.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroPrevisto
                                    if (cfgCustomer.Task_TempoDiLavoroPrevisto)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoDiLavoroPrevisto.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroEffettivo
                                    /*if (cfgCustomer.Task_TempoDiLavoroEffettivo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro effettive", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TempoDiLavoroEffettivo.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }*/

                                    // Task_Status
                                    if (cfgCustomer.Task_Status)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        String status = "";
                                        switch (lstProd[i].Tasks[j].Status)
                                        {
                                            case 'I':
                                                status = "In produzione"; break;
                                            case 'N':
                                                status = "Non ancora iniziato"; break;
                                            case 'P':
                                                status = "In pausa"; break;
                                            case 'F':
                                                status = "Terminato";
                                                break;
                                            default:
                                                status = "#ND";
                                                break;
                                        }
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(status, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_QuantitaProdotta
                                    /*if (cfgCustomer.Task_QuantitaProdotta)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }*/
                                }
                            }
                            tbl.SpacingAfter = 10;
                            cartPDF.Add(tbl);
                            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
                            p.SpacingAfter = 20;
                            cartPDF.Add(p);
                        }
                    }
                }

                // Stampa tasks in stato N
                if (Session["prodN"] != null)
                {
                    List<Articolo> lstProd = (List<Articolo>)Session["prodN"];
                    if (lstProd.Count > 0)
                    {
                        configCustomerOrderStatusReport cfgCustomer = new configCustomerOrderStatusReport(customer.CodiceCliente);
                        iTextSharp.text.Paragraph par = new Paragraph("Prodotti da pianificare in produzione", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        par.SpacingAfter = 20;
                        cartPDF.Add(par);

                        for (int i = 0; i < lstProd.Count; i++)
                        {
                            PdfPTable tbl = new PdfPTable(2);
                            tbl.WidthPercentage = 100;

                            // Commessa_IDCommessa
                            if (cfgCustomer.IDCommessa)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Commessa.ToString() + "/" + lstProd[i].AnnoCommessa.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Cliente
                            if (cfgCustomer.Cliente)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Cliente", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(customer.RagioneSociale, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Commessa_DataInserimentoOrdine
                            if (cfgCustomer.DataInserimentoOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data ricevimento ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.DataInserimento.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Note
                            if (cfgCustomer.NoteOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Note ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.Note, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IDProdotto
                            if (cfgCustomer.IDProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("ID prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].ID.ToString() + "/" + lstProd[i].Year.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeProdotto
                            if (cfgCustomer.NomeProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Linea prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.process.processName, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeVariante
                            if (cfgCustomer.NomeVariante)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Nome prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.variant.nomeVariante, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Matricola
                            if (cfgCustomer.Matricola)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Matricola", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Matricola, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Status
                            if (cfgCustomer.Status)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph("Da pianificare", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Reparto
                            /*if (cfgCustomer.Reparto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Reparto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].RepartoNome, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_DataPrevistaConsegna
                            if (cfgCustomer.DataPrevistaConsegna)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista consegna", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaConsegna.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaFineProduzione
                            /*if (cfgCustomer.DataPrevistaFineProduzione)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista fine produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaFineProduzione.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_EarlyStart
                            /*if (cfgCustomer.EarlyStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_EarlyFinish
                            /*if (cfgCustomer.EarlyFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_LateStart
                            /*if (cfgCustomer.LateStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_LateFinish
                            /*if (cfgCustomer.LateFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_QuantitaPrevista
                            if (cfgCustomer.Quantita)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi richiesti", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Quantita.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaProdotta
                            /*if (cfgCustomer.QuantitaProdotta)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi realizzati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_Ritardo
                            /*if (cfgCustomer.Ritardo)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Giorni di ritardo", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Ritardo.TotalDays.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_TempoDiLavoroTotale
                            /*if (cfgCustomer.TempoDiLavoroTotale)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro impiegate (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].TempoDiLavoroTotale.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_LeadTime
                            /*if (cfgCustomer.LeadTime)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Lead Time (parziale)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LeadTime.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_TempoDiLavoroPrevisto
                            /*if (cfgCustomer.TempoDiLavoroPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].TempoDiLavoroPrevisto.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks (n° tasks terminati / n° tasks totali)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTasks, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTempoPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks su tempo di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTempoPrevisto, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_GANTT
                            /*if (cfgCustomer.ViewGanttTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Timeline avanzamento", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_ViewElencoTasks
                            /*if (cfgCustomer.ViewElencoTasks)
                            {
                                PdfPCell[] clT = new PdfPCell[1];
                                clT[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt = new Paragraph("Elenco tasks", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD));
                                clT[0].AddElement(txt);
                                clT[0].Colspan = 2;
                                tbl.AddCell(clT[0]);

                                lstProd[i].loadTasksProduzione();
                                for (int j = 0; j < lstProd[i].Tasks.Count; j++)
                                {
                                    // Tasks_ID
                                    if (cfgCustomer.Task_ID)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Task ID", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TaskProduzioneID.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Nome
                                    if (cfgCustomer.Task_Nome)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Nome task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Name.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Descrizione
                                    if (cfgCustomer.Task_Descrizione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Descrizione task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Description.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Postazione
                                    if (cfgCustomer.Task_Postazione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Postazione di lavoro", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].PostazioneName.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_EarlyStart
                                    if (cfgCustomer.Task_EarlyStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_EarlyFinish
                                    if (cfgCustomer.Task_EarlyFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_LateStart
                                    if (cfgCustomer.Task_LateStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_LateFinish
                                    if (cfgCustomer.Task_LateFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_NumeroOperatori
                                    if (cfgCustomer.Task_NOperatori)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Numero operatori assegnati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].NumOperatori.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoCiclo
                                    if (cfgCustomer.Task_TempoCiclo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Tempo ciclo previsto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TempoC.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroPrevisto
                                    if (cfgCustomer.Task_TempoDiLavoroPrevisto)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TempoDiLavoroPrevisto.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroEffettivo
                                    if (cfgCustomer.Task_TempoDiLavoroEffettivo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro effettive", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TempoDiLavoroEffettivo.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Status
                                    if (cfgCustomer.Task_Status)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        String status = "";
                                        switch (lstProd[i].Tasks[j].Status)
                                        {
                                            case 'I':
                                                status = "In produzione"; break;
                                            case 'N':
                                                status = "Non ancora iniziato"; break;
                                            case 'P':
                                                status = "In pausa"; break;
                                            case 'F':
                                                status = "Terminato";
                                                break;
                                            default:
                                                status = "#ND";
                                                break;
                                        }
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(status, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_QuantitaProdotta
                                    if (cfgCustomer.Task_QuantitaProdotta)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }
                                }
                            }*/

                            tbl.SpacingAfter = 10;
                            cartPDF.Add(tbl);
                            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
                            p.SpacingAfter = 20;
                            cartPDF.Add(p);
                        }
                    }
                }

                // Stampa tasks in stato F
                if (Session["prodF"] != null)
                {
                    List<Articolo> lstProd = (List<Articolo>)Session["prodF"];
                    if (lstProd.Count > 0)
                    {
                        configCustomerOrderStatusReport cfgCustomer = new configCustomerOrderStatusReport(customer.CodiceCliente);
                        iTextSharp.text.Paragraph par = new Paragraph("Prodotti terminati", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD));
                        par.SpacingAfter = 20;
                        cartPDF.Add(par);

                        for (int i = 0; i < lstProd.Count; i++)
                        {
                            PdfPTable tbl = new PdfPTable(2);
                            tbl.WidthPercentage = 100;

                            // Commessa_IDCommessa
                            if (cfgCustomer.IDCommessa)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Commessa.ToString() + "/" + lstProd[i].AnnoCommessa.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Cliente
                            if (cfgCustomer.Cliente)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Cliente", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(customer.RagioneSociale, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Commessa_DataInserimentoOrdine
                            if (cfgCustomer.DataInserimentoOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data ricevimento ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.DataInserimento.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }
                            // Commessa_Note
                            if (cfgCustomer.NoteOrdine)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Note ordine", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                Commessa comm = new Commessa(lstProd[i].Commessa, lstProd[i].AnnoCommessa);
                                iTextSharp.text.Paragraph txt1 = new Paragraph(comm.Note, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IDProdotto
                            if (cfgCustomer.IDProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("ID prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].ID.ToString() + "/" + lstProd[i].Year.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeProdotto
                            if (cfgCustomer.NomeProdotto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Linea prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.process.processName, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_NomeVariante
                            if (cfgCustomer.NomeVariante)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Nome prodotto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Proc.variant.nomeVariante, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Matricola
                            if (cfgCustomer.Matricola)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Matricola", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Matricola, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Status
                            if (cfgCustomer.Status)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph("Finito", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Reparto
                            if (cfgCustomer.Reparto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Reparto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].RepartoNome, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaConsegna
                            if (cfgCustomer.DataPrevistaConsegna)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista consegna", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaConsegna.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_DataPrevistaFineProduzione
                            if (cfgCustomer.DataPrevistaFineProduzione)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Data prevista fine produzione", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].DataPrevistaFineProduzione.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyStart
                            if (cfgCustomer.EarlyStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_EarlyFinish
                            if (cfgCustomer.EarlyFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateStart
                            if (cfgCustomer.LateStart)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LateFinish
                            if (cfgCustomer.LateFinish)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaPrevista
                            if (cfgCustomer.Quantita)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi richiesti", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Quantita.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_QuantitaProdotta
                            if (cfgCustomer.QuantitaProdotta)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità pezzi realizzati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_Ritardo
                            if (cfgCustomer.Ritardo)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Giorni di ritardo", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Ritardo.TotalDays.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_TempoDiLavoroTotale
                            if (cfgCustomer.TempoDiLavoroTotale)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro impiegate", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].TempoDiLavoroTotale.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_LeadTime
                            if (cfgCustomer.LeadTime)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Lead Time", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                lstProd[i].loadLeadTimes();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].LeadTime.TotalHours,2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_TempoDiLavoroPrevisto
                            if (cfgCustomer.TempoDiLavoroPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].TempoDiLavoroPrevisto.TotalHours.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks (n° tasks terminati / n° tasks totali)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTasks, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_IndicatoreCompletamentoTasks
                            /*if (cfgCustomer.IndicatoreCompletamentoTempoPrevisto)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("% completamento tasks su tempo di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                tbl.AddCell(cl[0]);

                                cl[1] = new PdfPCell();
                                iTextSharp.text.Paragraph txt1 = new Paragraph((Math.Round(lstProd[i].IndicatoreCompletamentoTempoPrevisto, 2)).ToString() + "%", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[1].AddElement(txt1);
                                tbl.AddCell(cl[1]);
                            }*/

                            // Prodotto_GANTT
                            if (cfgCustomer.ViewGanttTasks)
                            {
                                PdfPCell[] cl = new PdfPCell[2];
                                cl[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt0 = new Paragraph("Timeline avanzamento\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                cl[0].AddElement(txt0);
                                cl[0].Colspan = 2;


                                lstProd[i].loadTasksProduzione();
                                PdfPTable gantt = new PdfPTable(lstProd[i].Tasks.Count);
                                gantt.WidthPercentage = 100;
                                for (int k = 0; k < lstProd[i].Tasks.Count; k++)
                                {
                                    PdfPCell tsk = new PdfPCell();
                                    iTextSharp.text.Paragraph pTask = new Paragraph(lstProd[i].Tasks[k].Name, new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL));
                                    if (lstProd[i].Tasks[k].Status == 'I')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Blue);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.White);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'N')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.White);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'F')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Green);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    else if (lstProd[i].Tasks[k].Status == 'P')
                                    {
                                        tsk.BackgroundColor = new BaseColor(System.Drawing.Color.Orange);
                                        pTask.Font.Color = new BaseColor(System.Drawing.Color.Black);
                                    }
                                    tsk.AddElement(pTask);
                                    gantt.AddCell(tsk);
                                }
                                cl[0].AddElement(gantt);
                                tbl.AddCell(cl[0]);
                            }

                            // Prodotto_ViewElencoTasks
                            if (cfgCustomer.ViewElencoTasks)
                            {
                                PdfPCell[] clT = new PdfPCell[1];
                                clT[0] = new PdfPCell();
                                iTextSharp.text.Paragraph txt = new Paragraph("Elenco tasks", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD));
                                clT[0].AddElement(txt);
                                clT[0].Colspan = 2;
                                tbl.AddCell(clT[0]);

                                lstProd[i].loadTasksProduzione();
                                for (int j = 0; j < lstProd[i].Tasks.Count; j++)
                                {
                                    System.Drawing.Color bgColor = (j % 2 == 0) ? System.Drawing.Color.FromArgb(240, 240, 240) : System.Drawing.Color.White;
                                    // Tasks_ID
                                    if (cfgCustomer.Task_ID)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Task ID", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].TaskProduzioneID.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Nome
                                    if (cfgCustomer.Task_Nome)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Nome task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Name.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_Descrizione
                                    if (cfgCustomer.Task_Descrizione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Descrizione task", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].Description.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Postazione
                                    if (cfgCustomer.Task_Postazione)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Postazione di lavoro", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].PostazioneName.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_EarlyStart
                                    if (cfgCustomer.Task_EarlyStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_EarlyFinish
                                    if (cfgCustomer.Task_EarlyFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Prima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Tasks_LateStart
                                    if (cfgCustomer.Task_LateStart)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data inizio programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateStart.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_LateFinish
                                    if (cfgCustomer.Task_LateFinish)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ultima data fine programmata", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].LateFinish.ToString("dd/MM/yyyy HH:mm:ss"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_NumeroOperatori
                                    if (cfgCustomer.Task_NOperatori)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Numero operatori assegnati", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].NumOperatori.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoCiclo
                                    if (cfgCustomer.Task_TempoCiclo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Tempo ciclo previsto", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoC.TotalHours, 2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroPrevisto
                                    if (cfgCustomer.Task_TempoDiLavoroPrevisto)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro previste", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoDiLavoroPrevisto.TotalHours, 2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_TempoDiLavoroEffettivo
                                    if (cfgCustomer.Task_TempoDiLavoroEffettivo)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Ore di lavoro effettive", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(Math.Round(lstProd[i].Tasks[j].TempoDiLavoroEffettivo.TotalHours, 2).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_Status
                                    if (cfgCustomer.Task_Status)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Status", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        String status = "";
                                        switch (lstProd[i].Tasks[j].Status)
                                        {
                                            case 'I':
                                                status = "In produzione"; break;
                                            case 'N':
                                                status = "Non ancora iniziato"; break;
                                            case 'P':
                                                status = "In pausa"; break;
                                            case 'F':
                                                status = "Terminato";
                                                break;
                                            default:
                                                status = "#ND";
                                                break;
                                        }
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(status, new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }

                                    // Task_QuantitaProdotta
                                    if (cfgCustomer.Task_QuantitaProdotta)
                                    {
                                        PdfPCell[] cl = new PdfPCell[2];
                                        cl[0] = new PdfPCell();
                                        cl[0].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt0 = new Paragraph("Quantità", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[0].AddElement(txt0);
                                        tbl.AddCell(cl[0]);

                                        cl[1] = new PdfPCell();
                                        cl[1].BackgroundColor = new BaseColor(bgColor);
                                        iTextSharp.text.Paragraph txt1 = new Paragraph(lstProd[i].Tasks[j].QuantitaProdotta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL));
                                        cl[1].AddElement(txt1);
                                        tbl.AddCell(cl[1]);
                                    }
                                }
                            }
                            tbl.SpacingAfter = 10;
                            cartPDF.Add(tbl);
                            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
                            p.SpacingAfter = 20;
                            cartPDF.Add(p);
                        }
                    }
                }

                cartPDF.Close();
                output.Close();
                output.Dispose();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('/Data/Reports/" + FileNamePDF + "', '_newtab')", true);
            }
            else
            {
                lbl1.Text = "Cliente non trovato.";
            }
        }
    }
}