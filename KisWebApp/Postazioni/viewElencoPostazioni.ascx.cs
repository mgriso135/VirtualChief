using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using iTextSharp;
using iTextSharp.text;
using KIS.App_Code;

namespace KIS.Postazioni
{
    public partial class viewElencoPostazioni : System.Web.UI.UserControl
    {
        int cont;
        ElencoPostazioni el;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione";
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
                cont = 0;
                if (!Page.IsPostBack)
                {
                    el = new ElencoPostazioni(Session["ActiveWorkspace"].ToString());
                    rptPostazioni.DataSource = el.elenco;
                    rptPostazioni.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptPostazioni.Visible = false;
            }
        }

        public void rptPostazioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                ((HiddenField)e.Item.FindControl("ID")).Value = el.elenco[cont].id.ToString();
                
                ImageButton del = (ImageButton)e.Item.FindControl("deletePostazione");
                del.CommandArgument = el.elenco[cont].id.ToString();
                del.CommandName = "delete";
                del.Command += new CommandEventHandler(postazione_modify);


                cont++;
            }
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    //tRow.BgColor = "#00FF00";
                    //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    //tRow.BgColor = "#C0C0C0";
                    //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }

        }

        protected void postazione_modify(object sender, CommandEventArgs e)
        {
            
           int pstID;
           try
           {
              pstID = Int32.Parse(e.CommandArgument.ToString());
           }
           catch
           {
               pstID = -1;
           }
           if (pstID != -1)
           {
               if (e.CommandName == "delete")
               {
                    Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                    if (p.id != -1)
                    {
                        p.loadTasks();
                        if (p.tasks.Count == 0)
                        {
                            // Cancello la postazione
                            bool rt = p.delete();
                            if (rt == true)
                            {
                                Response.Redirect(Request.RawUrl);
                            }
                            else
                            {
                                lbl1.Text = GetLocalResourceObject("lblGenericError").ToString() + ": " + p.log;
                            }

                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblDelKOTasks").ToString() + "<br />"; 
                            for (int i = 0; i < p.tasks.Count; i++)
                            {
                                lbl1.Text += p.tasks[i].processName + "<br/>";
                            }
                        }
                    }
                    
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblPostazioneNotFound").ToString();
                    }
                }
               else if (e.CommandName == "printBarCode")
               {
                   Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                   if (p.id != -1)
                   {
                       System.Drawing.Image code = GenCode128.Code128Rendering.MakeBarcodeImage("P" + p.id.ToString(), 2, true);
                       System.Drawing.Bitmap resized = new System.Drawing.Bitmap(code, 200, 200 * (code.Height) / code.Width);
                       String savePath = Server.MapPath(@"~\Data\Postazioni\");
                       String FileName = "postazione" + p.id.ToString() + ".jpg";
                       try
                       {
                           resized.Save(savePath + FileName);
                       }
                       catch (Exception ex)
                       {
                           lbl1.Text = "Could not save image.<br/>" + savePath + "<br/>" + ex.Message;
                       }

                       // Ora creo il pdf!
                       Document cartPDF = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
                       String FileNamePDF = "postazione" + p.id.ToString() + ".pdf";
                       // Controllo che il pdf non esista, e se esiste lo cancello.
                       if (System.IO.File.Exists(savePath + FileNamePDF))
                       {
                           System.IO.File.Delete(savePath + FileNamePDF);
                       }

                       System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create);
                       iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
                       cartPDF.Open();
                       iTextSharp.text.Paragraph nomePost = new iTextSharp.text.Paragraph(Server.HtmlDecode(p.name), new Font(Font.FontFamily.HELVETICA, 60, Font.BOLD));
                       nomePost.Alignment = Element.ALIGN_CENTER;
                       
                       cartPDF.Add(nomePost);
                       iTextSharp.text.Paragraph descPost = new iTextSharp.text.Paragraph(Server.HtmlDecode(p.desc), new Font(Font.FontFamily.COURIER, 30));
                       descPost.Alignment = Element.ALIGN_CENTER;
                       cartPDF.Add(descPost);
                       iTextSharp.text.Image bCode = iTextSharp.text.Image.GetInstance(savePath + FileName);
                       //bCode.SetAbsolutePosition(100, 100);
                       bCode.Alignment = Element.ALIGN_CENTER;
                       cartPDF.Add(bCode);
                       cartPDF.Close();
                       Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('../Data/Postazioni/" + FileNamePDF + "', '_newtab')", true);

                       //try
                       {
                           System.IO.File.Delete(savePath + FileName);
                       }
                       //catch
                       {
                       }
                   }
               }

               else
               {
                   lbl1.Text = e.CommandName;
               }
                
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPostazioneNotFound").ToString();
            }
            
        }
    }
}