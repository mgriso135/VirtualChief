using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using KIS.App_Code;


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

            private String _Author;
            public String Author
            {
                get
                {
                    return this._Author;
                }
                set {; }
            }

            public List<WIOlderVersion> OlderVersions;

            public List<WILabel> Labels;

            public List<WITaskProduct> listTasksProducts;

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
                this._Author = "";
                this.listTasksProducts = new List<WITaskProduct>();
                this.OlderVersions = new List<WIOlderVersion>();

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive, user FROM manuals WHERE "
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
                    this._Author = rdr.GetString(8);
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
                this.listTasksProducts = new List<WITaskProduct>();
                this.OlderVersions = new List<WIOlderVersion>();

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive, user FROM manuals WHERE "
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
                    this._Author = rdr.GetString(8);
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

            public void loadTaskProducts()
            {
                this.listTasksProducts = new List<WITaskProduct>();
                if (this.ID!=-1 && this.Version!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT tasksmanuals.taskid, tasksmanuals.taskrev, tasksmanuals.taskvarianti, tasksmanuals.validityInitialDate, tasksmanuals.expiryDate FROM "
                        + " tasksmanuals INNER JOIN processo ON (tasksmanuals.taskid=processo.processid AND tasksmanuals.taskrev=processo.revisione) "
                        + "INNER JOIN varianti ON (tasksmanuals.taskVarianti = varianti.idvariante)"
                        + " WHERE tasksmanuals.manualID = @ManualID"
                        + " AND tasksmanuals.manualVersion=@ManualVersion AND tasksmanuals.isactive=true";
                        //+ " AND tasksmanuals.validityInitialDate <= '"+DateTime.UtcNow.ToString("yyyy-MM-dd")
                        //+"' AND expiryDate >= '"+DateTime.UtcNow.ToString("yyyy-MM-dd")+"'";
                    cmd.Parameters.AddWithValue("@ManualID", this.ID);
                    cmd.Parameters.AddWithValue("@ManualVersion", this.Version);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        processo proc = new processo(rdr.GetInt32(0), rdr.GetInt32(1));
                        variante var = new variante(rdr.GetInt32(2));
                        if (proc.processID != -1 && proc.revisione != -1 && proc.processoPadre != -1 && proc.revPadre != -1)
                        {
                            int[] padre = proc.getPadre(var);
                            ProcessoVariante prodotti = new ProcessoVariante(new processo(padre[0], padre[1]), var);
                            WITaskProduct curr = new WITaskProduct(this.ID, this.Version, rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2));
                                curr.ProductID = prodotti.process.processID;
                                curr.ProductVersion = prodotti.process.revisione;
                                curr.VariantID = prodotti.variant.idVariante;
                                curr.ProductName = prodotti.variant.nomeVariante;
                                curr.ProductType = prodotti.process.processName;
                                curr.TaskID = rdr.GetInt32(0);
                                curr.TaskVersion = rdr.GetInt32(1);
                                curr.TaskName = proc.processName;

                                this.listTasksProducts.Add(curr);
                        }
                    }
                    conn.Close();
                }
            }

            public void loadOlderVersions()
            {
                this.OlderVersions = new List<WIOlderVersion>();
                if(this.ID!=-1 && this.Version!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT ID, Version, Name, Description, path, uploaddate, expirydate, isActive, user FROM manuals WHERE "
                        + "ID = @ID "
                        + " AND Version < @Version "
                        + " AND IsActive = false";
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    cmd.Parameters.AddWithValue("@Version", this.Version);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        WIOlderVersion curr = new WIOlderVersion();
                        curr.ID = rdr.GetInt32(0);
                        curr.Version = rdr.GetInt32(1);
                        curr.Name = rdr.GetString(2);
                        curr.Description = rdr.GetString(3);
                        curr.Path = rdr.GetString(4);
                        curr.UploadDate = rdr.GetDateTime(5);
                        curr.ExpiryDate = rdr.GetDateTime(6);
                        curr.IsActive = rdr.GetBoolean(7);
                        curr.Author = rdr.GetString(8);

                        this.OlderVersions.Add(curr);
                    }
                    rdr.Close();

                    conn.Clone();
                }
            }
            
            /*Returns
             * 0 if generic error
             * 1 if ok
             * 3 if error while adding
             * 4 if there is an overlap with other manuals for the same task
             */
            public int linkManualToTask(int TaskID, int TaskVersion, int VariantID, DateTime ValidityInitialDate, DateTime ValidityExpiryDate, int sequence, Boolean isActive)
            {
                int ret = 0;
                if(this.ID!=-1 && this.Version!=-1 && ValidityInitialDate < ValidityExpiryDate)
                {
                    TaskVariante tsk = new TaskVariante(new processo(TaskID, TaskVersion), new variante(VariantID));
                    tsk.loadWorkInstructions();
                    bool checkOverlaps = false;
                    for(int i = 0; i < tsk.WorkInstructions.Count && !checkOverlaps; i++)
                    {
                        if((ValidityExpiryDate >= tsk.WorkInstructions[i].InitialDate && ValidityExpiryDate <= tsk.WorkInstructions[i].ExpiryDate) || 
                            (ValidityInitialDate >= tsk.WorkInstructions[i].InitialDate && ValidityInitialDate <= tsk.WorkInstructions[i].ExpiryDate) ||
                            (ValidityInitialDate <= tsk.WorkInstructions[i].InitialDate && ValidityExpiryDate >= tsk.WorkInstructions[i].ExpiryDate)
                            )
                        {
                            checkOverlaps = true;
                            ret = 4;
                        }
                    }
                    tsk.loadWorkInstructionsArchive();
                    for (int i = 0; i < tsk.WorkInstructionsArchive.Count && !checkOverlaps; i++)
                    {
                        if ((ValidityExpiryDate >= tsk.WorkInstructionsArchive[i].InitialDate && ValidityExpiryDate <= tsk.WorkInstructionsArchive[i].ExpiryDate) ||
                            (ValidityInitialDate >= tsk.WorkInstructionsArchive[i].InitialDate && ValidityInitialDate <= tsk.WorkInstructionsArchive[i].ExpiryDate) ||
                            (ValidityInitialDate <= tsk.WorkInstructionsArchive[i].InitialDate && ValidityExpiryDate >= tsk.WorkInstructionsArchive[i].ExpiryDate)
                            )
                        {
                            checkOverlaps = true;
                            ret = 4;
                        }
                    }

                    if (!checkOverlaps)
                    { 

                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                        int maxSequence = 0;
                        cmd.CommandText = "SELECT MAX(sequence) FROM tasksmanuals WHERE TaskId=@TaskId AND TaskRev=@TaskRev AND taskVarianti=@TaskVariante";
                        cmd.Parameters.AddWithValue("@TaskId", TaskID);
                        cmd.Parameters.AddWithValue("@TaskRev", TaskVersion);
                        cmd.Parameters.AddWithValue("@TaskVariante", VariantID);
                        
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if(rdr.Read() && !rdr.IsDBNull(0))
                        {
                            maxSequence = rdr.GetInt32(0) + 1;
                        }
                        rdr.Close();

                        cmd.CommandText = "INSERT INTO tasksmanuals(taskID, taskRev, taskVarianti, manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive) "
                        + "VALUES(@TaskId, @TaskRev, @TaskVariante, @ManualID, @ManualVersion, @ValidityInitialDate, @ExpiryDate, @Sequence, @IsActive)";
                        cmd.Parameters.AddWithValue("@ManualID", this.ID);
                        cmd.Parameters.AddWithValue("@ManualVersion", this.Version);
                        cmd.Parameters.AddWithValue("@ValidityInitialDate", ValidityInitialDate);
                    cmd.Parameters.AddWithValue("@ExpiryDate", ValidityExpiryDate);
                    cmd.Parameters.AddWithValue("@Sequence", maxSequence);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

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
                        tr.Rollback();
                        ret = 3;
                    }

                    conn.Close();
                    }
                }
                return ret;
            }

            /* Returns:
             * 0, 0 if generic error
             * ManualID, ManualVersion if manual was reviewed correctly
             */
            public int[] ReviewManual(String FileName, DateTime InitialDate, DateTime ExpiryDate, String user)
            {
                int[] ret = new int[2];
                ret[0] = 0; ret[1] = 0;
                if(this.ID!=-1 && this.Version >=0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;

                    cmd.CommandText = "INSERT INTO manuals(ID, Version, Name, Description, path, uploaddate, expirydate, isactive, user) VALUES("
                        + "@id, @version, @name, @description, @path, @uploaddate, @expirydate, @isactive, @user)";
                    cmd.Parameters.AddWithValue("@id", this.ID);
                    cmd.Parameters.AddWithValue("@version", (this.Version + 1));
                    cmd.Parameters.AddWithValue("@name", this.Name);
                    cmd.Parameters.AddWithValue("@description", this.Description);
                    cmd.Parameters.AddWithValue("@path", FileName);
                    cmd.Parameters.AddWithValue("@uploaddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@expirydate", ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@isactive", true);
                    cmd.Parameters.AddWithValue("@user", user);

                    Boolean checkNewVer = false;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        checkNewVer = true;
                        ret[0] = this.ID;
                        ret[1] = this.Version + 1;
                    }
                    catch
                    {
                        checkNewVer = false;
                        tr.Rollback();
                    }

                    if(checkNewVer)
                    {
                        // Disable manual
                        if (this.ExpiryDate <= DateTime.UtcNow)
                        {
                            this.IsActive = false;
                        }
                        if (this.ExpiryDate >= ExpiryDate)
                        {
                            this.ExpiryDate = ExpiryDate.AddDays(-1);
                        }
                        this.IsActive = false;

                        WorkInstruction newWI = new WorkInstruction(this.ID, (this.Version) + 1);
                        // Copy tasks and set end date as ExpiryDate-1Day for tasks in current manual
                        this.loadTaskProducts();
                        for(int i = 0; i < this.listTasksProducts.Count; i++)
                        {
                            if (this.listTasksProducts[i].InitialDate >= InitialDate)
                            {
                                this.listTasksProducts[i].InitialDate = ExpiryDate.AddDays(-2);
                            }
                            if (this.listTasksProducts[i].ExpiryDate >= InitialDate)
                            {
                                this.listTasksProducts[i].ExpiryDate = ExpiryDate.AddDays(-1);
                            }
                            int lnkRet = newWI.linkManualToTask(this.listTasksProducts[i].TaskID,
                                this.listTasksProducts[i].TaskVersion,
                                this.listTasksProducts[i].VariantID,
                                InitialDate,
                                ExpiryDate,
                                this.listTasksProducts[i].Sequence,
                                true);
                            if(lnkRet ==1)
                            { 
                            this.listTasksProducts[i].IsActive = false;
                            }
                            else
                            {
                                ret[0] = -lnkRet;
                            }
                        }

                        // Copy labels
                        this.loadLabels();
                        for(int i = 0; i < this.Labels.Count; i++)
                        {
                            newWI.addLabel(this.Labels[i].WILabelID);
                        }


                    }

                    conn.Close();
                }
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

            public WorkInstructionsList(List<int> idLabels, Boolean onlyActives=true)
            {
                this._List = new List<WorkInstruction>();
                this._List = new List<WorkInstruction>();
                if (idLabels.Count > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    String strFilter = "";
                    for(int i =0; i < idLabels.Count; i++)
                    {
                        if(idLabels[i]==-1)
                        {
                            strFilter += "manualswilabels.LabelID IS NULL";
                        }
                        else
                        { 
                            strFilter += "manualswilabels.LabelID = " + idLabels[i].ToString();
                        }
                        if (i < idLabels.Count-1)
                        {
                            strFilter += " OR ";
                        }
                    }


                    String strOnlyActives = "";
                    if (onlyActives)
                    {
                        strOnlyActives = " AND (isActive = true AND expiryDate >= '" + DateTime.UtcNow.ToString("yyyy-MM-dd") + "')";
                    }

                    
                    cmd.CommandText = "SELECT DISTINCT(ID), Version, Name, Description, path, uploaddate, expirydate, isActive, user from manuals "
                        + " LEFT JOIN manualswilabels ON(manuals.ID = manualswilabels.ManualID AND manuals.Version = manualswilabels.manualVersion) "
                        + " WHERE ("+strFilter+")";
                    if (onlyActives)
                    {
                        cmd.CommandText += strOnlyActives;
                    }
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this._List.Add(new WorkInstruction(rdr.GetInt32(0), rdr.GetInt32(1)));
                    }
                    rdr.Close();
                    conn.Close();
                }
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
                    strOnlyActives = "WHERE isActive = true AND expiryDate >= '" + DateTime.UtcNow.ToString("yyyy-MM-dd") + "'";
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

        public class WITaskProduct
        {
            public int ManualID;
            public int ManualVersion;

            public int ProductID;
            public int ProductVersion;
            public int VariantID;
            public int TaskID;
            public int TaskVersion;

            public String ProductType;
            public String ProductName;
            public String TaskName;

            private DateTime _InitialDate;
            public DateTime InitialDate
            {
                get {return this._InitialDate; }
                set {
                    if (this.TaskID != -1 && this.TaskVersion != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE tasksmanuals SET validityInitialDate='" + value.ToString("yyyy-MM-dd") 
                            + "' WHERE taskid = " + this.TaskID.ToString() + " AND TaskRev = " + this.TaskVersion.ToString()
                            +" AND taskVarianti = " + this.VariantID
                            + " AND manualID = " + this.ManualID.ToString()
                            + " AND manualVersion = " + this.ManualVersion.ToString()
                            ;
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._InitialDate = value;
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                        conn.Close();
                    }
                }
            }

            private DateTime _ExpiryDate;
            public DateTime ExpiryDate
            {
                get { return this._ExpiryDate; }
                set
                {
                    if (this.TaskID != -1 && this.TaskVersion != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE tasksmanuals SET expiryDate='" + value.ToString("yyyy-MM-dd")
                            + "' WHERE taskid = " + this.TaskID.ToString() + " AND TaskRev = " + this.TaskVersion.ToString()
                            + " AND taskVarianti = " + this.VariantID
                            + " AND manualID = " + this.ManualID.ToString()
                            + " AND manualVersion = " + this.ManualVersion.ToString()
                            ;
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

            public int Sequence;

            private Boolean _IsActive;
            public Boolean IsActive {
                get { return this._IsActive; }
                set
                {
                    if (this.TaskID != -1 && this.TaskVersion != -1)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE tasksmanuals SET IsActive=" + value
                            + " WHERE taskid = " + this.TaskID.ToString() + " AND TaskRev = " + this.TaskVersion.ToString()
                            + " AND taskVarianti = " + this.VariantID
                            + " AND manualID = " + this.ManualID.ToString()
                            + " AND manualVersion = " + this.ManualVersion.ToString()
                            ;
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

            public WITaskProduct(int ManualID, int ManualVersion, int TaskID, int TaskVersion, int VariantID)
            {
                this.ManualID = -1;
                this.ManualVersion = -1;
                this.VariantID = -1;
                this.TaskID = -1;
                this.TaskVersion = -1;
                this._InitialDate = new DateTime(1970, 1, 1);
                this._ExpiryDate = new DateTime(1970, 1, 1);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE TaskID=@TaskId AND taskRev=@TaskVersion AND "
                    + " taskVarianti=@VariantID AND manualID=@ManualID AND manualVersion=@ManualVersion";
                cmd.Parameters.AddWithValue("@TaskId", TaskID); 
                cmd.Parameters.AddWithValue("@TaskVersion", TaskVersion);
                cmd.Parameters.AddWithValue("@VariantID", VariantID);
                cmd.Parameters.AddWithValue("@ManualID", ManualID);
                cmd.Parameters.AddWithValue("@ManualVersion", ManualVersion);

                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    this.ManualID = ManualID;
                    this.ManualVersion = ManualVersion;
                    this.TaskID = TaskID;
                    this.TaskVersion = TaskVersion;
                    this.VariantID = VariantID;
                    this._InitialDate = rdr.GetDateTime(0);
                    this._ExpiryDate = rdr.GetDateTime(1);
                }
                rdr.Close();
                conn.Close();
            }
        }

        public struct WIOlderVersion
        {
            public int ID;
            public int Version;
            public String Name;
            public String Description;
            public String Path;
            public DateTime UploadDate;
            public DateTime ExpiryDate;
            public Boolean IsActive;
            public String Author;
        }
    }
}