using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Users
{

    public partial class listPermessiGruppi : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Gruppi Permessi";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Gruppi Permessi";
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
                int GroupID;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        GroupID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        GroupID = -1;
                    }
                }
                else
                {
                    GroupID = -1;
                }

                if (GroupID != -1 && !Page.IsPostBack)
                {
                    Group grp = new Group(GroupID);
                    if (grp.ID != -1)
                    {
                        lblNome.Text = grp.Nome;
                        GroupPermissions grpPerm = new GroupPermissions(grp.ID);
                        rptPermessi.DataSource = grpPerm.Elenco;
                        rptPermessi.DataBind();
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptPermessi.Visible = false;
            }
            
        }
        

        protected void rptPermessi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }


        protected void ckR_CheckedChanged(object sender, EventArgs e)
        {
            int GroupID = -1;
            int permID = -1;
            CheckBox chk = (CheckBox)sender;
            HiddenField idPermF = (HiddenField)chk.Parent.FindControl("idPerm");

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    GroupID = Int32.Parse(Request.QueryString["id"]);
                    permID = Int32.Parse(idPermF.Value.ToString());
                }
                catch
                {
                    GroupID = -1;
                    permID = -1;
                }
            }
            else
            {
                GroupID = -1;
                permID = -1;
            }

            if (GroupID != -1 && permID != -1)
            {
                GroupPermission grpPrm = new GroupPermission(GroupID, new Permission(permID));
                grpPrm.R = chk.Checked;
            }
        }

        protected void ckW_CheckedChanged(object sender, EventArgs e)
        {
            int GroupID = -1;
            int permID = -1;
            CheckBox chk = (CheckBox)sender;
            HiddenField idPermF = (HiddenField)chk.Parent.FindControl("idPerm");

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    GroupID = Int32.Parse(Request.QueryString["id"]);
                    permID = Int32.Parse(idPermF.Value.ToString());
                }
                catch
                {
                    GroupID = -1;
                    permID = -1;
                }
            }
            else
            {
                GroupID = -1;
                permID = -1;
            }

            if (GroupID != -1 && permID != -1)
            {
                GroupPermission grpPrm = new GroupPermission(GroupID, new Permission(permID));
                grpPrm.W = chk.Checked;
            }
        }

        protected void ckX_CheckedChanged(object sender, EventArgs e)
        {
            int GroupID = -1;
            int permID = -1;
            CheckBox chk = (CheckBox)sender;
            HiddenField idPermF = (HiddenField)chk.Parent.FindControl("idPerm");

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    GroupID = Int32.Parse(Request.QueryString["id"]);
                    permID = Int32.Parse(idPermF.Value.ToString());
                }
                catch
                {
                    GroupID = -1;
                    permID = -1;
                }
            }
            else
            {
                GroupID = -1;
                permID = -1;
            }

            lbl1.Text = GroupID.ToString() + " " + permID.ToString();
            if (GroupID != -1 && permID != -1)
            {
                GroupPermission grpPrm = new GroupPermission(GroupID, new Permission(permID));
                grpPrm.X = chk.Checked;
            }

        }
    }
}