using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.App_Code;


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

        /* Returns:
          * Object with data, if account exists in Virtual Chief
          * id = -1 otherwise
          */
        public UserAccount(int id)
        {
            this._id = -1;
            this._userId = "";
            this._Workspaces = new List<Workspace>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, access_token, refresh_token, created_at "
                + " FROM useraccounts WHERE id=@userid";
            cmd.Parameters.AddWithValue("@userid", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
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
                    + " WHERE useraccountworkspaces.userid = @userId "
                    + " ORDER BY name";
                cmd.Parameters.AddWithValue("@userId", this.id);
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
         * workspace_id if added successfully
         * -2 if name incorrect
         * -3 if user not found
         * -4 error while adding workspace
         * -5 if error while linking new workspace to the useraccount
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
                        ret = -4;
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
                            ret = wsid;
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                            tr.Rollback();
                            ret = -5;
                        }
                    }

                    conn.Close();
                }
                else
                {
                    ret = -3;
                }
            }
            else
            {
                ret = -2;
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

        private List<Group> _Groups;
        public List<Group> Groups { get { return this._Groups; } }

        private List<GroupPermissions> _GroupPermissions;
        public List<GroupPermissions> GroupPermissions { get { return this._GroupPermissions; } }

        public void loadGroupPermissions(int WorkspaceId)
        {
            this._GroupPermissions = new List<GroupPermissions>();
            if(this.id !=-1 && WorkspaceId >=0)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupid FROM useraccountsgroups WHERE userid=@userid AND workspaceid=@workspaceid";
                cmd.Parameters.AddWithValue("@userid", this.id);
                cmd.Parameters.AddWithValue("@workspaceid", WorkspaceId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._GroupPermissions.Add(new GroupPermissions(rdr.GetInt32(0)));
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

        private List<UserAccount> _UserAccounts;
        public List<UserAccount> UserAccounts { get { return this._UserAccounts; } }

        public Workspace(int wsid)
        {
            this._id = -1;
            this._Name = "";
            this._UserAccounts = new List<UserAccount>();
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

        public Workspace(String wsid)
        {
            this._id = -1;
            this._Name = "";
            this._UserAccounts = new List<UserAccount>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, name, creationdate, creator, enabled, enableddate FROM workspaces WHERE name=@name";
            cmd.Parameters.AddWithValue("@name", wsid);
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

        public void loadUserAccounts()
        {
            this._UserAccounts = new List<UserAccount>();
            if(this.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT useraccounts.id FROM useraccountworkspaces "
                    + " INNER JOIN useraccounts ON(useraccountworkspaces.userid= useraccounts.id) "
                    + " WHERE workspaceid = @workspaceid "
                    + " ORDER BY useraccounts.userid";
                cmd.Parameters.AddWithValue("@workspaceid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._UserAccounts.Add(new UserAccount(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if invitation sent successfully
         * 2 if
         */
        public int InviteUser(UserAccount usr)
        {
            int ret = 0;
            return ret;
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
                    String strSQL = "UPDATE groupss SET nomeGruppo = @GroupName WHERE id = @ID";
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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

        private GroupPermissions _Permessi;
        public GroupPermissions Permessi
        {
            get { return this._Permessi; }
        }

        public Group(int groupID)
        {
            String strSQL = "SELECT * FROM groupss WHERE id = @ID";
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            cmd.Parameters.AddWithValue("@ID", groupID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            this._Permessi = new GroupPermissions(groupID);
            if (rdr.HasRows)
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

        public Group(String GroupName) : base()
        {
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, nomegruppo, descrizione FROM groupss WHERE nomeGruppo = @GroupName";
            cmd.Parameters.AddWithValue("@GroupName", GroupName);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idVoce FROM menugruppi WHERE gruppo = @ID"
                    + " ORDER BY ordinamento";
                cmd.Parameters.AddWithValue("@ID", this.ID);
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                this.loadMenu();
                int maxOrd = this.VociDiMenu.Count + 1;
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
                catch (Exception ex)
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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

        private List<String> _Utenti;
        public List<String> Utenti
        {
            get
            {
                return this._Utenti;
            }
        }

        public void loadUtenti(int workspaceid)
        {
            this._Utenti = new List<string>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT user FROM useraccountsgroups WHERE groupID = @ID AND workspaceid=@workspaceid";
                cmd.Parameters.AddWithValue("@ID", this.ID);
                cmd.Parameters.AddWithValue("@workspaceid", workspaceid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Utenti.Add(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }
        }

    /*    public List<Reparto> SegnalazioneRitardiReparto
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
                        // ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }
        }*/

        public List<Reparto> SegnalazioneRitardiReparto(String tenant)
        {
                List<Reparto> ret = new List<Reparto>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Ritardo' "
                        + "AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        // ret.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
        }

        public List<Commessa> SegnalazioneRitardiCommessa(String tenant)
        {

                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
        }

        public List<Articolo> SegnalazioneRitardiArticolo(String tenant)
        {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Ritardo' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
        }

        public List<Reparto> SegnalazioneWarningReparto(String tenant)
        {
                List<Reparto> ret = new List<Reparto>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT idReparto FROM eventorepartogruppi WHERE TipoEvento LIKE 'Warning' "
                        + "AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ret.Add(new Reparto(tenant, rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    conn.Close();
                }
                return ret;
        }

        public List<Commessa> SegnalazioneWarningCommessa(String tenant)
        {
                List<Commessa> ret = new List<Commessa>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT commessaid, commessaanno FROM eventocommessagruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Commessa cm = new Commessa(tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                        if (cm.Status != 'F')
                        {
                            ret.Add(cm);
                        }
                    }
                    conn.Close();
                }
                return ret;
        }

        public List<Articolo> SegnalazioneWarningArticolo(String tenant)
        {
                List<Articolo> ret = new List<Articolo>();
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT articoloid, articoloanno FROM eventoarticologruppi WHERE "
                        + "TipoEvento LIKE 'Warning' AND idGruppo = @ID";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Articolo cm = new Articolo(tenant, rdr.GetInt32(0), rdr.GetInt32(1));
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

    public class GroupList
    {
        public String log;

        public List<Group> Elenco;

        public GroupList()
        {
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM groupss ORDER BY nomeGruppo";
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
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
            catch (Exception ex)
            {
                rt = false;
                log = ex.Message;
                trn.Rollback();
            }
            conn.Close();
            return rt;
        }
    }

    public class GroupPermission
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

                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    // Controllo se esiste già il record. Se esiste lo aggiorno, altrimenti lo creo.
                    bool recExists;

                    cmd.CommandText = "SELECT * FROM groupspermissions WHERE groupid = @IDGroup AND permissionid = @IDPermesso";
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
                        cmd.CommandText = "UPDATE groupspermissions SET x = @x WHERE groupid = @IDGroup AND permissionid = @IDPermesso";
                        cmd.Parameters.AddWithValue("@x", value);
                        cmd.Parameters.AddWithValue("@IDGroup", this.GroupID);
                        cmd.Parameters.AddWithValue("@IDPermesso", this.IdPermesso);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO groupspermissions(groupid, permissionid, r, w, x) VALUES(@IDGroup, @IDPermesso, false, false, @x)";
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

        public GroupPermission(int grp, Permission prm)
        {
            bool check = false;
            if (prm.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                this.Permes = new Permission(prm.ID);
                this._NomePermesso = Permes.Nome;
                this._PermessoDesc = Permes.Descrizione;
                this._IdPermesso = Permes.ID;
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
    }

    public class GroupPermissions
    {
        public List<GroupPermission> Elenco;
        private int _GroupID;
        public int GroupID
        {
            get
            {
                return this._GroupID;
            }
        }

        public GroupPermissions(int grp)
        {
            bool check = false;
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
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
                this.Elenco = new List<GroupPermission>();
                PermissionsList elPrm = new PermissionsList();
                for (int i = 0; i < elPrm.Elenco.Count; i++)
                {
                    this.Elenco.Add(new GroupPermission(this.GroupID, elPrm.Elenco[i]));
                }

            }
            else
            {
                this._GroupID = -1;
                this.Elenco = null;
            }
        }
    }
}