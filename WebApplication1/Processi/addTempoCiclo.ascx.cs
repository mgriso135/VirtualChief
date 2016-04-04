using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Processi
{
    public partial class addTempoCiclo : System.Web.UI.UserControl
    {
        public TaskVariante prc;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (prc != null && prc.Task != null && prc.variant != null)
                {
                    taskID.Value = prc.Task.processID.ToString();
                    varID.Value = prc.variant.idVariante.ToString();
                    //if (!Page.IsPostBack)
                    {
                        /*secondi.Items.Clear();
                        minuti.Items.Clear();
                        ddlNumOp.Items.Clear();*/
                        for (int i = 0; i < 60; i++)
                        {
                            secondi.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            minuti.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            minSetup.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            secSetup.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        for (int i = 0; i < 500; i++)
                        {
                            ore.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            oreSetup.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        for (int i = 1; i <= 20; i++)
                        {
                            ddlNumOp.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        //frmAddTempo.Visible = false;
                    }
                }
            }
            else
            {
                lbl1.Text = "<br />Non hai il permesso di aggiungere tempi ciclo.<br/>";
                frmAddTempo.Visible = false;
                imgShowFrmAddTempo.Visible = false;
            }
        }

        protected void imgShowFrmAddTempo_Click(object sender, ImageClickEventArgs e)
        {
            
            if (frmAddTempo.Visible == false)
            {
                frmAddTempo.Visible = true;
                btnSave.Focus();
            }
            else
            {
                frmAddTempo.Visible = false;
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            ddlNumOp.SelectedValue = "1";
            ore.SelectedValue = "0";
            minuti.SelectedValue = "0";
            secondi.SelectedValue = "0";
            chkDefault.Checked = true;
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            
            int n_ops = -1;
            int sore, sminuti, ssecondi, soreSetup, sminSetup, ssecSetup;
            int task = -1;
            int varianteID = -1;

            lbl1.Text = ore.SelectedValue.ToString() + ":"
                + minuti.SelectedValue.ToString() + ":"
                + secondi.SelectedValue.ToString() + "<br />";

            try
            {
                n_ops = Int32.Parse(ddlNumOp.SelectedValue.ToString());
                sore = Int32.Parse(ore.SelectedValue.ToString());
                sminuti = Int32.Parse(minuti.SelectedValue.ToString());
                ssecondi = Int32.Parse(secondi.SelectedValue.ToString());
                task = Int32.Parse(taskID.Value);
                varianteID = Int32.Parse(varID.Value);

                soreSetup = Int32.Parse(oreSetup.SelectedValue.ToString());
                sminSetup = Int32.Parse(minSetup.SelectedValue.ToString());
                ssecSetup = Int32.Parse(secSetup.SelectedValue.ToString());
            }
            catch
            {
                n_ops = -1;
                sore = -1;
                sminuti = -1;
                ssecondi = -1;
                task = -1;
                varianteID = -1;
                lbl1.Text += "Errore.<br/>";
                soreSetup = -1;
                sminSetup = -1;
                ssecSetup = -1;
            }

            if (n_ops != -1 && task!=-1 && varianteID!=-1)
            {
                TaskVariante tsk = new TaskVariante(new processo(task), new variante(varianteID));
                TimeSpan tc = new TimeSpan(sore, sminuti, ssecondi);
                TimeSpan tSetup = new TimeSpan(soreSetup, sminSetup, ssecSetup);
                // Controllo che il numero di operatori inserito non sia già presente
                bool check = false;
                TempiCiclo ElencoTc = new TempiCiclo(tsk.Task.processID, tsk.Task.revisione, tsk.variant.idVariante);
                for (int i = 0; i < ElencoTc.Tempi.Count; i++)
                {
                    if (ElencoTc.Tempi[i].NumeroOperatori == n_ops)
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    // Aggiungo il tempo ciclo
                    bool rt = ElencoTc.Add(n_ops, tc, tSetup, chkDefault.Checked);
                    if (rt == false)
                    {
                        lbl1.Text += ElencoTc.log;
                    }
                    else
                    {
                        lbl1.Text = "Tempo ciclo aggiunto correttamente.<br />Ricaricare per visualizzarlo nell'elenco.";
                        //btnSave.Focus();
                    }
                }
            }
        }
    }
}