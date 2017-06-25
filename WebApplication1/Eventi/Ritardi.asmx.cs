using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using KIS.Commesse;
using KIS.App_Code;
namespace KIS.Eventi
{
    /// <summary>
    /// Questo webservice implementa le funzioni di ricerca dei ritardi e di segnalazione dei ritardi a chi di dovere
    /// Inoltre esegue la pulizia della tabella eventi da quelli segnalati
    /// </summary>
    [WebService(Namespace = "http://kis.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class Ritardi : System.Web.Services.WebService
    {

        [WebMethod]
        public void Main()
        {
            //bool rt = false;
            List<String[]> rt = TrovaRitardi();
            SegnalaRitardi();
            //rt = true;
            //return rt;
        }

        private List<String[]> TrovaRitardi()
        {
            List<String[]> ritardi = new List<string[]>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE status <> 'F' ORDER BY lateStart";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> tskRitardo = new List<int>();
            while (rdr.Read())
            {
                TaskProduzione tsk = new TaskProduzione(rdr.GetInt32(0));
                Reparto rp = new Reparto(tsk.RepartoID);
                rp.loadEventoRitardo();

                String[] ritardo = new String[4];
                if (tsk.ritardo > new TimeSpan(0, 0, 0))
                {
                    tskRitardo.Add(tsk.TaskProduzioneID);
                
                
                ritardo[0] = tsk.TaskProduzioneID.ToString();
                ritardo[1] = tsk.ritardo.Hours + ":" + tsk.ritardo.Minutes + ":" + tsk.ritardo.Seconds;
                ritardo[2] = rp.EventoRitardo.RitardoMinimoDaSegnalare.Hours + ":" + rp.EventoRitardo.RitardoMinimoDaSegnalare.Minutes + ":" + rp.EventoRitardo.RitardoMinimoDaSegnalare.Seconds;
                ritardi.Add(ritardo);
                }
            }
            rdr.Close();

            for (int i = 0; i < tskRitardo.Count; i++)
            {
                cmd.CommandText = "INSERT INTO registroeventiproduzione(TipoEvento, taskID, segnalato) VALUES('Ritardo', "
                    + tskRitardo[i].ToString() + ", false)";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            conn.Close();
            return ritardi;
        }
        
        private bool SegnalaRitardi()
        {
            bool rt = false;
            //String retu = "";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Ritardo' AND segnalato = false";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //retu += "DB: " + rdr.GetInt32(0).ToString() + "<br />";
                tskList.Add(new TaskProduzione(rdr.GetInt32(0)));
            }
            rdr.Close();
            for (int i = 0; i < tskList.Count; i++)
            {
                //retu += "Main cycle: " + tskList[i].TaskProduzioneID.ToString() + "<br />";
                // Ricerco tutti gli indirizzi cui inviare la mail
                List<System.Net.Mail.MailAddress> MailList = new List<System.Net.Mail.MailAddress>();
                // Ricerca per reparto
                Reparto rp = new Reparto(tskList[i].RepartoID);
                rp.loadEventoRitardo();
                if (rp.EventoRitardo.RitardoMinimoDaSegnalare != null && tskList[i].ritardo >= rp.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < rp.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == rp.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(rp.EventoRitardo.MailingList[j]);
                            //retu += "Reparto " + rp.EventoRitardo.MailingList[j].Address + "<br />";
                        }
                    }
                }

                // Ricerco per articolo
                Articolo art = new Articolo(tskList[i].ArticoloID, tskList[i].ArticoloAnno);
                art.loadEventoRitardo();
                if (art.EventoRitardo.RitardoMinimoDaSegnalare != null && tskList[i].ritardo >= art.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < art.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == art.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(art.EventoRitardo.MailingList[j]);
                        }
                    }
                }



                // Ricerco per commessa
                Commessa cm = new Commessa(art.Commessa, art.AnnoCommessa);
                cm.loadEventoRitardo();

                if (cm.EventoRitardo.RitardoMinimoDaSegnalare != null && tskList[i].ritardo >= cm.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < cm.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == cm.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(cm.EventoRitardo.MailingList[j]);
                            //retu += "Commessa " + cm.EventoRitardo.MailingList[j].Address + "<br />";
                        }
                    }
                }

                Postazione pst = new Postazione(tskList[i].PostazioneID);
                // Ricerca per articolo
                // Ricerca per specifico task
                //.................


                if (MailList.Count > 0)
                {
                    // Invio l'e-mail
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                    mMessage.From = new MailAddress("m.griso@hotmail.it", "Scheduler@KaizenIndicatorSystem");
                    mMessage.To.Add(new MailAddress("m.griso@hotmail.it", "Scheduler@KaizenIndicatorSystem"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }
                    mMessage.Subject = "KIS - Segnalazione ritardo";
                    mMessage.IsBodyHtml = true;



                    mMessage.Body = "Si è verificato un ritardo in produzione, con riferimento a:<br/>"
                        + "Reparto: " + rp.name + "<br />"
                        + "Postazione: " + pst.name + "<br />"
                        + "Commessa " + cm.ID.ToString() + "/" + cm.Year.ToString() + " per cliente " + cm.Cliente + "<br />"
                        + "Articolo " + art.ID.ToString() + "/" + art.Year.ToString() + " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante + "<br />"
                        + "Task " + tskList[i].TaskProduzioneID.ToString() + " " + tskList[i].Name;

                    SmtpClient smtpcli = new SmtpClient();
                    smtpcli.Send(mMessage);
                }


                if (tskList[i].ritardo >= rp.EventoRitardo.RitardoMinimoDaSegnalare && tskList[i].ritardo >= cm.EventoRitardo.RitardoMinimoDaSegnalare && tskList[i].ritardo >= art.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    // Metto il ritardo come già segnalato
                    cmd.CommandText = "UPDATE registroeventiproduzione SET segnalato = true WHERE TipoEvento LIKE 'Ritardo' AND taskID = " + tskList[i].TaskProduzioneID;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rt = true;
                    }
                    catch
                    {
                        rt = false;
                    }
                }
            }
            conn.Close();
            //return retu;
            return rt;
        }
    }
}
