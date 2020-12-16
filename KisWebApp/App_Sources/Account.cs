using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace KIS.App_Sources
{
    public class UserAccount
    {
        public String log;

        private int _id;
        public int id
        {
            get { return this._id; }
        }

        private String _userId;
        public String userId
        {
            get { return this._userId; }
        }
        private MailAddress _email;
        public MailAddress email
        {
            get { return this._email; }
        }
        private String _firstname;
        public String firstname
        {
            get { return this._firstname; }
        }
        private String _lastname;
        public String lastname
        {
            get { return this._lastname; }
        }
        private String _nickname;
        public String nickname
        {
            get { return this._nickname; }
        }
        private String _pictureUrl;
        public String pictureUrl
        {
            get { return this._pictureUrl; }
        }
        private String _locale;
        public String locale
        {
            get { return this._locale; }
        }
        private DateTime _updatedAt;
        public DateTime updatedAt
        {
            get { return this._updatedAt; }
        }
        private String _iss;
        public String iss
        {
            get { return this._iss; }
        }
        private String _nonce;
        public String nonce
        {
            get { return this._nonce; }
        }
        private String _access_token;
        public String access_token
        {
            get { return this._access_token; }
        }
        private String _refresh_token;
        public String refresh_token
        {
            get { return this._refresh_token; }
        }
        private DateTime _created_at;
        public DateTime created_at
        {
            get
            {
                return this._created_at;
            }
        }

        /* Returns:
         * Object with data, if account exists in Virtual Chief
         * id = -1 otherwise
         */
        public UserAccount(String id)
        {
            this._id = -1;
            this._userId = "";
            this._Workspaces = new List<Workspace>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, access_token, refresh_token, created_at "
                + " FROM useraccounts WHERE userid=@userid";
            cmd.Parameters.AddWithValue("@userid", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._userId = rdr.GetString(1);
                this._email = new MailAddress(rdr.GetString(2));
                this._firstname = rdr.GetString(3);
                this._lastname = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                this._nickname = rdr.GetString(5);
                this._pictureUrl = rdr.GetString(6);
                this._locale = rdr.IsDBNull(7) ? "en" : rdr.GetString(7);
                this._updatedAt = rdr.GetDateTime(8);
                this._iss = rdr.GetString(9);
                this._nonce = rdr.GetString(10);
                this._access_token = rdr.IsDBNull(11) ? "" : rdr.GetString(11);
                this._refresh_token = rdr.IsDBNull(12) ? "" : rdr.GetString(12);
                this._created_at = rdr.GetDateTime(13);
            }
            rdr.Close();
            conn.Close();
        }

        private List<Workspace> _Workspaces;
        public List<Workspace> workspaces
        {
            get { return this._Workspaces; }
        }

        private Workspace _DefaultWorkspace;
        public Workspace DefaultWorkspace
        {
            get { return this._DefaultWorkspace; }
        }

        public void loadWorkspaces()
        {
            this._Workspaces = new List<Workspace>();
            if(this.userId.Length > 1)
            { 
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM workspaces INNER JOIN useraccountworkspaces ON(useraccountworkspaces.workspaceid=workspaces.id) "
                    + " WHERE useraccountworkspaces.userid = @userId"
                    + "ORDER BY name";
                cmd.Parameters.AddWithValue("@userId", this.userId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.workspaces.Add(new Workspace(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }


        /* Returns:
         * 0 if generic error
         * 1 if added successfully
         * 2 if name incorrect
         * 3 if user not found
         * 4 error while adding workspace
         * 5 if error while linking new workspace to the useraccount
         */
        public int addWorkspace(String name)
        {
            int ret = 0;
            if (name.Length > 0 && name.Length < 255)
            {
                if (this.userId.Length > 1)
                {
                    Boolean defaultWS = true;
                    this.loadDefaultWorkspace();
                    if(this.DefaultWorkspace != null && this.DefaultWorkspace.id !=-1)
                    {
                        defaultWS = false;
                    }

                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO workspaces(name, creator, enabled, enableddate) VALUES(@wsname, @wscreator, @enabled, @enableddate)";
                    cmd.Parameters.AddWithValue("@wsname", name);
                    cmd.Parameters.AddWithValue("@wscreator", this.userId);
                    cmd.Parameters.AddWithValue("@enabled", true);
                    cmd.Parameters.AddWithValue("@enableddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    try
                    { 
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                        ret = 4;
                    }

                    cmd.CommandText = "SELECT MAX(id) FROM workspaces WHERE creator=@userid2 ORDER BY creationdate DESC";
                    cmd.Parameters.AddWithValue("@userid2", this.userId);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int wsid = -1;
                    if(rdr.Read() && !rdr.IsDBNull(0)) { wsid = rdr.GetInt32(0); }
                    rdr.Close();
                    if(wsid!=-1)
                    {
                        cmd.CommandText = "INSERT INTO useraccountworkspaces(userid, workspaceid, invite_sent, invite_sent_at, invitation_accepted, invitation_accepted_at, default_ws) "
                            + " VALUES(@userid3, @workspaceid, @invite_sent, @invite_sent_at, @invitation_accepted, @invitation_accepted_at, @default)";
                        cmd.Parameters.AddWithValue("@userid3", this.id);
                        cmd.Parameters.AddWithValue("@workspaceid", wsid);
                        cmd.Parameters.AddWithValue("@invite_sent", null);
                        cmd.Parameters.AddWithValue("@invite_sent_at", null);
                        cmd.Parameters.AddWithValue("@invitation_accepted", false);
                        cmd.Parameters.AddWithValue("@invitation_accepted_at", null);
                        cmd.Parameters.AddWithValue("@default", defaultWS);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                            tr.Rollback();
                            ret = 5;
                        }
                    }

                    conn.Close();
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public void loadDefaultWorkspace()
        {
            this._DefaultWorkspace = null;
            if (this.userId.Length > 1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM workspaces INNER JOIN useraccountworkspaces ON(useraccountworkspaces.workspaceid=workspaces.id) "
                    + " WHERE useraccountworkspaces.userid = @userId AND default_ws=@defaultws"
                    + " ORDER BY name";
                cmd.Parameters.AddWithValue("@userId", this.userId);
                cmd.Parameters.AddWithValue("@defaultws", true);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._DefaultWorkspace = new Workspace(rdr.GetInt32(0));
                }
                rdr.Close();
                conn.Close();
            }
        }
    }

    public class UserAccounts
    {
        public String log;

        public List<UserAccount> list;
        public UserAccounts()
        {
            this.list = new List<UserAccount>();
        }

        public void loadList()
        {
            this.list = new List<UserAccount>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userid FROM useraccounts ORDER BY email";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.list.Add(new UserAccount(rdr.GetString(0)));
            }
            rdr.Close();
            conn.Close();
        }

        /* Returns:
         * 1 if user added successfully
         * 0 if generic error
         * 2 if user already exists
         * 3 if other error while adding
         */
        public int Add(String userid, MailAddress email, String firstname, String lastname, String nickname, String picture_url, String locale, DateTime update_at,
            String iss, String nonce, String id_token, String access_token, String refresh_token)
        {
            int ret = 0;
            UserAccount curr = new UserAccount(userid);
            if(curr.id != -1 && curr.userId.Length > 0)
            {
                ret = 2;
            }
            else if(userid.Length > 0 && email.Address.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO useraccounts(userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, id_token, access_token, refresh_token, created_at) "
                    + "VALUES(@userid, @email, @firstname, @lastname, @nickname, @picture_url, @locale, @update_at, @iss, @nonce, @id_token, @access_token, @refresh_token, @created_at)";
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@email", email.Address);
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@lastname", lastname);
                cmd.Parameters.AddWithValue("@nickname", nickname);
                cmd.Parameters.AddWithValue("@picture_url", picture_url);
                cmd.Parameters.AddWithValue("@locale", locale);
                cmd.Parameters.AddWithValue("@update_at", update_at.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@iss", iss);
                cmd.Parameters.AddWithValue("@nonce", nonce);
                cmd.Parameters.AddWithValue("@id_token", id_token);
                cmd.Parameters.AddWithValue("@access_token", access_token);
                cmd.Parameters.AddWithValue("@refresh_token", refresh_token);
                cmd.Parameters.AddWithValue("@created_at", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    ret = 1;
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    ret = 3;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = 0;
            }
            return ret;
        }
    }

    public class Workspace
    {
        private int _id;
        public int id
        {
            get
            {
                return this._id;
            }
        }

        private String _Name;
        public String Name { get { return this._Name; } }
        private DateTime _creationDate;
        public DateTime CreationDate { get { return this._creationDate; } }

        private String _Creator;
        public String Creator { get { return this._Creator; } }
        private Boolean _enabled;
        public Boolean Enabled { get { return this._enabled; } }
        private DateTime _enabledDate;
        public DateTime enabledDate { get { return this._enabledDate; } }

        public Workspace(int wsid)
        {
            this._id = -1;
            this._Name = "";
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, name, creationdate, creator, enabled, enableddate FROM workspaces WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", wsid);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._Name = rdr.GetString(1);
                this._creationDate = rdr.GetDateTime(2);
                this._Creator = rdr.GetString(3);
                this._enabled = rdr.GetBoolean(4);
                this._enabledDate = rdr.GetDateTime(5);
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class Workspaces
    {
        public List<Workspace> workspaces;

        public void loadWorkspaces()
        {
            this.workspaces = new List<Workspace>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM workspaces ORDER BY name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.workspaces.Add(new Workspace(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }
}