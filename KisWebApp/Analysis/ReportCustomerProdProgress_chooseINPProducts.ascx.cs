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
    public partial class ReportCustomerProdProgress_chooseINPProducts1 : System.Web.UI.UserControl
    {
        public String customerID;
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkGoBack.Visible = false;
            imgGoBack.Visible = false;
            lnkGoFwd.Visible = false;
            imgGoFwd.Visible = false;
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
                        lnkGoFwd.Visible = true;
                        imgGoFwd.Visible = true;
                        lnkGoBack.Visible = true;
                        imgGoBack.Visible = true;
                        frmReport.Visible = true;
                        if (!Page.IsPostBack)
                        {
                            customer.loadArticoli('I');
                            rptProdottiI.DataSource = customer.listArticoli;
                            rptProdottiI.DataBind();
                            if (Session["prodI"] == null)
                            {
                                Session["prodI"] = customer.listArticoli;
                            }
                            customer.loadArticoli('P');
                            rptProdottiP.DataSource = customer.listArticoli;
                            rptProdottiP.DataBind();
                            if (Session["prodP"] == null)
                            {
                                Session["prodP"] = customer.listArticoli;
                            }
                            customer.loadArticoli('N');
                            rptProdottiN.DataSource = customer.listArticoli;
                            rptProdottiN.DataBind();
                            if (Session["prodN"] == null)
                            {
                                Session["prodN"] = customer.listArticoli;
                            }
                            lnkGoFwd.NavigateUrl = "ReportCustomerProdProgress_chooseFProducts.aspx?customerID=" + customer.CodiceCliente;
                        }
                    }
                    else
                    {
                        lnkGoFwd.Visible = false;
                        frmReport.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblCustomerNotFound").ToString();
                    }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmReport.Visible = false;
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            lblI.Text = "";
            lblN.Text = "";
            lblP.Text = "";
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
                Articolo art = new Articolo(artID, artYear);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (checkBox.Checked)
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodI"];
                        swap.Add(art);
                        Session["prodI"] = swap;
                        lblI.Text = GetLocalResourceObject("lblProdotto").ToString()
                            + " " + art.ID.ToString() + "/" + art.Year.ToString() 
                            + " " + GetLocalResourceObject("lblAddedToList").ToString() + "<br />";
                    }
                    else
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodI"];
                        var itemToRemove = swap.FirstOrDefault(u => (u.ID ==art.ID && u.Year == art.Year));
                        if (itemToRemove != null)
                        {
                            swap.Remove(itemToRemove);
                            lblI.Text += GetLocalResourceObject("lblProdotto").ToString()
                            + " " + art.ID.ToString() + "/" + art.Year.ToString()
                            + " " + GetLocalResourceObject("lblRemovedFromList").ToString() + "<br />";
                        }
                        else
                        {
                            lblI.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                        }                        
                        Session["prodI"] = swap;
                    }
                }
                else
                {
                    lblI.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                }
            }
            else
            {
                lblI.Text = GetLocalResourceObject("lblErrorData").ToString();
            }
        }

        protected void chkP_CheckedChanged(object sender, EventArgs e)
        {
            lblI.Text = "";
            lblN.Text = "";
            lblP.Text = "";
            CheckBox checkBox = (CheckBox)sender;
            HiddenField hArtID = (HiddenField)checkBox.Parent.FindControl("hArtID");
            HiddenField hArtYear = (HiddenField)checkBox.Parent.FindControl("hArtYear");
            int artID = -1, artYear = -1;
            try
            {
                artID = Int32.Parse(hArtID.Value.ToString());
                artYear = Int32.Parse(hArtYear.Value.ToString());
            }
            catch (Exception ex)
            {
                artID = -1;
                artYear = -1;
            }

            if (artID != -1 && artYear != -1)
            {
                Articolo art = new Articolo(artID, artYear);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (checkBox.Checked)
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodP"];
                        swap.Add(art);
                        Session["prodP"] = swap;
                        lblP.Text = GetLocalResourceObject("lblProdotto").ToString() 
                            + " " + art.ID.ToString() + "/" + art.Year.ToString() 
                            + " " + GetLocalResourceObject("lblAddedToList").ToString() + "<br />";
                    }
                    else
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodP"];
                        var itemToRemove = swap.FirstOrDefault(u => (u.ID == art.ID && u.Year == art.Year));
                        if (itemToRemove != null)
                        {
                            swap.Remove(itemToRemove);
                            lblP.Text = GetLocalResourceObject("lblProdotto").ToString() 
                                + " " + art.ID.ToString() + "/" + art.Year.ToString()
                                + " " + GetLocalResourceObject("lblRemovedFromList").ToString() + "<br />";
                        }
                        else
                        {
                            lblP.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                        }
                        Session["prodP"] = swap;
                    }
                }
                else
                {
                    lblP.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                }
            }
            else
            {
                lblP.Text = GetLocalResourceObject("lblErrorData").ToString();
            }
        }

        protected void chkN_CheckedChanged(object sender, EventArgs e)
        {
            lblI.Text = "";
            lblN.Text = "";
            lblP.Text = "";
            CheckBox checkBox = (CheckBox)sender;
            HiddenField hArtID = (HiddenField)checkBox.Parent.FindControl("hArtID");
            HiddenField hArtYear = (HiddenField)checkBox.Parent.FindControl("hArtYear");
            int artID = -1, artYear = -1;
            try
            {
                artID = Int32.Parse(hArtID.Value.ToString());
                artYear = Int32.Parse(hArtYear.Value.ToString());
            }
            catch (Exception ex)
            {
                artID = -1;
                artYear = -1;
            }

            if (artID != -1 && artYear != -1)
            {
                Articolo art = new Articolo(artID, artYear);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (checkBox.Checked)
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodN"];
                        swap.Add(art);
                        Session["prodN"] = swap;
                        lblN.Text = GetLocalResourceObject("lblProdotto").ToString() 
                            + " " + art.ID.ToString() + "/" + art.Year.ToString()
                            + " " + GetLocalResourceObject("lblAddedToList").ToString() + "<br />";
                    }
                    else
                    {
                        List<Articolo> swap = (List<Articolo>)Session["prodN"];
                        var itemToRemove = swap.FirstOrDefault(u => (u.ID == art.ID && u.Year == art.Year));
                        if (itemToRemove != null)
                        {
                            swap.Remove(itemToRemove);
                            lblN.Text += GetLocalResourceObject("lblProdotto").ToString()
                            + " " + art.ID.ToString() + "/" + art.Year.ToString()
                            + " " + GetLocalResourceObject("lblRemovedFromList").ToString() + "<br />";
                        }
                        else
                        {
                            lblN.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                        }
                        Session["prodN"] = swap;
                    }
                }
                else
                {
                    lblN.Text = GetLocalResourceObject("lblItemNotFound").ToString();
                }
            }
            else
            {
                lblN.Text = GetLocalResourceObject("lblErrorData").ToString();
            }
        }

        protected void rptProdottiI_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                catch (Exception ex)
                {
                    artID = -1;
                    artYear = -1;
                }

                if (artID != -1 && artYear != -1)
                {
                    Articolo art = new Articolo(artID, artYear);
                    if(art.ID!=-1 && art.Year!=-1)
                    {
                    if (Session["prodI"] != null)
                    {
                        List<Articolo> lstArt = (List<Articolo>)Session["prodI"];
                        int r = lstArt.FindIndex(t => art.ID == t.ID && art.Year == t.Year);
                        checkBox.Checked = r==-1?false:true;
                    }
                    }
                }
            }
        }

        protected void rptProdottiP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox checkBox = (CheckBox)e.Item.FindControl("chkP");
                HiddenField hArtID = (HiddenField)e.Item.FindControl("hArtID");
                HiddenField hArtYear = (HiddenField)e.Item.FindControl("hArtYear");
                int artID = -1, artYear = -1;
                try
                {
                    artID = Int32.Parse(hArtID.Value.ToString());
                    artYear = Int32.Parse(hArtYear.Value.ToString());
                }
                catch (Exception ex)
                {
                    artID = -1;
                    artYear = -1;
                }

                if (artID != -1 && artYear != -1)
                {
                    Articolo art = new Articolo(artID, artYear);
                    if (art.ID != -1 && art.Year != -1)
                    {
                        if (Session["prodP"] != null)
                        {
                            List<Articolo> lstArt = (List<Articolo>)Session["prodP"];
                            int r = lstArt.FindIndex(t => art.ID == t.ID && art.Year == t.Year);
                            checkBox.Checked = r == -1 ? false : true;
                        }
                    }
                }
            }
        }

        protected void rptProdottiN_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox checkBox = (CheckBox)e.Item.FindControl("chkN");
                HiddenField hArtID = (HiddenField)e.Item.FindControl("hArtID");
                HiddenField hArtYear = (HiddenField)e.Item.FindControl("hArtYear");
                int artID = -1, artYear = -1;
                try
                {
                    artID = Int32.Parse(hArtID.Value.ToString());
                    artYear = Int32.Parse(hArtYear.Value.ToString());
                }
                catch (Exception ex)
                {
                    artID = -1;
                    artYear = -1;
                }

                if (artID != -1 && artYear != -1)
                {
                    Articolo art = new Articolo(artID, artYear);
                    if (art.ID != -1 && art.Year != -1)
                    {
                        if (Session["prodN"] != null)
                        {
                            List<Articolo> lstArt = (List<Articolo>)Session["prodN"];
                            int r = lstArt.FindIndex(t => art.ID == t.ID && art.Year == t.Year);
                            checkBox.Checked = r == -1 ? false : true;
                        }
                    }
                }
            }
        }
    }
}