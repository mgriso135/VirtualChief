/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
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
            MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd1.ExecuteReader();
            rdr.Read();
            this._numRelations = rdr.GetInt32(0);
            list = new relazione[this._numRelations];
            rdr.Close();
            strSQL = "SELECT RelazioneID FROM relazioniprocessi ORDER BY name";
            MySqlCommand cmd2 = new MySqlCommand(strSQL, conn);
            rdr = cmd2.ExecuteReader();
            int i = 0;
            while(rdr.Read())
            {
                list[i] = new relazione(this.Tenant, rdr.GetInt32(0));
                i++;
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
            String strSQL = "SELECT * FROM relazioniprocessi WHERE relazioneID = " + relID.ToString();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd1.ExecuteReader();
            if (rdr.Read())
            {
                this._relationID = rdr.GetInt32(0);
                this._name = rdr.GetString(1);
                this._description = rdr.GetString(2);
                this._imgURL = rdr.GetString(3);
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

    }
}