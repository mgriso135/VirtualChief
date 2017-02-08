using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Operatori
{
    public partial class doTasksPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                User curr = (User)Session["User"];

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int pID = -1;
                    try
                    {
                        pID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        pID = -1;
                        lbl1.Text = "Wrong querystring.";
                    }

                    Postazione pst = new Postazione(pID);
                    

                    if (pst.id != -1)
                    {
                        bool controlloLogin = false;
                        pst.loadUtentiLoggati();
                        for (int i = 0; i < pst.UtentiLoggati.Count; i++)
                        {
                            if (pst.UtentiLoggati[i] == curr.username)
                            {
                                controlloLogin = true;
                            }
                        }

                        if (controlloLogin == true)
                        {
                            frmLstTaskAvviati.idPostazione = pst.id;
                            lblPostazione.Text = pst.name;
                            frmLstTaskAvviabili.idPostazione = pst.id;
                        }
                        else
                        {
                            lbl1.Text = "Attenzione: non hai eseguito il check-in nella postazione.";
                            frmLstTaskAvviabili.idPostazione = -1;
                            frmLstTaskAvviati.idPostazione = -1;
                            frmLstTaskAvviati.Visible = false;
                        }
                    }
                    else
                    {
                        frmLstTaskAvviati.idPostazione = -1;
                        frmLstTaskAvviati.Visible = false;
                        frmLstTaskAvviabili.idPostazione = -1;
                        lbl1.Text = "Postazione not found";
                    }
                }
                else
                {
                    frmLstTaskAvviati.idPostazione = -1;
                    frmLstTaskAvviati.Visible = false;
                    frmLstTaskAvviabili.idPostazione = -1;
                    frmLstTaskAvviabili.Visible = false;
                    lbl1.Text = "Querystring not found<br/>";
                }
            }
            else
            {
                frmLstTaskAvviati.idPostazione = -1;
                frmLstTaskAvviati.Visible = false;
                frmLstTaskAvviabili.idPostazione = -1;
                frmLstTaskAvviabili.Visible = false;
                lbl1.Text = "You're not logged in. Please <a href=\"/Login/login.aspx\">log in</a>.";
            }
        }
    }
}