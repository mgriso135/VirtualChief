using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Personal
{
    public partial class baseInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack)
                {
                    User curr = (User)Session["user"];
                    imgEditNome.Visible = true;
                    imgSaveNome.Visible = false;
                    imgUndoNome.Visible = false;
                    txtNome.Text = curr.name;
                    lblNome.Text = curr.name;
                    txtNome.Visible = false;

                    imgEditCognome.Visible = true;
                    imgSaveCognome.Visible = false;
                    imgUndoCognome.Visible = false;
                    txtCognome.Text = curr.cognome;
                    lblCognome.Text = curr.cognome;
                    txtCognome.Visible = false;
                }
            }
            else
            {
                upd1.Visible = false;
            }
        }

        protected void imgEditNome_Click(object sender, ImageClickEventArgs e)
        {
                User curr = (User)Session["user"];
                imgEditNome.Visible = false;
                imgSaveNome.Visible = true;
                imgUndoNome.Visible = true;
                txtNome.Visible = true;
                lblNome.Visible = false;
        }

        protected void imgSaveNome_Click(object sender, ImageClickEventArgs e)
        {
            User curr = (User)Session["user"];
            curr.name = Server.HtmlEncode(txtNome.Text);
            imgEditNome.Visible = true;
            imgSaveNome.Visible = false;
            imgUndoNome.Visible = false;
            txtNome.Text = curr.name;
            lblNome.Text = curr.name;
            txtNome.Visible = false;
            lblNome.Visible = true;
        }

        protected void imgUndoNome_Click(object sender, ImageClickEventArgs e)
        {
            User curr = (User)Session["user"];
            imgEditNome.Visible = true;
            imgSaveNome.Visible = false;
            imgUndoNome.Visible = false;
            lblNome.Visible = true;
            txtNome.Visible = false;
            txtNome.Text = curr.name;
        }

        protected void imgEditCognome_Click(object sender, ImageClickEventArgs e)
        {
            User curr = (User)Session["user"];
            imgEditCognome.Visible = false;
            imgSaveCognome.Visible = true;
            imgUndoCognome.Visible = true;
            txtCognome.Visible = true;
            lblCognome.Visible = false;
        }

        protected void imgSaveCognome_Click(object sender, ImageClickEventArgs e)
        {
            User curr = (User)Session["user"];
            curr.cognome = Server.HtmlEncode(txtCognome.Text);
            imgEditCognome.Visible = true;
            imgSaveCognome.Visible = false;
            imgUndoCognome.Visible = false;
            txtCognome.Text = curr.cognome;
            lblCognome.Text = curr.cognome;
            txtCognome.Visible = false;
            lblCognome.Visible = true;
        }

        protected void imgUndoCognome_Click(object sender, ImageClickEventArgs e)
        {
            User curr = (User)Session["user"];
            imgEditCognome.Visible = true;
            imgSaveCognome.Visible = false;
            imgUndoCognome.Visible = false;
            lblCognome.Visible = true;
            txtCognome.Visible = false;
            txtCognome.Text = curr.cognome;
            lblCognome.Text = curr.cognome;
        }
    }
}