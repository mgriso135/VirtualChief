using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
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
            protected String Tenant;

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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE manuals SET Name=@name WHERE ID = @id AND Version = @version",
                                        new { @name = value, @id = this.ID, @version = this.Version }, tr);
                                    tr.Commit();
                                    this._Name = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE manuals SET Description=@description WHERE ID = @id AND Version = @version",
                                        new { @description = value, @id = this.ID, @version = this.Version }, tr);
                                    tr.Commit();
                                    this._Description = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
                    }
                }
            }
            private String _Path;
            public String Path { get { return this._Path; }
                set
                {
                    if(this.ID!=-1 && this.Version!=-1)
                    {
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE manuals SET path=@path WHERE ID = @id AND Version = @version",
                                        new { @path = value, @id = this.ID, @version = this.Version }, tr);
                                    tr.Commit();
                                    this._Path = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE manuals SET expirydate=@expirydate WHERE ID = @id AND Version = @version",
                                        new { @expirydate = value.ToString("yyyy-MM-dd"), @id = this.ID, @version = this.Version }, tr);
                                    tr.Commit();
                                    this._ExpiryDate = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE manuals SET isActive=@isActive WHERE ID = @id AND Version = @version",
                                        new { @isActive = value, @id = this.ID, @version = this.Version }, tr);
                                    tr.Commit();
                                    this._IsActive = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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

            public WorkInstruction(String tenant, int id, int version)
            {
                this.Tenant = tenant;

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

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive, user FROM manuals WHERE "
                        + "ID = @id"
                        + " AND Version = @version";
                    ManualRow row = conn.QueryFirstOrDefault<ManualRow>(sql, new { @id = id, @version = version });
                    if (row != null)
                    {
                        this._ID = row.ID;
                        this._Version = row.Version;
                        this._Name = row.Name;
                        this._Description = row.Description;
                        this._Path = row.Path;
                        this._UploadDate = row.UploadDate;
                        this._ExpiryDate = row.ExpiryDate;
                        this._IsActive = row.IsActive;
                        this._Author = row.User;
                    }
                }
            }

            public WorkInstruction(String tenant, int id)
            {
                this.Tenant = tenant;

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

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT ID, version, Name, Description, path, uploaddate, expiryDate, isActive, user FROM manuals WHERE "
                        + "ID = @id"
                        + " ORDER BY version DESC";
                    ManualRow row = conn.QueryFirstOrDefault<ManualRow>(sql, new { @id = id });
                    if (row != null)
                    {
                        this._ID = row.ID;
                        this._Version = row.Version;
                        this._Name = row.Name;
                        this._Description = row.Description;
                        this._Path = row.Path;
                        this._UploadDate = row.UploadDate;
                        this._ExpiryDate = row.ExpiryDate;
                        this._IsActive = row.IsActive;
                        this._Author = row.User;
                    }
                }
            }

            private class ManualRow
            {
                public int ID { get; set; }
                public int Version { get; set; }
                public String Name { get; set; }
                public String Description { get; set; }
                public String Path { get; set; }
                public DateTime UploadDate { get; set; }
                public DateTime ExpiryDate { get; set; }
                public Boolean IsActive { get; set; }
                public String User { get; set; }
            }

            public void loadLabels()
            {
                this.Labels = new List<WILabel>();
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    var ids = conn.Query<int>("SELECT LabelID FROM manualswilabels WHERE ManualID = @id AND ManualVersion = @version",
                        new { @id = this.ID, @version = this.Version });
                    foreach (int lblID in ids)
                    {
                        this.Labels.Add(new WILabel(this.Tenant, lblID));
                    }
                }
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("INSERT INTO manualswilabels(ManualID, ManualVersion, LabelID) VALUES(@ManualID, @ManualVersion, @LabelID)",
                                new { @ManualID = this.ID.ToString(), @ManualVersion = this.Version, @LabelID = labelID }, tr);
                            tr.Commit();
                            ret = true;
                        }
                        catch(Exception ex)
                        {
                            tr.Rollback();
                            ret = false;
                        }
                    }
                }
                }
                return ret;
            }

            public Boolean addLabel(String labelName)
            {
                Boolean ret = false;
                WILabelList lblList = new WILabelList(this.Tenant);
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("DELETE FROM manualswilabels WHERE ManualID=@ManualID AND ManualVersion=@ManualVersion AND LabelID=@LabelID",
                                new { @ManualID = this.ID.ToString(), @ManualVersion = this.Version, @LabelID = labelID }, tr);
                            tr.Commit();
                            ret = true;
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            ret = false;
                        }
                    }

                    WILabel lblCurr = new WILabel(this.Tenant, labelID);
                    lblCurr.Delete();
                }
                return ret;
            }

            public void loadTaskProducts()
            {
                this.listTasksProducts = new List<WITaskProduct>();
                if (this.ID!=-1 && this.Version!=-1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT tasksmanuals.taskid, tasksmanuals.taskrev, tasksmanuals.taskvarianti, tasksmanuals.validityInitialDate, tasksmanuals.expiryDate FROM "
                            + " tasksmanuals INNER JOIN processo ON (tasksmanuals.taskid=processo.processid AND tasksmanuals.taskrev=processo.revisione) "
                            + "INNER JOIN varianti ON (tasksmanuals.taskVarianti = varianti.idvariante)"
                            + " WHERE tasksmanuals.manualID = @ManualID"
                            + " AND tasksmanuals.manualVersion=@ManualVersion AND tasksmanuals.isactive=true";
                            //+ " AND tasksmanuals.validityInitialDate <= '"+DateTime.UtcNow.ToString("yyyy-MM-dd")
                            //+"' AND expiryDate >= '"+DateTime.UtcNow.ToString("yyyy-MM-dd")+"'";
                        var rows = conn.Query<TaskManualRow>(sql, new { @ManualID = this.ID, @ManualVersion = this.Version });
                        foreach (var r in rows)
                        {
                            processo proc = new processo(this.Tenant, r.TaskId, r.TaskRev);
                            variante var = new variante(this.Tenant, r.TaskVarianti);
                            if (proc.processID != -1 && proc.revisione != -1 && proc.processoPadre != -1 && proc.revPadre != -1)
                            {
                                int[] padre = proc.getPadre(var);
                                ProcessoVariante prodotti = new ProcessoVariante(this.Tenant, new processo(this.Tenant, padre[0], padre[1]), var);
                                WITaskProduct curr = new WITaskProduct(this.Tenant, this.ID, this.Version, r.TaskId, r.TaskRev, r.TaskVarianti);
                                    curr.ProductID = prodotti.process.processID;
                                    curr.ProductVersion = prodotti.process.revisione;
                                    curr.VariantID = prodotti.variant.idVariante;
                                    curr.ProductName = prodotti.variant.nomeVariante;
                                    curr.ProductType = prodotti.process.processName;
                                    curr.TaskID = r.TaskId;
                                    curr.TaskVersion = r.TaskRev;
                                    curr.TaskName = proc.processName;

                                    this.listTasksProducts.Add(curr);
                            }
                        }
                    }
                }
            }

            private class TaskManualRow
            {
                public int TaskId { get; set; }
                public int TaskRev { get; set; }
                public int TaskVarianti { get; set; }
                public DateTime ValidityInitialDate { get; set; }
                public DateTime ExpiryDate { get; set; }
            }

            public void loadOlderVersions()
            {
                this.OlderVersions = new List<WIOlderVersion>();
                if(this.ID!=-1 && this.Version!=-1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT ID, Version, Name, Description, path, uploaddate, expirydate, isActive, user FROM manuals WHERE "
                            + "ID = @ID "
                            + " AND Version < @Version "
                            + " AND IsActive = false";
                        var rows = conn.Query<ManualRow>(sql, new { @ID = this.ID, @Version = this.Version });
                        foreach (var r in rows)
                        {
                            WIOlderVersion curr = new WIOlderVersion();
                            curr.ID = r.ID;
                            curr.Version = r.Version;
                            curr.Name = r.Name;
                            curr.Description = r.Description;
                            curr.Path = r.Path;
                            curr.UploadDate = r.UploadDate;
                            curr.ExpiryDate = r.ExpiryDate;
                            curr.IsActive = r.IsActive;
                            curr.Author = r.User;

                            this.OlderVersions.Add(curr);
                        }
                    }
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
                    TaskVariante tsk = new TaskVariante(this.Tenant, new processo(this.Tenant, TaskID, TaskVersion), new variante(this.Tenant, VariantID));
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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        int maxSequence = 0;
                        int? maxSeqRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(sequence) FROM tasksmanuals WHERE TaskId=@TaskId AND TaskRev=@TaskRev AND taskVarianti=@TaskVariante",
                            new { @TaskId = TaskID, @TaskRev = TaskVersion, @TaskVariante = VariantID });
                        if (maxSeqRow.HasValue)
                        {
                            maxSequence = maxSeqRow.Value + 1;
                        }

                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("INSERT INTO tasksmanuals(taskID, taskRev, taskVarianti, manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive) "
                                + "VALUES(@TaskId, @TaskRev, @TaskVariante, @ManualID, @ManualVersion, @ValidityInitialDate, @ExpiryDate, @Sequence, @IsActive)",
                                    new { @TaskId = TaskID, @TaskRev = TaskVersion, @TaskVariante = VariantID, @ManualID = this.ID, @ManualVersion = this.Version, @ValidityInitialDate = ValidityInitialDate, @ExpiryDate = ValidityExpiryDate, @Sequence = maxSequence, @IsActive = isActive }, tr);
                                tr.Commit();
                                ret = 1;
                            }
                            catch
                            {
                                tr.Rollback();
                                ret = 3;
                            }
                        }
                    }
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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        Boolean checkNewVer = false;
                        using (var tr = conn.BeginTransaction())
                        {
                            string sql = "INSERT INTO manuals(ID, Version, Name, Description, path, uploaddate, expirydate, isactive, user) VALUES("
                                + "@id, @version, @name, @description, @path, @uploaddate, @expirydate, @isactive, @user)";
                            try
                            {
                                conn.Execute(sql, new { @id = this.ID, @version = (this.Version + 1), @name = this.Name, @description = this.Description, @path = FileName, @uploaddate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), @expirydate = ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss"), @isactive = true, @user = user }, tr);
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

                            WorkInstruction newWI = new WorkInstruction(this.Tenant, this.ID, (this.Version) + 1);
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
                    }
                }
                return ret;
            }
        }

        public class WorkInstructionsList
        {
            protected String Tenant;

            private List<WorkInstruction> _List;
            public List<WorkInstruction> List
            {
                get { return this._List; }
            }

            public WorkInstructionsList(String tenant) {
                this.Tenant = tenant;

                this._List = new List<WorkInstruction>();
            }

            public WorkInstructionsList(String tenant, List<int> idLabels, Boolean onlyActives=true)
            {
                this.Tenant = tenant;

                this._List = new List<WorkInstruction>();
                this._List = new List<WorkInstruction>();
                if (idLabels.Count > 0)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        DynamicParameters par = new DynamicParameters();

                        String strFilter = "";
                        for(int i =0; i < idLabels.Count; i++)
                        {
                            if(idLabels[i]==-1)
                            {
                                strFilter += "manualswilabels.LabelID IS NULL";
                            }
                            else
                            { 
                                strFilter += "manualswilabels.LabelID = @labelId" + i;
                                par.Add("@labelId" + i, idLabels[i]);
                            }
                            if (i < idLabels.Count-1)
                            {
                                strFilter += " OR ";
                            }
                        }


                        String strOnlyActives = "";
                        if (onlyActives)
                        {
                            strOnlyActives = " AND (isActive = true AND expiryDate >= @today)";
                        }

                        string sql = "SELECT DISTINCT(ID), Version, Name, Description, path, uploaddate, expirydate, isActive, user from manuals "
                            + " LEFT JOIN manualswilabels ON(manuals.ID = manualswilabels.ManualID AND manuals.Version = manualswilabels.manualVersion) "
                            + " WHERE ("+strFilter+")";
                        if (onlyActives)
                        {
                            sql += strOnlyActives;
                            par.Add("@today", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                        }
                        var rows = conn.Query<(int ID, int Version)>(sql, par);
                        foreach (var r in rows)
                        {
                            this._List.Add(new WorkInstruction(this.Tenant, r.ID, r.Version));
                        }
                    }
                }
            }

            public void loadWorkInstructionList(Boolean onlyActives =true)
            {
                this._List = new List<WorkInstruction>();
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    String strOnlyActives = "";
                    if(onlyActives)
                    {
                        strOnlyActives = "WHERE isActive = true AND expiryDate >= @today";
                    }
                    string sql = "SELECT ID, Version FROM Manuals "+strOnlyActives+" ORDER BY Name";
                    var rows = conn.Query<(int ID, int Version)>(sql, onlyActives ? new { @today = DateTime.UtcNow.ToString("yyyy-MM-dd") } : null);
                    foreach (var r in rows)
                    {
                        this._List.Add(new WorkInstruction(this.Tenant, r.ID, r.Version));
                    }
                }
            }

            /* Returns:
             * -1 if error
             * WorkInstructionID, workInstructionVersion if all is ok
             */
            public int[] Add(String name, String description, String FileName, String user)
            {
                int[] ret = new int[2];
                ret[0] = -1; ret[1] = -1;

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    int maxID = 0;
                    int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(ID) FROM manuals");
                    if (maxRow.HasValue)
                    {
                        maxID = maxRow.Value + 1;
                    }

                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("INSERT INTO manuals(ID, Version, Name, Description, path, uploaddate, expirydate, isactive, user) VALUES("
                                + "@id, @version, @name, @description, @path, @uploaddate, @expirydate, @isactive, @user)",
                                new { @id = maxID, @version = 0, @name = name, @description = description, @path = FileName, @uploaddate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), @expirydate = (new DateTime(2199, 1, 1).ToString("yyyy-MM-dd HH:mm:ss")), @isactive = true, @user = user }, tr);
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
                    }
                }

                return ret;
            }
        }

        public class WILabel
        {
            protected String Tenant;

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

            public WILabel(String tenant, int LabelID)
            {
                this.Tenant = tenant;

                this.workInstructions = new List<WorkInstruction>();
                this._WILabelID = -1;
                this._WILabelName = "";

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    String lblName = conn.QueryFirstOrDefault<String>("SELECT wiLabelName FROM workinstructionslabel WHERE wiLabelID = @labelID",
                        new { @labelID = LabelID });
                    if (lblName != null)
                    {
                        this._WILabelID = LabelID;
                        this._WILabelName = lblName;
                    }
                }
            }

            public void loadWorkInstructions()
            {
                this.workInstructions = new List<WorkInstruction>();
                if(this.WILabelID!=-1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "select manuals.id, manuals.version from manuals INNER JOIN manualswilabels "
                            + " ON(manuals.id = manualswilabels.manualID and manuals.Version = manualswilabels.manualversion) "
                            + " WHERE manuals.isActive = true AND manualswilabels.LabelID=@labelID";
                        var rows = conn.Query<(int ID, int Version)>(sql, new { @labelID = this.WILabelID });
                        foreach (var r in rows)
                        {
                            this.workInstructions.Add(new WorkInstruction(this.Tenant, r.ID, r.Version));
                        }
                    }
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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "select manuals.id, manuals.version from manuals INNER JOIN manualswilabels "
                            + " ON(manuals.id = manualswilabels.manualID and manuals.Version = manualswilabels.manualversion) "
                            + " WHERE manuals.isActive = true AND manualswilabels.LabelID=@labelID";
                        var row = conn.QueryFirstOrDefault<(int ID, int Version)?>(sql, new { @labelID = this.WILabelID });
                        Boolean delete = false;
                        if (row.HasValue)
                        {
                            delete = false;
                        }
                        else
                        {
                            delete = true;
                        }

                        if (delete)
                        {
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("DELETE FROM workinstructionslabel WHERE wilabelID = @labelID",
                                        new { @labelID = this.WILabelID }, tr);
                                    tr.Commit();
                                    ret = 1;
                                }
                                catch
                                {
                                    ret = 3;
                                }
                            }
                        }
                        else
                        {
                            ret = 2;
                        }
                    }
                }
                return ret;
            }
        }

        public class WILabelList
        {
            protected String Tenant;

            public List<WILabel> List;

            public WILabelList(String tenant)
            {
                this.Tenant = tenant;

                this.List = new List<WILabel>();
            }

            public void loadLabelsList()
            {
                this.List = new List<WILabel>();

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    var ids = conn.Query<int>("SELECT wiLabelID FROM workinstructionslabel ORDER BY wilabelname");
                    foreach (int lblID in ids)
                    {
                        this.List.Add(new WILabel(this.Tenant, lblID));
                    }
                }
            }

            /* Returns:
             * 0 if error
             * LabelID if label already exists or label was added correctly
             */
            public int addLabel(String lblName)
            {
                int ret = -1;

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();

                    int? lblID = conn.QueryFirstOrDefault<int?>("SELECT wiLabelID FROM workinstructionslabel WHERE wiLabelName = @labelName",
                        new { @labelName = lblName });
                    if (lblID.HasValue)
                    {
                        ret = lblID.Value;
                    }

                    if(ret == -1)
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(wiLabelID) FROM workinstructionslabel");
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }

                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("INSERT INTO workinstructionslabel(wiLabelID, wiLabelName) VALUES(@labelId, @labelName)",
                                    new { @labelId = maxID, @labelName = lblName }, tr);
                                tr.Commit();
                                ret = maxID;
                                loadLabelsList();
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
                }
                return ret;
            }
        }

        public class WITaskProduct
        {
            protected String Tenant;

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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE tasksmanuals SET validityInitialDate=@initialDate"
                                        + " WHERE taskid = @taskid AND TaskRev = @taskRev"
                                        +" AND taskVarianti = @taskVarianti"
                                        + " AND manualID = @manualID"
                                        + " AND manualVersion = @manualVersion",
                                        new { @initialDate = value.ToString("yyyy-MM-dd"), @taskid = this.TaskID, @taskRev = this.TaskVersion, @taskVarianti = this.VariantID, @manualID = this.ManualID, @manualVersion = this.ManualVersion }, tr);
                                    tr.Commit();
                                    this._InitialDate = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE tasksmanuals SET expiryDate=@expiryDate"
                                        + " WHERE taskid = @taskid AND TaskRev = @taskRev"
                                        + " AND taskVarianti = @taskVarianti"
                                        + " AND manualID = @manualID"
                                        + " AND manualVersion = @manualVersion",
                                        new { @expiryDate = value.ToString("yyyy-MM-dd"), @taskid = this.TaskID, @taskRev = this.TaskVersion, @taskVarianti = this.VariantID, @manualID = this.ManualID, @manualVersion = this.ManualVersion }, tr);
                                    tr.Commit();
                                    this._ExpiryDate = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
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
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            using (var tr = conn.BeginTransaction())
                            {
                                try
                                {
                                    conn.Execute("UPDATE tasksmanuals SET IsActive=@isActive"
                                        + " WHERE taskid = @taskid AND TaskRev = @taskRev"
                                        + " AND taskVarianti = @taskVarianti"
                                        + " AND manualID = @manualID"
                                        + " AND manualVersion = @manualVersion",
                                        new { @isActive = value, @taskid = this.TaskID, @taskRev = this.TaskVersion, @taskVarianti = this.VariantID, @manualID = this.ManualID, @manualVersion = this.ManualVersion }, tr);
                                    tr.Commit();
                                    this._IsActive = value;
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            }
                        }
                    }
                }
            }

            public WITaskProduct(String tenant, int ManualID, int ManualVersion, int TaskID, int TaskVersion, int VariantID)
            {
                this.Tenant = tenant;

                this.ManualID = -1;
                this.ManualVersion = -1;
                this.VariantID = -1;
                this.TaskID = -1;
                this.TaskVersion = -1;
                this._InitialDate = new DateTime(1970, 1, 1);
                this._ExpiryDate = new DateTime(1970, 1, 1);
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE TaskID=@TaskId AND taskRev=@TaskVersion AND "
                        + " taskVarianti=@VariantID AND manualID=@ManualID AND manualVersion=@ManualVersion";
                    WITaskProductRow row = conn.QueryFirstOrDefault<WITaskProductRow>(sql, new { @TaskId = TaskID, @TaskVersion = TaskVersion, @VariantID = VariantID, @ManualID = ManualID, @ManualVersion = ManualVersion });
                    if (row != null)
                    {
                        this.ManualID = ManualID;
                        this.ManualVersion = ManualVersion;
                        this.TaskID = TaskID;
                        this.TaskVersion = TaskVersion;
                        this.VariantID = VariantID;
                        this._InitialDate = row.ValidityInitialDate;
                        this._ExpiryDate = row.ExpiryDate;
                    }
                }
            }

            private class WITaskProductRow
            {
                public DateTime ValidityInitialDate { get; set; }
                public DateTime ExpiryDate { get; set; }
                public int Sequence { get; set; }
                public Boolean IsActive { get; set; }
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