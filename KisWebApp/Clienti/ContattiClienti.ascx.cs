using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Clienti
{
    public partial class ContattiClienti : System.Web.UI.UserControl
    {
        public String idCliente;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Anagrafica Clienti Contatti";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                rptContattiClienti.Visible = false;
                if (!String.IsNullOrEmpty(idCliente) && idCliente.Length > 0)
                {
                    Cliente cln = new Cliente(Session["ActiveWorkspace"].ToString(), idCliente);
                    if (cln.CodiceCliente.Length > 0)
                    {
                        rptContattiClienti.Visible = true;
                        if (!Page.IsPostBack)
                        {
                            rptContattiClienti.DataSource = cln.ElencoContatti;
                            rptContattiClienti.DataBind();
                        }
                    }

                }
            }
        }

        protected void rptContattiClienti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblPhone = (Label)e.Item.FindControl("phone");
                Label lblEmail = (Label)e.Item.FindControl("email");
                HiddenField hID = (HiddenField)e.Item.FindControl("hID");

                int ID = -1;
                try
                {
                    ID = Int32.Parse(hID.Value);
                }
                catch
                {
                    ID = -1;
                }

                if (ID != -1 && lblPhone != null && lblEmail != null)
                {
                    Contatto contCln = new Contatto(Session["ActiveWorkspace"].ToString(), ID);
                    if (contCln.ID != -1)
                    {
                        String strPhone ="", strEmail="";
                        for (int i = 0; i < contCln.Phones.Count; i++)
                        {
                            strPhone += contCln.Phones[i].Phone;
                            if (i < contCln.Phones.Count - 1)
                            {
                                strPhone += ", ";
                            }
                        }

                        for (int i = 0; i < contCln.Emails.Count; i++)
                        {
                            strEmail += contCln.Emails[i].Email.Address;
                            if (i < contCln.Emails.Count - 1)
                            {
                                strEmail += ", ";
                            }
                        }

                        lblPhone.Text = strPhone;
                        lblEmail.Text = strEmail;
                    }
                }
            }
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {

        }

        protected void rptContattiClienti_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idContatto = -1;
            try
            {
                idContatto = Int32.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                idContatto = -1;
            }
            if(e.CommandName=="delete")
            {
                Contatto person = new Contatto(Session["ActiveWorkspace"].ToString(), idContatto);
                if (person.ID != -1)
                {
                    person.Delete();
                    lbl1.Text = "";
                    Cliente cln = new Cliente(Session["ActiveWorkspace"].ToString(), person.Cliente);
                    cln.loadContatti();
                    rptContattiClienti.Visible = true;
                    rptContattiClienti.DataSource = cln.ElencoContatti;
                    rptContattiClienti.DataBind();
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblError").ToString();
                }
            }
        }
    }
}