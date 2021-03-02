using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp;
using iTextSharp.text;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Users
{
    public partial class generaBarcode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Utenti";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                int usrID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        usrID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        usrID = -1;
                    }
                }

                if (usrID != -1)
                {
                    User usr = new User(Session["ActiveWorkspace"].ToString(), usrID);
                    String matricola = usr.ID.ToString();
                    /*while (matricola.Length < 10)
                    {
                        matricola = "0" + matricola;
                    }*/
                    lblNome.Text = usr.name + " " + usr.cognome;
                    lblUsername.Text = usr.username;
                    lblMatricola.Text = matricola;
                    System.Drawing.Image code = GenCode128.Code128Rendering.MakeBarcodeImage("U" + matricola, 2, true);
                    System.Drawing.Bitmap resized = new System.Drawing.Bitmap(code, 200, 200*(code.Height)/code.Width);
                    String savePath = Server.MapPath(@"~\Data\Users\");
                    String FileName = "cart" + usr.ID.ToString() + ".jpg";
                    try
                    {
                        code.Save(savePath + FileName);
                    }
                    catch(Exception ex)
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorSaveImage").ToString()+ " " 
                            + savePath + "<br/>" + ex.Message;
                    }
                    
                    // Ora creo il pdf!
                    Document cartPDF = new Document(PageSize.ID_1, 10, 10, 5, 5);
                    String FileNamePDF = "cart" + usr.ID.ToString() + ".pdf";
                    // Controllo che il pdf non esista, e se esiste lo cancello.
                    if(System.IO.File.Exists(savePath + FileNamePDF))
                    {
                        System.IO.File.Delete(savePath + FileNamePDF);
                    }

                    System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
                    cartPDF.Open();
                    iTextSharp.text.Paragraph nome = new iTextSharp.text.Paragraph(usr.name + " " + usr.cognome);
                    cartPDF.Add(nome);
                    iTextSharp.text.Paragraph matr = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblMatricola").ToString() 
                        + ": " + matricola + "; "+GetLocalResourceObject("lblUser")+":" + usr.username);
                    cartPDF.Add(matr);
                    iTextSharp.text.Image bCode = iTextSharp.text.Image.GetInstance(savePath + FileName);
                    bCode.SetAbsolutePosition(0, 40);
                    cartPDF.Add(bCode);
                    cartPDF.Close();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('/Data/Users/"+FileNamePDF+"', '_newtab')", true);

                    try
                    {
                        System.IO.File.Delete(savePath + FileName);
                    }
                    catch
                    {
                    }

                    lbl1.Text = "<a href=\"manageUsers.aspx\">"+GetLocalResourceObject("lblGoBack").ToString()+"</a>";

                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }

        }
    }
}