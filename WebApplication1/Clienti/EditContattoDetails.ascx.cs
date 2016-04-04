using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.Net.Mail;

namespace KIS.Clienti
{
    public partial class EditContattoDetails1 : System.Web.UI.UserControl
    {
        public int idContatto;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblContatto.Visible = false;
            btnShowAddPhone.Visible = false;
            btnShowAddEmail.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Anagrafica Clienti Contatti";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                Contatto contCln = new Contatto(idContatto);
                if (contCln.ID != -1)
                {
                    tblContatto.Visible = true;
                    btnShowAddPhone.Visible = true;
                    btnShowAddEmail.Visible = true;

                    if (contCln.Phones.Count > 0)
                    {
                        rptPhones.Visible = true;
                    }
                    else
                    {
                        rptPhones.Visible = false;
                    }

                    if (contCln.Emails.Count > 0)
                    {
                        rptEmails.Visible = true;
                    }
                    else
                    {
                        rptEmails.Visible = false;
                    }

                    if (!Page.IsPostBack)
                    {
                        txtNominativo.Text = contCln.Nominativo;
                        txtRuolo.Text = contCln.Ruolo;
                        rptPhones.DataSource = contCln.Phones;
                        rptPhones.DataBind();

                        rptEmails.DataSource = contCln.Emails;
                        rptEmails.DataBind();
                    }
                }
            }
            else
            {
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Contatto clnCont = new Contatto(idContatto);
            if (clnCont.ID != -1)
            {
                clnCont.Nominativo = Server.HtmlEncode(txtNominativo.Text);
                clnCont.Ruolo = Server.HtmlEncode(txtRuolo.Text);
            }
            else
            {
                lbl1.Text = "Errore.";
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            Contatto clnCont = new Contatto(idContatto);
            if (clnCont.ID != -1)
            {
                txtNominativo.Text=clnCont.Nominativo;
                txtRuolo.Text = clnCont.Ruolo;
            }
        }

        protected void rptPhones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Contatto cln = new Contatto(idContatto);
            if (cln.ID != -1)
            {
                if (e.CommandName == "Delete")
                {
                    bool rt = false;
                    for (int i = 0; i < cln.Phones.Count; i++)
                    {
                        if (cln.Phones[i].Phone == e.CommandArgument.ToString())
                        {
                            rt = cln.Phones[i].Delete();
                        }
                    }

                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore. " + cln.log;
                    }
                }
            }
        }

        protected void btnShowAddPhone_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddPhone.Visible == true)
            {
                frmAddPhone.Visible = false;
            }
            else
            {
                frmAddPhone.Visible = true;
            }
        }

        protected void btnNewPhoneSave_Click(object sender, ImageClickEventArgs e)
        {
            if (idContatto != -1)
            {
                Contatto cnt = new Contatto(idContatto);
                if (cnt.ID != -1)
                {
                    bool rt = cnt.addPhone(Server.HtmlEncode(txtNewPhone.Text), Server.HtmlEncode(txtNoteNewPhone.Text));
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore. " + cnt.log;
                    }
                }
            }
        }

        protected void btnNewPhoneUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtNewPhone.Text = "";
            txtNoteNewPhone.Text = "";
        }

        protected void btnShowAddEmail_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddEmail.Visible == true)
            {
                frmAddEmail.Visible = false;
            }
            else
            {
                frmAddEmail.Visible = true;
            }
        }

        protected void btnNewMailSave_Click(object sender, ImageClickEventArgs e)
        {
            if (idContatto != -1)
            {
                Contatto cnt = new Contatto(idContatto);
                if (cnt.ID != -1)
                {
                    bool check = false;
                    MailAddress mailAddr = null;
                    try
                    {
                        mailAddr = new MailAddress(Server.HtmlEncode(txtNewMail.Text));
                        check = true;
                    }
                    catch
                    {
                        check = false;
                    }
                    if (check == true)
                    {
                        bool rt = cnt.addEmail(mailAddr, Server.HtmlEncode(txtNoteNewMail.Text));
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = "Si è verificato un errore. " + cnt.log;
                        }
                    }
                    else
                    {
                        lbl1.Text = "Formato indirizzo e-mail non corretto.";
                    }
                }
            }
        }

        protected void btnNewMailUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtNewMail.Text = "";
            txtNoteNewMail.Text = "";
        }

        protected void rptEmails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Contatto cln = new Contatto(idContatto);
            if (cln.ID != -1)
            {
                if (e.CommandName == "Delete")
                {
                    bool rt = false;
                    for (int i = 0; i < cln.Emails.Count; i++)
                    {
                        if (cln.Emails[i].Email.Address == e.CommandArgument.ToString())
                        {
                            rt = cln.Emails[i].Delete();
                        }
                    }

                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore. " + cln.log;
                    }
                }
            }
        }
    }
}