using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql;
using WebApplication1;
using Dati;

namespace WebApplication1
{
    public class Group
    {
        private int _id;
        public int id
        {
            get { return this._id; }
        }
        private String _name;
        public String name
        {
            get { return this._name; }
            set
            {
                if (this._id != -1 && value.Length > 0)
                {
                    String strSQL = "UPDATE TABLE groups SET nomeGruppo = '" + value + "' WHERE id = " + this._id.ToString();
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        private String _description;
        public String description
        {
            get { return this._name; }
            set
            {
                if (this._id != -1 && value.Length > 0)
                {
                    String strSQL = "UPDATE TABLE groups SET descrizione = '" + value + "' WHERE id = " + this._id.ToString();
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        private int _numUsers;
        public int numUsers
        {
            get { return _numUsers; }
        }
        public User[] users;

        public Group()
        {
            this._id = -1;
            this._name = "";
            this._description = "";
            this.users = null;
        }

        public Group(int groupID)
        {
            String strSQL = "SELECT * FROM groups WHERE id = " + groupID.ToString();
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if(rdr.HasRows)
            {
                this._id = rdr.GetInt32(0);
                this._name = rdr.GetString(1);
                this._description = rdr.GetString(2);

                strSQL = "SELECT COUNT(*) FROM groupusers WHERE groupID = " + this._id.ToString();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr2 = cmd1.ExecuteReader();
                rdr2.Read();
                this._numUsers = rdr2.GetInt32(0);
                this.users = new User[this.numUsers];
                rdr2.Close();
                strSQL = "SELECT groupID, user FROM groupusers WHERE groupID = " + this.id.ToString();
                cmd1 = new MySqlCommand(strSQL, conn);
                rdr2 = cmd1.ExecuteReader();
                int i = 0;
                while (rdr2.Read() && i < this.numUsers)
                {
                    this.users[i] = new User(rdr2.GetString(1));
                    i++;
                }
            }
            else
            {
                this._id = -1;
                this._name = "";
                this._description = "";
                this.users = null;
            }
            rdr.Close();
            conn.Close();
        }

        public bool addUser(String usrID)
        {
            bool rt;
            if (this.id != -1 && usrID.Length > 0)
            {
                String strSQL = "INSERT INTO groupusers(groupID, user) VALUES(" + this.id + ", '" + usrID + "')";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool deleteUser(string usrID)
        {
            bool rt;
            if (this.id != -1 && usrID.Length > 0)
            {
                String strSQL = "DELETE FROM groupusers WHERE groupID = " + this.id + " AND user LIKE '" + usrID + "'";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }
    }

    public class UserList
    {
        public User[] elencoUtenti;
        private int _numUsers;
        public int numUsers
        {
            get { return this._numUsers; }
        }

        public UserList()
        {
            String strSQL = "SELECT COUNT(userID) FROM users ORDER BY userID";
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            rdr1.Read();
            this._numUsers = rdr1.GetInt32(0);
            elencoUtenti = new User[this.numUsers];
            rdr1.Close();

            strSQL = "SELECT userID FROM users ORDER BY userID";
            cmd = new MySqlCommand(strSQL, conn);
            rdr1 = cmd.ExecuteReader();
            int i = 0;
            while (i < this.numUsers && rdr1.Read())
            {
                elencoUtenti[i] = new User(rdr1.GetString(0));
                i++;
            }
            rdr1.Close();
            conn.Close();
        }
    }

    public class User
    {
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
                if(value.Length > 0 && username.Length > 0 && authenticated == true)
                {
                    String strSQL = "UPDATE users SET nome='" + value + "' WHERE username LIKE '" + this.username + "'";
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
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
                if (value.Length > 0 && username.Length > 0 && authenticated == true)
                {
                    String strSQL = "UPDATE users SET cognome='" + value + "' WHERE username LIKE '" + this.username + "'";
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private String _typeOfUser;
        public String typeOfUser
        {
            get { return this._typeOfUser; }
        }

        private DateTime _lastLogin;
        public DateTime lastLogin
        {
            get { return _lastLogin; }
            set
            {
                if (this.authenticated == true && this.username.Length > 0)
                {
                    string strSQL = "UPDATE users SET lastLogin = '" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE userID LIKE '" + this.username + "'";
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private int _numPermessiProcessi;
        public int numPermessiProcessi
        {
            get { return _numPermessiProcessi; }
        }

        private Permesso[] _permessiProcessi;
        public Permesso[] permessiProcessi
        {
            get { return _permessiProcessi; }
        }

        private int _numOwnedProcesses;
        public int numOwnedProcesses
        {
            get { return this._numOwnedProcesses; }
        }
        private int[] _ownedProcesses;
        public int[] ownedProcesses
        {
            get { return this._ownedProcesses; }
        }

        private int _numMainOwnedProcesses;
        public int numMainOwnedProcesses
        {
            get { return this._numMainOwnedProcesses; }
        }
        private int[] _mainOwnedProcesses;
        public int[] mainOwnedProcesses
        {
            get { return this._mainOwnedProcesses; }
        }


        public User(String usr, String pwd)
        {
            this._authenticated = false;
            String strSQL = "SELECT * FROM users WHERE userID LIKE '" + usr + "' AND password = MD5('" + pwd + "')";
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd.ExecuteReader();
            if (rdr1.Read() && !rdr1.IsDBNull(0))
            {
                this._username = rdr1.GetString(0);
                this._name = rdr1.GetString(2);
                this._cognome = rdr1.GetString(3);
                this._typeOfUser = rdr1.GetString(4);
                if (!rdr1.IsDBNull(5))
                {
                    this._lastLogin = rdr1.GetDateTime(5);
                }
                this._authenticated = true;
                this.lastLogin = DateTime.Now;
                loadPermessi();
            }
            rdr1.Close();
            conn.Close();
        }

        public User(String usr)
        {
            this._authenticated = false;
            String strSQL = "SELECT userID, nome, cognome, tipoUtente, lastLogin FROM users WHERE userID LIKE '" + usr + "'";
            MySqlConnection conn = Dati.Dati.mycon();
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
                loadPermessi();
            }
            rdr1.Close();
            conn.Close();
        }

        public User()
        {
            this._username = "";
            this._typeOfUser = "";
            this._name = "";
            this._cognome = "";
            this._authenticated = false;
        }

        private bool loadPermessi()
        {
            bool rt;
            if (this._username != "")
            {                
                String strSQL = "SELECT COUNT(*) FROM processo";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                this._numPermessiProcessi = rdr.GetInt32(0);
                this._permessiProcessi = new Permesso[this._numPermessiProcessi];
                rdr.Close();
                strSQL = "SELECT processID FROM processo ORDER BY Name";
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read() && i < this._numPermessiProcessi)
                {
                    this._permessiProcessi[i] = new Permesso(this.username, new processo(rdr.GetInt32(0)));
                    i++;
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

        /* Returns:
         * 0 if something wrong
         * 1 if _ownedProcesses array is load correctly
         * 2 if user is not authenticated (array will not be loaded)
         */
        public int loadOwnedProcesses()
        {
            int rt = 0;
            if (this.username != "")
            {
                String strSQL = "SELECT COUNT(process) FROM processOwners WHERE user = '" + this.username + "'";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                _numOwnedProcesses = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    _numOwnedProcesses = rdr.GetInt32(0);
                }
                this._ownedProcesses = new int[_numOwnedProcesses];
                rdr.Close();

                strSQL = "SELECT process FROM processowners WHERE user = '" + this.username + "'";
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                int cont = 0;
                while (rdr.Read() && cont < numOwnedProcesses)
                {
                    this._ownedProcesses[cont] = rdr.GetInt32(0);
                    cont++;
                }
                conn.Close();
                rt = 1;
            }
            return rt;
        }

        /* Returns:
 * 0 if something wrong
 * 1 if _ownedProcesses array is load correctly
 * 2 if user is not authenticated (array will not be loaded)
 */
        public int loadMainOwnedProcesses()
        {
            int rt = 0;
            if (!String.IsNullOrEmpty(this.username))
            {
                String strSQL = "SELECT COUNT(DISTINCT process) FROM processOwners INNER JOIN productionplan ON (processOwners.process = productionplan.processo) WHERE user = '" + this.username + "' GROUP BY processOwners.process";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                _numMainOwnedProcesses = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    _numMainOwnedProcesses = rdr.GetInt32(0);
                }
                this._mainOwnedProcesses = new int[_numMainOwnedProcesses];
                rdr.Close();

                strSQL = "SELECT process FROM processowners INNER JOIN productionplan ON (processOwners.process = productionplan.processo) WHERE user = '" + this.username + "' GROUP BY processOwners.process";
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                int cont = 0;
                while (rdr.Read() && cont < numMainOwnedProcesses)
                {
                    this._mainOwnedProcesses[cont] = rdr.GetInt32(0);
                    cont++;
                }
                conn.Close();
                rt = 1;
            }
            return rt;
        }

        public int findPermessoIndex(int processID)
        {
            int index = -1;
            for (int i = 0; i < this._numPermessiProcessi; i++)
            {
                if (permessiProcessi[i].process.processID == processID)
                {
                    index = i;
                    break;
                }
            }
            return index;
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
            MySqlConnection conn = Dati.Dati.mycon();
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
                strSQL = "INSERT INTO users(userID, password, nome, cognome, tipoUtente, lastLogin) VALUES('" + usr + "', MD5('" + pwd + "'), '" + nome + "', '" + cognome + "', '" + typeOf + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                rt = 1;
            }
            rdr1.Close();
            conn.Close();
            return rt;
        }


        private int _numOwnedTasks;
        public int numOwnedTasks
        {
            get { return this._numOwnedTasks; }
        }

        private int[] _ownedTasks;
        public int[] ownedTasks
        {
            get { return this._ownedTasks; }
        }
        /* Returns:
 * 0 if something wrong
 * 1 if _ownedTasks array is load correctly
 * 2 if user is not authenticated (array will not be loaded)
 */
        public int loadOwnedTasks()
        {
            int rt = 0;
            if (!String.IsNullOrEmpty(this.username))
            {
                String strSQL = "SELECT COUNT(taskID) FROM tasksproduzione WHERE user = '" + this.username + "' ORDER BY cadenza";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                _numMainOwnedProcesses = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._numOwnedTasks = rdr.GetInt32(0);
                }
                this._ownedTasks = new int[_numOwnedTasks];
                rdr.Close();

                strSQL = "SELECT taskID FROM tasksproduzione WHERE user = '" + this.username + "' ORDER BY cadenza";
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                int cont = 0;
                while (rdr.Read() && cont < numOwnedTasks)
                {
                    this._ownedTasks[cont] = rdr.GetInt32(0);
                    cont++;
                }
                conn.Close();
                rt = 1;
            }
            return rt;
        }


    }
}