using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.Web.UI.HtmlControls;
using KIS.App_Sources;

namespace KIS.Andon
{
    public partial class AndonCompletoViewFields : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto CampiDaVisualizzare";
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
                if (!Page.IsPostBack)
                {
                    AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    andonCfg.loadCampiVisualizzati();
                    rptFields.DataSource = andonCfg.CampiVisualizzati;
                    rptFields.DataBind();
                    rptCampiVisualizzabili.DataSource = andonCfg.FieldList;
                    rptCampiVisualizzabili.DataBind();

                    andonCfg.loadCampiVisualizzatiTasks();
                    rptFieldsTasks.DataSource = andonCfg.CampiVisualizzatiTasks;
                    rptFieldsTasks.DataBind();
                    rptCampiTasksVisualizzabili.DataSource = andonCfg.FieldListTasks;
                    rptCampiTasksVisualizzabili.DataBind();
                }
            }
            else
            {
                frmContainer.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("tr1");
                if (tr != null)
                {
                    AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    andonCfg.loadCampiVisualizzati();
                    ImageButton imgUp = (ImageButton)e.Item.FindControl("imgUp");
                    ImageButton imgDown = (ImageButton)e.Item.FindControl("imgDown");
                    ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
                    HiddenField hKey = (HiddenField)e.Item.FindControl("idVFieldName");

                        String chiave = hKey.Value.ToString();
                        var result = andonCfg.CampiVisualizzati.Where(x => x.Key == chiave).FirstOrDefault();
                        if (result.Key != null && result.Value == 0)
                        {
                            imgUp.Visible = false;
                        }
                        if(result.Key!=null && result.Value == andonCfg.CampiVisualizzati.Count -1)
                        {
                            imgDown.Visible = false;
                        }
                }
            }
        }

        protected void rptFields_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgUp = (ImageButton)e.Item.FindControl("imgUp");
            ImageButton imgDown = (ImageButton)e.Item.FindControl("imgDown");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
            String hKey = e.CommandArgument.ToString();
            AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
            if (hKey.Length>0 && imgUp != null && imgDown != null && imgDelete!=null)
            {
                if (e.CommandName == "Delete")
                {
                    Boolean rt = andonCfg.deleteCampoVisualizzato(hKey);
                    if (rt)
                    {
                        lblInfo.Text = GetLocalResourceObject(hKey.ToString()).ToString() + " " + GetLocalResourceObject("lblDeleted").ToString();
                        andonCfg.loadCampiVisualizzati();
                        rptFields.DataSource = andonCfg.CampiVisualizzati;
                        rptFields.DataBind();
                        rptCampiVisualizzabili.DataSource = andonCfg.FieldList;
                        rptCampiVisualizzabili.DataBind();
                    }
                    else
                    {
                        lblInfo.Text = "Error. " + andonCfg.log;
                    }
                }
                else if (e.CommandName == "Up")
                {
                    andonCfg.loadCampiVisualizzati();
                    var result = andonCfg.CampiVisualizzati.Where(x => x.Key == hKey).FirstOrDefault();
                    if (result.Key != null && result.Value>0)
                    {
                        int prec = result.Value - 1;
                        var result2 = andonCfg.CampiVisualizzati.Where(y => y.Value == prec).FirstOrDefault();
                        if (result2.Key != null)
                        {
                            andonCfg.setOrdineCampoVisualizzato(result.Key, prec);
                            andonCfg.setOrdineCampoVisualizzato(result2.Key, result.Value);
                            andonCfg.loadCampiVisualizzati();
                            rptFields.DataSource = andonCfg.CampiVisualizzati;
                            rptFields.DataBind();
                            rptCampiVisualizzabili.DataSource = andonCfg.FieldList;
                            rptCampiVisualizzabili.DataBind();
                        }
                    }
                }
                else if (e.CommandName == "Down")
                {
                    andonCfg.loadCampiVisualizzati();
                    var result = andonCfg.CampiVisualizzati.Where(x => x.Key == hKey).FirstOrDefault();
                    if (result.Key != null && result.Value < andonCfg.CampiVisualizzati.Count-1)
                    {
                        int next = result.Value + 1;
                        var result2 = andonCfg.CampiVisualizzati.Where(y => y.Value == next).FirstOrDefault();
                        if (result2.Key != null)
                        {
                            andonCfg.setOrdineCampoVisualizzato(result.Key, next);
                            andonCfg.setOrdineCampoVisualizzato(result2.Key, result.Value);
                            andonCfg.loadCampiVisualizzati();
                            rptFields.DataSource = andonCfg.CampiVisualizzati;
                            rptFields.DataBind();
                            rptCampiVisualizzabili.DataSource = andonCfg.FieldList;
                            rptCampiVisualizzabili.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptCampiVisualizzabili_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("tr2");
                HiddenField hID = (HiddenField)e.Item.FindControl("idPFieldName");
                if (tr != null && hID!=null)
                {
                    String chiave = hID.Value.ToString();
                    AndonCompleto cfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    cfg.loadCampiVisualizzati();
                    //var trova = cfg.CampiVisualizzati.FirstOrDefault(x => x.Key == chiave);
                    if (cfg.CampiVisualizzati.ContainsKey(chiave))
                    {
                        tr.Visible = false;
                    }
                }
            }
        }

        protected void rptCampiVisualizzabili_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgAdd = (ImageButton)e.Item.FindControl("imgAdd");
            String hKey = e.CommandArgument.ToString();
            AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
            if (hKey.Length > 0 && imgAdd != null)
            {
                if (e.CommandName == "Add")
                {
                    Boolean rt = andonCfg.addCampoVisualizzato(hKey);
                    if (rt)
                    {
                        lblInfo.Text = GetLocalResourceObject(hKey.ToString()).ToString() + " " + GetLocalResourceObject("lblAdded").ToString();
                        andonCfg.loadCampiVisualizzati();
                        rptFields.DataSource = andonCfg.CampiVisualizzati;
                        rptFields.DataBind();
                        rptCampiVisualizzabili.DataSource = andonCfg.FieldList;
                        rptCampiVisualizzabili.DataBind();
                    }
                    else
                    {
                        lblInfo.Text = "Error. " + andonCfg.log;
                    }
                }
            }
        }

        protected void rptFieldsTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgUp = (ImageButton)e.Item.FindControl("imgTaskUp");
            ImageButton imgDown = (ImageButton)e.Item.FindControl("imgTaskDown");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgTaskDelete");
            String hKey = e.CommandArgument.ToString();
            AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
            if (hKey.Length > 0 && imgUp != null && imgDown != null && imgDelete != null)
            {
                if (e.CommandName == "Delete")
                {
                    Boolean rt = andonCfg.deleteCampoVisualizzatoTasks(hKey);
                    if (rt)
                    {
                        lblInfo.Text = GetLocalResourceObject(hKey.ToString()).ToString() + " " + GetLocalResourceObject("lblDeleted").ToString();
                        andonCfg.loadCampiVisualizzatiTasks();
                        rptFieldsTasks.DataSource = andonCfg.CampiVisualizzatiTasks;
                        rptFieldsTasks.DataBind();
                        rptCampiTasksVisualizzabili.DataSource = andonCfg.FieldListTasks;
                        rptCampiTasksVisualizzabili.DataBind();
                    }
                    else
                    {
                        lblInfo.Text = "Error. " + andonCfg.log;
                    }
                }
                else if (e.CommandName == "Up")
                {
                    andonCfg.loadCampiVisualizzatiTasks();
                    var result = andonCfg.CampiVisualizzatiTasks.Where(x => x.Key == hKey).FirstOrDefault();
                    if (result.Key != null && result.Value > 0)
                    {
                        int prec = result.Value - 1;
                        var result2 = andonCfg.CampiVisualizzatiTasks.Where(y => y.Value == prec).FirstOrDefault();
                        if (result2.Key != null)
                        {
                            andonCfg.setOrdineCampoVisualizzatoTasks(result.Key, prec);
                            andonCfg.setOrdineCampoVisualizzatoTasks(result2.Key, result.Value);
                            andonCfg.loadCampiVisualizzatiTasks();
                            rptFieldsTasks.DataSource = andonCfg.CampiVisualizzatiTasks;
                            rptFieldsTasks.DataBind();
                            rptCampiTasksVisualizzabili.DataSource = andonCfg.FieldListTasks;
                            rptCampiTasksVisualizzabili.DataBind();
                        }
                    }
                }
                else if (e.CommandName == "Down")
                {
                    andonCfg.loadCampiVisualizzatiTasks();
                    var result = andonCfg.CampiVisualizzatiTasks.Where(x => x.Key == hKey).FirstOrDefault();
                    if (result.Key != null && result.Value < andonCfg.CampiVisualizzatiTasks.Count - 1)
                    {
                        int next = result.Value + 1;
                        var result2 = andonCfg.CampiVisualizzatiTasks.Where(y => y.Value == next).FirstOrDefault();
                        if (result2.Key != null)
                        {
                            andonCfg.setOrdineCampoVisualizzatoTasks(result.Key, next);
                            andonCfg.setOrdineCampoVisualizzatoTasks(result2.Key, result.Value);
                            andonCfg.loadCampiVisualizzatiTasks();
                            rptFieldsTasks.DataSource = andonCfg.CampiVisualizzatiTasks;
                            rptFieldsTasks.DataBind();
                            rptCampiTasksVisualizzabili.DataSource = andonCfg.FieldListTasks;
                            rptCampiTasksVisualizzabili.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptFieldsTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("tr1");
                if (tr != null)
                {
                    AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    andonCfg.loadCampiVisualizzatiTasks();
                    ImageButton imgUp = (ImageButton)e.Item.FindControl("imgTaskUp");
                    ImageButton imgDown = (ImageButton)e.Item.FindControl("imgTaskDown");
                    ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgTaskDelete");
                    HiddenField hKey = (HiddenField)e.Item.FindControl("idVFieldTaskName");

                    String chiave = hKey.Value.ToString();
                    var result = andonCfg.CampiVisualizzatiTasks.Where(x => x.Key == chiave).FirstOrDefault();
                    if (result.Key != null && result.Value == 0)
                    {
                        imgUp.Visible = false;
                    }
                    if (result.Key != null && result.Value == andonCfg.CampiVisualizzatiTasks.Count - 1)
                    {
                        imgDown.Visible = false;
                    }
                }
            }
        }

        protected void rptCampiTasksVisualizzabili_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("tr2");
                HiddenField hID = (HiddenField)e.Item.FindControl("idPFieldTaskName");
                if (tr != null && hID != null)
                {
                    String chiave = hID.Value.ToString();
                    AndonCompleto cfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    cfg.loadCampiVisualizzatiTasks();
                    //var trova = cfg.CampiVisualizzati.FirstOrDefault(x => x.Key == chiave);
                    if (cfg.CampiVisualizzatiTasks.ContainsKey(chiave))
                    {
                        tr.Visible = false;
                    }
                }
            }
        }

        protected void rptCampiTasksVisualizzabili_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ImageButton imgAdd = (ImageButton)e.Item.FindControl("imgTaskAdd");
            String hKey = e.CommandArgument.ToString();
            AndonCompleto andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
            if (hKey.Length > 0 && imgAdd != null)
            {
                if (e.CommandName == "Add")
                {
                    Boolean rt = andonCfg.addCampoVisualizzatoTasks(hKey);
                    if (rt)
                    {
                        lblInfo.Text = GetLocalResourceObject(hKey.ToString()).ToString() + " " + GetLocalResourceObject("lblAdded").ToString();
                        andonCfg.loadCampiVisualizzatiTasks();
                        rptFieldsTasks.DataSource = andonCfg.CampiVisualizzatiTasks;
                        rptFieldsTasks.DataBind();
                        rptCampiTasksVisualizzabili.DataSource = andonCfg.FieldListTasks;
                        rptCampiTasksVisualizzabili.DataBind();
                    }
                    else
                    {
                        lblInfo.Text = "Error. " + andonCfg.log;
                    }
                }
            }
        }
    }
}