using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.IO;

namespace KIS.Configuration
{
    public partial class wizConfigLogo1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            KISConfig cfg = new KISConfig();

            if (!cfg.WizLogoCompleted)
            {
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    Logo lg = new Logo();
                    if (lg.filePath.Length > 0)
                    {
                        imgCurrentLogo.Visible = true;
                        imgCurrentLogo.ImageUrl = lg.filePath;
                    }
                }
            }
            else
            {
                lbl1.Text = "Il logo è già stato configurato tramite wizard.<br />Per variarlo utilizzare il normale menu di configurazione accedendo con privilegi di Admin.";
                FileUpload1.Visible = false;
                btnUpload.Visible = false;
                btnUpload.Enabled = false;
                FileUpload1.Enabled = false;
            }
        }

        protected void Upload(object sender, EventArgs e)
        {
            lbl1.Text = "Carico...<br/>";
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.PostedFile.ContentLength <= 1024 * 1024)
                {
                    // Controllo che sia un'immagine
                    bool isValid = false;

                    string[] fileExtensions = { ".BMP", ".JPG", ".PNG", ".GIF", ".JPEG" };

                    for (int i = 0; i < fileExtensions.Length; i++)
                    {
                        if (FileUpload1.PostedFile.FileName.ToUpper().Contains(fileExtensions[i]))
                        {
                            isValid = true; break;
                        }
                    }

                    if (isValid == true)
                    {
                        try
                        {
                            //lbl1.Text = FileUpload1.PostedFile.ContentLength.ToString() + " " + FileUpload1.PostedFile.ContentType.ToString();
                            if (!System.IO.Directory.Exists(Server.MapPath("~/Data/Logo/")))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~/Data/Logo"));
                            }

                            // Pulisco la cartella
                            foreach (var file in Directory.GetFiles(Server.MapPath("~/Data/Logo/")))
                            {
                                File.Delete(file);
                            }

                            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Data/Logo/") + fileName);
                            Logo logo = new Logo();
                            logo.filePath = fileName;
                            lbl1.Text += "Log: " + logo.log;
                            KISConfig cfg = new KISConfig();
                            if (cfg.WizLogoCompleted)
                            {
                                lbl1.Text = "Logo caricato correttamente.";
                            }
                            Response.Redirect("~/Configuration/MainWizConfig.aspx");
                        }
                        catch (Exception ex)
                        {
                            lbl1.Text += ex.Message;
                        }
                    }
                    else
                    {
                        lbl1.Text = "Formato immagine non accettato.<br />";
                    }
                }
                else
                {
                    lbl1.Text = "Attenzione: l'immagine non può essere maggiore di 1024kb. Hai cercato di caricare una immagine di " + FileUpload1.PostedFile.ContentLength + " kb";
                }
            }
            else
            {
                lbl1.Text = "File non trovato.<br />";
            }
        }
    }
}