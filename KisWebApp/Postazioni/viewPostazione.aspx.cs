using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Postazioni
{
    public partial class viewPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmPstRisorse.Visible = false;
            frmEditPostazione.Visible = false;
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
                    lbl1.Text = GetLocalResourceObject("lblErrorInputData").ToString();
                    viewCalendarioPostazione.Visible = false;
                }
                if (pstID != -1)
                {
                    Postazione pst = new Postazione(pstID);
                    lblNomePost.Text = pst.name;
                    lblDescPost.Text = pst.desc;
                    viewCalendarioPostazione.idPostazione = pst.id;

                    frmPstRisorse.Visible = true;
                    frmPstRisorse.idPostazione = pst.id;

                    frmEditPostazione.Visible = true;
                    frmEditPostazione.pstID = pst.id;
                }
                else
                {
                    lblDescPost.Visible = false;
                    lblNomePost.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblErrorInputData").ToString();
                    viewCalendarioPostazione.Visible = false;
                    viewCalendarioPostazione.idPostazione = -1;
                    frmPstRisorse.Visible = false;

                    frmEditPostazione.Visible = false;
                    frmEditPostazione.pstID = -1;
                }

            }
            else
            {
                lblDescPost.Visible = false;
                lblNomePost.Visible = false;
                viewCalendarioPostazione.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblErrorInputData").ToString();
                viewCalendarioPostazione.idPostazione = -1;
                frmPstRisorse.Visible = false;

                frmEditPostazione.Visible = false;
                frmEditPostazione.pstID = -1;
            }
        }
    }
}