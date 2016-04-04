using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.Produzione
{
    public partial class addNewItemToProductionPlan : System.Web.UI.UserControl
    {
        public int repID;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
            if (repID != -1)
            {
                Reparto rp = new Reparto(repID);
                if (rp.id != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        rp.loadProcessiVarianti();
                        for (int i = 0; i < rp.processiVarianti.Count; i++)
                        {
                            ddlProcessiVarianti.Items.Add(new ListItem(rp.processiVarianti[i].process.processName.ToString() + " : " + rp.processiVarianti[i].variant.nomeVariante.ToString(), rp.processiVarianti[i].process.processID.ToString() + "," + rp.processiVarianti[i].variant.idVariante.ToString()));
                        }
                    }
                }
                else
                {
                    matricola.Visible = false;
                    imgSaveProduct.Visible = false;
                    ddlProcessiVarianti.Visible = false;
                }
            }
            else
            {
                matricola.Visible = false;
                imgSaveProduct.Visible = false;
                ddlProcessiVarianti.Visible = false;
            }
            
        }
            else
            {
                ddlProcessiVarianti.Visible = false;
                lbl1.Text = "Non hai il permesso di aggiungere un articolo in produzione.<br/>";
                imgSaveProduct.Visible = false;
                imgSaveProduct.Enabled = false;
            }
        }

        protected void imgSaveProduct_Click(object sender, EventArgs e)
        {
            
                Reparto rp = new Reparto(repID);
                if (rp.id != -1)
                {
                    if (matricola.Text.Length > 0)
                    {

                        String[] splitted = ddlProcessiVarianti.SelectedValue.Split(',');
                        int processID, varID;
                        try
                        {
                            processID = Int32.Parse(splitted[0]);
                            varID = Int32.Parse(splitted[1]);
                        }
                        catch
                        {
                            processID = -1;
                            varID = -1;
                        }
                        if (processID != -1 && varID != -1)
                        {
                            processo prc = new processo(processID);
                            variante vr = new variante(varID);
                            if (prc.processID != -1 && vr.idVariante != -1)
                            {
                                ProcessoVariante prvr = new ProcessoVariante(prc, vr);
                                if (prvr.process != null & prvr.variant != null)
                                {
                                    rp.loadProductionPlan();
                                    bool rt = rp.PianoProduzione.addProduct(matricola.Text, prvr);
                                    if (rt == true)
                                    {
                                        lbl1.Text = rp.PianoProduzione.err;
                                        //Response.Redirect(Request.RawUrl);
                                    }
                                    else
                                    {
                                        lbl1.Text = "Errore!!!" + rp.PianoProduzione.err;
                                    }
                                }
                            }
                        }
                    }

                }
            
            
        }
    }
}