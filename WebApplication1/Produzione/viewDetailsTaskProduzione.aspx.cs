﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Produzione
{
    public partial class viewDetailsTaskProduzione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    taskID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    taskID = -1;
                }
                if (taskID != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(taskID);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        frmViewDetails.taskID = tsk.TaskProduzioneID;
                    }
                    else
                    {
                        lbl1.Text = "Task non trovato.";
                        frmViewDetails.Visible = false;
                    }
                }
                else
                {
                    lbl1.Text = "Errore nella conversione dell'id.";
                    frmViewDetails.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "ID non trovato.<br/>";
                frmViewDetails.Visible = false;
            }
        }
    }
}