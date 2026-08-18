/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace KIS.App_Code
{
    /*
    public class Permesso
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
                if (this.ID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var trn = conn.BeginTransaction())
                        {
                            conn.Execute("UPDATE permessi SET nome = @pNome WHERE idpermesso = @pID",
                                new { @pNome = value, @pID = this.ID }, trn);
                            trn.Commit();
                            this._Nome = value;
                        }
                    }
                }
            }
        }
        private String _Descrizione;
        public String Descrizione
        {
            get { return _Descrizione; }
            set
            {
                if (this.ID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var trn = conn.BeginTransaction())
                        {
                            conn.Execute("UPDATE permessi SET descrizione = @pDescrizione WHERE idpermesso = @pID",
                                new { @pDescrizione = value, @pID = this.ID }, trn);
                            trn.Commit();
                            this._Descrizione = value;
                        }
                    }
                }
            }
        }

        public Permesso(String Tenant, int idPerm)
        {
            this.Tenant = Tenant;

            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                PermissionRow row = conn.QueryFirstOrDefault<PermissionRow>(
                    "SELECT idpermesso, nome, descrizione FROM permessi WHERE idpermesso = @idPermesso",
                    new { @idPermesso = idPerm });
                if (row != null)
                {
                    this._ID = row.idpermesso;
                    this._Nome = row.nome;
                    this._Descrizione = row.descrizione;
                }
                else
                {
                    this._ID = -1;
                    this._Nome = "";
                    this._Descrizione = "";
                }
            }
        }

        public Permesso(String Tenant, String nomePerm)
        {
            this.Tenant = Tenant;

            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                PermissionRow row = conn.QueryFirstOrDefault<PermissionRow>(
                    "SELECT idpermesso, nome, descrizione FROM permessi WHERE nome = @nome",
                    new { @nome = nomePerm });
                if (row != null)
                {
                    this._ID = row.idpermesso;
                    this._Nome = row.nome;
                    this._Descrizione = row.descrizione;
                }
                else
                {
                    this._ID = -1;
                    this._Nome = "";
                    this._Descrizione = "";
                }
            }
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var trn = conn.BeginTransaction())
                    {
                        conn.Execute("DELETE FROM permessi WHERE idpermesso = @pID",
                            new { @pID = this.ID }, trn);
                        trn.Commit();
                        rt = true;
                    }
                }
            }
            return rt;
        }
    
    }

    public class ElencoPermessi
    {
        public String log;

        public List<Permesso> Elenco;
        
        public ElencoPermessi(String Tenant)
        {
            this.Tenant = Tenant;

            Elenco = new List<Permesso>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var ids = conn.Query<int>("SELECT idpermesso FROM permessi ORDER BY nome").ToArray();
                foreach (int id in ids)
                {
                    Elenco.Add(new Permesso(this.Tenant, id));
                }
            }
        }

        public bool Add(String nomeP, String descP)
        {
            bool rt = false;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                using (var trn = conn.BeginTransaction())
                {
                    int? max = conn.ExecuteScalar<int?>("SELECT MAX(idpermesso) FROM permessi");
                    int maxID = max.HasValue ? max.Value + 1 : 0;
                    conn.Execute("INSERT INTO permessi(idpermesso, nome, descrizione) VALUES (@pID, @pNome, @pDescrizione)",
                        new { @pID = maxID, @pNome = nomeP, @pDescrizione = descP }, trn);
                    trn.Commit();
                    rt = true;
                }
            }
            return rt;
        }
    }
    */
    public class Permission
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
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE permissions SET nome = @pNome WHERE id = @pID",
                            new { pNome = value, pID = this.ID }, trn);
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
            get { return _Descrizione; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE permissions SET descrizione = @pDescrizione WHERE idpermesso = @pID",
                            new { pDescrizione = value, pID = this.ID }, trn);
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

        public Permission(int idPerm)
        {
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            PermissionRow row = conn.QueryFirstOrDefault<PermissionRow>(
                "SELECT id, name, description FROM permissions WHERE id = @pID",
                new { pID = idPerm });
            if (row != null)
            {
                this._ID = row.id;
                this._Nome = row.name;
                this._Descrizione = row.description;
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
            }
            conn.Close();
        }

        public Permission(String nomePerm)
        {
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            PermissionRow row = conn.QueryFirstOrDefault<PermissionRow>(
                "SELECT id, name, description FROM permissions WHERE nome = @pNome",
                new { pNome = nomePerm });
            if (row != null)
            {
                this._ID = row.id;
                this._Nome = row.name;
                this._Descrizione = row.description;
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
            }
            conn.Close();
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction trn = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM permissions WHERE id = @pID",
                        new { pID = this.ID }, trn);
                    trn.Commit();
                    rt = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    trn.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        private class PermissionRow
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }
    }

    public class PermissionsList
    {
        public String log;

        public List<Permission> Elenco;

        public PermissionsList()
        {
            Elenco = new List<Permission>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            int[] ids = conn.Query<int>("SELECT id FROM permissions ORDER BY name").ToArray();
            foreach (int id in ids)
            {
                Elenco.Add(new Permission(id));
            }
            conn.Close();
        }

        public bool Add(String nomeP, String descP)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            int? max = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM permissions");
            int maxID = max.HasValue ? max.Value + 1 : 0;
            MySqlTransaction trn = conn.BeginTransaction();
            try
            {
                conn.Execute("INSERT INTO permissions(id, name, description) VALUES (@pID, @pNome, @pDescrizione)",
                    new { pID = maxID, pNome = nomeP, pDescrizione = descP }, trn);
                trn.Commit();
                rt = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = false;
                trn.Rollback();
            }
            conn.Close();
            return rt;
        }
    }
}