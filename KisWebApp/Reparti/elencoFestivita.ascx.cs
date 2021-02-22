using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Reparti
{
    public partial class elencoFestivita : System.Web.UI.UserControl
    {
        public int idTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Festivita";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idTurno != -1)
                {
                    Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
                    if (trn.id != -1)
                    {
                        if (!Page.IsPostBack)
                        {
                            //ElencoFestivita elFest = new ElencoFestivita(rp.id);
                            trn.loadFestivita();
                            rptFest.DataSource = trn.festivita;
                            rptFest.DataBind();
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptFest.Visible = false;
            }
        }

        protected void rptFest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

        protected void rptFest_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
            if (trn.idReparto != -1)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), trn.idReparto);
                if (rp.id != -1)
                {
                    if (e.CommandName == "deleteFest")
                    {
                        int idFs = -1;
                        try
                        {
                            idFs = Int32.Parse(e.CommandArgument.ToString());
                        }
                        catch
                        {
                            idFs = -1;
                        }
                        if (idFs != -1)
                        {
                            Festivita fs = new Festivita(Session["ActiveWorkspace"].ToString(), idFs);
                            if (fs.idFestivita != -1)
                            {
                                bool rt = fs.delete();
                                if (rt == true)
                                {
                                    Response.Redirect(Request.RawUrl);
                                }
                                else
                                {
                                    lbl1.Text = fs.log;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}