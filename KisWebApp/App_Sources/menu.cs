/* Copyright © 2018 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace KIS.App_Code
{
    public class VoceMenu
    {
        public String log;
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private String _Titolo;
        public String Titolo
        {
            get { return this._Titolo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE menuvoci SET titolo = @titolo WHERE id= @id";
                cmd.Parameters.AddWithValue("@titolo", value);
                cmd.Parameters.AddWithValue("@id", this.ID);
                try
                {
                    this._Titolo = value;
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
                conn.Close();
            }
        }

        private String _Descrizione;
        public String Descrizione
        {
            get { return this._Descrizione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE menuvoci SET descrizione = @descrizione WHERE id= @id";
                cmd.Parameters.AddWithValue("@descrizione", value);
                cmd.Parameters.AddWithValue("@id", this.ID);
                try
                {
                    this._Descrizione = value;
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
                conn.Close();
            }
        }

        private String _URL;
        public String URL
        {
            get { return this._URL; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE menuvoci SET URL = @url WHERE id= @id";
                cmd.Parameters.AddWithValue("@url", value);
                cmd.Parameters.AddWithValue("@id", this.ID);
                try
                {
                    this._URL = value;
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
                conn.Close();
            }
        }

        public VoceMenu(int idVoce)
        {

            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name, description, url FROM menuvoci WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", idVoce);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = idVoce;
                this._Titolo = rdr.GetString(0);
                this._Descrizione = rdr.GetString(1);
                this._URL = rdr.GetString(2);
            }
            else
            {
                this._ID = -1;
            }
            rdr.Close();
            conn.Close();
        }

        private List<VoceMenu> _VociFiglie;
        public List<VoceMenu> VociFiglie
        {
            get { return this._VociFiglie; }
        }
    
        public void loadFigli()
        {
            this._VociFiglie = new List<VoceMenu>();
            if(this.ID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idFiglio FROM menualbero WHERE idPadre = @id"
                    + " ORDER BY ordinamento";
                cmd.Parameters.AddWithValue("@id", this.ID);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._VociFiglie.Add(new VoceMenu(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                this.loadFigli();
                if (this.VociFiglie.Count == 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Parameters.AddWithValue("@id", this.ID);
                    try
                    {
                        cmd.CommandText = "DELETE FROM menualbero WHERE idFiglio = @id";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM menugruppi WHERE idVoce = @id";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM menuvoci WHERE id = @id";
                        cmd.ExecuteNonQuery();
                        rt = true;
                    }
                    catch(Exception ex)
                    {
                        rt = false;
                        this.log = ex.Message;
                    }
                    conn.Close();
                }
                else
                {
                    rt = false;
                }
            }
            return rt;
        }

        public bool AddFiglio(String ttl, String desc, String lnk)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                int maxID = 0;
                cmd.CommandText = "SELECT MAX(id) FROM menuvoci";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO menuvoci(id, titolo, descrizione, url) VALUES(@maxID"
                    + ", @ttl, @desc, @lnk)";
                this.loadFigli();
                int maxOrd = this.VociFiglie.Count + 1;
                cmd.Parameters.AddWithValue("@maxID", maxID);
                cmd.Parameters.AddWithValue("@ttl", ttl);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@lnk", lnk);
                cmd.Parameters.AddWithValue("@idPadre", this.ID);
                cmd.Parameters.AddWithValue("@ordinamento", maxOrd);
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO menualbero(idPadre, idFiglio, ordinamento) VALUES(@idPadre"
                        + ", @maxID"
                        + ", @ordinamento"
                        + ")";
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

        /* Cambia l'ordinamento delle voci di menu figlie
         * direzione = true: sposta in su
         * direzione = false: sposta in giu
         */
        public bool SpostaVoceFiglia(VoceMenu vm, bool direzione)
        {
            log = "Entro in SpostaVoceFiglia()<br />";
            bool ret = false;
            if (this.ID != -1)
            {
                this.loadFigli();
                // Trovo la voce di menu attuale
                int indVM = -1;
                for (int i = 0; i < this.VociFiglie.Count; i++)
                {
                    if (vm.ID == this.VociFiglie[i].ID)
                    {
                        indVM = i;
                    }
                }


                if (indVM != -1)
                {
                    log = "Entro nella funzione.<br />Mi occupo dell'item: " + indVM.ToString();
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                            cmd.Parameters.AddWithValue("@ord1", indVM - 1);
                            cmd.Parameters.AddWithValue("@idPadre", this.ID);
                            cmd.Parameters.AddWithValue("@figlio1", this.VociFiglie[indVM].ID);
                            cmd.Parameters.AddWithValue("@ord2", indVM);
                            cmd.Parameters.AddWithValue("@figlio2", this.VociFiglie[indVM - 1].ID);
                            try
                            {
                                cmd.CommandText = "UPDATE menualbero SET ordinamento = @ord1"
                                    + " WHERE idpadre = @idPadre"
                                    + " AND idFiglio = @figlio1";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menualbero SET ordinamento = @ord2"
                                    + " WHERE idpadre = @idPadre"
                                    + " AND idFiglio = @figlio2";
                                cmd.ExecuteNonQuery();
                                tr.Commit();
                                ret = true;
                            }
                            catch (Exception ex)
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
                        if (indVM >= this.VociFiglie.Count - 1)
                        {
                            // Non sposto niente
                        }
                        else
                        {
                            MySqlTransaction tr = conn.BeginTransaction();
                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Transaction = tr;
                            cmd.Parameters.AddWithValue("@ord1", indVM + 1);
                            cmd.Parameters.AddWithValue("@idPadre", this.ID);
                            cmd.Parameters.AddWithValue("@figlio1", this.VociFiglie[indVM].ID);
                            cmd.Parameters.AddWithValue("@ord2", indVM);
                            cmd.Parameters.AddWithValue("@figlio2", this.VociFiglie[indVM + 1].ID);
                            try
                            {
                                cmd.CommandText = "UPDATE menualbero SET ordinamento = @ord1"
                                    + " WHERE idPadre = @idPadre"
                                    + " AND idFiglio = @figlio1";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "UPDATE menualbero SET ordinamento = @ord2"
                                    + " WHERE idPadre = @idPadre"
                                    + " AND idFiglio = @figlio2";
                                cmd.ExecuteNonQuery();
                                tr.Commit();
                                ret = true;
                            }
                            catch (Exception ex)
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


    }

    public class MainMenu
    {
        protected String Tenant;

        public String log;

        private List<VoceMenu> _Elenco;
        public List<VoceMenu> Elenco
        {
            get { return this._Elenco; }
        }

        public MainMenu(String Tenant)
        {
            this.Tenant = Tenant;

            this._Elenco = new List<VoceMenu>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT menuvoci.id FROM menuvoci LEFT JOIN menualbero ON(menuvoci.id = menualbero.idfiglio) WHERE menualbero.idpadre IS NULL";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new VoceMenu(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadAllMenuItems()
        {
            this._Elenco = new List<VoceMenu>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT menuvoci.id FROM menuvoci";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new VoceMenu(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public bool Add(String ttl, String desc, String lnk)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            int maxID = 0;
            cmd.CommandText = "SELECT MAX(id) FROM menuvoci";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO menuvoci(id, titolo, descrizione, url) VALUES(@maxID"
                + ", @ttl, @desc, @lnk)";
            cmd.Parameters.AddWithValue("@maxID", maxID);
            cmd.Parameters.AddWithValue("@ttl", ttl);
            cmd.Parameters.AddWithValue("@desc", desc);
            cmd.Parameters.AddWithValue("@lnk", lnk);
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