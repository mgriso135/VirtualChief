using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Operatori
{
    public partial class doTasksPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                User curr = (User)Session["user"];

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

                    Postazione pst = new Postazione(Session["ActiveWorkspace_Name"].ToString(), pID);
                    

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
                         //   frmLstTaskAvviati.idPostazione = pst.id;
                            lblPostazione.Text = pst.name;
                            //frmLstTaskAvviabili.idPostazione = pst.id;
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrorCheckIn").ToString();
                       //     frmLstTaskAvviabili.idPostazione = -1;
                       //     frmLstTaskAvviati.idPostazione = -1;
                       //     frmLstTaskAvviati.Visible = false;
                        }
                    }
                    else
                    {
                   //     frmLstTaskAvviati.idPostazione = -1;
                   //     frmLstTaskAvviati.Visible = false;
                        //frmLstTaskAvviabili.idPostazione = -1;
                        lbl1.Text = GetLocalResourceObject("lblPostazioneNotFound").ToString();
                    }
                }
                else
                {
               //     frmLstTaskAvviati.idPostazione = -1;
                 //   frmLstTaskAvviati.Visible = false;
                 //   frmLstTaskAvviabili.idPostazione = -1;
                 //   frmLstTaskAvviabili.Visible = false;
                    lbl1.Text = "Querystring not found<br/>";
                }
            
            }
            else
            {
                if (!Page.IsPostBack) { 
            //    frmLstTaskAvviati.idPostazione = -1;
            //    frmLstTaskAvviati.Visible = false;
            //    frmLstTaskAvviabili.idPostazione = -1;
              //  frmLstTaskAvviabili.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblNotLoggedIn1").ToString()
                    + " <a href=\"/Login/login.aspx\">"+ GetLocalResourceObject("lblNotLoggedIn2").ToString() + "</a>.";
                }
            }
        }
    }
}