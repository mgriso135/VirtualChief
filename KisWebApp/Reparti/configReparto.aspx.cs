using System;
using System.Web.UI;
using KIS.App_Code;

namespace KIS.configReparto
{
    public partial class configReparto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                configCadenza.Visible = false;
                configSplitTasksTurni.Visible = false;
                configTurni.Visible = false;                
                frmPersonale.Visible = false;
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
                Reparto rep = new Reparto(repID);
                if (rep.id != -1)
                {
                    if (Page.IsPostBack)
                    {
                        lbl1.Text = Page.Request.Params.Get("__EVENTTARGET");
                    }
                    DepartmentID.Value = rep.id.ToString();
                    lblTitle.Text = rep.name;
                    lblDesc.Text = rep.description;
                    configCadenza.Visible = true;
                    configCadenza.repID = rep.id;
                    configSplitTasksTurni.Visible = true;
                    configSplitTasksTurni.idReparto = rep.id;
                    configTurni.Visible = true;
                    configTurni.repID = rep.id;
                    frmPersonale.Visible = true;
                    frmPersonale.idReparto = rep.id;
                    frmEvRitardo.Visible = true;
                    frmEvRitardo.idReparto = rep.id;
                    frmEvWarning.Visible = true;
                    frmEvWarning.idReparto = rep.id;
                    accordion1.Visible = true;
                    frmModoCalcoloTC.idReparto = rep.id;
                    frmModoCalcoloTC.Visible = true;
                    frmConfigAndonReparto.idReparto = rep.id;
                    frmConfigAndonReparto.Visible = true;
                    frmAvvioTask.idReparto = rep.id;
                    frmAvvioTask.Visible = true;
                    frmAndonViewFields.Visible = true;
                    frmAndonViewFields.idReparto = rep.id;
                    frmConfigTimezone.idReparto = rep.id;
                    frmConfigTimezone.Visible = true;
                    
                    // Abilito la gestione a kanban solo se è attivo KanbanBox by Sintesia
                    frmConfigKanban.idReparto = rep.id;
                    KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                    if (kboxCfg.KanbanBoxEnabled == true)
                    {
                        frmConfigKanban.Visible = true;
                    }
                    else
                    {
                        frmConfigKanban.Visible = false;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorRepNotFound").ToString();
                    lblTitle.Visible = false;
                    accordion1.Visible = false;
                    titleConfig.Visible = false;
                    configCadenza.Visible = false;
                    configSplitTasksTurni.Visible = false;
                    configSplitTasksTurni.idReparto = -1;
                    configTurni.Visible = false;
                    frmPersonale.Visible = false;
                    frmPersonale.idReparto = -1;
                    frmEvWarning.Visible = false;
                    frmEvWarning.idReparto = -1;                    
                    frmModoCalcoloTC.idReparto = -1;
                    frmModoCalcoloTC.Visible = false;
                    frmConfigAndonReparto.idReparto = -1;
                    frmConfigAndonReparto.Visible = false;
                    frmAvvioTask.idReparto = -1;
                    frmAvvioTask.Visible = false;
                    frmAndonMaxDays.Visible = false;
                    frmConfigKanban.Visible = false;
                    frmAndonViewFields.Visible = false;
                    frmConfigTimezone.Visible = false;
                }
            }
            else
            {
                repID = -1;
                titleConfig.Visible = false;
                accordion1.Visible = false;
                configCadenza.Visible = false;
                configSplitTasksTurni.Visible = false;
                configSplitTasksTurni.idReparto = -1;
                configTurni.Visible = false;                              
                frmPersonale.Visible = false;
                frmPersonale.idReparto = -1;
                frmEvRitardo.Visible = false;
                frmEvWarning.Visible = false;
                frmEvWarning.idReparto = -1;                
                frmModoCalcoloTC.idReparto = -1;
                frmModoCalcoloTC.Visible = false;
                frmConfigAndonReparto.idReparto = -1;
                frmConfigAndonReparto.Visible = false;
                frmAvvioTask.idReparto = -1;
                frmAvvioTask.Visible = false;
                frmAndonMaxDays.Visible = false;
                frmConfigKanban.Visible = false;
                frmConfigTimezone.Visible = false;
            }

        }
 
    }
}