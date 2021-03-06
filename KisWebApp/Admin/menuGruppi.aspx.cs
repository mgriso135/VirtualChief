using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.Menu;
using System.Web.UI.HtmlControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class menuGruppi : System.Web.UI.Page
    {
        public int gID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Gruppi Menu";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Gruppi Menu";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                gID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        gID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        gID = -1;
                    }
                }
                else
                {
                    lblGruppo.Visible = false;
                }

                if (gID != -1)
                {
                    Group grp = new Group(gID);
                    frmListVoci.idGruppo = grp.ID;
                    frmAddVoce.idGruppo = grp.ID;
                    if (!Page.IsPostBack)
                    {
                        if (grp.ID != -1)
                        {
                            lblGruppo.Text = grp.Nome;
                        }
                    }
                }
                else
                {
                    lblGruppo.Visible = false;
                    frmListVoci.idGruppo = -1;
                    frmListVoci.Visible = false;
                    frmAddVoce.Visible = false;
                    frmAddVoce.idGruppo = -1;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmListVoci.idGruppo = -1;
                frmAddVoce.Visible = false;
                frmAddVoce.idGruppo = -1;
            }
        }




    }
}