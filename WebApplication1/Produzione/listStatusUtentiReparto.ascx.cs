using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.Commesse;

namespace KIS.Produzione
{
    public partial class listStatusUtentiReparto : System.Web.UI.UserControl
    {
        public int repID;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {*/
            Reparto rp = new Reparto(repID);
            if (!Page.IsPostBack && !Page.IsCallback)
                {
                    UserList usrList = new UserList(new Permesso("Task Produzione"));
                    if (usrList.listUsers.Count > 0)
                    {
                        rptUserList.DataSource = usrList.listUsers;
                        rptUserList.DataBind();
                }
                    else
                    {
                        rptUserList.Visible = false;
                        lblData.Visible = false;
                    }
                }
            /*}
            else
            {
                lblUser.Text = "Non hai il permesso.<br/>";
            }*/
        }


        protected void rptUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblUtentiAttivi = (Label)e.Item.FindControl("lblUtente");
                HiddenField lblUsername = (HiddenField)e.Item.FindControl("lblIDUser");

                if (lblUsername != null)
                {
                    

                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        Reparto rp = new Reparto(repID);
                        User curr = new User(lblUsername.Value.ToString());
                        curr.loadTaskAvviati();

                        //char andonConfig = (new AndonCompleto()).PostazioniFormatoUsername;
                        char andonConfig = rp.AndonPostazioniFormatoUsername;

                        if (andonConfig == '0')
                        {
                            lblUtentiAttivi.Text = curr.username;
                        }
                        else if (andonConfig == '1')
                        {
                            lblUtentiAttivi.Text = curr.name;
                        }
                        else if (andonConfig == '2')
                        {
                            lblUtentiAttivi.Text = curr.name + " " + curr.cognome.Substring(0, 1);
                        }
                        else
                        {
                            lblUtentiAttivi.Text = curr.name + " " + curr.cognome;
                        }

                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFD700'");
                        if (curr.TaskAvviati.Count == 0)
                        {
                            tRow.BgColor = "#FFFFFF";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                        else
                        {
                            tRow.BgColor = "#00FF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                        }

                        


                    }

                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            Reparto rp = new Reparto(repID);
            UserList usrList = new UserList(new Permesso("Task Produzione"));
            if (usrList.listUsers.Count > 0)
            {
                rptUserList.DataSource = usrList.listUsers;
                rptUserList.DataBind();
            }
            else
            {
                rptUserList.Visible = false;
                lblData.Visible = false;
            }
        }
   
    }
}