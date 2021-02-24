/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

/* CHANGELOG
* 20130713
* Aggiunta classe UserPhoneNumber
* Aggiunta classe UserEmail
* Aggiunto attributo Email in User
* Aggiunto attributo PhoneNumber nella classe User
* Aggiunto metodo AddEmail nella classe User
* Aggiunto metodo loadEmails nella classe User
* Aggiunto metodo addPhoneNumber nella classe User
* Aggiunto metodo loadPhoneNumbers nella classe User
* 
* 20171221
* Add customer user's login feature!!!
* 
* 20181126 --> Added story 2018_12
*/

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using KIS.App_Sources;

namespace KIS.App_Code
{
    /*public class Group
    {
        protected String Tenant;
        public String log;
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }
        private String _Nome;
        public String Nome
        {
            get { return this._Nome; }
            set
            {
                if (this._ID != -1 && value.Length > 0)
                {
                    String strSQL = "UPDATE groupss SET nomeGruppo = @GroupName WHERE id = @ID";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();

                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@GroupName", value);
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    cmd.Transaction = trn;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        trn.Commit();
                        this._Nome = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        trn.Rollback();
                    }
                    conn.Close();
                }
            }
        }
        private String _Descrizione;
        public String Descrizione
        {
            get { return this._Descrizione; }
            set
            {
                if (this.ID != -1 && value.Length > 0)
                {
                    String strSQL = "UPDATE groupss SET descrizione = @desc WHERE id = @ID";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();

                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@desc", value);
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    cmd.Transaction = trn;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        trn.Commit();
                        this._Descrizione = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        trn.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private GruppoPermessi _Permessi;
        public GruppoPermessi Permessi
        {
            get { return this._Permessi; }
        }

        public Group(String tenant, int groupID)
        {
            this.Tenant = tenant;
            String strSQL = "SELECT * FROM groupss WHERE id = @ID";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@ID", groupID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            this._Permessi = new GruppoPermessi(this.Tenant, groupID);
            if(rdr.HasRows)
            {
                this._ID = rdr.GetInt32(0);
                this._Nome = rdr.GetString(1);
                this._Descrizione = rdr.GetString(2);
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
            }
            rdr.Close();
            conn.Close();
        }

        public Group(String tenant, String GroupName) : base()
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, nomegruppo, descrizione FROM groupss WHERE nomeGruppo = @GroupName";
            cmd.Parameters.AddWithValue("@GroupName", GroupName);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Nome = rdr.GetString(1);
                this._Descrizione = rdr.GetString(2);
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
            }
            rdr.Close();
            conn.Close();
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trn = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trn;
                

                try
                {
                    cmd.CommandText = "DELETE FROM gruppipermessi WHERE idgroup = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM groupss WHERE id = " + this.ID.ToString();
                    cmd.ExecuteNonQuery();
                    trn.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    trn.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        private List<VoceMenu> _VociDiMenu;
        public List<VoceMenu> VociDiMenu
        {
            get { return this._VociDiMenu; }
        }

        public void loadMenu()
        {
            this._VociDiMenu = new List<VoceMenu>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idVoce FROM menugruppi WHERE gruppo = @ID"
                    + " ORDER BY ordinamento";
                cmd.Parameters.AddWithValue("@ID", this.ID);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._VociDiMenu.Add(new VoceMenu(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public bool AddMenu(VoceMenu vm)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                this.loadMenu();
                int maxOrd = this.VociDiMenu.Count+1;
                cmd.CommandText = "INSERT INTO menugruppi(gruppo, idVoce, ordinamento) VALUES(@ID, @vmid, @maxOrd)";
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@vmid", vm.ID);
                cmd.Parameters.AddWithValue("@maxOrd", maxOrd);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
            }
            return rt;
        }

        public bool DeleteMenu(VoceMenu vm)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM menugruppi WHERE gruppo = @ID AND idVoce = @vmid";
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@vmid", vm.ID);
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
            }
            return rt;
        }

        /* Cambia l'ordinamento delle voci di menu
         * direzione = true: sposta in su
         * direzione = false: sposta in giu
         */
        /*public bool SpostaVoce(VoceMenu vm, bool direzione)
        {
            log = "Entro in SpostaVoce()<br />";
            bool ret = false;
            if (this.ID != -1)
            {
                this.loadMenu();
                // Trovo la voce di menu attuale
                int indVM = -1;
                for (int i = 0; i < this.VociDiMenu.Count; i++)
                {
                    if (vm.ID == this.VociDiMenu[i].ID)
                    {
                        indVM = i;
                    }
                }


                if (indVM != -1)
                {
                    log = "Entro nella funzione.<br />Mi occupo dell'item: " + indVM.ToString();
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    if (direzione == true)
                    {
                        log += "Sposto in su<br/>";
                        if (indVM == 0)
                        {
                            // Non sposto niente
                        }
                        else
                        {
                            MySqlTransaction tr = conn.BeginTransaction();
                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Transaction = tr;
                            try
                            {
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = @ordinamento1"
                                    + " WHERE gruppo = @ID"
                                    + " AND idVoce = @idVoce1";
                                cmd.Parameters.AddWithValue("@ordinamento1", (indVM - 1));
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@idVoce1", this.VociDiMenu[indVM].ID);

                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = @ordinamento2" 
                                    + " WHERE gruppo = @ID "
                                    + " AND idVoce = @idVoce2";
                                cmd.Parameters.AddWithValue("@ordinamento2", indVM);
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@idVoce2", this.VociDiMenu[indVM - 1].ID);
                                cmd.ExecuteNonQuery();
                                tr.Commit();
                                ret = true;
                            }
                            catch(Exception ex)
                            {
                                ret = false;
                                log = ex.Message;
                                tr.Rollback();
                            }
                        }
                    }
                    else
                    {
                        log += "Sposto in giu<br/>";
                        if (indVM >= this.VociDiMenu.Count - 1)
                        {
                            // Non sposto niente
                        }
                        else
                        {
                            MySqlTransaction tr = conn.BeginTransaction();
                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Transaction = tr;
                            try
                            {
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = @ordinamento1"
                                    + " WHERE gruppo = @ID"
                                    + " AND idVoce = @idVoce1";
                                cmd.Parameters.AddWithValue("@ordinamento1", (indVM + 1));
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@idVoce1", this.VociDiMenu[indVM].ID);
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = @ordinamento2"
                                    + " WHERE gruppo = @ID" 
                                    + " AND idVoce = @idVoce2";
                                cmd.Parameters.AddWithValue("@ordinamento2", indVM);
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@idVoce2", this.VociDiMenu[indVM + 1].ID);
                                cmd.ExecuteNonQuery();
                                tr.Commit();
                                ret = true;
                            }
                            catch(Exception ex)
                            {
                                ret = false;
                                log = ex.Message;
                                tr.Rollback();
                            }
                        }
                    }
                    conn.Close();
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
        }

        private List<String> _Utenti;
        public List<String> Utenti
        {
            get
            {
                return this._Utenti;
            }
        }

        public void loadUtenti()
        {
            this._Utenti = new List<string>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT user FROM groupusers WHERE groupID = @ID";
                cmd.Parameters.AddWithValue("@ID", this.ID);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Utenti.Add(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public List<Reparto> SegnalazioneRitardiReparto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Ritardo' "
                        + "AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }
        }

        public List<Commessa> SegnalazioneRitardiCommessa
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }

        public List<Articolo> SegnalazioneRitardiArticolo
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }

        public List<Reparto> SegnalazioneWarningReparto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Warning' "
                        + "AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }
        }

        public List<Commessa> SegnalazioneWarningCommessa
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }

        public List<Articolo> SegnalazioneWarningArticolo
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }
    }*/

