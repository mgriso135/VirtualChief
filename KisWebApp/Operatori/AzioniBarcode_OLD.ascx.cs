using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Operatori
{
    public partial class AzioniBarcode1 : System.Web.UI.UserControl
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
            String Stask = box2.Text;
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
                            if (p.UtentiLoggati[i] == usr.username)
                            {
                                check = true;
                            }
                        }
                        //log.Text = check.ToString();
                        if (check == false)
                        {
                            // Entro nella postazione
                            rt = usr.DoCheckIn(p);
                        }
                        else
                        {
                            //log.Text = "Eseguo il check-out";
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
            else if (((Stask[0] == 'I' || Stask[0] == 'A' || Stask[0] == 'F' || Stask[0] == 'W') && Sutente[0] == 'U') || (((Sutente[0] == 'I' || Sutente[0] == 'A' || Sutente[0] == 'F' || Sutente[0] == 'W') && Stask[0] == 'U')))
            {
                if ((Sutente[0] == 'I' || Sutente[0] == 'A' || Sutente[0] == 'F' || Sutente[0] == 'W') && Stask[0] == 'U')
                {
                    // Li scambio
                    String swap = Sutente;
                    Sutente = Stask;
                    Stask = swap;
                }

                log.Text += Sutente + " " + Stask + "<br/>";
                String action = Stask.Substring(0, 1);
                Sutente = Sutente.Substring(1, Sutente.Length - 1);
                Stask = Stask.Substring(1, Stask.Length - 1);

                int taskID = -1;
                try
                {
                    usrID = Int32.Parse(Sutente);
                    taskID = Int32.Parse(Stask);
                }
                catch
                {
                    usrID = -1;
                    taskID = -1;
                }

                User usr = new User(usrID);
                TaskProduzione tsk = new TaskProduzione(taskID);

                log.Text += usr.username + " " + tsk.TaskProduzioneID.ToString() + " " + action + "<br/>";

                if (action == "I")
                {
                    log.Text += "START<BR/>";
                    rt = tsk.Start(usr);
                }
                else if (action == "A")
                {
                    log.Text += "PAUSA<BR/>";
                    rt = tsk.Pause(usr);
                }
                else if (action == "F")
                {
                    log.Text += "COMPLETE<BR/>";
                    rt = tsk.Complete(usr);
                }
                else if (action == "W")
                {
                    log.Text += "WARNING<BR/>";
                    rt = tsk.generateWarning(usr);
                }
                log.Text += rt.ToString() + "<br/>" + tsk.log;
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