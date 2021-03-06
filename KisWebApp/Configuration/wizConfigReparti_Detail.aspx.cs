using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Configuration
{
    public partial class wizConfigReparti_Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            configTurni.Visible = false;
            configTurni.repID = -1;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    //configTurni.Visible = false;
                    //configSplitTasksTurni.Visible = false;
                    accordion1.Visible = false;
                }

                int repID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        repID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        repID = -1;
                    }
                    Reparto rep = new Reparto(Session["ActiveWorkspace_Name"].ToString(), repID);
                    if (rep.id != -1)
                    {
                        if (Page.IsPostBack)
                        {
                            //lbl1.Text = Page.Request.Params.Get("__EVENTTARGET");
                        }
                        lblTitle.Text = rep.name;
                        lblDesc.Text = rep.description;
                        accordion1.Visible = true;
                        configTurni.Visible = true;
                        configTurni.repID = rep.id;
                        configSplitTasksTurni.Visible = true;
                        configSplitTasksTurni.idReparto = rep.id;
                        frmEvRitardo.Visible = true;
                        frmEvRitardo.idReparto = rep.id;
                        frmEvWarning.Visible = true;
                        frmEvWarning.idReparto = rep.id;                        
                        frmModoCalcoloTC.idReparto = rep.id;
                        frmModoCalcoloTC.Visible = true;
                        frmConfigAndonReparto.idReparto = rep.id;
                        frmConfigAndonReparto.Visible = true;
                        frmAvvioTask.idReparto = rep.id;
                        frmAvvioTask.Visible = true;
                        frmAndonViewFields.Visible = true;
                        frmAndonViewFields.idReparto = rep.id;

                        // Abilito la gestione a kanban solo se è attivo KanbanBox by Sintesia
                        /*                    frmConfigKanban.idReparto = rep.id;
                                            KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                                            if (kboxCfg.KanbanBoxEnabled == true)
                                            {
                                                frmConfigKanban.Visible = true;
                                            }
                                            else
                                            {
                                                frmConfigKanban.Visible = false;
                                            }*/
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblDeptNotFound").ToString()+ ".<br />";
                        lblTitle.Visible = false;
                        accordion1.Visible = false;
                        configTurni.Visible = false;
                        titleConfig.Visible = false;
                        configSplitTasksTurni.Visible = false;
                        configSplitTasksTurni.idReparto = -1;
                        frmEvWarning.Visible = false;
                        frmEvWarning.idReparto = -1;
                        frmModoCalcoloTC.idReparto = -1;
                        frmModoCalcoloTC.Visible = false;
                        frmConfigAndonReparto.idReparto = -1;
                        frmConfigAndonReparto.Visible = false;
                        frmAvvioTask.idReparto = -1;
                        frmAvvioTask.Visible = false;
                        frmAndonViewFields.Visible = false;
                    }
                }
                else
                {
                    repID = -1;
                    titleConfig.Visible = false;
                    accordion1.Visible = false;
                    configTurni.Visible = false;
                    configSplitTasksTurni.Visible = false;
                    configSplitTasksTurni.idReparto = -1;
                    frmEvRitardo.Visible = false;
                    frmEvWarning.Visible = false;
                    frmEvWarning.idReparto = -1;
                    frmModoCalcoloTC.idReparto = -1;
                    frmModoCalcoloTC.Visible = false;
                    frmConfigAndonReparto.idReparto = -1;
                    frmConfigAndonReparto.Visible = false;
                    frmAvvioTask.idReparto = -1;
                    frmAvvioTask.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "<a href=\"../Login/login.aspx"
                    + "?red=/Configuration/wizConfigReparti_Main.aspx\">"
                    + GetLocalResourceObject("lblLnkLogin").ToString()
                    +".</a>";
                accordion1.Visible = false;
                titleConfig.Visible = false;
                configTurni.Visible = false;
                configSplitTasksTurni.Visible = false;
                configSplitTasksTurni.idReparto = -1;
                frmEvRitardo.Visible = false;
                frmEvWarning.Visible = false;
                frmEvWarning.idReparto = -1;                
                frmModoCalcoloTC.idReparto = -1;
                frmModoCalcoloTC.Visible = false;
                frmConfigAndonReparto.idReparto = -1;
                frmConfigAndonReparto.Visible = false;
                frmAvvioTask.idReparto = -1;
                frmAvvioTask.Visible = false;
            }
        }
    }
}