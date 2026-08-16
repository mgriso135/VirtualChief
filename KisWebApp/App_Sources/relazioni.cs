/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace KIS.App_Code
{
    public class relations
    {
        protected String Tenant;

        public relazione[] list;
        private int _numRelations;
        public int numRelations
        {
            get { return _numRelations; }
        }

        public relations(String tenant)
        {
            this.Tenant = tenant;
            string strSQL = "SELECT COUNT(*) FROM relazioniprocessi ORDER BY name";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this._numRelations = conn.ExecuteScalar<int>(strSQL);
            list = new relazione[this._numRelations];
            strSQL = "SELECT RelazioneID FROM relazioniprocessi ORDER BY name";
            int[] ids = conn.Query<int>(strSQL).ToArray();
            for (int i = 0; i < ids.Length && i < list.Length; i++)
            {
                list[i] = new relazione(this.Tenant, ids[i]);
            }
            conn.Close();
        }
    }

    public class relazione
    {
        protected String Tenant;

        private int _relationID;
        public int relationID { get { return _relationID; } }
        private String _name;
        public String Name
        {
            get { return _name; }
        }

        private String _description;
        public String Description
        {
            get { return _description; }
        }

        private String _imgURL;
        public String imgURL
        {
            get { return _imgURL; }
        }

        public relazione(String tenant)
        {
            this.Tenant = tenant;
            this._relationID = -1;
            this._name = "";
            this._description = "";
            this._imgURL = "";
        }

        public relazione(String tenant, int relID)
        {
            this.Tenant = tenant;
            String strSQL = "SELECT * FROM relazioniprocessi WHERE relazioneID = @pRelazioneID";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            RelazioneRow row = conn.QueryFirstOrDefault<RelazioneRow>(strSQL, new { pRelazioneID = relID });
            if (row != null)
            {
                this._relationID = row.RelazioneID;
                this._name = row.Name;
                this._description = row.Description;
                this._imgURL = row.imgUrl;
            }
            else
            {
                this._relationID = -1;
                this._name = "";
                this._description = "";
                this._imgURL = "";   
            }
            conn.Close();
        }

        private class RelazioneRow
        {
            public int RelazioneID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string imgUrl { get; set; }
        }

    }
}