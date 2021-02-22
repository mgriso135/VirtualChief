using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.Commesse;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_chooseFProducts1 : System.Web.UI.UserControl
    {
        public String customerID;
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkGoFwd.Visible = false;
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
                    frmReport.Visible = true;
                    lnkGoFwd.Visible = true;
                    imgGoFwd.Visible = true;
                    lnkGoBack.Visible = true;
                    imgGoBack.Visible = true;
                    lnkGoBack.NavigateUrl = "~/Analysis/ReportCustomerProdProgress_chooseINPProducts.aspx?customerID=" + customer.CodiceCliente;
                    if (!Page.IsPostBack)
                    {
                        if (Session["prodF"] == null)
                        {
                            List<Articolo> blankList = new List<Articolo>();
                            Session["prodF"] = blankList;
                        }
                        lnkGoFwd.NavigateUrl = "~/Analysis/ReportCustomerProdProgress_summary.aspx?customerID=" + customer.CodiceCliente;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorCustomerNotFound").ToString();
                    frmReport.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmReport.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime inizio = new DateTime(1970, 1, 1);
            DateTime fine = new DateTime(1970, 1, 1);
            String sInizio = Server.HtmlEncode(txtProductDateStart.Text);
            String sFine = Server.HtmlEncode(txtProductDateEnd.Text);
            try
            {
                String[] aInizio = sInizio.Split('/');
                int ggI = -1, mmI = -1, yyI = -1;
                ggI = Int32.Parse(aInizio[0]);
                mmI = Int32.Parse(aInizio[1]);
                yyI = Int32.Parse(aInizio[2]);
                inizio = new DateTime(yyI, mmI, ggI);

                String[] aFine = sFine.Split('/');
                int ggF = -1, mmF = -1, yyF = -1;
                ggF = Int32.Parse(aFine[0]);
                mmF = Int32.Parse(aFine[1]);
                yyF = Int32.Parse(aFine[2]);
                fine = new DateTime(yyF, mmF, ggF);
            }
            catch
            {
                inizio = new DateTime(1970, 1, 1);
                fine = new DateTime(1970, 1, 1);
            }
            if (inizio > new DateTime(1970, 1, 1) && fine > new DateTime(1970, 1, 1) && fine >= inizio)
            {
                rptProductsF.Visible = true;
                Cliente customer = new Cliente(customerID);
                customer.loadArticoli(null, 'F', inizio, fine);
                rptProductsF.DataSource = customer.listArticoli;
                rptProductsF.DataBind();
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
                rptProductsF.Visible = false;
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            HiddenField hArtID = (HiddenField)checkBox.Parent.FindControl("hArtID");
            HiddenField hArtYear = (HiddenField)checkBox.Parent.FindControl("hArtYear");
            int artID = -1, artYear = -1;
            try
            {
                artID = Int32.Parse(hArtID.Value.ToString());
                artYear = Int32.Parse(hArtYear.Value.ToString());
            }
            catch
            {
                artID = -1;
                artYear = -1;
            }

            if (artID != -1 && artYear != -1)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (checkBox.Checked)
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodF"];
                        swap.Add(art);
                        Session["prodF"] = swap;
                        lbl1.Text = GetLocalResourceObject("lblProdotto").ToString()
                            + " " + art.ID.ToString() + "/" + art.Year.ToString() 
                            + " " + GetLocalResourceObject("lblAddToList").ToString() + ".<br />";
                    }
                    else
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodF"];
                        var itemToRemove = swap.FirstOrDefault(u => (u.ID == art.ID && u.Year == art.Year));
                        if (itemToRemove != null)
                        {
                            swap.Remove(itemToRemove);
                            lbl1.Text += GetLocalResourceObject("lblProdotto").ToString() + " " 
                                + art.ID.ToString() + "/" + art.Year.ToString()
                                + " " + GetLocalResourceObject("lblRemovedFromList").ToString() + ".<br />";
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                        }
                        Session["prodF"] = swap;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
            }
        }

        protected void rptProductsF_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox checkBox = (CheckBox)e.Item.FindControl("chk");
                HiddenField hArtID = (HiddenField)e.Item.FindControl("hArtID");
                HiddenField hArtYear = (HiddenField)e.Item.FindControl("hArtYear");
                int artID = -1, artYear = -1;
                try
                {
                    artID = Int32.Parse(hArtID.Value.ToString());
                    artYear = Int32.Parse(hArtYear.Value.ToString());
                }
                catch
                {
                    artID = -1;
                    artYear = -1;
                }

                if (artID != -1 && artYear != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                    if (art.ID != -1 && art.Year != -1)
                    {
                        if (Session["prodF"] != null)
                        {
                            List<Articolo> lstArt = (List<Articolo>)Session["prodF"];
                            int r = lstArt.FindIndex(t => art.ID == t.ID && art.Year == t.Year);
                            checkBox.Checked = r == -1 ? false : true;
                        }
                    }
                }
            }
        }
    }
}