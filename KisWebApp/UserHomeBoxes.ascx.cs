using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.Web.UI.HtmlControls;

namespace KIS.HomePage
{
    public partial class UserHomeBoxes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            tblOptions.Visible = false;
            if (Session["user"] != null)
            {
                User utente = (User)Session["user"];
                if (utente !=null && utente.username.Length > 0)
                {
                    if (!Page.IsPostBack)
                    {
                        HomeBoxesListUser boxList = new HomeBoxesListUser(Session["ActiveWorkspace_Name"].ToString(), utente);
                        if (boxList.Elenco.Count > 0)
                        {
                            tblOptions.Visible = true;
                            rptUL.DataSource = boxList.Elenco;
                            rptUL.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptUL_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hBoxID = (HiddenField)e.Item.FindControl("boxID");
                HtmlGenericControl lItem = (HtmlGenericControl)e.Item.FindControl("li1");

                int boxID = -1;
                try
                {
                    boxID = Int32.Parse(hBoxID.Value.ToString());
                }
                catch
                {
                    boxID = -1;
                }

                if (boxID != -1)
                {
                    HomeBox box = new HomeBox(Session["ActiveWorkspace_Name"].ToString(), boxID);
                    if (box.ID != -1)
                    {
                        //lItem.Text = box.Nome;
                        var controllo = LoadControl(box.Path);
                        lItem.Controls.Add(controllo);
                    }
                }
            }
        }
    }
}