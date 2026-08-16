/* Copyright © 2018 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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
                try
                {
                    conn.Execute("UPDATE menuvoci SET titolo = @titolo WHERE id= @id",
                        new { titolo = value, id = this.ID });
                    this._Titolo = value;
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
                try
                {
                    conn.Execute("UPDATE menuvoci SET descrizione = @descrizione WHERE id= @id",
                        new { descrizione = value, id = this.ID });
                    this._Descrizione = value;
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
                try
                {
                    conn.Execute("UPDATE menuvoci SET URL = @url WHERE id= @id",
                        new { url = value, id = this.ID });
                    this._URL = value;
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
            VoceMenuRow row = conn.QueryFirstOrDefault<VoceMenuRow>(
                "SELECT name, description, url FROM menuvoci WHERE id = @id",
                new { id = idVoce });
            if (row != null)
            {
                this._ID = idVoce;
                this._Titolo = row.name;
                this._Descrizione = row.description;
                this._URL = row.url;
            }
            else
            {
                this._ID = -1;
            }
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
                int[] ids = conn.Query<int>("SELECT idFiglio FROM menualbero WHERE idPadre = @id"
                    + " ORDER BY ordinamento", new { id = this.ID }).ToArray();
                foreach (int id in ids)
                {
                    this._VociFiglie.Add(new VoceMenu(id));
                }
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
                    try
                    {
                        conn.Execute("DELETE FROM menualbero WHERE idFiglio = @id", new { id = this.ID });
                        conn.Execute("DELETE FROM menugruppi WHERE idVoce = @id", new { id = this.ID });
                        conn.Execute("DELETE FROM menuvoci WHERE id = @id", new { id = this.ID });
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
                int? max = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM menuvoci");
                int maxID = max.HasValue ? max.Value + 1 : 0;
                MySqlTransaction tr = conn.BeginTransaction();
                this.loadFigli();
                int maxOrd = this.VociFiglie.Count + 1;
                try
                {
                    conn.Execute("INSERT INTO menuvoci(id, titolo, descrizione, url) VALUES(@maxID"
                        + ", @ttl, @desc, @lnk)",
                        new { maxID, ttl, desc, lnk }, tr);
                    conn.Execute("INSERT INTO menualbero(idPadre, idFiglio, ordinamento) VALUES(@idPadre"
                        + ", @maxID"
                        + ", @ordinamento"
                        + ")",
                        new { idPadre = this.ID, maxID, ordinamento = maxOrd }, tr);

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
                            try
                            {
                                conn.Execute("UPDATE menualbero SET ordinamento = @ord1"
                                    + " WHERE idpadre = @idPadre"
                                    + " AND idFiglio = @figlio1",
                                    new { ord1 = indVM - 1, idPadre = this.ID, figlio1 = this.VociFiglie[indVM].ID }, tr);
                                conn.Execute("UPDATE menualbero SET ordinamento = @ord2"
                                    + " WHERE idpadre = @idPadre"
                                    + " AND idFiglio = @figlio2",
                                    new { ord2 = indVM, idPadre = this.ID, figlio2 = this.VociFiglie[indVM - 1].ID }, tr);
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
                            try
                            {
                                conn.Execute("UPDATE menualbero SET ordinamento = @ord1"
                                    + " WHERE idPadre = @idPadre"
                                    + " AND idFiglio = @figlio1",
                                    new { ord1 = indVM + 1, idPadre = this.ID, figlio1 = this.VociFiglie[indVM].ID }, tr);
                                conn.Execute("UPDATE menualbero SET ordinamento = @ord2"
                                    + " WHERE idPadre = @idPadre"
                                    + " AND idFiglio = @figlio2",
                                    new { ord2 = indVM, idPadre = this.ID, figlio2 = this.VociFiglie[indVM + 1].ID }, tr);
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

        private class VoceMenuRow
        {
            public string name { get; set; }
            public string description { get; set; }
            public string url { get; set; }
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
            int[] ids = conn.Query<int>("SELECT menuvoci.id FROM menuvoci LEFT JOIN menualbero ON(menuvoci.id = menualbero.idfiglio) WHERE menualbero.idpadre IS NULL").ToArray();
            foreach (int id in ids)
            {
                this._Elenco.Add(new VoceMenu(id));
            }
            conn.Close();
        }

        public void loadAllMenuItems()
        {
            this._Elenco = new List<VoceMenu>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int[] ids = conn.Query<int>("SELECT menuvoci.id FROM menuvoci").ToArray();
            foreach (int id in ids)
            {
                this._Elenco.Add(new VoceMenu(id));
            }
            conn.Close();
        }

        public bool Add(String ttl, String desc, String lnk)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? max = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM menuvoci");
            int maxID = max.HasValue ? max.Value + 1 : 0;
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute("INSERT INTO menuvoci(id, titolo, descrizione, url) VALUES(@maxID"
                    + ", @ttl, @desc, @lnk)",
                    new { maxID, ttl, desc, lnk }, tr);
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