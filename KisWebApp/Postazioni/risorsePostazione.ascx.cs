using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Postazioni
{
    public partial class risorsePostazione : System.Web.UI.UserControl
    {
        public int idPostazione;
        protected void Page_Load(object sender, EventArgs e)
        {
            rowBody.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Turno PostazioneRisorse";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), idPostazione);
                if (pst.id != -1)
                {
                    rowBody.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        pst.loadReparti();
                        List<Turno> elTurni = new List<Turno>();
                        for (int i = 0; i < pst.ElencoIDReparti.Count; i++)
                        {
                            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), pst.ElencoIDReparti[i]);
                            rp.loadTurni();
                            for (int j = 0; j < rp.Turni.Count; j++)
                            {
                                elTurni.Add(rp.Turni[j]);
                            }
                        }
                        rptTurni.DataSource = elTurni;                        
                        rptTurni.DataBind();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblPostazioneNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptTurni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label txtNomeReparto = (Label)e.Item.FindControl("lblNomeReparto");
                DropDownList ddlRisorse = (DropDownList)e.Item.FindControl("ddlRisorse");
                HiddenField hTurnoID = (HiddenField)e.Item.FindControl("hIDTurno");
                if (ddlRisorse != null && hTurnoID != null)
                {
                    int idTurno = -1;
                    try
                    {
                        idTurno = Int32.Parse(hTurnoID.Value.ToString());
                    }
                    catch
                    {
                        idTurno = -1;
                    }
                    if (idTurno != -1)
                    {
                        Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), idPostazione);
                        if (pst != null && pst.id != -1)
                        {
                            for (int i = 0; i <= 1000; i++)
                            {
                                ddlRisorse.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            }
                            Turno turno = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
                            RisorsePostazioneTurno resPost = new RisorsePostazioneTurno(Session["ActiveWorkspace"].ToString(), pst, turno);
                            if (resPost != null && resPost.postazione != null && resPost.postazione.id != -1 && resPost.turno != null && resPost.turno.id != -1)
                            {
                                ddlRisorse.SelectedValue = resPost.NumRisorse.ToString();
                            }

                            Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), turno.idReparto);
                            txtNomeReparto.Text = rep.name;
                        }
                    }
                }
            }
        }

        protected void ddlRisorse_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlRisorse = (DropDownList)sender;
            HiddenField hTurno = (HiddenField)ddlRisorse.Parent.FindControl("hIDTurno");
            int idTurno = -1;
            int numRes = -1;
            try
            {
                idTurno = Int32.Parse(hTurno.Value);
                numRes = Int32.Parse(ddlRisorse.SelectedValue);
            }
            catch(Exception ex)
            {
                idTurno = -1;
                numRes = -1;
            }

            if (idTurno != -1 && numRes != -1 && idPostazione != -1)
            {
                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), idPostazione);
                if (pst.id != -1)
                {
                    Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
                    if (trn.id != -1)
                    {
                        RisorsePostazioneTurno resPost = new RisorsePostazioneTurno(Session["ActiveWorkspace"].ToString(), pst, trn);
                        resPost.NumRisorse = numRes;
                        ddlRisorse.SelectedValue = resPost.NumRisorse.ToString();
                        lbl1.Text = GetLocalResourceObject("lblAggRisorse").ToString() + " (" + resPost.NumRisorse.ToString()
                            + ") "+ GetLocalResourceObject("lblAggRisorse2").ToString() +" " + pst.name + ", turno " + trn.Nome
                            + " " + GetLocalResourceObject("lblAggRisorse3").ToString();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblTurnoNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblPostazioneNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
            }

                
        }
    }
}