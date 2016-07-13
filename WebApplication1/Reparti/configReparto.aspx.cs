using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;

namespace KIS.configReparto
{
    public partial class configReparto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                addReparti.Visible = false;
                configCadenza.Visible = false;
                configTurni.Visible = false;
                configSplitTasksTurni.Visible = false;
                configAnticipoMinimoTasks.Visible = false;
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
                listReparti.repID = repID;
                Reparto rep = new Reparto(repID);
                if (rep.id != -1)
                {
                    if (Page.IsPostBack)
                    {
                        lbl1.Text = Page.Request.Params.Get("__EVENTTARGET");
                    }
                    lblTitle.Text = rep.name;
                    lblDesc.Text = rep.description;
                    addReparti.Visible = false;
                    lnkShowAddReparti.Visible = false;
                    configCadenza.Visible = true;
                    configCadenza.repID = rep.id;
                    configTurni.Visible = true;
                    configTurni.repID = rep.id;
                    //configProcessiVarianti.Visible = true;
                    //configProcessiVarianti.repID = rep.id;
                    configSplitTasksTurni.Visible = true;
                    configSplitTasksTurni.idReparto = rep.id;
                    configAnticipoMinimoTasks.repID = rep.id;
                    //configAnticipoMinimoTasks.Visible = true;
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
                    frmAndonMaxDays.idReparto = rep.id;
                    frmAndonMaxDays.Visible = true;
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
                    lbl1.Text = "Non riesco a trovare il reparto che mi hai indicato.<br />";
                    lblTitle.Visible = false;
                    listReparti.Visible = false;
                    configCadenza.Visible = false;
                    configTurni.Visible = false;
                    titleConfig.Visible = false;
                    listReparti.repID = -1;
                    lnkShowAddReparti.Visible = false;
                    showAddReparti.Visible = false;
                    //configProcessiVarianti.Visible = false;
                    configSplitTasksTurni.Visible = false;
                    configSplitTasksTurni.idReparto = -1;
                    configAnticipoMinimoTasks.Visible = false;
                    configAnticipoMinimoTasks.repID = -1;
                    frmPersonale.Visible = false;
                    frmPersonale.idReparto = -1;
                    frmEvWarning.Visible = false;
                    frmEvWarning.idReparto = -1;
                    accordion1.Visible = false;
                    frmModoCalcoloTC.idReparto = -1;
                    frmModoCalcoloTC.Visible = false;
                    frmConfigAndonReparto.idReparto = -1;
                    frmConfigAndonReparto.Visible = false;
                    frmAvvioTask.idReparto = -1;
                    frmAvvioTask.Visible = false;
                    frmAndonMaxDays.idReparto = -1;
                    frmAndonMaxDays.Visible = false;
                    frmConfigKanban.Visible = false;
                    frmAndonViewFields.Visible = false;
                    frmConfigTimezone.Visible = false;
                }
            }
            else
            {
                listReparti.repID = -1;
                repID = -1;
                configTurni.Visible = false;
                configCadenza.Visible = false;
                titleConfig.Visible = false;
                //configProcessiVarianti.Visible = false;
                //configProcessiVarianti.repID = -1;
                configSplitTasksTurni.Visible = false;
                configSplitTasksTurni.idReparto = -1;
                configAnticipoMinimoTasks.Visible = false;
                configAnticipoMinimoTasks.repID = -1;
                frmPersonale.Visible = false;
                frmPersonale.idReparto = -1;
                frmEvRitardo.Visible = false;
                frmEvWarning.Visible = false;
                frmEvWarning.idReparto = -1;
                accordion1.Visible = false;
                frmModoCalcoloTC.idReparto = -1;
                frmModoCalcoloTC.Visible = false;
                frmConfigAndonReparto.idReparto = -1;
                frmConfigAndonReparto.Visible = false;
                frmAvvioTask.idReparto = -1;
                frmAvvioTask.Visible = false;
                frmAndonMaxDays.idReparto = -1;
                frmAndonMaxDays.Visible = false;
                frmConfigKanban.Visible = false;
                frmConfigTimezone.Visible = false;
            }

        }

        protected void showAddReparti_Click(object sender, ImageClickEventArgs e)
        {
            if (addReparti.Visible == false)
            {
                addReparti.Visible = true;
            }
            else
            {
                addReparti.Visible = false;
            }
        }

 
    }
}