using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Postazioni
{
    public partial class viewPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmPstRisorse.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["pstID"]))
            {
                int pstID = -1;
                try
                {
                    pstID = Int32.Parse(Request.QueryString["pstID"]);
                }
                catch
                {
                    pstID = -1;
                    lbl1.Text = "Errore nei dati di input<br/>";
                    viewCalendarioPostazione.Visible = false;
                }
                if (pstID != -1)
                {
                    Postazione pst = new Postazione(pstID);
                    lblNomePost.Text = pst.name;
                    lblDescPost.Text = pst.desc;
                    viewCalendarioPostazione.idPostazione = pst.id;
                    frmPstWorkload.Visible = true;
                    frmPstWorkload.postID = pst.id;
                    frmPstRisorse.Visible = true;
                    frmPstRisorse.idPostazione = pst.id;
                }
                else
                {
                    frmPstWorkload.Visible = false;
                    lblDescPost.Visible = false;
                    lblNomePost.Visible = false;
                    lbl1.Text = "Errore nei dati di input<br/>";
                    viewCalendarioPostazione.Visible = false;
                    viewCalendarioPostazione.idPostazione = -1;
                    frmPstRisorse.Visible = false;
                }

            }
            else
            {
                lblDescPost.Visible = false;
                lblNomePost.Visible = false;
                viewCalendarioPostazione.Visible = false;
                lbl1.Text = "Errore nei dati di input<br/>";
                viewCalendarioPostazione.idPostazione = -1;
                frmPstRisorse.Visible = false;
            }
        }
    }
}