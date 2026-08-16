/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using KIS.App_Sources;
using MySql.Data.MySqlClient;
//using KIS.Commesse;

namespace KIS.App_Code
{
    public abstract class evento
    {
        protected String Tenant;
        protected String _Nome;
        public String Nome
        {
            get
            {
                return this._Nome;
            }
        }

        public evento(String tenant)
        {
            this.Tenant = tenant;
        }
    }

    public class WarningEvent : evento
    {
        public WarningEvent(String tenant)
            : base(tenant)
        {
            this.Tenant = tenant;
            this._Nome = "Warning";
        }
    }

    public class Ritardo : evento
    {
        public Ritardo(String tenant)
            : base(tenant)
        {
            this.Tenant = tenant;
            this._Nome = "Ritardo";
        }
    }

    public abstract class ConfigurazioneEventoAmbito
    {
        protected String Tenant;

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
                    Workspace ws = new Workspace(this.Tenant);
                    currGroup.loadUtenti(ws.id);
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
                    UserAccount currUsr = new UserAccount(this.ListUsers[i]);
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
                    Workspace ws = new Workspace(this.Tenant);
                    currGroup.loadUtenti(ws.id);
                    for (int j = 0; j < currGroup.Utenti.Count; j++)
                    {
                        UserAccount currUsr = new UserAccount(currGroup.Utenti[j]);
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

        public ConfigurazioneEventoAmbito(String Tenant) 
        {
            this.Tenant = Tenant;
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

        public ConfigurazioneRitardoAmbito(String Tenant)
            : base(Tenant)
        {
            this.Tenant = Tenant;
            this._TipoEvento = new Ritardo(this.Tenant);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventorepartoconfig WHERE "
                    + " TipoEvento LIKE @pTipoEvento"
                    + " AND Reparto = @pReparto";
                cmd.Parameters.AddWithValue("@pTipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@pReparto", this.RepartoID);
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
                cmd.Parameters.AddWithValue("@pRitardo", value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString());
                if (found == true)
                {
                    cmd.CommandText = "UPDATE eventorepartoconfig set RitardoMinimoDaSegnalare = @pRitardo"
                        + " WHERE tipoevento LIKE @pTipoEvento AND reparto = @pReparto";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventorepartoconfig(TipoEvento, Reparto, RitardoMinimoDaSegnalare) VALUES(@pTipoEvento, @pReparto, @pRitardo)";
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


        public ConfigurazioneRitardoReparto(String Tenant, int repID)
            : base(Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, Reparto, RitardoMinimoDaSegnalare FROM eventorepartoconfig WHERE Reparto = @pReparto"
                + " AND TipoEvento LIKE @pTipoEvento";
            cmd.Parameters.AddWithValue("@pReparto", repID);
            cmd.Parameters.AddWithValue("@pTipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventorepartogruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND idReparto = @idReparto";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventorepartoutenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND RepartoID = @idReparto";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
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

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventorepartoutenti(TipoEvento, repartoID, userID) VALUES(@tipoEvento, @idReparto, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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
            
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventorepartogruppi(TipoEvento, idReparto, idGruppo) VALUES(@tipoEvento, @idReparto, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartogruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "idReparto = @idReparto AND "
                + "idGruppo = @idGruppo";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartoutenti WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "RepartoID = @idReparto AND "
                + "userID LIKE @userID";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
            cmd.Parameters.AddWithValue("@userID", usr.username);
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
        // protected String Tenant;

        private WarningEvent _TipoEvento;
        public WarningEvent TipoEvento
        {
            get { return this._TipoEvento; }

        }

        public ConfigurazioneWarningAmbito(String Tenant)
            : base(Tenant)
        {
            this.Tenant = Tenant;
            this._TipoEvento = new WarningEvent(this.Tenant);
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

        public ConfigurazioneWarningReparto(String Tenant, int repID)
            : base(Tenant)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, Reparto, RitardoMinimoDaSegnalare FROM eventorepartoconfig WHERE Reparto = @repID"
                + " AND TipoEvento LIKE @tipoEvento";
            cmd.Parameters.AddWithValue("@repID", repID);
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventorepartogruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND idReparto = @idReparto";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventorepartoutenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND RepartoID = @idReparto";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventorepartoutenti(TipoEvento, repartoID, userID) VALUES(@tipoEvento, @idReparto, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventorepartogruppi(TipoEvento, idReparto, idGruppo) VALUES(@tipoEvento, @idReparto, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartogruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "idReparto = @idReparto AND "
                + "idGruppo = @idGruppo";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventorepartoutenti WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "RepartoID = @idReparto AND "
                + "userID LIKE @userID";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@idReparto", this.RepartoID);
            cmd.Parameters.AddWithValue("@userID", usr.username);

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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventocommessaconfig WHERE "
                    + " TipoEvento LIKE @tipoEvento"
                    + " AND CommessaID = @commessaID"
                    + " AND CommessaAnno = @commessaAnno";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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
                    cmd.CommandText = "UPDATE eventocommessaconfig set RitardoMinimoDaSegnalare = @ritardoMinimo"
                        + " WHERE tipoevento LIKE @tipoEvento"
                        + " AND commessaID = @commessaID"
                        + " AND commessaAnno = @commessaAnno";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventocommessaconfig(TipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare) VALUES("
                        + "@tipoEvento, @commessaID, @commessaAnno, @ritardoMinimo)";
                }
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
                cmd.Parameters.AddWithValue("@ritardoMinimo",
                    value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString());

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


        public ConfigurazioneRitardoCommessa(String Tenant, Commessa comm)
            : base(Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare FROM eventocommessaconfig WHERE CommessaID = "
                + "@commessaID AND CommessaAnno = @commessaAnno"
                + " AND TipoEvento LIKE @tipoEvento";
            cmd.Parameters.AddWithValue("@commessaID", comm.ID);
            cmd.Parameters.AddWithValue("@commessaAnno", comm.Year);
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventocommessagruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND CommessaID = @commessaID"
                + " AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventocommessautenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND CommessaID = @commessaID"
                + " AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventocommessautenti(TipoEvento, commessaID, commessaAnno, userID) VALUES(@tipoEvento, @commessaID, @commessaAnno, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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
            
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO eventocommessagruppi(TipoEvento, commessaID, commessaAnno, idGruppo) VALUES(@tipoEvento, @commessaID, @commessaAnno, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessagruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "commessaID = @commessaID AND "
                + "idGruppo = @idGruppo"
                + " AND commessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessautenti WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "CommessaID = @commessaID AND "
                + "userID LIKE @userID AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@userID", usr.username);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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
        public String Tenant;

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

        public ConfigurazioneWarningCommessa(String Tenant, Commessa cm)
            : base(Tenant)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, CommessaID, CommessaAnno, RitardoMinimoDaSegnalare FROM eventocommessaconfig WHERE CommessaID = "
                + "@commessaID"
                + " AND CommessaAnno = @commessaAnno"
                + " AND TipoEvento LIKE @tipoEvento";
            cmd.Parameters.AddWithValue("@commessaID", cm.ID);
            cmd.Parameters.AddWithValue("@commessaAnno", cm.Year);
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventocommessagruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND CommessaID = @commessaID"
                + " AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventocommessautenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND CommessaID = @commessaID"
                + " AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventocommessautenti(TipoEvento, commessaID, commessaAnno, userID) VALUES(@tipoEvento, @commessaID, @commessaAnno, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventocommessagruppi(TipoEvento, commessaID, commessaAnno, idGruppo) VALUES(@tipoEvento, @commessaID, @commessaAnno, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
                cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessagruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "commessaID = @commessaID AND "
                + "idGruppo = @idGruppo"
                + " AND commessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventocommessautenti WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "CommessaID = @commessaID AND "
                + "userID LIKE @userID"
                + " AND CommessaAnno = @commessaAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@commessaID", this.CommessaID);
            cmd.Parameters.AddWithValue("@userID", usr.username);
            cmd.Parameters.AddWithValue("@commessaAnno", this.CommessaAnno);

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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ritardominimodasegnalare FROM eventoarticoloconfig WHERE "
                    + " TipoEvento LIKE @tipoEvento"
                    + " AND ArticoloID = @articoloID"
                    + " AND ArticoloAnno = @articoloAnno";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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
                    cmd.CommandText = "UPDATE eventoarticoloconfig set RitardoMinimoDaSegnalare = @ritardoMinimo"
                        + " WHERE tipoevento LIKE @tipoEvento"
                        + " AND ArticoloID = @articoloID"
                        + " AND ArticoloAnno = @articoloAnno";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO eventoarticoloconfig(TipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare) VALUES("
                        + "@tipoEvento, @articoloID, @articoloAnno, @ritardoMinimo)";
                }
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
                cmd.Parameters.AddWithValue("@ritardoMinimo",
                    value.Hours.ToString() + ":" + value.Minutes.ToString() + ":" + value.Seconds.ToString());

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


        public ConfigurazioneRitardoArticolo(String Tenant, Articolo art)
            : base(Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare FROM eventoarticoloconfig WHERE ArticoloID = "
                + "@articoloID AND ArticoloAnno = @articoloAnno"
                + " AND TipoEvento LIKE @tipoEvento";
            cmd.Parameters.AddWithValue("@articoloID", art.ID);
            cmd.Parameters.AddWithValue("@articoloAnno", art.Year);
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventoarticologruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND ArticoloID = @articoloID"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventoarticoloutenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND ArticoloID = @articoloID"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticoloutenti(TipoEvento, ArticoloID, ArticoloAnno, userID) VALUES(@tipoEvento, @articoloID, @articoloAnno, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticologruppi(TipoEvento, ArticoloID, ArticoloAnno, idGruppo) VALUES(@tipoEvento, @articoloID, @articoloAnno, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticologruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "ArticoloID = @articoloID AND "
                + "idGruppo = @idGruppo"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticoloutenti WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "ArticoloID = @articoloID AND "
                + "userID LIKE @userID AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@userID", usr.username);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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

        public ConfigurazioneWarningArticolo(String Tenant, Articolo art)
            : base(Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tipoEvento, ArticoloID, ArticoloAnno, RitardoMinimoDaSegnalare FROM eventoarticoloconfig WHERE ArticoloID = "
                + "@articoloID"
                + " AND ArticoloAnno = @articoloAnno"
                + " AND TipoEvento LIKE @tipoEvento";
            cmd.Parameters.AddWithValue("@articoloID", art.ID);
            cmd.Parameters.AddWithValue("@articoloAnno", art.Year);
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idGruppo FROM eventoarticologruppi WHERE "
                + " tipoevento = @tipoEvento"
                + " AND ArticoloID = @articoloID"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM eventoarticoloutenti WHERE "
                + " tipoevento = @tipoEvento"
                + " AND ArticoloID = @articoloID"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticoloutenti(TipoEvento, ArticoloID, ArticoloAnno, userID) VALUES(@tipoEvento, @articoloID, @articoloAnno, @userID)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
                cmd.Parameters.AddWithValue("@userID", curr.username);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO eventoarticologruppi(TipoEvento, ArticoloID, ArticoloAnno, idGruppo) VALUES(@tipoEvento, @articoloID, @articoloAnno, @idGruppo)";
                cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
                cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
                cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
                cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticologruppi WHERE "
                + "TipoEvento LIKE @tipoEvento AND "
                + "ArticoloID = @articoloID AND "
                + "idGruppo = @idGruppo"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@tipoEvento", this.TipoEvento.Nome);
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@idGruppo", grp.ID);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);
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

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM eventoarticoloutenti WHERE "
                + "ArticoloID = @articoloID AND "
                + "userID LIKE @userID"
                + " AND ArticoloAnno = @articoloAnno";
            cmd.Parameters.AddWithValue("@articoloID", this.ArticoloID);
            cmd.Parameters.AddWithValue("@userID", usr.username);
            cmd.Parameters.AddWithValue("@articoloAnno", this.ArticoloAnno);

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