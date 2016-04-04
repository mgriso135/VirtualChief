using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public class Permesso
    {
        private String _utente;
        public String utente
        {
            get { return _utente; }
        }

        private processo _process;
        public processo process
        {
            get { return _process; }
        }
        private Char _idPermesso;
        public Char idPermesso
        {
            get { return this._idPermesso; }
            set
            {
                if(this._utente != "" && this._process != null)
                {
                    String strSQL = "SELECT COUNT(*) FROM permessiprocessi WHERE processo = " + this.process.processID + " AND userID LIKE '" + this.utente + "'";
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    int numRec = rdr.GetInt32(0);
                    rdr.Close();
                    if (numRec == 0)
                    {
                        // Add a record
                        strSQL = "INSERT INTO permessiprocessi(userID, permesso, processo) VALUES('" + this.utente + "', '" + value + "', " + this.process.processID.ToString() + ")";
                        MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        // Update the record
                        strSQL = "UPDATE permessiprocessi SET permesso='" + value + "' WHERE userID = '" + this.utente + "' AND processo = " + this.process.processID;
                        MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                        cmd1.ExecuteNonQuery();
                    }
                    rdr.Close();
                    conn.Close(); 
                }
            }
        }
        
        private String _descrizione;
        public String descrizione
        {
            get { return _descrizione; }
        }

        public Permesso()
        {
            this._utente = null;
            this._process = null;
            this._descrizione = "";
            this._idPermesso = '\0';
        }

        public Permesso(String currUser, processo currProc)
        {
            if (currUser != "" && currProc.processID != -1)
            {
                String strSQL = "SELECT userID, processo, permesso, tipipermessi.descrizione FROM permessiprocessi INNER JOIN tipipermessi ON (permessiprocessi.permesso = tipipermessi.id) WHERE userID LIKE '" + currUser + "' AND processo = " + currProc.processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                try
                {
                    this._idPermesso = rdr.GetChar(2);
                    this._descrizione = rdr.GetString(3);
                }
                catch
                {
                    this._idPermesso = 'r';
                    this._descrizione = "";
                }
                this._utente = currUser;
                this._process = currProc;
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._utente = null;
                this._process = null;
                this._descrizione = "";
                this._idPermesso = '\0';                
            }
        }

     }

}