using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;


namespace KIS.App_Sources
{
    public class WorkInstructions
    {
        // Files will be stored in ~/Data/WorkInstructions
        // Only pdf will be allowed


        public class WorkInstruction
        {
            private int _ID;
            public int ID
            {
                get
                {
                    return this._ID;
                }
            }

            private int _Version;
            public int Version
            {
                get { return this._Version; }
            }
            private String _Name;
            public String Name
            {
                get { return this._Name; }
                set
                {
                    if (this.ID != -1 && this.Version != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE manuals SET Name='" + value + "' WHERE ID = " + this.ID.ToString() + " AND Version = " + this.Version.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._Name = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }

            private String _Description;
            public String Description
            {
                get { return this._Description; }
                set
                {
                    if (this.ID != -1 && this.Version != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE manuals SET Description='" + value + "' WHERE ID = " + this.ID.ToString() + " AND Version = " + this.Version.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._Description = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }
            private String _Path;
            public String Path { get { return this._Path; }
                set
                {
                    if(this.ID!=-1 && this.Version!=-1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE manuals SET path='" + value + "' WHERE ID = " + this.ID.ToString() + " AND Version = " + this.Version.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._Path = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }

            private DateTime _UploadDate;
            public DateTime UploadDate
            {
                get { return this._UploadDate; }
            }

            private DateTime _ExpiryDate;
            public DateTime ExpiryDate
            {
                get
                {
                    return this._ExpiryDate;
                }
                set
                {
                    if (this.ID != -1 && this.Version != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE manuals SET expirydate='" + value.ToString("yyyy-MM-dd") + "' WHERE ID = " + this.ID.ToString() + " AND Version = " + this.Version.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._ExpiryDate = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }

            private Boolean _IsActive;
            public Boolean IsActive
            {
                get
                {
                    return this._IsActive;
                }
                set
                {
                    if (this.ID != -1 && this.Version != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE manuals SET isActive=" + value + " WHERE ID = " + this.ID.ToString() + " AND Version = " + this.Version.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._IsActive = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }

            public List<WILabel> Labels;

            public WorkInstruction(int id, int version)
            {
                this._ID = -1;
                this._Version = -1;
                this._Name = "";
                this._Description = "";
                this._Path = "";
                this._UploadDate = new DateTime(1970, 1, 1);
                this._ExpiryDate = new DateTime(1970, 1, 1);
                this._IsActive = false;
                this.Labels = new List<WILabel>();

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive FROM manuals WHERE "
                    + "ID = " + id.ToString()
                    + " AND Version = " + version.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._ID = rdr.GetInt32(0);
                    this._Version = rdr.GetInt32(1);
                    this._Name = rdr.GetString(2);
                    this._Description = rdr.GetString(3);
                    this._Path = rdr.GetString(4);
                    this._UploadDate = rdr.GetDateTime(5);
                    this._ExpiryDate = rdr.GetDateTime(6);
                    this._IsActive = rdr.GetBoolean(7);

                }
                rdr.Close();
                conn.Close();

            }

            public WorkInstruction(int id)
            {
                this._ID = -1;
                this._Version = -1;
                this._Name = "";
                this._Description = "";
                this._Path = "";
                this._UploadDate = new DateTime(1970, 1, 1);
                this._ExpiryDate = new DateTime(1970, 1, 1);
                this._IsActive = false;
                this.Labels = new List<WILabel>();

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive FROM manuals WHERE "
                    + "ID = " + id.ToString()
                    + " ORDER BY version DESC";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._ID = rdr.GetInt32(0);
                    this._Version = rdr.GetInt32(1);
                    this._Name = rdr.GetString(2);
                    this._Description = rdr.GetString(3);
                    this._Path = rdr.GetString(4);
                    this._UploadDate = rdr.GetDateTime(5);
                    this._ExpiryDate = rdr.GetDateTime(6);
                    this._IsActive = rdr.GetBoolean(7);

                }
                rdr.Close();
                conn.Close();

            }

            public void loadLabels()
            {
                this.Labels = new List<WILabel>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT LabelID FROM manualswilabels WHERE ManualID = " + this.ID.ToString() + " AND ManualVersion = " + this.Version.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Labels.Add(new WILabel(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }

            public Boolean addLabel(int labelID)
            {
                Boolean ret = false;
                this.loadLabels();
                bool bfound = false;
                try
                { 
                var found = this.Labels.First(x => x.WILabelID == labelID);
                    bfound = true;
                }
                catch
                {
                    bfound = false;
                }

                if(!bfound)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO manualswilabels(ManualID, ManualVersion, LabelID) VALUES(@ManualID, @ManualVersion, @LabelID)";
                cmd.Parameters.AddWithValue("@ManualID", this.ID.ToString());
                cmd.Parameters.AddWithValue("@ManualVersion", this.Version);
                cmd.Parameters.AddWithValue("@LabelID", labelID);

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    ret = false;
                }
                conn.Close();
                }
                return ret;
            }

            public Boolean addLabel(String labelName)
            {
                Boolean ret = false;
                WILabelList lblList = new WILabelList();
                int lblID = lblList.addLabel(labelName);
                if(lblID!=-1)
                {
                    ret = this.addLabel(lblID);
                }
                return ret;
            }

            public Boolean deleteLabel(int labelID)
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM manualswilabels WHERE ManualID=@ManualID AND ManualVersion=@ManualVersion AND LabelID=@LabelID";
                cmd.Parameters.AddWithValue("@ManualID", this.ID.ToString());
                cmd.Parameters.AddWithValue("@ManualVersion", this.Version);
                cmd.Parameters.AddWithValue("@LabelID", labelID);

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    ret = false;
                }

                WILabel lblCurr = new WILabel(labelID);
                lblCurr.Delete();

                conn.Close();
                return ret;
            }
        }

        public class WorkInstructionsList
        {
            private List<WorkInstruction> _List;
            public List<WorkInstruction> List
            {
                get { return this._List; }
            }

            public WorkInstructionsList() {
                this._List = new List<WorkInstruction>();
            }

            public void loadWorkInstructionList(Boolean onlyActives =true)
            {
                this._List = new List<WorkInstruction>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                String strOnlyActives = "";
                if(onlyActives)
                {
                    strOnlyActives = "WHERE isActive = true ";
                }
                cmd.CommandText = "SELECT ID, Version FROM Manuals "+strOnlyActives+" ORDER BY Name";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._List.Add(new WorkInstruction(rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }

            /* Returns:
             * -1 if error
             * WorkInstructionID, workInstructionVersion if all is ok
             */
            public int[] Add(String name, String description, String FileName, String user)
            {
                int[] ret = new int[2];
                ret[0] = -1; ret[1] = -1;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(ID) FROM manuals";
                MySqlDataReader rdr = cmd.ExecuteReader();
                int maxID = 0;
                if(rdr.Read()&&!rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                cmd.CommandText = "INSERT INTO manuals(ID, Version, Name, Description, path, uploaddate, expirydate, isactive, user) VALUES("
                    + "@id, @version, @name, @description, @path, @uploaddate, @expirydate, @isactive, @user)";
                cmd.Parameters.AddWithValue("@id", maxID);
                cmd.Parameters.AddWithValue("@version", 0);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@path", FileName);
                cmd.Parameters.AddWithValue("@uploaddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@expirydate", (new DateTime(2199, 1, 1).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.AddWithValue("@isactive", true);
                cmd.Parameters.AddWithValue("@user", user);

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret[0] = maxID;
                    ret[1] = 0;
                }
                catch(Exception ex)
                {
                    ret[0] = -1;
                    ret[1] = -1;
                    tr.Rollback();
                }
                

                conn.Close();

                return ret;
            }
        }

        public class WILabel
        {
            private int _WILabelID;
            public int WILabelID
            {
                get { return this._WILabelID; }
            }

            private String _WILabelName;
            public String WILabelName
            {
                get { return this._WILabelName; }
            }

            public List<WorkInstruction> workInstructions;

            public WILabel(int LabelID)
            {
                this.workInstructions = new List<WorkInstruction>();
                this._WILabelID = -1;
                this._WILabelName = "";

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT wiLabelName FROM workinstructionslabel WHERE wiLabelID = " + LabelID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._WILabelID = LabelID;
                    this._WILabelName = rdr.GetString(0);
                }
                rdr.Close();
                conn.Close();
            }

            public void loadWorkInstructions()
            {
                this.workInstructions = new List<WorkInstruction>();
                if(this.WILabelID!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select manuals.id, manuals.version from manuals INNER JOIN manualswilabels "
                        + " ON(manuals.id = manualswilabels.manualID and manuals.Version = manualswilabels.manualversion) "
                        + " WHERE manuals.isActive = true AND manualswilabels.LabelID=" + this.WILabelID.ToString();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        this.workInstructions.Add(new WorkInstruction(rdr.GetInt32(0), rdr.GetInt32(1)));
                    }
                    rdr.Close();
                    conn.Close();
                }
            }

            /*Returns:
             * 0 if generic error
             * 1 if all is ok
             * 2 if could not delete because other work instructions uses the same label
             * 3 if error while deleting
             */
            public int Delete()
            {
                int ret = 0;
                if(this.WILabelID!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select manuals.id, manuals.version from manuals INNER JOIN manualswilabels "
                        + " ON(manuals.id = manualswilabels.manualID and manuals.Version = manualswilabels.manualversion) "
                        + " WHERE manuals.isActive = true AND manualswilabels.LabelID=" + this.WILabelID.ToString();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    Boolean delete = false;
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        delete = false;
                    }
                    else
                    {
                        delete = true;
                    }
                    rdr.Close();

                    if (delete)
                    {
                        cmd.CommandText = "DELETE FROM workinstructionslabel WHERE wilabelID = " + this.WILabelID.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            ret = 1;
                        }
                        catch
                        {
                            ret = 3;
                        }
                    }
                    else
                    {
                        ret = 2;
                    }

                    conn.Close();
                }
                return ret;
            }
        }

        public class WILabelList
        {
            public List<WILabel> List;

            public WILabelList()
            {
                this.List = new List<WILabel>();
            }

            public void loadLabelsList()
            {
                this.List = new List<WILabel>();

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT wiLabelID FROM workinstructionslabel ORDER BY wilabelname";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.List.Add(new WILabel(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }

            /* Returns:
             * 0 if error
             * LabelID if label already exists or label was added correctly
             */
            public int addLabel(String lblName)
            {
                int ret = -1;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT wiLabelID FROM workinstructionslabel WHERE wiLabelName = @labelName";
                cmd.Parameters.AddWithValue("@labelName", lblName);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = rdr.GetInt32(0);
                }
                rdr.Close();

                if(ret == -1)
                {
                    int maxID = 0;
                    cmd.CommandText = "SELECT MAX(wiLabelID) FROM workinstructionslabel";
                    rdr = cmd.ExecuteReader();
                    if(rdr.Read() && !rdr.IsDBNull(0))
                    {
                        maxID = rdr.GetInt32(0) + 1;
                    }
                    rdr.Close();

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO workinstructionslabel(wiLabelID, wiLabelName) VALUES(@labelId, @labelName)";
                    cmd.Parameters.AddWithValue("@labelId", maxID);
                    cmd.Parameters.AddWithValue("@labelName", lblName);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = maxID;
                        loadLabelsList();
                    }
                    catch
                    {
                        tr.Rollback();
                    }

                }

                conn.Close();
                return ret;
            }
        }
    }
}