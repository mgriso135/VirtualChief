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
            set { this._firstname = ""; }
        }
        private String _lastname;
        public String lastname
        {
            get { return this._lastname; }
            set { this._lastname = value; }
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

        public String Language { get { return this.locale; } 
            set { this.Language = value; }
        }

        public List<Group> groups;

        private String _DestinationURL;
        public String DestinationURL { get { return this._DestinationURL; } 
            set { this._DestinationURL = value; }
        }
        public List<UserEmail> Email;

        private Boolean _GlobalAdmin;
        public Boolean GlobalAdmin { get { return this._GlobalAdmin; } }

        public List<WorkspaceInvite> WorkspaceInvites;

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
            cmd.CommandText = "SELECT id, userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, access_token, refresh_token, created_at, "
                +" lastlogin, globaladmin "
                + " FROM useraccounts WHERE userid=@userid";
            cmd.Parameters.AddWithValue("@userid", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._userId = rdr.GetString(1);
                this._email = new MailAddress(rdr.GetString(2));
                this._firstname = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
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
                this._LastLogin = rdr.GetDateTime(14);
                this._GlobalAdmin = rdr.GetBoolean(15);
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
            cmd.CommandText = "SELECT id, userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, access_token, refresh_token, created_at, "
                + " lastlogin, globaladmin "
                + " FROM useraccounts WHERE id=@userid";
            cmd.Parameters.AddWithValue("@userid", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._userId = rdr.GetString(1);
                this._email = new MailAddress(rdr.GetString(2));
                this._firstname = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
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
                this._LastLogin = rdr.GetDateTime(14);
                this._GlobalAdmin = rdr.GetBoolean(15);
            }
            rdr.Close();
            conn.Close();
        }

        /* Returns:
          * Object with data, if account exists in Virtual Chief
          * id = -1 otherwise
          */
        public UserAccount(MailAddress id)
        {
            this._id = -1;
            this._userId = "";
            this._Workspaces = new List<Workspace>();
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, userid, email, firstname, lastname, nickname, picture_url, locale, updated_at, iss, nonce, access_token, refresh_token, created_at, "
                + " lastlogin, globaladmin "
                + " FROM useraccounts WHERE email=@userid";
            cmd.Parameters.AddWithValue("@userid", id.Address);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._userId = rdr.GetString(1);
                this._email = new MailAddress(rdr.GetString(2));
                this._firstname = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
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
                this._LastLogin = rdr.GetDateTime(14);
                this._GlobalAdmin = rdr.GetBoolean(15);
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

        private DateTime _LastLogin;
        public DateTime LastLogin { get { return this._LastLogin; } 
            set
            {
                if(this.id!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE useraccounts SET lastlogin=@lastlogin WHERE id=@userid";
                    cmd.Parameters.AddWithValue("@lastlogin", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@userid", this.id);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
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
                        cmd.CommandText = "INSERT INTO useraccountworkspaces(userid, workspaceid, invite_sent, invite_sent_date, invitation_accepted, invitation_accepted_date, "
                            + " default_ws, invite_checksum) "
                            + " VALUES(@userid3, @workspaceid, @invite_sent, @invite_sent_at, @invitation_accepted, @invitation_accepted_at, @default, @invite_checksum)";
                        String chksum = Dati.Utilities.getRandomString(16);
                        cmd.Parameters.AddWithValue("@userid3", this.id);
                        cmd.Parameters.AddWithValue("@workspaceid", wsid);
                        cmd.Parameters.AddWithValue("@invite_sent", null);
                        cmd.Parameters.AddWithValue("@invite_sent_at", null);
                        cmd.Parameters.AddWithValue("@invitation_accepted", false);
                        cmd.Parameters.AddWithValue("@invitation_accepted_at", null);
                        cmd.Parameters.AddWithValue("@default", defaultWS);
                        cmd.Parameters.AddWithValue("@invite_checksum", chksum);

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

        private List<GroupPermissions> _GroupPermissions;
        public List<GroupPermissions> GroupPermissions { get { return this._GroupPermissions; } }

        public bool loadGroups(int workspaceid)
        {
            bool rt;
            this.groups = new List<Group>();
            if (this.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupID FROM useraccountsgroups INNER JOIN groupss ON (groupss.id = useraccountsgroups.groupid)  "
                    + " WHERE useraccountsgroups.userid=@user "
                    + " AND workspaceid=@wsid"
                    + " ORDER BY groupss.name";
                cmd.Parameters.AddWithValue("@user", this.id);
                cmd.Parameters.AddWithValue("@wsid", workspaceid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.groups.Add(new Group(rdr.GetInt32(0)));
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

        /* Returns:
         * 0 if generic error
         * 1 if delete successfully
         * 2 if workspace not found or group not found
         * 3 if error while deleting
         */
        public int DeleteWorkspaceGroup(int workspace, int group)
        {
            int ret = 0;
            if(this.id!=-1 && workspace>=0 && group>=0)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM useraccountsgroups WHERE groupid=@group AND workspaceid=@ws AND userid=@usr";
                cmd.Parameters.AddWithValue("@group", group);
                cmd.Parameters.AddWithValue("@ws", workspace);
                cmd.Parameters.AddWithValue("@usr", this.id);

                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    ret = 3;
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

        public bool ValidatePermissions(String workspace, List<String[]> elencoPrm)
        {
            bool rt = false;
            List<bool> trovato = new List<bool>();
            Workspace ws = new Workspace(workspace);
            if(ws.id!=-1)
            {
                int workspaceid = ws.id;
                for (int i = 0; i < elencoPrm.Count; i++)
                {
                    bool found = false;
                    this.loadGroups(workspaceid);
                    for (int j = 0; j < this.groups.Count; j++)
                    {
                        for (int k = 0; k < this.groups[j].Permissions.Elenco.Count; k++)
                        {
                            if (this.groups[j].Permissions.Elenco[k].NomePermesso == elencoPrm[i][0])
                            {
                                if (elencoPrm[i][1] == "R" && this.groups[j].Permissions.Elenco[k].R == true)
                                {
                                    found = true;
                                }
                                else if (elencoPrm[i][1] == "W" && this.groups[j].Permissions.Elenco[k].W == true)
                                {
                                    found = true;
                                }
                                else if (elencoPrm[i][1] == "X" && this.groups[j].Permissions.Elenco[k].X == true)
                                {
                                    found = true;
                                }

                            }
                        }

                    }
                    trovato.Add(found);
                }
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

        public void loadEmails()
        {
            Email = new List<UserEmail>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT email FROM useremail WHERE userID LIKE @userID ORDER BY note";
                cmd.Parameters.AddWithValue("@userID", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Email.Add(new UserEmail(this.userId, rdr.GetString(0)));
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
            if (mailAddr != null && this.userId != "")
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO useremail(userid, email, forAlarm, note) VALUES("
                    + "@userID, @email, @forAlarm, @note)";
                cmd.Parameters.AddWithValue("@userID", this.id);
                cmd.Parameters.AddWithValue("@email", mailAddr.Address);
                cmd.Parameters.AddWithValue("@forAlarm", forAlarm);
                cmd.Parameters.AddWithValue("@note", note);
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

        public List<UserPhoneNumber> PhoneNumbers;
        public void loadPhoneNumbers()
        {
            PhoneNumbers = new List<UserPhoneNumber>();
            if (this.userId != "")
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT phoneNumber FROM userphonenumbers WHERE userID LIKE @userID ORDER BY note";
                cmd.Parameters.AddWithValue("@userID", this.userId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.PhoneNumbers.Add(new UserPhoneNumber(this.userId, rdr.GetString(0)));
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
            if (phoneINT != -1 && this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO userphoneNumbers(userid, phoneNumber, forAlarm, note) VALUES("
                    + "@userid, @phoneNumber, @forAlarm, @note)";
                cmd.Parameters.AddWithValue("@userid", this.userId);
                cmd.Parameters.AddWithValue("@phoneNumber", phone);
                cmd.Parameters.AddWithValue("@forAlarm", forAlarm);
                cmd.Parameters.AddWithValue("@note", note);
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

        public void loadWorkspaceInvites()
        {
            this.WorkspaceInvites = new List<WorkspaceInvite>();
            if(this.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(workspace) FROM workspacesinvites WHERE mail LIKE @mail AND accepted IS false" 
                    + " AND sent_date >= NOW() - INTERVAL 2 DAY ORDER BY sent_date DESC";
                cmd.Parameters.AddWithValue("@mail", this.email.Address.ToString());
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    WorkspaceInvite curr = new WorkspaceInvite(this.email, new Workspace(rdr.GetInt32(0)));
                    this.WorkspaceInvites.Add(curr);
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if invite accepted successfully
         * 2 if no invite found
         * 3 if error while updating the databases
         */
         public int AcceptWorkspaceInvite(Workspace ws)
        {
            int ret = 0;
            if(this.id!=-1)
            {
                WorkspaceInvite wsinv = new WorkspaceInvite(this.email, ws);
                if(!wsinv.Accepted && wsinv.SentDate >= DateTime.UtcNow.AddDays(-2))
                {
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;

                    try
                    {
                        cmd.CommandText = "UPDATE workspacesinvites SET accepted=true, accepted_date=NOW() WHERE workspace=@wsid "
                            + " AND mail=@mail AND sent_date >= NOW() - INTERVAL 2 DAY";
                        cmd.Parameters.AddWithValue("@wsid", ws.id.ToString());
                        cmd.Parameters.AddWithValue("@mail", this.email.Address.ToString());
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "INSERT INTO useraccountworkspaces(userid, workspaceid, invite_sent, invite_sent_date, invite_checksum, "
                            + " invitation_accepted, invitation_accepted_date, default_ws, destinationUrl) "
                            + "VALUES(@userid, @workspaceid, @invite_sent, @invite_sent_date, @invite_checksum, "
                            + " @invitation_accepted, @invitation_accepted_date, @default_ws, @destinationUrl)";
                        cmd.Parameters.AddWithValue("@userid", this.id.ToString());
                        cmd.Parameters.AddWithValue("@workspaceid", ws.id.ToString());
                        cmd.Parameters.AddWithValue("@invite_sent", true);
                        cmd.Parameters.AddWithValue("@invite_sent_date", wsinv.SentDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@invite_checksum", wsinv.checksum);
                        cmd.Parameters.AddWithValue("@invitation_accepted", true);
                        cmd.Parameters.AddWithValue("@invitation_accepted_date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@default_ws", false);
                        cmd.Parameters.AddWithValue("@destinationUrl", "");
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "INSERT INTO useraccountsgroups(groupid, userid, workspaceid) "
                            + "VALUES(@groupid, @userid, @workspaceid)";
                        cmd.Parameters.AddWithValue("@groupid", 14);
                        cmd.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        ret = 3;
                        this.log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();
                }
                else
                {
                    ret = 2;
                }
            }
            return ret;
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
        public String log;

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

        public List<WorkspaceInvite> Invites;

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
         * 2 if error while inserting record in the database
         * 3 if mail is not valid or workspace not valid
         * 4 if error while sending mail
         * 5 if user is already part of this workspace
         */
        public int InviteUser(MailAddress email, UserAccount host)
        {
            int ret = 0;
            if(email.Address.Length > 0 && this.id != -1)
            {
                UserAccount newUsr = new UserAccount(email);
                this.loadUserAccounts();
                bool alreadyInvited = false;

                try
                {
                    var found = this.UserAccounts.First(x => x.email.Address == email.Address);
                    alreadyInvited = true;
                }
                catch(Exception ex)
                {
                    alreadyInvited = false;
                }

                if (!alreadyInvited)
                {
                    String checksum = Dati.Utilities.getRandomString(16);
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO workspacesinvites(mail, workspace, invited_by, checksum) VALUES(@mail, @workspace, @invited_by, @checksum)";
                    cmd.Parameters.AddWithValue("@mail", email.Address);
                    cmd.Parameters.AddWithValue("@workspace", this.id.ToString());
                    cmd.Parameters.AddWithValue("@invited_by", host.id);
                    cmd.Parameters.AddWithValue("@checksum", checksum);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        ret = 1;
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        ret = 2;
                        tr.Rollback();
                    }

                    // Sends e-mail
                    if (ret == 1)
                    {
                        // Invio l'e-mail
                        System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                        mMessage.From = new MailAddress("matteo.griso@virtualchief.net", "Matteo@VirtualChief");
                        mMessage.To.Add(new MailAddress(email.Address, email.DisplayName));
                        mMessage.Subject = "[Virtual Chief] " + ResAccountMgm.Workspace.lblInviteMailTitle;
                        mMessage.IsBodyHtml = true;
                        mMessage.Body = "<html><body><div>"
                            + ResAccountMgm.Workspace.lblInviteMailBody + " - "
                            + "<a href='http://www.virtual-chief.com/AccountsMgm/Workspaces/ViewInvites'>" + this.Name + "</a>"
                            + "</div></body></html>";

                        SmtpClient smtpcli = new SmtpClient();
                        smtpcli.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpcli.EnableSsl = true;

                        try
                        {
                            smtpcli.Send(mMessage);
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                            ret = 4;
                        }
                    }

                    conn.Close();
                }
                else
                {
                    ret = 5;
                }
            }
            else
            {
                ret = 3;
            }
            return ret;
        }

        public void loadInvites()
        {
            this.Invites = new List<WorkspaceInvite>();
            if(this.id !=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT mail FROM workspacesinvites WHERE workspace=@wsid";
                cmd.Parameters.AddWithValue("@wsid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Invites.Add(new WorkspaceInvite(new MailAddress(rdr.GetString(0)), this));
                }
                rdr.Close();
                conn.Close();
            }
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
        public GroupPermissions Permissions
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
            cmd.CommandText = "SELECT id, name, description FROM groupss WHERE name=@GroupName";
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
                cmd.CommandText = "SELECT menuitemid FROM menugroups WHERE groupid=@ID "
                    + " ORDER BY sequence";
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

        /* Returns:
         * 0 if generic error
         * 1 if user added successfully
         */
        public int addUser(UserAccount usr, Workspace ws)
        {
            int ret = 0;
            if(usr.id!=-1 && ws.id!=-1 && this.ID!=-1)
            { 
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO useraccountsgroups(groupid, userid, workspaceid) VALUES(@groupid, @userid, @workspaceid)";
                cmd.Parameters.AddWithValue("@groupid", this.ID);
                cmd.Parameters.AddWithValue("@userid", usr.id);
                cmd.Parameters.AddWithValue("@workspaceid", ws.id);

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
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
                cmd.CommandText = "SELECT r, w, x FROM groupspermissions WHERE groupid=@GroupID AND permissionid=@PermesID";
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE useremail SET forAlarm=@forAlarm WHERE userID LIKE @userID AND email LIKE @email";
                cmd.Parameters.AddWithValue("@forAlarm", value);
                cmd.Parameters.AddWithValue("@userID", this.UserID);
                cmd.Parameters.AddWithValue("@email", this.Email);
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
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE useremail SET note = @note WHERE userID LIKE @userID"
                        + " AND email LIKE @email";
                    cmd.Parameters.AddWithValue("@note", value);
                    cmd.Parameters.AddWithValue("@userID", this.UserID);
                    cmd.Parameters.AddWithValue("@email", this.Email);
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
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, email, forAlarm, Note FROM useremail WHERE userID LIKE @usr AND email LIKE @email";
            cmd.Parameters.AddWithValue("@usr", usr);
            cmd.Parameters.AddWithValue("@email", email);
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM useremail WHERE userID LIKE @usr AND email LIKE @email";
                cmd.Parameters.AddWithValue("@usr", this.UserID);
                cmd.Parameters.AddWithValue("@email", this.Email);
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE userphonenumbers SET forAlarm=@forAlarm WHERE userID LIKE @usr"
                    + " AND phoneNumber LIKE @phone";
                cmd.Parameters.AddWithValue("@forAlarm", value);
                cmd.Parameters.AddWithValue("@usr", this.UserID);
                cmd.Parameters.AddWithValue("@phone", this.PhoneNumber);
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
                    MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE userphonenumbers SET note=@note WHERE userID LIKE @usr AND phoneNumber LIKE @phone";
                    cmd.Parameters.AddWithValue("@note", value);
                    cmd.Parameters.AddWithValue("@usr", this.UserID);
                    cmd.Parameters.AddWithValue("@phone", this.PhoneNumber);
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
            MySqlConnection conn = (new Dati.Dati()).VCMainConn();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID, PhoneNumber, forAlarm, Note FROM userphonenumbers WHERE userID LIKE @usr "
                + " AND PhoneNumber LIKE @phone";
            cmd.Parameters.AddWithValue("@usr", usr);
            cmd.Parameters.AddWithValue("@phone", phone);
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
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM userphonenumbers WHERE userID LIKE @userID AND phonenumber LIKE @phonenumber";
                cmd.Parameters.AddWithValue("@userID", this.UserID);
                cmd.Parameters.AddWithValue("@phonenumber", this.PhoneNumber);
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

    public class UserAccountWorkspace
    {
        public UserAccount UserAccount;
        public Workspace workspace;

        private int _WorkspaceId;
        public int WorkspaceId { get { return this._WorkspaceId; } }

        private int _UserId;
        public int UserId { get { return this._UserId; } }

        private String _destinationUrl;
        public String destinationUrl { get { return this._destinationUrl; } }

        private String _inviteSent;
        public String inviteSent
        {
            get
            {
                return this._inviteSent;
            }
        }

        private DateTime _inviteSentDate;
        public DateTime inviteSentDate
        {
            get
            {
                return this._inviteSentDate;
            }
        }

        private String _inviteChecksum;
        public String inviteChecksum { get { return this._inviteChecksum; } }

        public UserAccountWorkspace(UserAccount usr, Workspace ws)
        {
            this.UserAccount = null;
            this._UserId = -1;
            this._WorkspaceId = -1;
            this.workspace = null;

            if(usr.id!=-1 && ws.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT userid, workspaceid FROM useraccountworkspaces "
                    + " INNER JOIN useraccounts ON(useraccountworkspaces.userid = useraccounts.id) "
                    + " INNER JOIN workspaces ON(useraccountworkspaces.workspaceid = workspaces.id)"
                    + " WHERE "
                    + " useraccounts.id = @userid "
                    + " AND workspaces.id = @wsid";
                cmd.Parameters.AddWithValue("@userid", usr.id);
                cmd.Parameters.AddWithValue("@wsid", ws.id);
                MySqlDataReader rdr = cmd.ExecuteReader();

                Boolean alreadyPresent = false;
                while(rdr.Read())
                {
                    alreadyPresent = true;
                }

                if(alreadyPresent)
                {
                    // Update checksum and re-send the invite
                }
                else
                {
                    // addd the user and send the invite
                }
                rdr.Close();
                conn.Close();
            }
        }
    }

    public class WorkspaceInvite
    {
        private int _WorkspaceId;
        public int WorkspaceId
        {
            get { return this._WorkspaceId; }
        }

        private String _WorkspaceName;
        public String WorkspaceName
        {
            get { return this._WorkspaceName; }
        }

        private String _Email;
        public String Email
        {
            get { return this._Email; }
        }

        private int _InvitedBy;
        public int InvitedBy
        {
            get { return this._InvitedBy; }
        }

        private DateTime _SentDate;
        public DateTime SentDate
        {
            get { return this._SentDate; }
        }

        private String _checksum;
        public String checksum
        {
            get { return this._checksum; }
        }

        private Boolean _Accepted;
        public Boolean Accepted
        {
            get { return this._Accepted; }
        }

        private DateTime _AcceptedDate;
        public DateTime AcceptedDate
        {
            get { return this._AcceptedDate; }
        }

        public WorkspaceInvite(MailAddress mail, Workspace ws)
        {
            this._WorkspaceId = -1;
            this._Email = "";
            if(mail.Address.Length > 0 && ws.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT invited_by, sent_date, accepted, accepted_date FROM workspacesinvites WHERE mail LIKE @mail AND workspace=@wsid ORDER BY sent_date DESC";
                cmd.Parameters.AddWithValue("@mail", mail.Address);
                cmd.Parameters.AddWithValue("@wsid", ws.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    this._WorkspaceId = ws.id;
                    this._Email = mail.Address;
                    this._InvitedBy = rdr.GetInt32(0);
                    this._SentDate = rdr.GetDateTime(1);
                    this._Accepted = rdr.GetBoolean(2);
                    this._AcceptedDate = rdr.IsDBNull(3) ? new DateTime(1970,1,1) : rdr.GetDateTime(3);
                    this._WorkspaceName = ws.Name;
                }
                rdr.Close();
                conn.Close();
            }
        }

        public WorkspaceInvite()
        {
            this._WorkspaceId = -1;
            this._Email = "";
            this._InvitedBy = -1;
            this._SentDate = new DateTime(1970, 1, 1);
            this._checksum = "";
            this._Accepted = false;
            this._AcceptedDate = new DateTime(1970, 1, 1);
        }
    }
}