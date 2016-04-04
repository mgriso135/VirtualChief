using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.Commesse;

namespace KIS.eventi
{
    public abstract class evento
    {
        protected String _Nome;
        public String Nome
        {
            get
            {
                return this._Nome;
            }
        }

        public evento()
        {
        }
    }

    public class WarningEvent : evento
    {
        public WarningEvent()
            : base()
        {
            this._Nome = "Warning";
        }
    }

    public class Ritardo : evento
    {
        public Ritardo()
            : base()
        {
            this._Nome = "Ritardo";
        }
    }

    public abstract class ConfigurazioneEventoAmbito
    {
        

        public List<String> ListUsers;
        public List<int> ListGroupsID;
        public List<System.Net.Mail.MailAddress> MailingList
        {
            get
            {
                List<System.Net.Mail.MailAddress> indirizzi = new List<System.Net.Mail.MailAddress>();
                // Cerco gli indirizzi degli utenti singoli
                for (int i = 0; i < this.ListUsers.Count; i++)
                {
                    User currUsr = new User(this.ListUsers[i]);
                    currUsr.loadEmails();
                    for (int j = 0; j < currUsr.Email.Count; j++)
                    {
                        // Se l'indirizzo e-mail è per gli allarmi...
                        if (currUsr.Email[j].ForAlarm == true)
                        {
                            // Verifico che non sia tra i precedenti
                            bool found = false;
                            for (int q = 0; q < indirizzi.Count && found == false; q++)
                            {
                                if (indirizzi[q].Address == currUsr.Email[j].Email)
                                {
                                    found = true;
                                }
                            }
                            // Se non l'ho trovato lo aggiungo alla lista
                            if (found == false)
                            {
                                indirizzi.Add(new System.Net.Mail.MailAddress(currUsr.Email[j].Email, currUsr.name + " " + currUsr.cognome));
                            }
                        }
                    }
                }

                // Cerco gli indirizzi all'interno dei gruppi
                for (int i = 0; i < this.ListGroupsID.Count; i++)
                {
                    Group currGroup = new Group(this.ListGroupsID[i]);
                    currGroup.loadUtenti();
                    for (int j = 0; j < currGroup.Utenti.Count; j++)
                    {
                        User currUsr = new User(currGroup.Utenti[j]);
                        currUsr.loadEmails();
                        for (int k = 0; k < currUsr.Email.Count; k++)
                        {
                            // Se l'indirizzo e-mail è per gli allarmi...
                            if (currUsr.Email[k].ForAlarm)
                            {
                                // Verifico che non sia tra i precedenti
                                bool found = false;
                                for (int q = 0; q < indirizzi.Count && found == false; q++)
                                {
                                    if (indirizzi[q].Address == currUsr.Email[k].Email)
                                    {
                                        found = true;
                                    }
                                }
                                // Se non l'ho trovato lo aggiungo alla lista
                                if (found == false)
                                {
                                    indirizzi.Add(new System.Net.Mail.MailAddress(currUsr.Email[k].Email, currUsr.name + " " + currUsr.cognome));
                                }
                            }
                        }
                    }
                }

                return indirizzi;
            }
        }
        public List<String> SMSList
        {
            get
            {
                List<String> numeri = new List<String>();
                // Cerco il numero di telefono degli utenti singoli
                for (int i = 0; i < this.ListUsers.Count; i++)
                {
                    User currUsr = new User(this.ListUsers[i]);
                    currUsr.loadPhoneNumbers();
                    for (int j = 0; j < currUsr.PhoneNumbers.Count; j++)
                    {
                        // Se il numero di telefono è per gli allarmi...
                        if (currUsr.PhoneNumbers[j].ForAlarm)
                        {
                            // Verifico che non sia tra i precedenti
                            bool found = false;
                            for (int q = 0; q < numeri.Count && found == false; q++)
                            {
                                if (numeri[q] == currUsr.PhoneNumbers[j].PhoneNumber)
                                {
                                    found = true;
                                }
                            }
                            // Se non l'ho trovato lo aggiungo alla lista
                            if (found == false)
                            {
                                numeri.Add(currUsr.PhoneNumbers[j].PhoneNumber);
                            }
                        }
                    }
                }

                // Cerco gli indirizzi all'interno dei gruppi
                for (int i = 0; i < this.ListGroupsID.Count; i++)
                {
                    Group currGroup = new Group(this.ListGroupsID[i]);
                    currGroup.loadUtenti();
                    for (int j = 0; j < currGroup.Utenti.Count; j++)
                    {
                        User currUsr = new User(currGroup.Utenti[j]);
                        currUsr.loadPhoneNumbers();
                        for (int k = 0; k < currUsr.PhoneNumbers.Count; k++)
                        {
                            // Se il numero di telefono è per gli allarmi...
                            if (currUsr.PhoneNumbers[k].ForAlarm)
                            {
                                // Verifico che non sia tra i precedenti
                                bool found = false;
                                for (int q = 0; q < numeri.Count && found == false; q++)
                                {
                                    if (numeri[q] == currUsr.PhoneNumbers[k].PhoneNumber)
                                    {
                                        found = true;
                                    }
                                }
                                // Se non l'ho trovato lo aggiungo alla lista
                                if (found == false)
                                {
                                    numeri.Add(currUsr.PhoneNumbers[k].PhoneNumber);
                                }
                            }
                        }
                    }
                }

                return numeri;
            }
        }

