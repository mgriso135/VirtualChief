using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class listCommesse : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "Commesse";
                prmUser[1] = "R";
                elencoPermessi.Add(prmUser);
                bool checkUser = false;
                if (Session["user"] != null)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
                }

                if (checkUser == true)
                {
                    if (!Page.IsPostBack)
                    {
                        ElencoCommesseAperte elComm = new ElencoCommesseAperte(Session["ActiveWorkspace"].ToString());
                        /*List<Commessa> lstCommAperte = new List<Commessa>();

                        for (int i = 0; i < elComm.Commesse.Count; i++)
                        {
                            if (elComm.Commesse[i].Status != 'F')
                            {
                                lstCommAperte.Add(elComm.Commesse[i]);
                            }
                        }


                        rptCommesse.DataSource = lstCommAperte;*/
                        rptCommesse.DataSource = elComm.Commesse;
                        rptCommesse.DataBind();
                    }
                }
                else
                {
                    rptCommesse.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                }
        }

        protected void rptCommesse_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hID = (HiddenField)e.Item.FindControl("hID");
                HiddenField hYear = (HiddenField)e.Item.FindControl("hYear");
                ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
                int cID = -1;
                int cYear = -1;
                try
                {
                    cID = Int32.Parse(hID.Value);
                    cYear = Int32.Parse(hYear.Value);
                }
                catch
                {
                    cID = -1;
                    cYear = -1;
                }

                if (cID != -1 && cYear!=-1)
                {
                    Commessa comm = new Commessa(Session["ActiveWorkspace"].ToString(), cID, cYear);
                    if (comm.Status == 'N')
                    {
                        imgDelete.Visible = true;
                    }
                    else
                    {
                        imgDelete.Visible = false;
                    }
                }
            }

            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {/*
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptCommesse_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idCommessa = -1;
            int annoCommessa = -1;

            try
            {
                String[] sID = e.CommandArgument.ToString().Split('/');
                idCommessa = Int32.Parse(sID[0]);
                annoCommessa = Int32.Parse(sID[1]);
            }
            catch
            {
                idCommessa = -1;
                annoCommessa = -1;
            }

            if (idCommessa != -1&&annoCommessa !=-1)
            {
                Commessa comm = new Commessa(Session["ActiveWorkspace"].ToString(), idCommessa, annoCommessa);
                if (comm.ID != -1)
                {
                    if (e.CommandName == "delete")
                    {
                        if (comm.Status == 'N')
                        {
                            comm.loadArticoli();
                            if (comm.Articoli.Count == 0)
                            {
                                bool rt = comm.Delete();
                                if (rt == true)
                                {
                                    Response.Redirect(Request.RawUrl);
                                }
                                else
                                {
                                    lbl1.Text = comm.log;
                                }
                            }
                            else
                            {
                                lbl1.Text = GetLocalResourceObject("lblErrorNeedToDelete").ToString();
                            }
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrorCantDelete").ToString();
                        }
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorInputData").ToString();
                }
            }
        }

        
    }
}