using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Postazioni
{
    public partial class barCodeCheckIn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                box1.Focus();
            }
        }

        protected void box1_TextChanged(object sender, EventArgs e)
        {
            if (box1.Text.Length > 0 && box2.Text.Length > 0)
            {
                // Elaboro i dati e provo ad eseguire una azione!
                bool rt = elabora();
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    box1.Text = "";
                    box2.Text = "";
                    box1.Focus();
                    log.Text = "ERRORE!";
                }
            }
            else
            {
                lbl1.Text = box1.Text;
                box2.Focus();
            }
        }

        protected void box2_TextChanged(object sender, EventArgs e)
        {
            if (box1.Text.Length > 0 && box2.Text.Length > 0)
            {
                // Elaboro i dati e provo ad eseguire una azione!
                bool rt = elabora();
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    box1.Text = "";
                    box2.Text = "";
                    box1.Focus();
                    log.Text = "ERRORE!";
                }
            }
            else
            {
                lbl2.Text = box2.Text;
                box1.Focus();
            }
        }

        protected bool elabora()
        {
            bool rt = false;
            int usrID = -1;
            int postID = -1;
            String Sutente = box1.Text;
            String Spost = box2.Text;
            if ((Sutente[0] == 'U' && Spost[0] == 'P') || ((Sutente[0] == 'P' && Spost[0] == 'U')))
            {
                if (Sutente[0] == 'P' && Spost[0] == 'U')
                {
                    // Li scambio
                    String swap = Sutente;
                    Sutente = Spost;
                    Spost = swap;
                }

                Sutente = Sutente.Substring(1, Sutente.Length - 1);
                Spost = Spost.Substring(1, Spost.Length - 1);
                lbl1.Text = Sutente;
                lbl2.Text = Spost;

                try
                {
                    usrID = Int32.Parse(Sutente);
                    postID = Int32.Parse(Spost);
                }
                catch
                {
                    usrID = -1;
                    postID = -1;
                }

                if (usrID != -1 && postID != -1)
                {
                    User usr = new User(usrID);
                    Postazione p = new Postazione(postID);
                    if (usr.username.Length > 0 && p.id != -1)
                    {
                        rt = true;
                        lbl1.Text = usr.username;
                        lbl2.Text = p.name;
                        // Controllo che l'utente non sia loggato
                        p.loadUtentiLoggati();
                        bool check = false;
                        for (int i = 0; i < p.UtentiLoggati.Count; i++)
                        {
                            if(p.UtentiLoggati[i] == usr.username)
                            {
                                check = true;
                            }
                        }
                        log.Text = check.ToString();
                        if (check == false)
                        {
                            log.Text = "Eseguo il check-in";
                            // Entro nella postazione
                            rt = usr.DoCheckIn(p);
                        }
                        else
                        {
                            log.Text = "Eseguo il check-out";
                            // Esco dalla postazione
                            rt = usr.DoCheckOut(p);
                        }
                    }
                    else
                    {
                        box1.Text = "";
                        box2.Text = "";
                        rt = false;
                    }
                }
                else
                {
                    box1.Text = "";
                    box2.Text = "";
                    rt = false;
                }
            }
            else
            {
                box1.Text = "";
                box2.Text = "";
                lbl1.Focus();
                rt = false;
            }
            return rt;
        }
    }
}