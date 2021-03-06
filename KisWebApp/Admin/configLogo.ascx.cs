using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class configLogo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Configurazione Logo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"]!=null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    Logo lg = new Logo(Session["ActiveWorkspace_Name"].ToString());
                    if (lg.filePath.Length > 0)
                    {
                        imgCurrentLogo.Visible = true;
                        imgCurrentLogo.ImageUrl = lg.filePath;
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKO").ToString();
                FileUpload1.Visible = false;
                btnUpload.Visible = false;
                btnUpload.Enabled = false;
                FileUpload1.Enabled = false;
            }
        }

        protected void Upload(object sender, EventArgs e)
        {
            lbl1.Text = GetLocalResourceObject("lblCarico").ToString(); 
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.PostedFile.ContentLength <= 1024*1024)
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
                            if(!System.IO.Directory.Exists(Server.MapPath("~/Data/Logo/")))
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
                                Logo logo = new Logo(Session["ActiveWorkspace_Name"].ToString());
                                logo.filePath = fileName;
                                lbl1.Text += "Log: "+logo.log;
                                Response.Redirect(Request.RawUrl);
                        }
                        catch(Exception ex)
                        {
                            lbl1.Text += ex.Message;
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblFormatoKO").ToString(); 
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorDimensioni").ToString() + FileUpload1.PostedFile.ContentLength + " kb";
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblFileNotFound").ToString();
            }
        }
    }
}