        public virtual void loadGruppi()
        {
        }
        public virtual void loadUsers()
        { }

        public ConfigurazioneEventoAmbito() 
        { 
        }
    }

    public abstract class ConfigurazioneRitardoAmbito : ConfigurazioneEventoAmbito
    {
        private Ritardo _TipoEvento;
        public Ritardo TipoEvento
        {
            get { return this._TipoEvento; }

        }

        protected TimeSpan _RitardoMinimoDaSegnalare;
        public virtual TimeSpan RitardoMinimoDaSegnalare
        {
            get { return this._RitardoMinimoDaSegnalare; }
            set { }
        }

        public ConfigurazioneRitardoAmbito()
            : base()
        {
            this._TipoEvento = new Ritardo();
            this._RitardoMinimoDaSegnalare = new TimeSpan(0, 0, 0);
        }
    }

    public class ConfigurazioneRitardoReparto : ConfigurazioneRitardoAmbito
    {
        public String log;
        private int _repartoID;
        public int RepartoID
        {
            get { return this._repartoID; }
        }

        public override TimeSpan RitardoMinimoDaSegnalare
        {
            get
            {
                return this._RitardoMinimoDaSegnalare;
            }
            set
            {
                // Ricerco se è già impostato
                bool found = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventorepartoconfig WHERE "
                    + " TipoEvento LIKE '" + this.TipoEvento.Nome + "'"
                    + " AND Reparto = " + this.RepartoID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();


                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                if (found == true)
                {
                    cmd.CommandText = "UPDATE eventorepartoconfig set RitardoMinimoDaSegnalare = '" 
                        + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString()
                        + "' WHERE tipoevento LIKE '" + this.TipoEvento.Nome + "' AND reparto = " + this.RepartoID.ToString();
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventorepartoconfig(TipoEvento, Reparto, RitardoMinimoDaSegnalare) VALUES('"
                        + this.TipoEvento.Nome + "', "
                        + this.RepartoID.ToString() 
                        + ", '" + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString() + "')";
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }

                conn.Close();
            }
        }


        public ConfigurazioneRitardoReparto(int repID)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, Reparto, RitardoMinimoDaSegnalare FROM eventorepartoconfig WHERE Reparto = " + repID.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._repartoID = rdr.GetInt32(1);
                this._RitardoMinimoDaSegnalare = rdr.GetTimeSpan(2);
            }
            else
            {
                this._repartoID = repID;
                this._RitardoMinimoDaSegnalare = new TimeSpan(0, 0, 0);
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventorepartogruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND idReparto = " + this.RepartoID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventorepartoutenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND RepartoID = " + this.RepartoID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventorepartoutenti(TipoEvento, repartoID, userID) VALUES('"
                    + this.TipoEvento.Nome + "', "
                    + this.RepartoID.ToString() + ", "
                    + "'" + curr.username + "')";
                log = cmd.CommandText;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch
                {
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            
            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;
            
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventorepartogruppi(TipoEvento, idReparto, idGruppo) VALUES('"
                    + this.TipoEvento.Nome + "', "
                    + this.RepartoID.ToString() + ", "
                    + grp.ID.ToString() + ")";
                log = cmd.CommandText;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            
            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartogruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "idReparto = " + this.RepartoID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartoutenti WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "RepartoID = " + this.RepartoID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "'";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }
    }

    public abstract class ConfigurazioneWarningAmbito : ConfigurazioneEventoAmbito
    {
        private WarningEvent _TipoEvento;
        public WarningEvent TipoEvento
        {
            get { return this._TipoEvento; }

        }

        public ConfigurazioneWarningAmbito()
            : base()
        {
            this._TipoEvento = new WarningEvent();
        }

    }

    public class ConfigurazioneWarningReparto : ConfigurazioneWarningAmbito
    {
        public String log;
        private int _repartoID;
        public int RepartoID
        {
            get { return this._repartoID; }
        }

        public ConfigurazioneWarningReparto(int repID)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, Reparto, RitardoMinimoDaSegnalare FROM eventorepartoconfig WHERE Reparto = " + repID.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._repartoID = rdr.GetInt32(1);
            }
            else
            {
                this._repartoID = repID;
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventorepartogruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND idReparto = " + this.RepartoID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventorepartoutenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND RepartoID = " + this.RepartoID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventorepartoutenti(TipoEvento, repartoID, userID) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.RepartoID.ToString() + ", "
                + "'" + curr.username + "')";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch
            {
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventorepartogruppi(TipoEvento, idReparto, idGruppo) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.RepartoID.ToString() + ", "
                + grp.ID.ToString() + ")";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartogruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "idReparto = " + this.RepartoID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartoutenti WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "RepartoID = " + this.RepartoID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "'";

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

    }

    public class ConfigurazioneRitardoCommessa : ConfigurazioneRitardoAmbito
    {
        public String log;
        private int _CommessaID;
        public int CommessaID
        {
            get { return this._CommessaID; }
        }

        private int _CommessaAnno;
        public int CommessaAnno
        {
            get { return this._CommessaAnno; }
        }

        public override TimeSpan RitardoMinimoDaSegnalare
        {
            get
            {
                return this._RitardoMinimoDaSegnalare;
            }
            set
            {
                // Ricerco se è già impostato
                bool found = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventocommessaconfig WHERE "
                    + " TipoEvento LIKE '" + this.TipoEvento.Nome + "'"
                    + " AND CommessaID = " + this.CommessaID.ToString()
                    + " AND CommessaAnno = " + this.CommessaAnno.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                if (found == true)
                {
                    cmd.CommandText = "UPDATE eventocommessaconfig set RitardoMinimoDaSegnalare = '" 
                        + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString()
                        + "' WHERE tipoevento LIKE '" + this.TipoEvento.Nome 
                        + "' AND commessaID = " + this.CommessaID.ToString()
                        + " AND commessaAnno = " + this.CommessaAnno.ToString();
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventocommessaconfig(TipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare) VALUES('"
                        + this.TipoEvento.Nome + "', "
                        + this.CommessaID.ToString() + ", " + this.CommessaAnno.ToString() + ", '"
                            + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString() + "')";
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }

                conn.Close();
            }
        }


        public ConfigurazioneRitardoCommessa(KIS.Commesse.Commessa comm)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare FROM eventocommessaconfig WHERE CommessaID = "
                + comm.ID.ToString() + " AND CommessaAnno = " + comm.Year.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._CommessaID = rdr.GetInt32(1);
                this._CommessaAnno = rdr.GetInt32(2);
                this._RitardoMinimoDaSegnalare = rdr.GetTimeSpan(3);
            }
            else
            {
                this._CommessaID = comm.ID;
                this._CommessaAnno = comm.Year;
                this._RitardoMinimoDaSegnalare = new TimeSpan(0, 0, 0);
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventocommessagruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND CommessaID = " + this.CommessaID.ToString()
                + " AND CommessaAnno = " + this.CommessaAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventocommessautenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND CommessaID = " + this.CommessaID.ToString()
                + " AND CommessaAnno = " + this.CommessaAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventocommessautenti(TipoEvento, commessaID, commessaAnno, userID) VALUES('"
                    + this.TipoEvento.Nome + "', "
                    + this.CommessaID.ToString() + ", " 
                    + this.CommessaAnno.ToString() + ", "
                    + "'" + curr.username + "')";
                log = cmd.CommandText;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch
                {
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            
            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;
            
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventocommessagruppi(TipoEvento, commessaID, commessaAnno, idGruppo) VALUES('"
                    + this.TipoEvento.Nome + "', "
                    + this.CommessaID.ToString() + ", "
                    + this.CommessaAnno.ToString() + ", "
                    + grp.ID.ToString() + ")";
                log = cmd.CommandText;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            
            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessagruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "commessaID = " + this.CommessaID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString() 
                + " AND commessaAnno = " + this.CommessaAnno.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessautenti WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "CommessaID = " + this.CommessaID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "' AND CommessaAnno = " + this.CommessaAnno.ToString();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }
    }
    
    public class ConfigurazioneWarningCommessa : ConfigurazioneWarningAmbito
    {
        public String log;
        private int _CommessaID;
        public int CommessaID
        {
            get { return this._CommessaID; }
        }

        private int _CommessaAnno;
        public int CommessaAnno
        {
            get
            {
                return this._CommessaAnno;
            }
        }

        public ConfigurazioneWarningCommessa(KIS.Commesse.Commessa cm)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare FROM eventocommessaconfig WHERE CommessaID = "
                + cm.ID.ToString()
                + " AND CommessaAnno = " + cm.Year.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._CommessaID = rdr.GetInt32(1);
                this._CommessaAnno = rdr.GetInt32(2);
            }
            else
            {
                this._CommessaID = cm.ID;
                this._CommessaAnno = cm.Year;
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventocommessagruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND CommessaID = " + this.CommessaID.ToString()
                + " AND CommessaAnno = " + this.CommessaAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventocommessautenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND CommessaID = " + this.CommessaID.ToString()
                + " AND CommessaAnno = " + this.CommessaAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventocommessautenti(TipoEvento, commessaID, commessaAnno, userID) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.CommessaID.ToString() + ", "
                + this.CommessaAnno.ToString() + ", "
                + "'" + curr.username + "')";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch
            {
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventocommessagruppi(TipoEvento, commessaID, commessaAnno, idGruppo) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.CommessaID.ToString() + ", "
                + this.CommessaAnno.ToString() + ", "
                + grp.ID.ToString() + ")";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessagruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "commessaID = " + this.CommessaID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString()
                + " AND commessaAnno = " + this.CommessaAnno.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessautenti WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "CommessaID = " + this.CommessaID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "'"
                + " AND CommessaAnno = " + this.CommessaAnno.ToString();

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

    }

    public class ConfigurazioneRitardoArticolo : ConfigurazioneRitardoAmbito
    {
        public String log;
        private int _ArticoloID;
        public int ArticoloID
        {
            get { return this._ArticoloID; }
        }

        private int _ArticoloAnno;
        public int ArticoloAnno
        {
            get { return this._ArticoloAnno; }
        }

        public override TimeSpan RitardoMinimoDaSegnalare
        {
            get
            {
                return this._RitardoMinimoDaSegnalare;
            }
            set
            {
                // Ricerco se è già impostato
                bool found = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventoarticoloconfig WHERE "
                    + " TipoEvento LIKE '" + this.TipoEvento.Nome + "'"
                    + " AND ArticoloID = " + this.ArticoloID.ToString()
                    + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                if (found == true)
                {
                    cmd.CommandText = "UPDATE eventoarticoloconfig set RitardoMinimoDaSegnalare = '"
                        + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString()
                        + "' WHERE tipoevento LIKE '" + this.TipoEvento.Nome
                        + "' AND ArticoloID = " + this.ArticoloID.ToString()
                        + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventoarticoloconfig(TipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare) VALUES('"
                        + this.TipoEvento.Nome + "', "
                        + this.ArticoloID.ToString() + ", " + this.ArticoloAnno.ToString() + ", '"
                            + value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString() + "')";
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }

                conn.Close();
            }
        }


        public ConfigurazioneRitardoArticolo(KIS.Commesse.Articolo art)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare FROM eventoarticoloconfig WHERE ArticoloID = "
                + art.ID.ToString() + " AND ArticoloAnno = " + art.Year.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._ArticoloID = rdr.GetInt32(1);
                this._ArticoloAnno = rdr.GetInt32(2);
                this._RitardoMinimoDaSegnalare = rdr.GetTimeSpan(3);
            }
            else
            {
                this._ArticoloID = art.ID;
                this._ArticoloAnno = art.Year;
                this._RitardoMinimoDaSegnalare = new TimeSpan(0, 0, 0);
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventoarticologruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND ArticoloID = " + this.ArticoloID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventoarticoloutenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND ArticoloID = " + this.ArticoloID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticoloutenti(TipoEvento, ArticoloID, ArticoloAnno, userID) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.ArticoloID.ToString() + ", "
                + this.ArticoloAnno.ToString() + ", "
                + "'" + curr.username + "')";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch
            {
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticologruppi(TipoEvento, ArticoloID, ArticoloAnno, idGruppo) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.ArticoloID.ToString() + ", "
                + this.ArticoloAnno.ToString() + ", "
                + grp.ID.ToString() + ")";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticologruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "ArticoloID = " + this.ArticoloID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticoloutenti WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "ArticoloID = " + this.ArticoloID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "' AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }
    }

    public class ConfigurazioneWarningArticolo : ConfigurazioneWarningAmbito
    {
        public String log;
        private int _ArticoloID;
        public int ArticoloID
        {
            get { return this._ArticoloID; }
        }

        private int _ArticoloAnno;
        public int ArticoloAnno
        {
            get
            {
                return this._ArticoloAnno;
            }
        }

        public ConfigurazioneWarningArticolo(Articolo art)
            : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare FROM eventoarticoloconfig WHERE ArticoloID = "
                + art.ID.ToString()
                + " AND ArticoloAnno = " + art.Year.ToString()
                + " AND TipoEvento LIKE '" + this.TipoEvento.Nome + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(2))
            {
                this._ArticoloID = rdr.GetInt32(1);
                this._ArticoloAnno = rdr.GetInt32(2);
            }
            else
            {
                this._ArticoloID = art.ID;
                this._ArticoloAnno = art.Year;
            }

            this.loadGruppi();
            this.loadUsers();
            rdr.Close();
            conn.Close();
        }

        public override void loadGruppi()
        {
            this.ListGroupsID = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventoarticologruppi WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND ArticoloID = " + this.ArticoloID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListGroupsID.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
        }

        public override void loadUsers()
        {
            this.ListUsers = new List<String>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventoarticoloutenti WHERE "
                + " tipoevento = '" + this.TipoEvento.Nome + "' "
                + " AND ArticoloID = " + this.ArticoloID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ListUsers.Add(rdr.GetString(0));
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(User curr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticoloutenti(TipoEvento, ArticoloID, ArticoloAnno, userID) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.ArticoloID.ToString() + ", "
                + this.ArticoloAnno.ToString() + ", "
                + "'" + curr.username + "')";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch
            {
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticologruppi(TipoEvento, ArticoloID, ArticoloAnno, idGruppo) VALUES('"
                + this.TipoEvento.Nome + "', "
                + this.ArticoloID.ToString() + ", "
                + this.ArticoloAnno.ToString() + ", "
                + grp.ID.ToString() + ")";
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticologruppi WHERE "
                + "TipoEvento LIKE '" + this.TipoEvento.Nome + "' AND "
                + "ArticoloID = " + this.ArticoloID.ToString() + " AND "
                + "idGruppo = " + grp.ID.ToString()
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();
            log = cmd.CommandText;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log += ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

        public bool deleteUtente(User usr)
        {
            bool rt = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticoloutenti WHERE "
                + "ArticoloID = " + this.ArticoloID.ToString() + " AND "
                + "userID LIKE '" + usr.username.ToString() + "'"
                + " AND ArticoloAnno = " + this.ArticoloAnno.ToString();

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = false;
                tr.Rollback();
            }
            conn.Close();

            return rt;
        }

    }

}