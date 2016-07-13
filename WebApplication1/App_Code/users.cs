﻿/* CHANGELOG
 * 20130713
 * Aggiunta classe UserPhoneNumber
 * Aggiunta classe UserEmail
 * Aggiunto attributo Email in User
 * Aggiunto attributo PhoneNumber nella classe User
 * Aggiunto metodo AddEmail nella classe User
 * Aggiunto metodo loadEmails nella classe User
 * Aggiunto metodo addPhoneNumber nella classe User
 * Aggiunto metodo loadPhoneNumbers nella classe User
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql;
using KIS;
using Dati;
using KIS.Menu;
using KIS.Commesse;
using KIS.App_Code;

namespace KIS
{
    public class Group
    {
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
                    String strSQL = "UPDATE groups SET nomeGruppo = '" + value + "' WHERE id = " + this.ID.ToString();
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();

                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
                    String strSQL = "UPDATE groups SET descrizione = '" + value + "' WHERE id = " + this.ID.ToString();
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();

                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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

        public Group(int groupID)
        {
            String strSQL = "SELECT * FROM groups WHERE id = " + groupID.ToString();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            this._Permessi = new GruppoPermessi(groupID);
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

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trn = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trn;
                

                try
                {
                    cmd.CommandText = "DELETE FROM gruppipermessi WHERE idgroup = " + this.ID.ToString();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM groups WHERE id = " + this.ID.ToString();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idVoce FROM menugruppi WHERE gruppo = " + this.ID.ToString()
                    + " ORDER BY ordinamento";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._VociDiMenu.Add(new VoceMenu(rdr.GetInt32(0)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                this.loadMenu();
                int maxOrd = this.VociDiMenu.Count+1;
                cmd.CommandText = "INSERT INTO menugruppi(gruppo, idVoce, ordinamento) VALUES(" + this.ID.ToString() + ", " + vm.ID.ToString() + ", " + maxOrd.ToString() + ")";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM menugruppi WHERE gruppo = " + this.ID.ToString() + " AND idVoce = " + vm.ID.ToString();
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
        public bool SpostaVoce(VoceMenu vm, bool direzione)
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
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
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = " + (indVM - 1).ToString()
                                    + " WHERE gruppo = " + this.ID.ToString()
                                    + " AND idVoce = " + this.VociDiMenu[indVM].ID.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = " + indVM.ToString()
                                    + " WHERE gruppo = " + this.ID.ToString()
                                    + " AND idVoce = " + this.VociDiMenu[indVM - 1].ID.ToString();
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
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = " + (indVM + 1).ToString()
                                    + " WHERE gruppo = " + this.ID.ToString()
                                    + " AND idVoce = " + this.VociDiMenu[indVM].ID.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menugruppi SET ordinamento = " + indVM.ToString()
                                    + " WHERE gruppo = " + this.ID.ToString()
                                    + " AND idVoce = " + this.VociDiMenu[indVM + 1].ID.ToString();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT user FROM groupusers WHERE groupID = " + this.ID.ToString();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Ritardo' "
                        + "AND idGruppo = '" + this.ID.ToString() + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(rdr.GetInt32(0)));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = " + this.ID.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = " + this.ID.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Warning' "
                        + "AND idGruppo = '" + this.ID.ToString() + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(rdr.GetInt32(0)));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = " + this.ID.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = " + this.ID.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(rdr.GetInt32(0), rdr.GetInt32(1));
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
    }

    public class GroupList
    {
        public String log;

        public List<Group> Elenco;

        public GroupList()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM groups ORDER BY nomeGruppo";
            MySqlDataReader rdr = cmd.ExecuteReader();
            Elenco = new List<Group>();
            while (rdr.Read())
            {
                Elenco.Add(new Group(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public bool Add(String nomeG, String descG)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM groups";
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
            cmd.CommandText = "INSERT INTO groups(id, nomeGruppo, descrizione) VALUES(" + maxID.ToString() + ", '" + 
                nomeG + "', '" + descG + "')";
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

    public class GruppoPermesso
    {
        public String log;

        private int _GroupID;
        public int GroupID
        {
            get
            {
                return this._GroupID;
            }
        }

        public Permesso Permes;

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
                    
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = " + this.GroupID.ToString() +
                        " AND idpermesso = " + this.IdPermesso.ToString();
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
                        cmd.CommandText = "UPDATE gruppipermessi SET r = " + value.ToString() + " WHERE idGroup = " + this.GroupID.ToString()
                            + " AND idpermesso = " + this.IdPermesso.ToString();
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(" + this.GroupID.ToString()
                            + ", " + this.IdPermesso.ToString() + ", " + value.ToString() + ", false, false)";
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
        private bool _W;
        public bool W
        {
            get { return this._W; }
            set
            {
                if (this.GroupID != -1 && this.IdPermesso != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = " + this.GroupID.ToString() +
                        " AND idpermesso = " + this.IdPermesso.ToString();
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
                        cmd.CommandText = "UPDATE gruppipermessi SET w = " + value.ToString() + " WHERE idGroup = " + this.GroupID.ToString()
                            + " AND idpermesso = " + this.IdPermesso.ToString();                        
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(" + this.GroupID.ToString()
                            + ", " + this.IdPermesso.ToString() + ", false, " + value.ToString() + ", false)";

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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM gruppipermessi WHERE idGroup = " + this.GroupID.ToString() +
                        " AND idpermesso = " + this.IdPermesso.ToString();
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
                        cmd.CommandText = "UPDATE gruppipermessi SET x = " + value.ToString() + " WHERE idGroup = " + this.GroupID.ToString()
     + " AND idpermesso = " + this.IdPermesso.ToString();
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO gruppipermessi(idgroup, idpermesso, r, w, x) VALUES(" + this.GroupID.ToString()
                            + ", " + this.IdPermesso.ToString() + ", false, false, " + value.ToString() + ")";
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

        public GruppoPermesso(int grp, Permesso prm)
        {
            bool check = false;
            if (prm.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM groups WHERE id = " + grp.ToString();
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
                this.Permes = new Permesso(prm.ID);
                this._NomePermesso = Permes.Nome;
                this._PermessoDesc = Permes.Descrizione;
                this._IdPermesso = Permes.ID;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT r, w, x FROM gruppipermessi WHERE idgroup = " + grp.ToString() + " AND idpermesso = " + Permes.ID.ToString();
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
    }

    public class GruppoPermessi
    {
        public List<GruppoPermesso> Elenco;
        private int _GroupID;
        public int GroupID
        {
            get
            {
                return this._GroupID;
            }
        }

        public GruppoPermessi(int grp)
        {
            bool check = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM groups WHERE id = " + grp.ToString();
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
                ElencoPermessi elPrm = new ElencoPermessi();
                for (int i = 0; i < elPrm.Elenco.Count; i++)
                {
                    this.Elenco.Add(new GruppoPermesso(this.GroupID, elPrm.Elenco[i]));
                }

            }
            else
            {
                this._GroupID = -1;
                this.Elenco = null;
            }
        }
    }

    public class UserList
    {
        public User[] elencoUtenti;
        public List<User> listUsers;
        private int _numUsers;
        public int numUsers
        {
            get { return this._numUsers; }
        }

        public UserList()
        {
            String strSQL = "SELECT COUNT(userID) FROM users ORDER BY userID";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            this._numUsers = rdr1.GetInt32(0);
            elencoUtenti = new User[this.numUsers];
            rdr1.Close();

            listUsers = new List<User>();

            strSQL = "SELECT userID FROM users ORDER BY userID";
            cmd = new MySqlCommand(strSQL, conn);
            rdr1 = cmd.ExecuteReader();
            int i = 0;
            while (i < this.numUsers && rdr1.Read())
            {
                elencoUtenti[i] = new User(rdr1.GetString(0));
                listUsers.Add(new User(rdr1.GetString(0)));
                i++;
            }
            rdr1.Close();
            conn.Close();
        }

        public UserList(Permesso prm)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            this.listUsers = new List<User>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT(users.userID) FROM users INNER JOIN groupusers ON (users.userid = groupusers.user) "
            + " INNER JOIN gruppipermessi ON (groupusers.groupid = gruppipermessi.idgroup) INNER JOIN permessi ON "
            +" (gruppipermessi.idpermesso = permessi.idpermesso) WHERE permessi.idpermesso = " + prm.ID.ToString();
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
                    String strSQL = "UPDATE users SET nome='" + value + "' WHERE userID LIKE '" + this.username + "'";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
                    String strSQL = "UPDATE users SET cognome='" + value + "' WHERE userID LIKE '" + this.username + "'";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(_lastLogin, fuso.tzFusoOrario);
            }
            set
            {
                if (this.authenticated == true && this.username.Length > 0)
                {
                    DateTime lastL = new DateTime(value.Ticks, DateTimeKind.Local);
                    FusoOrario fuso = new FusoOrario();
                    string strSQL = "UPDATE users SET lastLogin = '" 
                        + TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss") 
                        + "' WHERE userID LIKE '" + this.username + "'";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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

        public List<Group> Gruppi;

        public User(String usr, String pwd)
        {
            this._homeBoxes = null;
            this._authenticated = false;
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID FROM users WHERE userID LIKE '" + usr + "' AND password = MD5('" + pwd + "')";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
                FusoOrario fuso = new FusoOrario();
                this.lastLogin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario);
                this._ID = rdr1.GetInt32(5);
            }
            rdr1.Close();
            conn.Close();
        }

        public User(String usr)
        {
            this._homeBoxes = null;
            this._authenticated = false;
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID FROM users WHERE userID LIKE '" + usr + "'";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
            }
            else
            {
                this._username = "";
            }
            rdr1.Close();
            conn.Close();
        }

        public User(int IDn)
        {
            this._homeBoxes = null;
            this._authenticated = false;
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin, ID FROM users WHERE ID = " + IDn.ToString();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
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
            }
            else
            {
                this._username = "";
            }
            rdr1.Close();
            conn.Close();
        }

        public User()
        {
            this._homeBoxes = null;
            this._username = "";
            this._typeOfUser = "";
            this._name = "";
            this._cognome = "";
            this._authenticated = false;
        }

         /* Returns:
         * 1 if user correctly added
         * 2 if user already exists
         * 0 if generic error
         */
        public int add(String usr, String pwd, String nome, String cognome, String typeOf)
        {
            int rt = 0;
            String strSQL = "SELECT * FROM users WHERE userID LIKE '" + usr + "'";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            if (rdr1.HasRows)
            {
                rt = 2;
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
                    strSQL = "INSERT INTO users(userID, password, nome, cognome, tipoUtente, lastLogin, ID) VALUES('"
                        + usr + "', MD5('" + pwd + "'), '" + nome + "', '" + cognome + "', '" + typeOf + "', '"
                        + DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                        + maxID.ToString()
                        + ")";
                    cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    rt = 1;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    rt = 0;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupID FROM groupusers INNER JOIN groups ON (groupusers.groupID = groups.ID) WHERE groupusers.user = '" +
                    this.username + "' ORDER BY groups.nomeGruppo";
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO groupusers(groupID, user) VALUES("+grp.ID.ToString()+", '" + this.username + "')";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM groupusers WHERE groupID = " + grp.ID.ToString() + " AND user = '" + this.username + "'";
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT postazione FROM registrooperatoripostazioni WHERE logout IS null AND username = '" + this.username + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._PostazioniAttive = new List<Postazione>();
            while (rdr.Read())
            {
                this._PostazioniAttive.Add(new Postazione(rdr.GetInt32(0)));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO registrooperatoripostazioni(username, postazione, login, logout) VALUES("
                        + "'"+this.username+"', "+p.id.ToString()+", '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', null)";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE registrooperatoripostazioni SET logout='" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "'"
                    + " WHERE logout IS null AND username = '" + this.username + "' AND postazione = " + p.id.ToString();
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

        public void loadTaskAvviati()
        {
            this._TaskAvviati = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tasksproduzione.taskID, evento FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON("
                + "tasksproduzione.taskID = registroeventitaskproduzione.task) WHERE tasksproduzione.status = 'I' "
                + " AND registroeventitaskproduzione.user = '" + this.username + "' ORDER BY registroeventitaskproduzione.data DESC";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT email FROM useremail WHERE userID LIKE '" + this.username + "' ORDER BY note";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Email.Add(new UserEmail(this.username, rdr.GetString(0)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO useremail(userid, email, forAlarm, note) VALUES("
                    + "'" + this.username + "', '" + mailAddr.Address + "', " + forAlarm.ToString()
                    + ", '" + note + "')";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT phoneNumber FROM userphonenumbers WHERE userID LIKE '" + this.username + "' ORDER BY note";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.PhoneNumbers.Add(new UserPhoneNumber(this.username, rdr.GetString(0)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO userphoneNumbers(userid, phoneNumber, forAlarm, note) VALUES("
                    + "'" + this.username + "', '" + phone + "', " + forAlarm.ToString()
                    + ", '" + note + "')";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE users SET password = MD5('" + newPass + "') WHERE userID = '" + this.username + "'";
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT repartoID FROM eventorepartoutenti WHERE TipoEvento LIKE 'Ritardo' "
                        + "AND userID = '" + this.username.ToString() + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(rdr.GetInt32(0)));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneRitardiReparto.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneRitardiReparto[j]);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessautenti WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND userID = '" + this.username + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneRitardiCommessa.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneRitardiCommessa[j]);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloID, ArticoloAnno FROM eventoarticoloutenti WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND userID = '" + this.username + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneRitardiArticolo.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneRitardiArticolo[j]);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT repartoID FROM eventorepartoutenti WHERE TipoEvento LIKE 'Warning' "
                        + "AND userID = '" + this.username.ToString() + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(rdr.GetInt32(0)));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneWarningReparto.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneWarningReparto[j]);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessautenti WHERE "
                        + "TipoEvento LIKE 'Warning' AND userID = '" + this.username + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneWarningCommessa.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneWarningCommessa[j]);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloID, ArticoloAnno FROM eventoarticoloutenti WHERE "
                        + "TipoEvento LIKE 'Warning' AND userID = '" + this.username + "'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(rdr.GetInt32(0), rdr.GetInt32(1));
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
                    for (int j = 0; j < this.Gruppi[i].SegnalazioneWarningArticolo.Count; j++)
                    {
                        ret.Add(this.Gruppi[i].SegnalazioneWarningArticolo[j]);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user='" + this.username
                    + "' ORDER BY data";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < ElencoTasksOperatore.Count; i++)
                {
                    if (ElencoTasksOperatore[i].Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " 
                            + ElencoTasksOperatore[i].TaskProduzioneID.ToString()
                            + " AND user LIKE '" + this.username + "'"
                             + " ORDER BY data";
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
                                    Postazione pst = new Postazione(ElencoTasksOperatore[i].PostazioneID);
                                    curr.nomePostazione = pst.name;
                                    curr.nomeTask = ElencoTasksOperatore[i].Name;
                                    curr.idProdotto = ElencoTasksOperatore[i].ArticoloID;
                                    curr.annoProdotto = ElencoTasksOperatore[i].ArticoloAnno;
                                    Articolo art = new Articolo(ElencoTasksOperatore[i].ArticoloID, ElencoTasksOperatore[i].ArticoloAnno);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(task) FROM registroeventitaskproduzione WHERE user='" + this.username + "' "
                    + " AND data >= '"+ start.ToString("yyyy/MM/dd") + "'"
                    + " AND data <= '"+ end.ToString("yyyy/MM/dd") + "'"
                    + " ORDER BY data";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ElencoTasksOperatore.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();

                // -- FUNZIONE INTERVALLI

                for (int i = 0; i < ElencoTasksOperatore.Count; i++)
                {
                    if (ElencoTasksOperatore[i].Status == 'F')
                    {
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = "
                            + ElencoTasksOperatore[i].TaskProduzioneID.ToString()
                            + " AND user LIKE '" + this.username + "'"
                             + " ORDER BY data";
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
                                    Postazione pst = new Postazione(ElencoTasksOperatore[i].PostazioneID);
                                    curr.nomePostazione = pst.name;
                                    curr.nomeTask = ElencoTasksOperatore[i].Name;
                                    curr.idProdotto = ElencoTasksOperatore[i].ArticoloID;
                                    curr.annoProdotto = ElencoTasksOperatore[i].ArticoloAnno;
                                    Articolo art = new Articolo(ElencoTasksOperatore[i].ArticoloID, ElencoTasksOperatore[i].ArticoloAnno);
                                    curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                    Cliente customer = new Cliente(art.Cliente);
                                    curr.ragioneSocialeCliente = customer.RagioneSociale;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE users SET password = MD5('" + newPass + "') WHERE userID LIKE '" + this.username + "'";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan where status <> 'I' AND status <> 'F' "
                    + " AND planner LIKE '" + this.username + "' ORDER BY dataPrevistaFineProduzione";
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    artList.Add(new Articolo(rdr.GetInt32(0), rdr.GetInt32(1)));
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
            this._homeBoxes = new HomeBoxesListUser(this);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO homeboxesuser(idHomeBox, user, ordine) VALUES("
                    + box.ID.ToString() + ", '" + this.username + "', " + ordine.ToString() + ")";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM homeboxesuser WHERE idHomeBox = " + box.ID.ToString()
                    + " AND user ='"+this.username+"'";
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

    public class UserEmail
    {
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE useremail SET forAlarm = " + value + " WHERE userID LIKE '" + this.UserID + "'"
                    + " AND email LIKE '" + this.Email + "'";
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE useremail SET note = '" + value + "' WHERE userID LIKE '" + this.UserID + "'"
                        + " AND email LIKE '" + this.Email + "'";
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

        public UserEmail(String usr, String email)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, email, forAlarm, Note FROM useremail WHERE userID LIKE '" + usr + "' "
                + " AND email LIKE '" + email + "'";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM useremail WHERE userID LIKE '" + this.UserID
                    + "' AND email LIKE '" + this.Email + "'";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE userphonenumbers SET forAlarm = " + value + " WHERE userID LIKE '" + this.UserID + "'"
                    + " AND phoneNumber LIKE '" + this.PhoneNumber + "'";
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE userphonenumbers SET note = '" + value + "' WHERE userID LIKE '" + this.UserID + "'"
                        + " AND phoneNumber LIKE '" + this.PhoneNumber + "'";
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

        public UserPhoneNumber(String usr, String phone)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, PhoneNumber, forAlarm, Note FROM userphonenumbers WHERE userID LIKE '" + usr + "' "
                + " AND PhoneNumber LIKE '" + phone + "'";
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM userphonenumbers WHERE userID LIKE '" + this.UserID
                    + "' AND phonenumber LIKE '" + this.PhoneNumber + "'";
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
}