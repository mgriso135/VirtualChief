using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Clienti
{
    public partial class listClienti : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rpt1.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Anagrafica Clienti";
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
                rpt1.Visible = true;
                if (!Page.IsPostBack)
                {
                    PortafoglioClienti elencoCli = new PortafoglioClienti();
                    rpt1.DataSource = elencoCli.Elenco;
                    rpt1.DataBind();
                }
            }
            else
            {
                rpt1.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rpt1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String codCliente = e.CommandArgument.ToString();
            if(e.CommandName == "delete")
            {
                Cliente customer = new Cliente(codCliente);
                if (customer.CodiceCliente.Length > 0)
                {
                    bool ret = customer.Delete();
                    if (ret == true)
                    {
                        lbl1.Text = GetLocalResourceObject("lblDeleteOk").ToString();
                        PortafoglioClienti elencoCli = new PortafoglioClienti();
                        rpt1.DataSource = elencoCli.Elenco;
                        rpt1.DataBind();
                    }
                    else
                    {
                        lbl1.Text =GetLocalResourceObject("lblDeleteKo").ToString()+ " " + customer.RagioneSociale+".";

                    }
                }
            }
        }

        protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");
                HiddenField hCodCliente = (HiddenField)e.Item.FindControl("hcodCliente");
                String codCliente = hCodCliente.Value.ToString();
                Cliente cli = new Cliente(codCliente);
                cli.loadCommesse(new DateTime(1970,1,1), (DateTime.Now.AddYears(10)));
                if(cli.listCommesse.Count == 0)
                {
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                }
            }
        }
    }
}