   /* public class GroupList
    {
        protected String Tenant;
        public String log;

        public List<Group> Elenco;

        public GroupList(String tenant)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM groupss ORDER BY nomeGruppo";
            MySqlDataReader rdr = cmd.ExecuteReader();
            Elenco = new List<Group>();
            while (rdr.Read())
            {
                Elenco.Add(new Group(this.Tenant, rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public bool Add(String nomeG, String descG)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM groupss";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            else
            {
                maxID = 0;
            }
            rdr.Close();

            MySqlTransaction trn = conn.BeginTransaction();
            cmd.Transaction = trn;
            cmd.CommandText = "INSERT INTO groupss(id, nomeGruppo, descrizione) VALUES(@ID, @NAME, @DESC)";
            cmd.Parameters.AddWithValue("@ID", maxID);
            cmd.Parameters.AddWithValue("@NAME", nomeG);
            cmd.Parameters.AddWithValue("@DESC", descG);
            try
            {
                cmd.ExecuteNonQuery();
                trn.Commit();
                rt = true;
            }
            catch(Exception ex)
            {
                rt = false;
                log = ex.Message;
                trn.Rollback();
            }
            conn.Close();
            return rt;
        }
    }
    */
    /*public class GruppoPermesso
    {
        protected String Tenant;
        public String log;

        private int _GroupID;
        public int GroupID
        {
            get
            {
                return this._GroupID;
            }
        }

        public Permission Permes;

        private int _IdPermesso;
        public int IdPermesso
        {
            get { return this._IdPermesso; }
        }

        private String _NomePermesso;
        public String NomePermesso
        {
            get { return this._NomePermesso; }
        }
        private String _PermessoDesc;
        public String PermessoDesc
        {
            get { return this._PermessoDesc; }
        }

        private bool _R;
        public bool R
        {
            get { return this._R; }
            set
            {
                
                if (this.GroupID != -1 && this.IdPermesso != -1)
                {
                    
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = @IDGroup"+
                        " AND idpermesso = @IDPermesso";
                    cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                    cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        recExists = true;
                    }
                    else
                    {
                        recExists = false;
                    }
                    rdr.Close();

                    log = "OK " + recExists.ToString();

                    if (recExists)
                    {
                        cmd.CommandText = "UPDATE gruppipermessi SET r = @r WHERE idGroup = @IDGroup"
                            + " AND idpermesso = @IDPermesso";
                        
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(@IDGroup, @IDPermesso, @r, false, false)";
                    }
                    cmd.Parameters.AddWithValue("@r", value);

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
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
        }
        private bool _W;
        public bool W
        {
            get { return this._W; }
            set
            {
                if (this.GroupID != -1 && this.IdPermesso != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = @IDGroup" + 
                        " AND idpermesso = @IDPermesso";
                    cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                    cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        recExists = true;
                    }
                    else
                    {
                        recExists = false;
                    }
                    rdr.Close();

                    if (recExists)
                    {
                        cmd.CommandText = "UPDATE gruppipermessi SET w = @w WHERE idGroup = @IDGroup AND idpermesso = @IDPermesso";
                        cmd.Parameters.AddWithValue("@w", value);
                        cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                        cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(@IDGroup, @IDPermesso, false, @w, false)";
                        cmd.Parameters.AddWithValue("@w", value);
                        cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                        cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);

                    }

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
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
        }

        private bool _X;
        public bool X
        {
            get { return this._X; }
            set
            {
                if (this.GroupID != -1 && this.IdPermesso != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = @IDGroup AND idpermesso = @IDPermesso";
                    cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                    cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        recExists = true;
                    }
                    else
                    {
                        recExists = false;
                    }
                    rdr.Close();

                    if (recExists)
                    {
                        cmd.CommandText = "UPDATE gruppipermessi SET x = @x WHERE idGroup = @IDGroup AND idpermesso = @IDPermesso";
                        cmd.Parameters.AddWithValue("@x", value);
                        cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                        cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(@IDGroup, @IDPermesso, false, false, @x)";
                        cmd.Parameters.AddWithValue("@x", value);
                        cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                        cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    }
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
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
        }

        public GruppoPermesso(String tenant, int grp, Permission prm)
        {
            this.Tenant = tenant;
            bool check = false;
            if (prm.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM groupss WHERE id = @ID";
                cmd.Parameters.AddWithValue("@ID", grp);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    check = true;
                }
                rdr.Close();
                conn.Close();
            }

            if (check == true)
            {
                this._GroupID = grp;
                this.Permes = new Permission(this.Tenant, prm.ID);
                this._NomePermesso = Permes.Nome;
                this._PermessoDesc = Permes.Descrizione;
                this._IdPermesso = Permes.ID;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT r, w, x FROM gruppipermessi WHERE idgroup = @GroupID AND idpermesso = @PermesID";
                cmd.Parameters.AddWithValue("@GroupID", grp);
                cmd.Parameters.AddWithValue("@PermesID", Permes.ID);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._R = rdr.GetBoolean(0);
                    this._W = rdr.GetBoolean(1);
                    this._X = rdr.GetBoolean(2);
                }
                else
                {
                    this._R = false;
                    this._W = false;
                    this._X = false;
                    
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._GroupID = -1;
                this.Permes = null;
                this._R = false;
                this._W = false;
                this._X = false;
            }
        }
    }*/
    /*
    public class GruppoPermessi
    {
        protected String Tenant;
        public List<GruppoPermesso> Elenco;
        private int _GroupID;
        public int GroupID
        {
            get
            {
                return this._GroupID;
            }
        }

        public GruppoPermessi(String tenant, int grp)
        {
            this.Tenant = tenant;
            bool check = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM groupss WHERE id = @GroupID";
            cmd.Parameters.AddWithValue("@GroupID", grp);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                check = true;
            }
            else
            {
                check = false;
            }
            rdr.Close();
            conn.Close();

            if (check == true)
            {
                this._GroupID = grp;
                this.Elenco = new List<GruppoPermesso>();
                ElencoPermessi elPrm = new ElencoPermessi(this.Tenant);
                for (int i = 0; i < elPrm.Elenco.Count; i++)
                {
                    this.Elenco.Add(new GruppoPermesso(this.Tenant, this.GroupID, elPrm.Elenco[i]));
                }

            }
            else
            {
                this._GroupID = -1;
                this.Elenco = null;
            }
        }
    }*/

    public class UserList
    {
        protected String Tenant;
        public User[] elencoUtenti;
        public List<User> listUsers;
        private int _numUsers;
        public int numUsers
        {
            get { return this._numUsers; }
        }

        public UserList(String workspace)
        {
            // String strSQL = "SELECT COUNT(userID) FROM useraccounts INNER JOIN WHERE verified = true AND enabled = true ORDER BY userID";
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            /*MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            this._numUsers = rdr1.GetInt32(0);
            elencoUtenti = new User[this.numUsers];
            rdr1.Close();*/

            listUsers = new List<User>();

            String strSQL = "SELECT * FROM useraccounts INNER JOIN useraccountworkspaces ON (useraccounts.id=useraccountworkspaces.userid) WHERE workspaceid=@workspaceid";
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@workspaceid", workspace);
            MySqlDataReader rdr = cmd.ExecuteReader();
            int i = 0;
            while (i < this.numUsers && rdr.Read())
            {
                elencoUtenti[i] = new User(rdr.GetString(0));
                listUsers.Add(new User(rdr.GetString(0)));
                i++;
            }
            rdr.Close();
            conn.Close();
        }

        public UserList(int workspaceid)
        {
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            listUsers = new List<User>();
            String strSQL = "SELECT * FROM useraccounts INNER JOIN useraccountworkspaces ON (useraccounts.id=useraccountworkspaces.userid) WHERE workspaceid=@workspaceid";
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@workspaceid", workspaceid);
            MySqlDataReader rdr = cmd.ExecuteReader();
            int i = 0;
            while (i < this.numUsers && rdr.Read())
            {
                elencoUtenti[i] = new User(rdr.GetString(0));
                listUsers.Add(new User(rdr.GetString(0)));
                i++;
            }
            rdr.Close();
            conn.Close();
        }

