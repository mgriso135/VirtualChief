using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.Produzione
{
    public partial class productionPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int repID;
                try
                {
                    repID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    repID = -1;
                    showProductionPlan.Visible = false;
                    addNewItemForm.Visible = false;
                    showProductionPlan.repID = -1;
                    showAddItemForm.Visible = false;
                    hideAddItemForm.Visible = false;
                }
                if (repID != -1)
                {
                    Reparto current = new Reparto(repID);
                    lblTitle.Text = "Programma di produzione del reparto " + current.name;
                    if (current.id != -1)
                    {
                        if (!Page.IsPostBack)
                        {
                            addNewItemForm.Visible = false;
                            hideAddItemForm.Visible = false;
                            
                        }
                    }
                }
            }
            else
            {
                showProductionPlan.Visible = false;
                addNewItemForm.Visible = false;
                showProductionPlan.repID = -1;
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