using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Personal
{
    public partial class HomeBoxUser : System.Web.UI.UserControl
    {
        private List<HomeBox> lstHomeBox;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack)
                {
                    User curr = (User)Session["user"];
                    lstHomeBox = new List<HomeBox>();
                    curr.loadHomeBoxes();
                    for (int i = 0; i < curr.homeBoxes.Elenco.Count; i++)
                    {
                        lstHomeBox.Add(curr.homeBoxes.Elenco[i].homeBox);
                    }

                    HomeBoxesList lstTotal = new HomeBoxesList();
                    for (int i = 0; i < lstTotal.Elenco.Count; i++)
                    {
                        try
                        {
                            HomeBox cerca = lstHomeBox.First(c => c.ID == lstTotal.Elenco[i].ID);
                        }
                        catch
                        {
                            lstHomeBox.Add(lstTotal.Elenco[i]);
                        }
                    }

                    rptHomeBoxes.DataSource = lstHomeBox;
                    rptHomeBoxes.DataBind();
                }
            }
            else
            {
                upd1.Visible = false;
            }
        }

        protected void rptHomeBoxes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                //if (tRow != null)
                {
                    ImageButton btnAdd = (ImageButton)e.Item.FindControl("btnAdd");
                    ImageButton btnArrowUp = (ImageButton)e.Item.FindControl("btnArrowUp");
                    ImageButton btnArrowDown = (ImageButton)e.Item.FindControl("btnArrowDown");
                    ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");

                    HiddenField hBoxId = (HiddenField)e.Item.FindControl("hID");

                    int boxID = -1;
                    try
                    {
                        boxID = Int32.Parse(hBoxId.Value.ToString());
                    }
                    catch
                    {
                        boxID = -1;
                    }

                    btnAdd.Visible = false;
                    btnArrowDown.Visible = false;
                    btnArrowUp.Visible = false;
                    btnDelete.Visible = false;

                    if (boxID != -1)
                    {
                        if (Session["user"] != null)
                        {
                            User curr = (User)Session["user"];
                            curr.loadHomeBoxes();

                            try
                            {
                                HomeBox boxCurr = new HomeBox(boxID);
                                KIS.App_Code.HomeBoxUser cerca = curr.homeBoxes.Elenco.First(c => c.homeBox.ID == boxCurr.ID);
                                btnAdd.Visible = false;
                                btnDelete.Visible = true;
                                btnArrowUp.Visible = true;
                                btnArrowDown.Visible = true;

                                btnArrowUp.Visible = !(cerca.ordine == 0);
                                HomeBoxesListUser lstBox = new HomeBoxesListUser(curr);
                                btnArrowDown.Visible = !(cerca.ordine == lstBox.Elenco.Count-1);
                            }
                            catch
                            {
                                btnAdd.Visible = true;
                                btnDelete.Visible = false;
                                btnArrowUp.Visible = false;
                                btnArrowDown.Visible = false;
                            }
                        }

                    }
                    else
                    {
                        btnAdd.Visible = false;
                        btnArrowDown.Visible = false;
                        btnArrowUp.Visible = false;
                    }
                }
            }
        }

        protected void rptHomeBoxes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int homeBID = -1;
            try
            {
                homeBID = Int32.Parse(e.CommandArgument.ToString());
            }
            catch 
            { 
                homeBID = -1;
            }
            if (homeBID != -1)
            {
                if (Session["user"] != null)
                {
                    User curr = (User)Session["user"];
                    HomeBox hBox = new HomeBox(homeBID);
                    if (hBox.ID != -1 && curr.username.Length > 0)
                    {
                        Boolean ret = false;
                        HomeBoxesListUser hUser = new HomeBoxesListUser(curr);
                        int ind = -1;
                        switch (e.CommandName)
                        {
                            case "add":
                                ret = curr.addHomeBox(hBox);
                                break;
                            case "up":
                                ind = -1;
                                for (int i = 0; i < hUser.Elenco.Count; i++)
                                {
                                    if (hUser.Elenco[i].homeBox.ID == hBox.ID)
                                    {
                                        ind = i;
                                    }
                                }

                                // Se non è il primo, ordino
                                if (hUser.Elenco[ind].ordine > 0)
                                {
                                    hUser.Elenco[ind].ordine--;
                                }
                                if (ind > 0)
                                {
                                    hUser.Elenco[ind-1].ordine++;
                                }
                                ret = true;
                                break;
                            case "down":
                                ind = -1;
                                for (int i = 0; i < hUser.Elenco.Count; i++)
                                {
                                    if (hUser.Elenco[i].homeBox.ID == hBox.ID)
                                    {
                                        ind = i;
                                    }
                                }

                                // Se non è l'ultimo, ordino
                                if (ind < hUser.Elenco.Count - 1)
                                {
                                    hUser.Elenco[ind].ordine++;
                                    hUser.Elenco[ind+1].ordine--;
                                }
                                ret = true;
                                break;
                            case "delete":
                                ret = curr.deleteHomeBox(hBox);
                                hUser = new HomeBoxesListUser(curr);
                                for (int i = 0; i < hUser.Elenco.Count; i++)
                                {
                                    hUser.Elenco[i].ordine = i;
                                }
                                break;
                            default:
                                break;
                        }
                        if (ret)
                        {
                            loadBoxesRepeater();
                        }
                        else
                        {
                            lbl1.Text = "E' avvenuto un errore.<br />";
                        }
                    }
                }
            }
        }

        protected void loadBoxesRepeater()
        {
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                lstHomeBox = new List<HomeBox>();
                curr.loadHomeBoxes();
                for (int i = 0; i < curr.homeBoxes.Elenco.Count; i++)
                {
                    lstHomeBox.Add(curr.homeBoxes.Elenco[i].homeBox);
                }

                HomeBoxesList lstTotal = new HomeBoxesList();
                for (int i = 0; i < lstTotal.Elenco.Count; i++)
                {
                    try
                    {
                        HomeBox cerca = lstHomeBox.First(c => c.ID == lstTotal.Elenco[i].ID);
                    }
                    catch
                    {
                        lstHomeBox.Add(lstTotal.Elenco[i]);
                    }
                }

                rptHomeBoxes.DataSource = lstHomeBox;
                rptHomeBoxes.DataBind();
            }
        }
    }
}