        public UserList(int workspaceid, Permission prm)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this.listUsers = new List<User>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT(useraccounts.id) FROM useraccounts "
                       + " INNER JOIN useraccountsgroups ON(useraccounts.id = useraccountsgroups.userid) "
                       + "  INNER JOIN useraccountworkspaces ON(useraccounts.id= useraccountworkspaces.userid) "
                       + "   INNER JOIN groupspermissions ON(useraccountsgroups.groupid = groupspermissions.groupid) "
                       + "   INNER JOIN permissions ON "
                       + "   (groupspermissions.permissionid = permissions.id) "
                       + "   WHERE 1 = 1  "
                       + "   AND permissions.id=@idpermesso "
                       + "   AND useraccountworkspaces.workspaceid = @workspaceid";
            cmd.Parameters.AddWithValue("@idpermesso", prm.ID);
            cmd.Parameters.AddWithValue("@workspaceid", workspaceid);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.listUsers.Add(new User(rdr.GetString(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class User
    {
        protected String Tenant;
        public String log;

        private bool _authenticated;
        public bool authenticated
        {
            get { return _authenticated; }
        }
        
        private String _username;
        public String username
        {
            get
            {
                return this._username;
            }
        }

        private String _name;
        public String name
        {
            get { return this._name; }
            set
            {
                if(value.Length > 0)
                {
                    String strSQL = "UPDATE users SET nome=@name WHERE userID LIKE @username";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@name", value);
                    cmd.Parameters.AddWithValue("@username", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._name = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private String _cognome;
        public String cognome
        {
            get { return this._cognome; }
            set
            {
                if (value.Length > 0)
                {
                    String strSQL = "UPDATE users SET cognome=@lastname WHERE userID LIKE @username";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@lastname", value);
                    cmd.Parameters.AddWithValue("@username", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._cognome = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public String FullName
        {
            get { return this.name + " " + this.cognome; }
        }

        private String _typeOfUser;
        public String typeOfUser
        {
            get { return this._typeOfUser; }
        }

        private DateTime _lastLogin;
        public DateTime lastLogin
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(_lastLogin, fuso.tzFusoOrario);
            }
            set
            {
                if (this.authenticated == true && this.username.Length > 0)
                {
                    DateTime lastL = new DateTime(value.Ticks, DateTimeKind.Local);
                    FusoOrario fuso = new FusoOrario(this.Tenant);
                    string strSQL = "UPDATE users SET lastLogin = @lastlogin WHERE userID LIKE @username";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@lastlogin", TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@username", this.username);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private String _Language;
        public String Language
        {
            get { return _Language; }
            set {
                if(value.Length <=5)
                { 
                string strSQL = "UPDATE users SET language = @language WHERE userID LIKE @userid";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@language", value);
                    cmd.Parameters.AddWithValue("@userid", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                
                conn.Close();
                }
            }
        }

        /* Destination URL after login */
        private String _DestinationURL;
        public String DestinationURL
        {
            get
            {
                return this._DestinationURL;
            }
            set
            {
                if (value.Length <= 255)
                {
                    string strSQL = "UPDATE users SET destinationURL = @destinationURL WHERE userID LIKE @userID";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@destinationURL", value);
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._DestinationURL = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public Boolean FullyConfigured
        {
            get
            {
                Boolean ret = false;
                this.loadGruppi();
                ret = this.Gruppi.Count > 0 ? true : false;

                return ret;
            }
        }

        private DateTime _CreationDate;
        public DateTime CreationDate
        {
            get { return this._CreationDate; }
        }

        private Boolean _Enabled;
        public Boolean Enabled
        {
            get { return this._Enabled; }
            set
            {
                if(this.username.Length > 0)
                { 
                    string strSQL = "UPDATE users SET enabled = @enabled WHERE userID LIKE @userID";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@enabled", value);
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Enabled = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();
                }
            }
        }

        public List<Group> Gruppi;

        public List<String> Customers;

        /* Returns the productivity of the operator.
         * To load values, use methods
         *      loadProductivity(int TaskID) --> Returns Productivity of the TaskID task
         *      loadProductivity(DateTime start, DateTime end) --> Returns average Productivity of all tasks ended between start and end
         */      
        private Double _Productivity;
        public Double Productivity
        {
            get
            {
                return this._Productivity;
            }
        }

        /* Returns the occupation KPI of the operator, defined as the ratio between real working time (without considering superposition of tasks) and the shift planned hours
         * To load the value, use LoadOccupation(DateTime start, DateTime end) method
         */
        private Double _Occupation;
        public Double Occupation
        {
            get
            {
                return this._Occupation;
            }
        }

        public User(String tenant, String usr, String pwd)
        {
            this.Tenant = tenant;
            this._Language = "";
            this._homeBoxes = null;
            this._authenticated = false;
            this._DestinationURL = "";
            this.Customers = new List<String>();
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID, language, creationdate, destinationURL, enabled "
                +" FROM users WHERE enabled=true AND verified = true AND userID LIKE @user AND password = MD5(@pwd)";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@user", usr);
            cmd.Parameters.AddWithValue("@pwd", pwd);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            if (rdr1.Read() && !rdr1.IsDBNull(0))
            {
                this._username = rdr1.GetString(0);
                this._name = rdr1.GetString(1);
                this._cognome = rdr1.GetString(2);
                this._typeOfUser = rdr1.GetString(3);
                if (!rdr1.IsDBNull(4))
                {
                    this._lastLogin = rdr1.GetDateTime(4);
                }
                this._authenticated = true;
                FusoOrario fuso = new FusoOrario(this.Tenant);
                this.lastLogin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario);
                this._ID = rdr1.GetInt32(5);
                if(!rdr1.IsDBNull(6))
                {
                    this._Language = rdr1.GetString(6);
                }
                else
                {
                    KISConfig kisCfg = new KISConfig(this.Tenant);
                    this._Language = kisCfg.Language;
                }
                if(!rdr1.IsDBNull(7))
                {
                    this._CreationDate = rdr1.GetDateTime(7);
                }
                if (!rdr1.IsDBNull(8))
                {
                    this._DestinationURL = rdr1.GetString(8);
                }
                this._Enabled = rdr1.GetBoolean(9);
            }
            else
            {
                this._username = "";
                this._Enabled = false;
            }
            rdr1.Close();
            conn.Close();
        }

        public User(String tenant, String usr)
        {
            this.Tenant = tenant;
            this.Customers = new List<String>();
            this._homeBoxes = null;
            this._authenticated = false;
            this._DestinationURL = "";
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID, language, creationdate, destinationURL, enabled "
                + " FROM users WHERE enabled=true AND userID LIKE @userID";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@userID", usr);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            if (rdr1.Read() && !rdr1.IsDBNull(0))
            {
                this._username = rdr1.GetString(0);
                this._name = rdr1.GetString(1);
                this._cognome = rdr1.GetString(2);
                this._typeOfUser = rdr1.GetString(3);
                this._lastLogin = rdr1.GetDateTime(4);
                this._authenticated = false;
                this._ID = rdr1.GetInt32(5);
                if (!rdr1.IsDBNull(6))
                {
                    this._Language = rdr1.GetString(6);
                }
                else
                {
                    KISConfig kisCfg = new KISConfig(this.Tenant);
                    this._Language = kisCfg.Language;
                }
                if (!rdr1.IsDBNull(7))
                {
                    this._CreationDate = rdr1.GetDateTime(7);
                }
                if (!rdr1.IsDBNull(8))
                {
                    this._DestinationURL = rdr1.GetString(8);
                }
                this._Enabled = rdr1.GetBoolean(9);
            }
            else
            {
                this._username = "";
                this._Enabled = false;
            }
            rdr1.Close();
            conn.Close();
        }

        public User(String tenant, int IDn)
        {
            this.Tenant = tenant;
            this.Customers = new List<String>();
            this._homeBoxes = null;
            this._authenticated = false;
            this._DestinationURL = "";
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID, language, creationdate, destinationURL, enabled "
                + " FROM users WHERE enabled=true AND ID = @ID";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@ID", IDn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            if (rdr1.Read() && !rdr1.IsDBNull(0))
            {
                this._username = rdr1.GetString(0);
                this._name = rdr1.GetString(1);
                this._cognome = rdr1.GetString(2);
                this._typeOfUser = rdr1.GetString(3);
                this._lastLogin = rdr1.GetDateTime(4);
                this._authenticated = false;
                this._ID = rdr1.GetInt32(5);
                if (!rdr1.IsDBNull(6))
                {
                    this._Language = rdr1.GetString(6);
                }
                else
                {
                    KISConfig kisCfg = new KISConfig(this.Tenant);
                    this._Language = kisCfg.Language;
                }
                if (!rdr1.IsDBNull(7))
                {
                    this._CreationDate = rdr1.GetDateTime(7);
                }
                if (!rdr1.IsDBNull(8))
                {
                    this._DestinationURL = rdr1.GetString(8);
                }
                this._Enabled = rdr1.GetBoolean(9);
            }
            else
            {
                this._username = "";
                this._Enabled = false;
            }
            rdr1.Close();
            conn.Close();
        }

        public User(String tenant)
        {
            this.Tenant = tenant;
            this._homeBoxes = null;
            this._username = "";
            this._typeOfUser = "";
            this._name = "";
            this._cognome = "";
            this._authenticated = false;
            KISConfig kisCfg = new KISConfig(this.Tenant);
            this._Language = kisCfg.Language;
            this.Customers = new List<String>();
            this._DestinationURL = "";
            this._Enabled = false;
        }

         /* Returns:
         * checsum if user correctly added
         * 2 if user already exists
         * 0 if generic error
         */
        public String add(String usr, String pwd, String nome, String cognome, String typeOf, String idioma,
            bool skipVerify, System.Net.Mail.MailAddress email)
        {
            String checksum = DateTime.UtcNow.Ticks.ToString();
            String rt = "0";
            String strSQL = "SELECT * FROM users WHERE userID LIKE @usr";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@usr", usr);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            if (rdr1.HasRows)
            {
                rt = "2";
                rdr1.Close();
            }
            else
            {
                rdr1.Close();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                int maxID = 0;
                cmd.CommandText = "SELECT MAX(ID) FROM users";
                rdr1 = cmd.ExecuteReader();
                if (rdr1.Read() && !rdr1.IsDBNull(0))
                {
                    maxID = rdr1.GetInt32(0) + 1;
                }
                rdr1.Close();

                try
                {
                    strSQL = "INSERT INTO users(userID, password, nome, cognome, tipoUtente, lastLogin, ID, language, "
                        + "verified, checksum, creationdate, enabled) VALUES(@userID, MD5(@password), @nome, @cognome, @tipoUtente, @lastLogin,"
                        + "@ID, @language, @verified, @checksum, "
                        + "@creationdate, @enabled"
                        + ")";
                    cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@userID", usr);
                    cmd.Parameters.AddWithValue("@password", pwd);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@cognome", cognome);
                    cmd.Parameters.AddWithValue("@tipoUtente", typeOf);
                    cmd.Parameters.AddWithValue("@lastLogin", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"));
                    cmd.Parameters.AddWithValue("@ID", maxID);
                    cmd.Parameters.AddWithValue("@language", idioma);
                    cmd.Parameters.AddWithValue("@verified", skipVerify);
                    cmd.Parameters.AddWithValue("@checksum", checksum);
                    cmd.Parameters.AddWithValue("@creationdate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@enabled", true);
                    cmd.ExecuteNonQuery();

                    tr.Commit();

                    rt = checksum;

                    if(email != null)
                    {
                        User curr = new User(username);
                        curr.addEmail(email.Address, "Default", true);
                    }
                }
                catch (Exception ex)
                {
                    rt = "0";
                    log = ex.Message;
                    tr.Rollback();
                }
            }
            rdr1.Close();
            conn.Close();
            return rt;
        }

        public bool loadGruppi()
        {
            bool rt;
            this.Gruppi = new List<Group>();
            if (this.username.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupID FROM groupusers INNER JOIN groupss ON (groupusers.groupID = groupss.ID) WHERE groupusers.user = @user " +
                    " ORDER BY groupss.nomeGruppo";
                cmd.Parameters.AddWithValue("@user", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Gruppi.Add(new Group(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool addGruppo(Group grp)
        {
            bool ret = false;
            if (this.username.Length > 0)
            {
                // Verifico che non ci sia già
                this.loadGruppi();
                bool check = false;
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    if (grp.ID == this.Gruppi[i].ID)
                    {
                        check = true;
                    }
                }

                if (check == false)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO groupusers(groupID, user) VALUES(@GroupID, @userID)";
                    cmd.Parameters.AddWithValue("@GroupID", grp.ID);
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;

                    try
                    {
                        
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        ret = false;
                        tr.Rollback();
                        
                    }
                    conn.Close();
                }
            }
            return ret;
        }

        public bool deleteGruppo(Group grp)
        {
            bool rt = false;
            if (this.username.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM groupusers WHERE groupID = @GroupID AND user = @userID";
                cmd.Parameters.AddWithValue("@GroupID", grp.ID);
                cmd.Parameters.AddWithValue("@userID", this.username);
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        private List<Postazione> _PostazioniAttive;
        public List<Postazione> PostazioniAttive
        {
            get { return this._PostazioniAttive; }
        }

        public void loadPostazioniAttive()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT postazione FROM registrooperatoripostazioni WHERE logout IS null AND username = @username";
            cmd.Parameters.AddWithValue("@username", this.username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._PostazioniAttive = new List<Postazione>();
            while (rdr.Read())
            {
                this._PostazioniAttive.Add(new Postazione(this.Tenant, rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public bool DoCheckIn(Postazione p)
        {
            bool rt = false;
            if (this.username.Length > 0)
            {
                this.loadPostazioniAttive();
                bool found = false;
                for (int i = 0; i < this.PostazioniAttive.Count; i++)
                {
                    if (p.id == this.PostazioniAttive[i].id)
                    {
                        found = true;
                    }
                }

                // Aggiungo solo se non ho trovato tra le postazioni attive
                if (found == false)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO registrooperatoripostazioni(username, postazione, login, logout) VALUES("
                        + "@username, @postazione, @login, @logout)";
                    cmd.Parameters.AddWithValue("@username", this.username);
                    cmd.Parameters.AddWithValue("@postazione", p.id);
                    cmd.Parameters.AddWithValue("@login", DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@logout", null);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        rt = true;
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        rt = false;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
            return rt;
        }

        public bool DoCheckOut(Postazione p)
        {
            bool rt = false;
            // Verifico che l'utente non abbia tasks avviati nella postazione
            bool checkTaskAvviati = false;
            p.loadTaskAvviati(this);
            if (p.TaskAvviatiUtente.Count == 0)
            {
                checkTaskAvviati = false;
            }
            else
            {
                checkTaskAvviati = true;
            }
            if (checkTaskAvviati == false)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE registrooperatoripostazioni SET logout='" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "'"
                    + " WHERE logout IS null AND username = @username AND postazione = @postazione";
                cmd.Parameters.AddWithValue("@logout", DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@username", this.username);
                cmd.Parameters.AddWithValue("@postazione", p.id);
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
            }
            return rt;
        }

        private List<int> _TaskAvviati;
        public List<int> TaskAvviati
        {
            get { return this._TaskAvviati; }
        }

        private List<int> _ExecutableTasks;
        public List<int> ExecutableTasks
        {
            get { return this._ExecutableTasks; }
        }

        public void loadTaskAvviati()
        {
            this._TaskAvviati = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tasksproduzione.taskID, evento FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON("
                + "tasksproduzione.taskID = registroeventitaskproduzione.task) WHERE tasksproduzione.status = 'I' "
                + " AND registroeventitaskproduzione.user = @user ORDER BY registroeventitaskproduzione.data DESC";
            cmd.Parameters.AddWithValue("@user", this.username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> DaNonInserire = new List<int>();
            while (rdr.Read())
            {
                log += "Task: " + rdr.GetInt32(0).ToString() + " " + rdr.GetChar(1);
                if (rdr.GetChar(1) == 'P' || rdr.GetChar(1) == 'F')
                {
                    log += " da non inserire<br/>";
                    DaNonInserire.Add(rdr.GetInt32(0));
                }
                else if (rdr.GetChar(1) == 'I')
                {
                    log += " da verificare --> ";
                    // Verifico che non sia nella lista di quelli da non inserire, e nemmeno in quella dei già inseriti!
                    bool checkN = false;
                    bool checkI = false;
                    for (int q = 0; q < DaNonInserire.Count; q++)
                    {
                        if (DaNonInserire[q] == rdr.GetInt32(0))
                        {
                            log += " da non inserire";
                            checkN = true;
                        }
                    }
                    for (int q = 0; q < this._TaskAvviati.Count; q++)
                    {
                        if (this._TaskAvviati[q] == rdr.GetInt32(0))
                        {
                            log += " già inserito";
                            checkI = true;
                        }
                    }
                    if (checkN == false && checkI == false)
                    {
                        log += "aggiunto.<br/>";
                        this._TaskAvviati.Add(rdr.GetInt32(0));
                    }
                    else
                    {
                        log += "<br/>";
                    }
                }
            }
            rdr.Close();
            conn.Close();
        }

        // Verifica se l'utente ha tutti i permessi contenuti in elencoPrm. Ogni elemento è un array String[2].
        // elencoPrm[i][0] contiene il nome del permesso
        // elencoPrm[i][1] contiene il tipo di azione (R, W, X)
        public bool ValidatePermessi(List<String[]> elencoPrm)
        {
            bool rt = false;
            List<bool> trovato = new List<bool>();
            for (int i = 0; i < elencoPrm.Count; i++)
            {
                bool found = false;
                this.loadGruppi();
                for (int j = 0; j < this.Gruppi.Count; j++)
                {
                    for (int k = 0; k < this.Gruppi[j].Permessi.Elenco.Count; k++)
                    {
                        if (this.Gruppi[j].Permessi.Elenco[k].NomePermesso == elencoPrm[i][0])
                        {
                            if (elencoPrm[i][1] == "R" && this.Gruppi[j].Permessi.Elenco[k].R == true)
                            {
                                found = true;
                            }
                            else if (elencoPrm[i][1] == "W" && this.Gruppi[j].Permessi.Elenco[k].W == true)
                            {
                                found = true;
                            }
                            else if (elencoPrm[i][1] == "X" && this.Gruppi[j].Permessi.Elenco[k].X == true)
                            {
                                found = true;
                            }
                            
                        }
                    }
                    
                }
                trovato.Add(found);
            }

            rt = true;
            for (int i = 0; i < trovato.Count; i++)
            {
                if (trovato[i] == false)
                {
                    rt = false;
                }
            }
            return rt;
        }

        public List<UserEmail> Email;
        public void loadEmails()
        {
            Email = new List<UserEmail>();
            if (this.username != "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT email FROM useremail WHERE userID LIKE @userID ORDER BY note";
                cmd.Parameters.AddWithValue("@userID", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Email.Add(new UserEmail(this.Tenant, this.username, rdr.GetString(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }
        public bool addEmail(String email, String note, bool forAlarm)
        {
            bool rt = false;
            System.Net.Mail.MailAddress mailAddr = null;
            try
            {
                mailAddr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                mailAddr = null;
            }
            if (mailAddr != null && this.username!= "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO useremail(userid, email, forAlarm, note) VALUES("
                    + "@userID, @email, @forAlarm, @note)";
                cmd.Parameters.AddWithValue("@userID", this.username);
                cmd.Parameters.AddWithValue("@email", mailAddr.Address);
                cmd.Parameters.AddWithValue("@forAlarm", forAlarm);
                cmd.Parameters.AddWithValue("@note", note);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public List<UserPhoneNumber> PhoneNumbers;
        public void loadPhoneNumbers()
        {
            PhoneNumbers = new List<UserPhoneNumber>();
            if (this.username != "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT phoneNumber FROM userphonenumbers WHERE userID LIKE @userID ORDER BY note";
                cmd.Parameters.AddWithValue("@userID", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.PhoneNumbers.Add(new UserPhoneNumber(this.Tenant, this.username, rdr.GetString(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }
        public bool addPhoneNumber(String phone, String note, bool forAlarm)
        {
            bool rt = false;
            double phoneINT = -1;
            try
            {
                phoneINT = Double.Parse(phone);
            }
            catch
            {
                phoneINT = -1;
            }
            if (phoneINT != -1 && this.username != "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO userphoneNumbers(userid, phoneNumber, forAlarm, note) VALUES("
                    + "@userid, @phoneNumber, @forAlarm, @note)";
                cmd.Parameters.AddWithValue("@userid", this.username);
                cmd.Parameters.AddWithValue("@phoneNumber", phone);
                cmd.Parameters.AddWithValue("@forAlarm", forAlarm);
                cmd.Parameters.AddWithValue("@note", note);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch (Exception ex)
                {
                    this.log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public String ResetPassword()
        {
            String newPass = "";
            if (this.username != "" && this.username.Length > 0)
            {
                newPass = System.Web.Security.Membership.GeneratePassword(16, 2);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE users SET password = MD5(@newPass) WHERE userID = @userID";
                cmd.Parameters.AddWithValue("@newPass", newPass);
                cmd.Parameters.AddWithValue("@userID", this.username);
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return newPass;
        }

        public List<Reparto> SegnalazioneRitardiReparto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                if (this.username.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT repartoID FROM eventorepartoutenti WHERE TipoEvento LIKE 'Ritardo' "
                        + "AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Reparto> SegnalazioneRitardiRepartoCompleto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                for (int i = 0; i < this.SegnalazioneRitardiReparto.Count; i++)
                {
                    ret.Add(this.SegnalazioneRitardiReparto[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Reparto> ritDept = this.Gruppi[i].SegnalazioneRitardiReparto(this.Tenant);
                    for (int j = 0; j < ritDept.Count; j++)
                    {
                        ret.Add(ritDept[j]);
                    }
                }
                ret = ret.GroupBy(p => p.id).Select(g => g.First()).ToList();
                ret = ret.OrderBy(r => r.name).ToList();
                return ret;
            }
        }

        public List<Commessa> SegnalazioneRitardiCommessa
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessautenti WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(this.Tenant , rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Commessa> SegnalazioneRitardiCommessaCompleto
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                for (int i = 0; i < this.SegnalazioneRitardiCommessa.Count; i++)
                {
                    ret.Add(this.SegnalazioneRitardiCommessa[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Commessa> ritComm = this.Gruppi[i].SegnalazioneRitardiCommessa(this.Tenant);
                    for (int j = 0; j < ritComm.Count; j++)
                    {
                        ret.Add(ritComm[j]);
                    }
                }
                ret = ret.GroupBy(p => new { p.ID, p.Year, p.Cliente, p.DataInserimento, p.Status }).Select(g => g.First()).ToList();
                return ret;
            }
        }

        public List<Articolo> SegnalazioneRitardiArticolo
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloID, ArticoloAnno FROM eventoarticoloutenti WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Articolo> SegnalazioneRitardiArticoloCompleto
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                for (int i = 0; i < this.SegnalazioneRitardiArticolo.Count; i++)
                {
                    ret.Add(this.SegnalazioneRitardiArticolo[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Articolo> ritArt = this.Gruppi[i].SegnalazioneRitardiArticolo(this.Tenant);
                    for (int j = 0; j < ritArt.Count; j++)
                    {
                        ret.Add(ritArt[j]);
                    }
                }
                ret = ret.GroupBy(p => new { p.ID, p.Year, p.Cliente, p.DataPrevistaFineProduzione, p.Status }).Select(g => g.First()).ToList();
                return ret;
            }
        }
    
        /* WARNING PERSONALI */
        public List<Reparto> SegnalazioneWarningReparto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                if (this.username.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT repartoID FROM eventorepartoutenti WHERE TipoEvento LIKE 'Warning' "
                        + "AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Reparto> SegnalazioneWarningRepartoCompleto
        {
            get
            {
                List<Reparto> ret = new List<Reparto>();
                for (int i = 0; i < this.SegnalazioneWarningReparto.Count; i++)
                {
                    ret.Add(this.SegnalazioneWarningReparto[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Reparto> wrnDept = this.Gruppi[i].SegnalazioneWarningReparto(this.Tenant);
                    for (int j = 0; j < wrnDept.Count; j++)
                    {
                        ret.Add(wrnDept[j]);
                    }
                }
                ret = ret.GroupBy(p => p.id).Select(g => g.First()).ToList();
                ret = ret.OrderBy(r => r.name).ToList();
                return ret;
            }
        }

        public List<Commessa> SegnalazioneWarningCommessa
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessautenti WHERE "
                        + "TipoEvento LIKE 'Warning' AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Commessa> SegnalazioneWarningCommessaCompleto
        {
            get
            {
                List<Commessa> ret = new List<Commessa>();
                for (int i = 0; i < this.SegnalazioneWarningCommessa.Count; i++)
                {
                    ret.Add(this.SegnalazioneWarningCommessa[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Commessa> wrnComm = this.Gruppi[i].SegnalazioneWarningCommessa(this.Tenant);
                    for (int j = 0; j < wrnComm.Count; j++)
                    {
                        ret.Add(wrnComm[j]);
                    }
                }
                ret = ret.GroupBy(p => new { p.ID, p.Year, p.Cliente, p.DataInserimento, p.Status }).Select(g => g.First()).ToList();
                return ret;
            }
        }

        public List<Articolo> SegnalazioneWarningArticolo
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloID, ArticoloAnno FROM eventoarticoloutenti WHERE "
                        + "TipoEvento LIKE 'Warning' AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
        }
        public List<Articolo> SegnalazioneWarningArticoloCompleto
        {
            get
            {
                List<Articolo> ret = new List<Articolo>();
                for (int i = 0; i < this.SegnalazioneWarningArticolo.Count; i++)
                {
                    ret.Add(this.SegnalazioneWarningArticolo[i]);
                }
                this.loadGruppi();
                for (int i = 0; i < this.Gruppi.Count; i++)
                {
                    List<Articolo> wrnArt = this.Gruppi[i].SegnalazioneWarningArticolo(this.Tenant);
                    for (int j = 0; j < wrnArt.Count; j++)
                    {
                        ret.Add(wrnArt[j]);
                    }
                }
                ret = ret.GroupBy(p => new { p.ID, p.Year, p.Cliente, p.DataPrevistaFineProduzione, p.Status }).Select(g => g.First()).ToList();
                return ret;
            }
        }

        public void logout()
        {
            this._authenticated = false;
            this._cognome = "";
            this._ID = -1;
            this._lastLogin = new DateTime(1970, 1, 1);
            this._name = "";
            this._PostazioniAttive = null;
            this._TaskAvviati = null;
            this._typeOfUser = "";
            this._username = "";
        }

        private List<IntervalliDiLavoroEffettivi> _IntervalliDiLavoroOperatore;
        public List<IntervalliDiLavoroEffettivi> IntervalliDiLavoroOperatore
        {
            get
            {
                return this._IntervalliDiLavoroOperatore;
            }
        }

        public void loadIntervalliDiLavoroOperatore()
        {
            log = "";
            this._IntervalliDiLavoroOperatore = new List<IntervalliDiLavoroEffettivi>();
            List<TaskProduzione> ElencoTasksOperatore = new List<TaskProduzione>();
            if (this.username.Length > 0 && !String.IsNullOrEmpty(this.username))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user=@userID"
                    + " ORDER BY data";
                cmd.Parameters.AddWithValue("@userID", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < ElencoTasksOperatore.Count; i++)
                {
                    if (ElencoTasksOperatore[i].Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND user LIKE @userID"
                             + " ORDER BY data";
                        cmd.Parameters.AddWithValue("@task", ElencoTasksOperatore[i].TaskProduzioneID);
                        cmd.Parameters.AddWithValue("@userID", this.username);
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            if (rdr.Read())
                            {
                                log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                    curr.user = usrI;
                                    curr.Inizio = inizio;
                                    curr.Fine = fine;
                                    curr.Intervallo = fine - inizio;
                                    curr.TaskID = ElencoTasksOperatore[i].TaskProduzioneID;
                                    curr.idPostazione = ElencoTasksOperatore[i].PostazioneID;
                                    Postazione pst = new Postazione(this.Tenant, ElencoTasksOperatore[i].PostazioneID);
                                    curr.nomePostazione = pst.name;
                                    curr.nomeTask = ElencoTasksOperatore[i].Name;
                                    curr.idProdotto = ElencoTasksOperatore[i].ArticoloID;
                                    curr.annoProdotto = ElencoTasksOperatore[i].ArticoloAnno;
                                    Articolo art = new Articolo(this.Tenant, ElencoTasksOperatore[i].ArticoloID, ElencoTasksOperatore[i].ArticoloAnno);
                                    curr.idReparto = art.Reparto;
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    this._IntervalliDiLavoroOperatore.Add(curr);
                                }
                            }
                        }
                        rdr.Close();
                    }
                }


                // -- FINE FUNZIONE CARICO INTERVALLI



                conn.Close();
            }
        }

        public void loadIntervalliDiLavoroOperatore(DateTime start, DateTime end)
        {
            log = "";
            this._IntervalliDiLavoroOperatore = new List<IntervalliDiLavoroEffettivi>();
            List<TaskProduzione> ElencoTasksOperatore = new List<TaskProduzione>();
            if (this.username.Length > 0 && !String.IsNullOrEmpty(this.username))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user='" + this.username + "' "
                    + " AND data >= @start AND data <= @end"
                    + " ORDER BY data";
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@end", end.AddDays(1).ToString("yyyy/MM/dd"));

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < ElencoTasksOperatore.Count; i++)
                {
                    if (ElencoTasksOperatore[i].Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento, id FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND user LIKE @user ORDER BY data";
                        cmd.Parameters.AddWithValue("@task", ElencoTasksOperatore[i].TaskProduzioneID);
                        cmd.Parameters.AddWithValue("@user", this.username);
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            int IDEventoI = rdr.GetInt32(3);
                            if (rdr.Read())
                            {
                                log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                int IDEventoF = rdr.GetInt32(3);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                    curr.user = usrI;
                                    curr.Inizio = inizio;
                                    curr.Fine = fine;
                                    curr.Intervallo = fine - inizio;
                                    curr.TaskID = ElencoTasksOperatore[i].TaskProduzioneID;
                                    curr.idPostazione = ElencoTasksOperatore[i].PostazioneID;
                                    //Postazione pst = new Postazione(ElencoTasksOperatore[i].PostazioneID);
                                    TaskProduzione tsk = new TaskProduzione(this.Tenant, ElencoTasksOperatore[i].TaskProduzioneID);
                                    curr.nomePostazione = tsk.PostazioneName;
                                    curr.nomeTask = ElencoTasksOperatore[i].Name;
                                    curr.idProdotto = ElencoTasksOperatore[i].ArticoloID;
                                    curr.annoProdotto = ElencoTasksOperatore[i].ArticoloAnno;
                                    Articolo art = new Articolo(this.Tenant, ElencoTasksOperatore[i].ArticoloID, ElencoTasksOperatore[i].ArticoloAnno);
                                    curr.idReparto = art.Reparto;
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    curr.ProductStatus = art.Status;
                                    curr.ragioneSocialeCliente = art.RagioneSocialeCliente;
                                    curr.EndEventStatus = EventoF;
                                    curr.StartEventID = IDEventoI;
                                    curr.EndEventID = IDEventoF;
                                    curr.TaskStatus = ElencoTasksOperatore[i].Status;
                                    curr.PlannedWorkingTime = tsk.TempoC;
                                    this._IntervalliDiLavoroOperatore.Add(curr);
                                }
                            }

                        }
                        rdr.Close();
                    }
                }


                // -- FINE FUNZIONE CARICO INTERVALLI



                conn.Close();
            }
        }

        public void loadIntervalliDiLavoroOperatore(int TaskID)
        {
            log = "";
            this._IntervalliDiLavoroOperatore = new List<IntervalliDiLavoroEffettivi>();
            if (this.username.Length > 0 && !String.IsNullOrEmpty(this.username))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                TaskProduzione currTask = new TaskProduzione(this.Tenant, TaskID);
                    if (currTask!=null && currTask.Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento, id FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND user LIKE @user"
                             + " ORDER BY data";
                    cmd.Parameters.AddWithValue("@task", currTask.TaskProduzioneID);
                    cmd.Parameters.AddWithValue("@user", this.username);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            int IDEventoI = rdr.GetInt32(3);
                            if (rdr.Read())
                            {
                                log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                int IDEventoF = rdr.GetInt32(3);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                    curr.user = usrI;
                                    curr.Inizio = inizio;
                                    curr.Fine = fine;
                                    curr.Intervallo = fine - inizio;
                                    curr.TaskID = currTask.TaskProduzioneID;
                                    curr.idPostazione = currTask.PostazioneID;
                                    //Postazione pst = new Postazione(ElencoTasksOperatore[i].PostazioneID);
                                    curr.nomePostazione = currTask.PostazioneName;
                                    curr.nomeTask = currTask.Name;
                                    curr.idProdotto = currTask.ArticoloID;
                                    curr.annoProdotto = currTask.ArticoloAnno;
                                    Articolo art = new Articolo(this.Tenant, currTask.ArticoloID, currTask.ArticoloAnno);
                                    curr.idReparto = art.Reparto;
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    curr.ProductStatus = art.Status;
                                    curr.ragioneSocialeCliente = art.RagioneSocialeCliente;
                                    curr.EndEventStatus = EventoF;
                                    curr.StartEventID = IDEventoI;
                                    curr.EndEventID = IDEventoF;
                                    curr.TaskStatus = currTask.Status;
                                    curr.PlannedWorkingTime = currTask.TempoC;
                                    this._IntervalliDiLavoroOperatore.Add(curr);
                                }
                            }

                        }
                        rdr.Close();
                    }
                //}


                // -- FINE FUNZIONE CARICO INTERVALLI



                conn.Close();
            }
        }

        public void loadIntervalliDiLavoroOperatore(List<int> TasksID)
        {
            log = "";
            this._IntervalliDiLavoroOperatore = new List<IntervalliDiLavoroEffettivi>();
            //List<TaskProduzione> ElencoTasksOperatore = new List<TaskProduzione>();
            if (this.username.Length > 0 && !String.IsNullOrEmpty(this.username))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                /*cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user='" + this.username
                    + "' ORDER BY data";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();*/

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < TasksID.Count; i++)
                {
                    TaskProduzione currTask = new TaskProduzione(this.Tenant, TasksID[i]);
                    if (currTask.Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND user LIKE @user"
                             + " ORDER BY data";
                        cmd.Parameters.AddWithValue("@task", TasksID[i]);
                        cmd.Parameters.AddWithValue("@user", this.username);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            if (rdr.Read())
                            {
                                log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                    curr.user = usrI;
                                    curr.Inizio = inizio;
                                    curr.Fine = fine;
                                    curr.Intervallo = fine - inizio;
                                    curr.TaskID = currTask.TaskProduzioneID;
                                    curr.idPostazione = currTask.PostazioneID;
                                    curr.nomePostazione = currTask.PostazioneName;
                                    curr.nomeTask = currTask.Name;
                                    curr.idProdotto = currTask.ArticoloID;
                                    curr.annoProdotto = currTask.ArticoloAnno;
                                    Articolo art = new Articolo(this.Tenant, currTask.ArticoloID, currTask.ArticoloAnno);
                                    curr.idReparto = art.Reparto;
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    this._IntervalliDiLavoroOperatore.Add(curr);
                                }
                            }
                        }
                        rdr.Close();
                    }
                }


                // -- FINE FUNZIONE CARICO INTERVALLI



                conn.Close();
            }
        }

        public void loadWorkTimespansAllStatus(DateTime start, DateTime end)
        {
            log = "";
            this._IntervalliDiLavoroOperatore = new List<IntervalliDiLavoroEffettivi>();
            List<TaskProduzione> ElencoTasksOperatore = new List<TaskProduzione>();
            if (this.username.Length > 0 && !String.IsNullOrEmpty(this.username))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user=@user "
                    + " AND data >= @start"
                    + " AND data <= @end"
                    + " ORDER BY data";
                cmd.Parameters.AddWithValue("@user", this.username);
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@end", end.ToString("yyyy/MM/dd"));
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < ElencoTasksOperatore.Count; i++)
                {
                    //if (ElencoTasksOperatore[i].Status == 'F')
                    //{
                        cmd.CommandText = "SELECT user, data, evento, id FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND user LIKE @user"
                             + " ORDER BY data";
                    cmd.Parameters.AddWithValue("@task", ElencoTasksOperatore[i].TaskProduzioneID);
                    cmd.Parameters.AddWithValue("@user", this.username);
                    rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            int IDEventoI = rdr.GetInt32(3);
                            if (rdr.Read())
                            {
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                int IDEventoF = rdr.GetInt32(3);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                    curr.user = usrI;
                                    curr.Inizio = inizio;
                                    curr.Fine = fine;
                                    curr.Intervallo = fine - inizio;
                                    curr.TaskID = ElencoTasksOperatore[i].TaskProduzioneID;
                                    curr.idPostazione = ElencoTasksOperatore[i].PostazioneID;
                                    Postazione pst = new Postazione(this.Tenant, ElencoTasksOperatore[i].PostazioneID);
                                    curr.nomePostazione = pst.name;
                                    curr.nomeTask = ElencoTasksOperatore[i].Name;
                                    curr.idProdotto = ElencoTasksOperatore[i].ArticoloID;
                                    curr.annoProdotto = ElencoTasksOperatore[i].ArticoloAnno;
                                    Articolo art = new Articolo(this.Tenant, ElencoTasksOperatore[i].ArticoloID, ElencoTasksOperatore[i].ArticoloAnno);
                                    curr.idReparto = art.Reparto;
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    curr.ProductStatus = art.Status;
                                    Cliente customer = new Cliente(this.Tenant, art.Cliente);
                                    curr.ragioneSocialeCliente = customer.RagioneSociale;
                                    curr.EndEventStatus = EventoF;
                                    curr.StartEventID = IDEventoI;
                                    curr.EndEventID = IDEventoF;
                                curr.TaskStatus = ElencoTasksOperatore[i].Status;
                                    this._IntervalliDiLavoroOperatore.Add(curr);
                                }
                            }

                        }
                        rdr.Close();
                    //}
                }


                // -- FINE FUNZIONE CARICO INTERVALLI



                conn.Close();
            }
        }


        /* Returns:
         * 0 if gener error
         * 1 if everything is ok
         * 2 if start event not found
         * 3 if end event not found
         * 4 if the start event and end event are not near
         * 5 if product is in "F" status
         * 6 if task not found
         */
        public int deleteIntervalloDiLavoroOperatore(int StartEventID, int EndEventID)
        {
            this.log = "";
            int ret = 0;
            // Check integrity before deleting
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            DateTime startD = new DateTime(2999, 1, 1);
            DateTime endD = new DateTime(1970, 1, 1);
            cmd.CommandText = "SELECT data, task FROM registroeventitaskproduzione WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", StartEventID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            int taskID = -1;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                startD = rdr.GetDateTime(0);
                taskID = rdr.GetInt32(1);
            }
            rdr.Close();
            cmd.CommandText = "SELECT data FROM registroeventitaskproduzione WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", EndEventID);
            rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                endD = rdr.GetDateTime(0);
            }
            rdr.Close();

            this.log = taskID + " " + startD.ToString("dd/MM/yyyy") + " " + endD.ToString("dd/MM/yyyy");

            if(taskID!=-1)
            {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, taskID);
                if(tsk!=null && tsk.TaskProduzioneID!=-1)
                {

                    // Check if product status is not 'F'
                    Articolo art = new Articolo(this.Tenant, tsk.ArticoloID, tsk.ArticoloAnno);
                    if(art!=null && art.ID!=-1 && art.Status!='F')
                    { 
                        if(startD < endD)
                        {
                            this.loadIntervalliDiLavoroOperatore(startD.AddDays(-2), endD.AddDays(2));
                            try
                            {
                                var itm = this.IntervalliDiLavoroOperatore.Where(x => x.StartEventID == StartEventID && x.EndEventID == EndEventID);
                                this.log = "Empty after loadIntervalliDiLavoro. ";
                                ret = 1;
                            }
                            catch(Exception ex)
                            {
                                ret = 4;
                                this.log = ex.Message;
                            }
                        }
                    }
                    else
                    {
                        ret = 5;
                    }
                }
                else
                {
                    ret = 6;
                }
            }
            else
            {
                ret = 6;
            }


            this.log = "Intermediate check: " + ret.ToString() + " ";
            // Everything is ok, delete!
            if(ret == 1)
            {
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                { 
                    cmd.CommandText = "DELETE FROM registroeventitaskproduzione WHERE id = @startEVid OR id = @endEVid";
                    cmd.Parameters.AddWithValue("@startEVid", StartEventID);
                    cmd.Parameters.AddWithValue("@endEVid", EndEventID);
                    this.log += cmd.CommandText + " ";
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    ret = 0;
                    tr.Rollback();
                    this.log += ex.Message;
                }
            }

            conn.Close();
            this.log += ret.ToString();
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if start >= end
         * 3 if producedQuantity > Planned Quantity
         * 4 if registration ends in the future
         * 5 if super position
         * 6 if user is currently working on the task
         * 7 if task not found
         * 8 if previous tasks were not ended
         * 9 if task not found or status == 'F'
         * 10 if error during the sql insert into commands in Task.Start function
         * 11 if user is already working on the max number of tasks
         * 12 if user is currently working on this task
         * 13 if user is not logged in the workstation
         * 14 if task is already in status F
         * 15 if user is not currently working on the task
         * 16 if all previous tasks are not finished
         * 17 if there are some parameters that needs to be defined
         * 18 if there are problems during the insert into queries
         */
        public int addIntervalloDiLavoroOperatore(TaskProduzione tsk, User usr, bool completed, int producedQuantity, DateTime start, DateTime end)
        {
            int ret = 0;
            this.log = "Entro 1";
            if(tsk !=null && tsk.TaskProduzioneID!=-1)
            {
                this.log = "Entro 2";
                if (start < end)
                {
                    Postazione p = new Postazione(this.Tenant, tsk.PostazioneID);
                    p.loadTaskAvviati(usr);
                    Boolean currentlyWorking = false;
                    for (int i = 0; i < p.TaskAvviatiUtente.Count; i++)
                    {
                        if (p.TaskAvviatiUtente[i] == tsk.TaskProduzioneID)
                        {
                            currentlyWorking = true;
                        }
                    }

                    if (!currentlyWorking)
                    {
                        this.log = "Entro 3";
                        if (producedQuantity <= tsk.QuantitaPrevista)
                        {
                            this.log = "Entro 4";
                            Reparto dept = new Reparto(this.Tenant, tsk.RepartoID);
                            if (TimeZoneInfo.ConvertTimeToUtc(end, dept.tzFusoOrario) < DateTime.UtcNow)
                            {
                                this.log = "Entro 5";
                                // check for superposition with other timespans
                                usr.loadWorkTimespansAllStatus(start.AddDays(-5), end.AddDays(5));

                                bool superPos = false;
                                foreach (var n in usr.IntervalliDiLavoroOperatore)
                                {
                                    if (n.TaskProduzioneID == tsk.TaskProduzioneID && ((end >= n.DataInizio && end <= n.DataFine) || (start >= n.DataInizio && start <= n.DataFine)))
                                    {
                                        superPos = true;
                                    }
                                }

                                //this.log += " superPos: " + superPos.ToString();

                                if (!superPos)
                                {


                                    this.log += " Entro 7";
                                    usr.DoCheckIn(p);
                                    int retS = tsk.Start(usr, start);
                                    switch (retS)
                                    {
                                        case 2: ret = 9; break;
                                        case 3: ret = 10; break;
                                        case 4: ret = 8; break;
                                        case 5: ret = 11; break;
                                        case 6: ret = 12; break;
                                        case 7: ret = 13; break;
                                        default: break;
                                    }

                                    this.log += tsk.log + " retS: " + retS.ToString();
                                    int retE = -1;
                                    if (retS == 1)
                                    {
                                        if (completed)
                                        {
                                            retE = tsk.Complete(usr, end);
                                            this.log = "tsk.Complete log: " + tsk.log;
                                            switch (retE)
                                            {
                                                case 2: ret = 14; break;
                                                case 3: ret = 15; break;
                                                case 4: ret = 16; break;
                                                case 5: ret = 17; break;
                                                case 6: ret = 18; break;
                                                default: break;
                                            }
                                        }
                                        else
                                        {
                                            this.log = " Pause: ";
                                            retE = tsk.Pause(usr, end);
                                            this.log += retE.ToString();
                                        }
                                    }
                                    else
                                    {
                                        //this.log += " retS: " + retS.ToString();
                                    }
                                    tsk.QuantitaProdotta = tsk.QuantitaPrevista;
                                    usr.DoCheckOut(p);

                                    if (retS == 1 && retE == 1)
                                    {
                                        ret = 1;
                                    }
                                }
                                else
                                {
                                    ret = 5;
                                }
                            }
                            else
                            {
                                ret = 4;
                            }
                        }
                        else
                        {
                            ret = 3;
                        }
                        
                    }
                    else
                    {
                        ret = 6;
                    }
                }                
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 7;
            }
            return ret;
        }

        /*Returns:
         * 0 if generic error
         * 1 if password has been changed correctly
         * 2 if old password does not match the current password
         */
        public int changePassword(String oldPass, String newPass)
        {
            int ret = 0;
            User curr = new User(this.username, oldPass);
            if (curr.authenticated == true)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE users SET password = MD5(@newPass) WHERE userID LIKE @userID";
                cmd.Parameters.AddWithValue("@newPass", newPass);
                cmd.Parameters.AddWithValue("@userID", this.username);
                try
                {
                    log = cmd.CommandText;
                    cmd.ExecuteNonQuery();
                    ret = 1;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    ret = 0;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = 2;
            }
            return ret;

        }

        private List<Articolo> _NextProgrammedProducts;
        public List<Articolo> NextProgrammedProducts { get { return this._NextProgrammedProducts; } }

        public void loadNextProgrammedProducts()
        {
            this._NextProgrammedProducts = new List<Articolo>();
            if (this.username.Length > 0)
            {
                List<Articolo> artList = new List<Articolo>();
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan where status <> 'I' AND status <> 'F' "
                    + " AND planner LIKE @planner ORDER BY dataPrevistaFineProduzione";
                cmd.Parameters.AddWithValue("@planner", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    artList.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();

                this._NextProgrammedProducts = artList.OrderBy(x => x.EarlyStart).ToList();
            }
        }

        private HomeBoxesListUser _homeBoxes;
        public HomeBoxesListUser homeBoxes
        {
            get { return this._homeBoxes; }
        }

        public void loadHomeBoxes()
        {
            this._homeBoxes = new HomeBoxesListUser(this.Tenant, this);
            /*if (this.username.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idHomeBox FROM homeboxesuser WHERE user ='"+this.username+"' ORDER BY ordine";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._homeBoxes.Add(new HomeBox(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }*/
        }

        public Boolean addHomeBox(HomeBox box)
        {
            Boolean rt = false;
            if (this.username.Length > 0 && box.ID!=-1)
            {
                this.loadHomeBoxes();
                int ordine = this.homeBoxes.Elenco.Count;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO homeboxesuser(idHomeBox, user, ordine) VALUES(@idHomeBox, @user, @ordine)";
                cmd.Parameters.AddWithValue("@idHomeBox", box.ID);
                cmd.Parameters.AddWithValue("@user", this.username);
                cmd.Parameters.AddWithValue("@ordine", ordine);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public Boolean deleteHomeBox(HomeBox box)
        {
            Boolean rt = false;
            if (this.username.Length > 0 && box.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM homeboxesuser WHERE idHomeBox = @idHomeBox AND user = @user";
                cmd.Parameters.AddWithValue("@idHomeBox", box.ID);
                cmd.Parameters.AddWithValue("@user", this.username);
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
            }
            return rt;
        }

        public void loadCustomer()
        {
            this.Customers = new List<String>();
            if(this.username.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT cliente FROM contatticlienti INNER JOIN users ON"
                    +"(contatticlienti.user = users.userID) WHERE userID=@user";
                cmd.Parameters.AddWithValue("@user", this.username);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read() && !rdr.IsDBNull(0))
                {
                    this.Customers.Add(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean Activate(String username, String checksum)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT checksum FROM users WHERE userID = @user";
            cmd.Parameters.AddWithValue("@user", this.username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            String chk = "";
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                chk = rdr.GetString(0);
            }
            rdr.Close();
            if(chk.Length > 0 && chk == checksum)
            {
                cmd.CommandText = "UPDATE users SET verified = true WHERE userID = @user";
                cmd.Parameters.AddWithValue("@user", username);
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
            }
            conn.Close();
            return ret;
        }

        public Boolean UserExists(String username)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE userID = @user";
            cmd.Parameters.AddWithValue("@user", username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                ret = true;
            }
            rdr.Close();
            conn.Close();
            return ret;
        }

        public void LoadExecutableTasks()
        {
            this._ExecutableTasks = new List<int>();
            if (this.username.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskuser.taskID FROM tasksproduzione INNER JOIN taskuser ON (tasksproduzione.taskid=taskuser.taskid) "
                    +" WHERE (status = 'N' OR status = 'I' OR status = 'P') "
                 + "AND taskuser.user = @User ORDER BY lateStart, earlyStart, idArticolo";
                cmd.Parameters.AddWithValue("@User", this.username);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    // Verifico che tutti i precedenti siano terminati (se ConstraintType=1) oppure se siano avviati (se ConstraintType=0)
                    TaskProduzione tsk = new TaskProduzione(this.Tenant, rdr.GetInt32(0));

                    if (tsk.TaskProduzioneID != -1)
                    {

                        tsk.loadPrecedenti();
                        bool controllo = true;
                        for (int i = 0; i < tsk.PreviousTasks.Count; i++)
                        {
                            TaskProduzione prec = new TaskProduzione(this.Tenant, tsk.PreviousTasks[i].NearTaskID);
                            if (tsk.PreviousTasks[i].ConstraintType == 0)
                            {
                                if (prec.Status == 'N')
                                {
                                    controllo = false;
                                }
                            }
                            else
                            {
                                if (prec.Status != 'F')
                                {
                                    controllo = false;
                                }
                            }
                        }
                        if (controllo == true)
                        {
                            this._ExecutableTasks.Add(rdr.GetInt32(0));
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        /*  Returns Productivity of the currend user in the TaskID task
         *  -1 if user did not work on the specific task
         *  -2 if task does not exist or task not ended
         *  Result is loaded in _Productivity variable
         */
        public void LoadProductivity(int TaskID)
        {
            this._Productivity = -1;
            TaskProduzione tsk = new TaskProduzione(this.Tenant, TaskID);
            if(tsk!=null && tsk.TaskProduzioneID!=-1 && tsk.Status == 'F')
            {
                this.loadIntervalliDiLavoroOperatore(TaskID);
                if(this.IntervalliDiLavoroOperatore!= null && this.IntervalliDiLavoroOperatore.Count > 0)
                { 
                    var RealWorkingTime = this.IntervalliDiLavoroOperatore.Sum(x => x.Intervallo.TotalSeconds);
                    Double PlannedWorkingTime = tsk.TempoC.TotalSeconds / tsk.NumOperatori;
                    this._Productivity = PlannedWorkingTime / RealWorkingTime;
                }
                else
                {
                    this._Productivity= -1;
                }
            }
            else
            {
                this._Productivity = -2;
            }
        }
       
        /*  Returns average Productivity of all tasks ended between start and end
         *  -1 if user did not work in the start-end timespan        
         *  -3 if start >= end
            Result is loaded in _Productivity variable
            */
        public void LoadProductivity(DateTime start, DateTime end)
        {
            if(start < end)
            {
                Double PlannedWorkingTime = 0.0;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(taskID), tempociclo, nOperatori FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON(tasksproduzione.taskID = registroeventitaskproduzione.task) "
                    +" WHERE user LIKE @user AND endDateReal >= @start AND endDateReal <= @end";

                cmd.Parameters.AddWithValue("@user", this.username);
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));

                List<int> taskIDs = new List<int>();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    taskIDs.Add(rdr.GetInt32(0));
                    if(!rdr.IsDBNull(1) && !rdr.IsDBNull(2))
                    {
                        TimeSpan tc = rdr.GetTimeSpan(1);
                        int nop = rdr.GetInt32(2);
                        if(nop>0)
                        { 
                            PlannedWorkingTime += tc.TotalSeconds / nop;
                        }
                    }
                    
                }
                rdr.Close();
                conn.Close();

                if(taskIDs.Count > 0)
                {
                    this.loadIntervalliDiLavoroOperatore(taskIDs);
                    if(this.IntervalliDiLavoroOperatore!=null && this.IntervalliDiLavoroOperatore.Count > 0)
                    {
                        var RealWorkingTime = this.IntervalliDiLavoroOperatore.Sum(x => x.Intervallo.TotalSeconds);
                        if(RealWorkingTime > 0)
                        { 
                            this._Productivity = PlannedWorkingTime / RealWorkingTime;
                        }
                        else
                        {
                            this._Productivity = -1;
                        }
                    }
                    else
                    {
                        this._Productivity = -1;
                    }
                }
                else
                {
                    this._Productivity = -1;
                }
            }
            else
            {
                this._Productivity = -3;
            }
        }

        /* Returns ratio between real working time (without superposition of time) and shift hours
         */
         public void LoadOccupation(DateTime start, DateTime end)
        {
            this.log = "";
            if(start <= end && this.username.Length > 0)
            {
                this._Occupation = 0.0;
                /* loads operator real working timespans*/
                this.loadIntervalliDiLavoroOperatore(start, end);
                this.log = "this.IntervalliDiLavoroOperatore \n <br />";
                
                List<IntervalliDiLavoroEffettivi> filteredlist1 = this.IntervalliDiLavoroOperatore;

                /* filter parallel tasks */
                var filteredlist = filteredlist1.OrderBy(x => x.Inizio).ToList();
                for (int i = 0; i < filteredlist.Count; i++)
                {
                    this.log += filteredlist[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + filteredlist[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " \n<br />";
                }

                for (int i = 0; i < filteredlist.Count -1;i++)
                {
                    if(filteredlist[i+1].Inizio <= filteredlist[i].Fine)
                    {
                        DateTime minstart = filteredlist[i].Inizio < filteredlist[i+1].Inizio ? filteredlist[i].Inizio : filteredlist[i + 1].Inizio;
                        DateTime maxend = filteredlist[i].Fine > filteredlist[i + 1].Fine ? filteredlist[i].Fine : filteredlist[i + 1].Fine;
                        IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                        curr.Inizio = minstart;
                        curr.Fine = maxend;
                        filteredlist.RemoveAt(i + 1);
                        filteredlist.RemoveAt(i);
                        filteredlist.Add(curr);
                        filteredlist = filteredlist.OrderBy(x => x.Inizio).ToList();
                        i = -1;
                    }
                }

                /* calculate working time */
                TimeSpan realworkingtime = new TimeSpan(0, 0, 0);
                this.log += "filteredlist \n <br />";
                for (int i = 0; i < filteredlist.Count; i++)
                {
                    this.log += filteredlist[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + filteredlist[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " \n<br />";
                    realworkingtime = realworkingtime.Add(filteredlist[i].Fine - filteredlist[i].Inizio);
                }

                /* loads shifts and calculates planned time*/
                // Find all the departments where the operator worked, laod shifts, merge timespans then calculate the sum
                List<IntervalliDiLavoroEffettivi> plannedshifts1 = new List<IntervalliDiLavoroEffettivi>();
                var deptList = this.IntervalliDiLavoroOperatore.GroupBy(x => x.idReparto).ToList();
                for(int i = 0; i < deptList.Count; i++)
                {
                    Reparto rp = new Reparto(this.Tenant, deptList[i].Key);
                    rp.loadTurni();
                    for(int j=0; j < rp.Turni.Count; j++)
                    {
                        rp.Turni[j].loadCalendario(start, end);
                        foreach(var m in rp.Turni[j].CalendarioTrn.Intervalli)
                        {
                            IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                            curr.Inizio = m.Inizio;
                            curr.Fine = m.Fine;
                            plannedshifts1.Add(curr);
                        }
                    }
                }

                var plannedshifts = plannedshifts1.OrderBy(x => x.Inizio).ToList();
                for(int i = 0; i <plannedshifts.Count-1; i++)
                {
                    if (plannedshifts[i + 1].Inizio <= plannedshifts[i].Fine)
                    {
                        DateTime minstart = plannedshifts[i].Inizio < plannedshifts[i + 1].Inizio ? plannedshifts[i].Inizio : plannedshifts[i + 1].Inizio;
                        DateTime maxend = plannedshifts[i].Fine > plannedshifts[i + 1].Fine ? plannedshifts[i].Fine : plannedshifts[i + 1].Fine;
                        IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                        curr.Inizio = minstart;
                        curr.Fine = maxend;
                        plannedshifts.RemoveAt(i + 1);
                        plannedshifts.RemoveAt(i);
                        plannedshifts.Add(curr);
                        plannedshifts = plannedshifts.OrderBy(x => x.Inizio).ToList();
                        i = -1;
                    }
                    else if (plannedshifts[i + 1].Inizio >= plannedshifts[i].Inizio && plannedshifts[i + 1].Fine <= plannedshifts[i].Inizio)
                    {
                        plannedshifts.RemoveAt(i + 1);
                        i = -1;
                    }
                }

                this.log += "Plannedshifts:";
                TimeSpan plannedworkingtime = new TimeSpan(0, 0, 0);
                for (int i = 0; i < plannedshifts.Count; i++)
                {
                    this.log += plannedshifts[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + plannedshifts[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " \n<br />";
                    plannedworkingtime = plannedworkingtime.Add(plannedshifts[i].Fine - plannedshifts[i].Inizio);
                }

                /* calculate the ratio and loads in Occupation */
                this._Occupation = realworkingtime.TotalSeconds / plannedworkingtime.TotalSeconds;


                this.log += "Real Working time: " + realworkingtime.TotalSeconds + "<br />";
                this.log += "Planned shifts time: " + plannedworkingtime.TotalSeconds;
                }
            else
            {
                this._Occupation = -1;
            }
        }

    }
    public class UserEmail
    {
        protected String Tenant;
        public String log;

        private String _UserID;
        public String UserID
        {
            get
            {
                return this._UserID;
            }
        }

        private String _Email;
        public String Email
        {
            get { return this._Email; }
        }

        private bool _ForAlarm;
        public bool ForAlarm
        {
            get { return this._ForAlarm; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE useremail SET forAlarm=@forAlarm WHERE userID LIKE @userID AND email LIKE @email";
                cmd.Parameters.AddWithValue("@forAlarm", value);
                cmd.Parameters.AddWithValue("@userID", this.UserID);
                cmd.Parameters.AddWithValue("@email", this.Email);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }

                conn.Close();
            }
        }

        private String _Note;
        public String Note
        {
            get { return this._Note; }
            set
            {
                if (this.Email != "" && this.UserID != "")
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE useremail SET note = @note WHERE userID LIKE @userID"
                        + " AND email LIKE @email";
                    cmd.Parameters.AddWithValue("@note", value);
                    cmd.Parameters.AddWithValue("@userID", this.UserID);
                    cmd.Parameters.AddWithValue("@email", this.Email);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    
                    conn.Close();
                }
            }
        }

        public UserEmail(String tenant, String usr, String email)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, email, forAlarm, Note FROM useremail WHERE userID LIKE @usr AND email LIKE @email";
            cmd.Parameters.AddWithValue("@usr", usr);
            cmd.Parameters.AddWithValue("@email", email);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._UserID = rdr.GetString(0);
                this._Email = rdr.GetString(1);
                this._ForAlarm = rdr.GetBoolean(2);
                this._Note = rdr.GetString(3);
            }
            else
            {
                this._UserID = "";
                this._Email = "";
                this._ForAlarm = false;
                this._Note = "";
            }
            rdr.Close();
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.UserID != "" && this.Email != "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM useremail WHERE userID LIKE @usr AND email LIKE @email";
                cmd.Parameters.AddWithValue("@usr", this.UserID);
                cmd.Parameters.AddWithValue("@email", this.Email);
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
            }
            return rt;
        }
    }

    public class UserPhoneNumber
    {
        protected String Tenant;
        public String log;

        private String _UserID;
        public String UserID
        {
            get
            {
                return this._UserID;
            }
        }

        private String _PhoneNumber;
        public String PhoneNumber
        {
            get { return this._PhoneNumber; }
        }

        private bool _ForAlarm;
        public bool ForAlarm
        {
            get { return this._ForAlarm; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE userphonenumbers SET forAlarm=@forAlarm WHERE userID LIKE @usr"
                    + " AND phoneNumber LIKE @phone";
                cmd.Parameters.AddWithValue("@forAlarm", value);
                cmd.Parameters.AddWithValue("@usr", this.UserID);
                cmd.Parameters.AddWithValue("@phone", this.PhoneNumber);
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }

                conn.Close();
            }
        }

        private String _Note;
        public String Note
        {
            get { return this._Note; }
            set
            {
                if (this.PhoneNumber != "" && this.UserID != "")
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE userphonenumbers SET note=@note WHERE userID LIKE @usr AND phoneNumber LIKE @phone";
                    cmd.Parameters.AddWithValue("@note", value);
                    cmd.Parameters.AddWithValue("@usr", this.UserID);
                    cmd.Parameters.AddWithValue("@phone", this.PhoneNumber);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    
                    conn.Close();
                }
            }
        }

        public UserPhoneNumber(String tenant, String usr, String phone)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, PhoneNumber, forAlarm, Note FROM userphonenumbers WHERE userID LIKE @usr "
                + " AND PhoneNumber LIKE @phone";
            cmd.Parameters.AddWithValue("@usr", usr);
            cmd.Parameters.AddWithValue("@phone", phone);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._UserID = rdr.GetString(0);
                this._PhoneNumber = rdr.GetString(1);
                this._ForAlarm = rdr.GetBoolean(2);
                this._Note = rdr.GetString(3);
            }
            else
            {
                this._UserID = "";
                this._PhoneNumber = "";
                this._ForAlarm = false;
                this._Note = "";
            }
            rdr.Close();
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.UserID != "" && this.PhoneNumber != "")
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM userphonenumbers WHERE userID LIKE @userID AND phonenumber LIKE @phonenumber";
                cmd.Parameters.AddWithValue("@userID", this.UserID);
                cmd.Parameters.AddWithValue("@phonenumber", this.PhoneNumber);
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
            }
            return rt;
        }
  
    }

    public class DisabledUser
    {
        protected String Tenant;
        public String log;

        private String _username;
        public String username
        {
            get { return this._username; }
        }

        private String _FirstName;
        public String FirstName
        {
            get { return this._FirstName; }
        }

        private String _LastName;
        public String LastName
        {
            get { return this._LastName; }
        }

        private Boolean _Enabled;
        public Boolean Enabled
        {
            get { return this._Enabled; }
            set
            {
                if (this.username.Length > 0)
                {
                    string strSQL = "UPDATE users SET enabled = @enabled WHERE userID LIKE @userid";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.Parameters.AddWithValue("@enabled", value);
                    cmd.Parameters.AddWithValue("@userid", this.username);
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Enabled = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();
                }
            }
        }

        public DisabledUser(String tenant, String username)
        {
            this.Tenant = tenant;

            String strSQL = "SELECT userID, nome, cognome, enabled "
                + " FROM users WHERE enabled=false AND userID LIKE @userid";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@userid", username);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            if (rdr1.Read() && !rdr1.IsDBNull(0))
            {
                this._username = rdr1.GetString(0);
                this._FirstName = rdr1.GetString(1);
                this._LastName = rdr1.GetString(2);
                this._Enabled = rdr1.GetBoolean(3);
            }
            else
            {
                this._username = "";
                this._FirstName = "";
                this._LastName = "";
                this._Enabled = false;
            }
            rdr1.Close();
            conn.Close();
        }
    }

    public class DisabledUsers
    {
        protected String Tenant;
        public List<DisabledUser> UserList;

        public DisabledUsers(String tenant)
        {
            this.Tenant = tenant;
            this.UserList = new List<DisabledUser>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM users WHERE verified=true AND enabled=false";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.UserList.Add(new DisabledUser(this.Tenant, rdr.GetString(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }
}