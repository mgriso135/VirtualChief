using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class productionPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int procID = Int32.Parse(Request.QueryString["id"]);
                processo current = new processo(procID);
                int checkConsistency = current.checkConsistencyPERT();
                if (checkConsistency == 1)
                {
                    showProductionPlan.processID = Int32.Parse(Request.QueryString["id"]);
                    addNewItemForm.origProcID = Int32.Parse(Request.QueryString["id"]);
                    addNewItemForm.Visible = false;
                    hideAddItemForm.Visible = false;
                }
                else
                {
                    lbl.Text = "Error: " + checkConsistency.ToString() + "<br/>";
                    if (checkConsistency == 0)
                    {
                        lbl.Text += "Generic error.";
                    }
                    else if (checkConsistency == 2)
                    {
                        lbl.Text += "Almeno un task non è collegato allo stream, cioè non ha né precedenti né successivi. Verifica la definizione del processo.";
                    }
                    else if (checkConsistency == 3)
                    {
                        lbl.Text += "Sei finito qui per errore, il tipo di processo non è un PERT.";
                    }
                    else if (checkConsistency == 4)
                    {
                        lbl.Text += "Al processo principale manca il KPI \"Cadenza\", oppure il suo baseValue è = 0";
                    }
                    else if (checkConsistency == 5)
                    {
                        lbl.Text += "Ad almeno un task manca il KPI \"Tempo ciclo\" oppure il suo baseValue è = 0";
                    }
                    else if (checkConsistency == 6)
                    {
                        lbl.Text += "Ad almeno un task manca il process owner.";
                    }
                    showProductionPlan.Visible = false;
                    addNewItemForm.Visible = false;
                    showProductionPlan.processID = -1;
                    showAddItemForm.Visible = false;
                    hideAddItemForm.Visible = false;
                }
            }
            else
            {
                showProductionPlan.Visible = false;
                addNewItemForm.Visible = false;
                showProductionPlan.processID = -1;
                showAddItemForm.Visible = false;
                hideAddItemForm.Visible = false;
            }
        }

        protected void showAddItemForm_Click(object sender, EventArgs e)
        {
            addNewItemForm.Visible = true;
            hideAddItemForm.Visible = true;
            showAddItemForm.Visible = false;
        }

        protected void hideAddItemForm_Click(object sender, EventArgs e)
        {
            addNewItemForm.Visible = false;
            showAddItemForm.Visible = true;
            hideAddItemForm.Visible = false;
        }
    }
}