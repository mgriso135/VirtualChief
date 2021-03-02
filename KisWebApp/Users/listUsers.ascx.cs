using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using KIS;
using iTextSharp.text;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class listUsers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String permessoRichiesto = "Utenti";
            bool checkUser = false;
            if (Session["User"] != null && Session["ActiveWorkspace"]!=null)
            {
                Workspace ws = new Workspace(Session["ActiveWorkspace"].ToString());
                UserAccount curr = (UserAccount)Session["user"];
                curr.loadGroups(ws.id);
                for (int i = 0; i < curr.groups.Count; i++)
                {
                    for (int j = 0; j < curr.groups[i].Permissions.Elenco.Count; j++)
                    {
                        if (curr.groups[i].Permissions.Elenco[j].NomePermesso == permessoRichiesto && curr.groups[i].Permissions.Elenco[j].R == true)
                        {
                            checkUser = true;
                        }
                    }
                }
            }

            if (checkUser == true)
            {
                if(!Page.IsPostBack && !Page.IsCallback)
                { 
                UserList lst = new UserList(Session["ActiveWorkspace"].ToString());
            if (lst.numUsers > 0)
            {
                
                    rptUsers.DataSource = lst.elencoUtenti;
                    rptUsers.DataBind();
                
            }
            else
            {
                rptUsers.Visible = false;
                        lblLstUsers.Text = GetLocalResourceObject("lblNoUsers").ToString();
            }
            }
            }
            else
            {
                lblLstUsers.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptUsers_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {
            String usrID = e.CommandArgument.ToString();
            KISConfig vcCfg = new KISConfig(Session["ActiveWorkspace"].ToString());
            String checksum = vcCfg.basePath;

            if(e.CommandName== "printBarcode")
            { 
                if(checksum.Length > 0)
                { 
            User usr = new User(usrID);
            String matricola = usr.ID.ToString();
            System.Drawing.Image code = GenCode128.Code128Rendering.MakeBarcodeImage(
                "U" +checksum+";"+ matricola, 2, true);
            System.Drawing.Bitmap resized = new System.Drawing.Bitmap(code, 200, 200 * (code.Height) / code.Width);
            String savePath = Server.MapPath(@"~\Data\Users\");
            String FileName = "cart" + usr.ID.ToString() + ".jpg";
            try
            {
                code.Save(savePath + FileName);
            }
            catch (Exception ex)
            {
                    lblLstUsers.Text = GetLocalResourceObject("lblErrorSave").ToString()+ " " + savePath + "<br/>" + ex.Message;
            }

            // Ora creo il pdf!
            Document cartPDF = new Document(PageSize.ID_1, 10, 10, 5, 5);
            String FileNamePDF = "cart" + usr.ID.ToString() + ".pdf";
            // Controllo che il pdf non esista, e se esiste lo cancello.
            if (System.IO.File.Exists(savePath + FileNamePDF))
            {
                System.IO.File.Delete(savePath + FileNamePDF);
            }

            System.IO.FileStream output = new System.IO.FileStream(savePath + FileNamePDF, System.IO.FileMode.Create);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, output);
            cartPDF.Open();
            iTextSharp.text.Paragraph nome = new iTextSharp.text.Paragraph(usr.name + " " + usr.cognome);
            cartPDF.Add(nome);
            iTextSharp.text.Paragraph matr = new iTextSharp.text.Paragraph(GetLocalResourceObject("lblMatricola").ToString() + ": " + matricola + "; "+GetLocalResourceObject("lblUser")+":" + usr.username);
            cartPDF.Add(matr);
            iTextSharp.text.Image bCode = iTextSharp.text.Image.GetInstance(savePath + FileName);
                    float origHeight = bCode.Height;
                    float origWidth = bCode.Width;
                    bCode.ScaleToFit(200, 200 * origHeight / origWidth);
            bCode.SetAbsolutePosition(0, 40);
            cartPDF.Add(bCode);
            cartPDF.Close();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "window.open('../Data/Users/" + FileNamePDF + "', '_newtab')", true);

            try
            {
                System.IO.File.Delete(savePath + FileName);
            }
            catch
            {
            }
            }
            }
            else
            {
                lblLstUsers.Text = GetLocalResourceObject("lblChecksumError").ToString();
            }
        }

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblImg = (Label)e.Item.FindControl("lblImg");
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    HiddenField hdUsername = (HiddenField)e.Item.FindControl("hdUsername");
                    if (hdUsername.Value.Length > 0)
                    {
                        System.Web.UI.WebControls.Image imgOk = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgOk");
                        System.Web.UI.WebControls.Image imgKo = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgKo");
                        User curr = new User(hdUsername.Value);
                        if (curr != null && imgOk != null && imgKo != null)
                        {
                            Boolean configured = curr.FullyConfigured;
                            imgOk.Visible = configured;
                            imgKo.Visible = !configured;
                        }
                    }
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }
    }
}