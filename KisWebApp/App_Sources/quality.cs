/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

/*  Copyright of Matteo Griso.
 *  Develop started on 29/08/2017 in Berazategui, Buenos Aires, Argentina
 */

using System;
using System.Collections.Generic;
using System.Web;
using MySql.Data.MySqlClient;
using KIS.App_Code;
using System.IO;


namespace KIS.App_Sources
{
    public class NonCompliance
    {
        public String log;

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private int _Year;
        public int Year
        {
            get { return this._Year; }
        }

        private int _Quantity;
        public int Quantity
        {
            get
            {
                return this._Quantity;
            }
            set
            {
                if(value > 0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET Quantity = " + value.ToString()
                    + " WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Quantity = value;
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                conn.Close();
                }
            }
        }

        private DateTime _OpeningDate;
        public DateTime OpeningDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._OpeningDate, fuso.tzFusoOrario);
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE NonCompliances SET OpeningDate = '" + value.ToString()
                        + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    this._OpeningDate = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
            }
        }

        private String _UserID;
        public String UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE NonCompliances SET User = '" + value.ToString()
                        + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    this._UserID = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }

        public User user
        {
            get
            {
                User curr = new User(this.UserID);
                return curr;
            }
            set
            {
                if(value!=null && value.username.Length>0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET User = '" + value.username.ToString()
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
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

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET Description = '" + value.ToString()
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ImmediateAction;
        public String ImmediateAction
        {
            get
            {
                return this._ImmediateAction;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET ImmediateAction = '" + value.ToString()
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ImmediateAction = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private Double _Cost;
        public Double Cost
        {
            get
            {
                return this._Cost;
            }
            set
            {
                if(value >=0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET Cost = " + value.ToString()
                    + " WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Cost = value;
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

        /* Status values:
         * O = Open
         * C = Closed
         */
        private Char _Status;
        public Char Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    if(value == 'O') { 
                        cmd.CommandText = "UPDATE NonCompliances SET Status = '" + value.ToString()
                        + "', ClosureDate = null WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE NonCompliances SET Status = '" + value.ToString()
                        + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    }
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private DateTime _ClosureDate;
        public DateTime ClosureDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._ClosureDate, fuso.tzFusoOrario); ;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliances SET ClosureDate = '" + value.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ClosureDate = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<NonComplianceType> Categories;

        public List<NonComplianceCause> Causes;

        public List<NonComplianceProduct> Products;

        public List<String> Files;

        private Char _Severity;
        public Char Severity { get { return this._Severity; }
            set
            {
                if (this.ID != -1 && this.Year != -1 && (value == '1' || value == '2' || value == '3' || value == '4'))
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE NonCompliances SET severity=@value WHERE ID=@ncid AND Year=@ncyear";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@ncid", this.ID);
                        cmd.Parameters.AddWithValue("@ncyear", this.Year);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Severity = value;
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
        private Char _Urgency;
        public Char Urgency { get { return this._Urgency; }
            set
            {
                if (this.ID != -1 && this.Year != -1 && (value == '1' || value == '2' || value == '3' || value == '4'))
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE NonCompliances SET urgency=@value WHERE ID=@ncid AND Year=@ncyear";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@ncid", this.ID);
                        cmd.Parameters.AddWithValue("@ncyear", this.Year);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Urgency = value;
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
        private Char _Priority;
        public Char Priority { get { return this._Priority; }
            set
            {
                if (this.ID != -1 && this.Year != -1 && (value == '1' || value == '2' || value == '3' || value == '4'))
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE NonCompliances SET priority=@value WHERE ID=@ncid AND Year=@ncyear";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@ncid", this.ID);
                        cmd.Parameters.AddWithValue("@ncyear", this.Year);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Priority = value;
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

        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate 
        { 
            get { return this._LastUpdatedDate; }
            set
            {
                if (this.ID != -1 && this.Year != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE NonCompliances SET lastupdated=@value WHERE ID=@ncid AND Year=@ncyear";
                        cmd.Parameters.AddWithValue("@value", value.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@ncid", this.ID);
                        cmd.Parameters.AddWithValue("@ncyear", this.Year);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._LastUpdatedDate = value;
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

        private String _LastUpdatedBy;
        public String LastUpdatedBy { get { return this._LastUpdatedBy; }
            set
            {
                if (this.ID != -1 && this.Year != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE NonCompliances SET lastupdatedby=@value WHERE ID=@ncid AND Year=@ncyear";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@ncid", this.ID);
                        cmd.Parameters.AddWithValue("@ncyear", this.Year);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._LastUpdatedBy = value;
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

        public NonCompliance(int NCID, int NCYear)
        {
            this._ID = -1;
            this._Year = -1;
            this._Quantity = -1;
            this.Categories = new List<NonComplianceType>();
            this.Causes = new List<NonComplianceCause>();
            this.Products = new List<NonComplianceProduct>();
            this.Files = new List<String>();

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year, Quantity, OpeningDate, User, Description, ImmediateAction, "
                + "Cost, Status, ClosureDate, lastupdated, lastupdatedby, severity, priority, urgency "
                +" FROM NonCompliances WHERE ID =@ncid AND Year=@ncyear";
            cmd.Parameters.AddWithValue("@ncid", NCID);
            cmd.Parameters.AddWithValue("@ncyear", NCYear);

            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0) && !rdr.IsDBNull(1))
            {
                this._ID = rdr.GetInt32(0);
                this._Year = rdr.GetInt32(1);
                this._Quantity = rdr.GetInt32(2);
                this._OpeningDate = new DateTime();
                if(!rdr.IsDBNull(3))
                { 
                    this._OpeningDate = rdr.GetDateTime(3);
                }
                this._UserID = rdr.GetString(4);
                this._Description = rdr.GetString(5);
                this._ImmediateAction = rdr.GetString(6);
                this._Cost = rdr.GetDouble(7);
                this._Status = rdr.GetChar(8);
                this._ClosureDate = new DateTime();
                if(!rdr.IsDBNull(9))
                { 
                    this._ClosureDate = rdr.GetDateTime(9);
                }
                this._LastUpdatedBy = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                this._LastUpdatedDate = rdr.IsDBNull(11) ? new DateTime(1970,1,1) : rdr.GetDateTime(11);
                this._Severity = rdr.IsDBNull(12) ? '\0' : rdr.GetChar(12);
                this._Priority = rdr.IsDBNull(13) ? '\0' : rdr.GetChar(13);
                this._Urgency = rdr.IsDBNull(14) ? '\0' : rdr.GetChar(14);
            }
            rdr.Close();
            conn.Close();
        }

        public void CategoryLoad()
        {
            this.Categories = new List<NonComplianceType>();
            if(this.ID!=-1 && this.Year!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT TypeID FROM noncompliancestype_nc WHERE "
                    + "NCID = " + this.ID.ToString()
                    + " AND NCYear = " + this.Year.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Categories.Add(new NonComplianceType(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean CategoryAdd(int catID)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year!=-1)
            { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO noncompliancestype_nc(TypeID, NCID, NCYear) VALUES("
                    + catID.ToString() + ", "
                    + this.ID.ToString() + ", "
                    + this.Year.ToString()
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CategoryAdd(String catName)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                int typeID = -1;
                cmd.CommandText = "SELECT ID FROM NonCompliancesTypes WHERE name = '" + catName + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    typeID = rdr.GetInt32(0);
                }
                rdr.Close();
                conn.Close();

                if(typeID!=-1)
                {
                    ret = this.CategoryAdd(typeID);
                }
            }
            return ret;
        }

        public Boolean CategoryDel(int catID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM noncompliancestype_nc WHERE TypeID = " + catID.ToString() 
                    + " AND NCID = " + this.ID.ToString()
                    + " AND NCYear = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    NonComplianceTypes ncCatList = new NonComplianceTypes();
                    ncCatList.Delete(catID);
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void CauseLoad()
        {
            this.Causes = new List<NonComplianceCause>();
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT CauseID FROM noncompliancescause_nc WHERE "
                    + "NCID = " + this.ID.ToString()
                    + " AND NCYear = " + this.Year.ToString()
                    + " ORDER BY CauseID";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Causes.Add(new NonComplianceCause(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean CauseAdd(int causeID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO noncompliancescause_nc(CauseID, NCID, NCYear) VALUES("
                    + causeID.ToString() + ", "
                    + this.ID.ToString() + ", "
                    + this.Year.ToString()
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CauseAdd(String causeName)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                int typeID = -1;
                cmd.CommandText = "SELECT ID FROM NonCompliancesCause WHERE name = '" + causeName + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    typeID = rdr.GetInt32(0);
                }
                rdr.Close();
                conn.Close();

                if (typeID != -1)
                {
                    ret = this.CauseAdd(typeID);
                }
            }
            return ret;
        }

        public Boolean CauseDel(int causeID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM noncompliancescause_nc WHERE CauseID = " + causeID.ToString()
                    + " AND NCID = " + this.ID.ToString()
                    + " AND NCYear = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    NonComplianceCauses ncCauseList = new NonComplianceCauses();
                    ncCauseList.Delete(causeID);
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void ProductsLoad()
        {
            this.Products = new List<NonComplianceProduct>();
            if(this.ID!=-1 && this.Year > 1970) { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ProductID, ProductYear FROM noncompliances_products WHERE "
                    + " NonComplianceID = " + this.ID.ToString()
                    + " AND NonComplianceYear = " + this.Year.ToString()
                    + " ORDER BY ProductYear, ProductID";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Products.Add(new NonComplianceProduct(this.ID, this.Year, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
            conn.Close();
            }
        }

        public Boolean ProductAdd(int prdID, int prdYear)
        {
            this.log = "NC: " + this.ID + " " + this.Year + " Product: " + prdID + " " + prdYear;
            Boolean ret = false;
            if(this.ID!=-1 && this.Year>1970)
            {
                log = "if1. ";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            // Check if already added
            bool check = false;
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ProductID FROM noncompliances_products WHERE NonComplianceID = " + this.ID.ToString()
                + " AND NonComplianceYear = " + this.Year.ToString()
                + " AND ProductID = " + prdID.ToString()
                + " AND ProductYear = " + prdYear.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                check = true;
            }
            rdr.Close();

                log += "Check: " + check.ToString() + ". ";
            if (!check)
            {
                 Articolo art = new Articolo(prdID, prdYear);
                 if(art.ID!=-1 && art.Year > 1970)
                 {
                        log += "if2. ";
                    char source = art.Status == 'F' ? 'C' : 'P';
                    int qty = art.QuantitaProdotta;
                    cmd.CommandText = "INSERT INTO noncompliances_products(NonComplianceID, NonComplianceYear, "
                        + "ProductID, ProductYear, Source, WarningID, WorkStation, Quantity) VALUES("
                        + this.ID.ToString() + ", "
                        + this.Year.ToString() + ", "
                        + prdID.ToString() + ", "
                        + prdYear.ToString() + ", "
                        + "'" + source + "', "
                        + "null, "
                        + "null, "
                        + qty.ToString() 
                        + ")";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = true;
                    }
                    catch(Exception ex)
                    {
                        log += ex.Message + " " + cmd.CommandText;
                        tr.Rollback();
                        ret = false;
                    }
                  }
                }
            conn.Close();
            }
            return ret;
        }

        public Boolean ProductAdd(Warning wrn)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year > 1970 && wrn.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                // Check if already added
                bool check = false;
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ProductID FROM noncompliances_products WHERE NonComplianceID = " + this.ID.ToString()
                    + " AND NonComplianceYear = " + this.Year.ToString()
                    + " AND WarningID = " + wrn.ID.ToString();

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    check = true;
                }
                rdr.Close();

                if (!check)
                {
                    TaskProduzione tsk = new TaskProduzione(wrn.TaskID);
                    Articolo art = new Articolo(tsk.ArticoloID, tsk.ArticoloAnno);
                    char source ='P';
                    int qty = art.Quantita;
                    cmd.CommandText = "INSERT INTO noncompliances_products(NonComplianceID, NonComplianceYear, "
                        + "ProductID, ProductYear, Source, WarningID, WorkStation, Quantity) VALUES("
                        + this.ID.ToString() + ", "
                        + this.Year.ToString() + ", "
                        + art.ID.ToString() + ", "
                        + art.Year.ToString() + ", "
                        + "'" + source + "', "
                        + wrn.ID.ToString() + ", "
                        + tsk.PostazioneID.ToString() + ", "
                        + qty.ToString()
                        + ")";
                    log = cmd.CommandText;
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        log = ex.Message + " " + cmd.CommandText;
                        tr.Rollback();
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean ProductDel(int prdID, int prdYear)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM noncompliances_products WHERE NonComplianceID = " + this.ID.ToString()
                    + " AND NonComplianceYear = " + this.Year.ToString()
                    + " AND ProductID = " + prdID.ToString()
                    + " AND ProductYear = " + prdYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                log = cmd.CommandText;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log += " " + ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void FilesLoad()
        {
            this.Files = new List<String>();
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString()));
                FileInfo[] Files = d.GetFiles();
                foreach (FileInfo file in Files)
                {
                    this.Files.Add(file.Name);
                }
            }
        }

        public Boolean FileDelete(String fileName)
        {
            Boolean ret = false;
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString()));
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString() + "/" + fileName)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString() + "/" + fileName));
                    ret = true;
                }
            }
            return ret;
        }
    }

    public class NonCompliances
    {
        public String log;

        public List<NonCompliance> NonCompliancesList;

        public NonCompliances()
        {
            this.NonCompliancesList = new List<NonCompliance>();
        }

        public void loadNonCompliances()
        {
            this.NonCompliancesList = new List<NonCompliance>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM NonCompliances ORDER BY Year desc, ID desc";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.NonCompliancesList.Add(new NonCompliance(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean Add(int qty, DateTime opDate, String usr, String desc, String immAction, Double cst, char stat, DateTime closure)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) From NonCompliances WHERE Year = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO NonCompliances(ID, Year, Quantity, OpeningDate, User, Description, "
                + " ImmediateAction, Cost, Status, ClosureDate) VALUES("
                + maxID.ToString() + ", "
                + DateTime.UtcNow.Year.ToString()
                + "'" + opDate.ToString() +"', "
                + "'" + usr.ToString() + "', "
                + "'" + desc + "', "
                + "'" + immAction + "', "
                + cst.ToString() + ", "
                + "'" + stat + "', "
                + "'" + closure.ToString() + "'"
                +")";
            MySqlTransaction tr = conn.BeginTransaction();

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                ret = false;
                tr.Rollback();
                log = ex.Message + "<br />" + cmd.CommandText;
            }

            conn.Close();
            return ret;
        }

        public int[] Add(String usr)
        {
            int[] ret = new int[2];
            ret[0] = -1; ret[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) From NonCompliances WHERE Year = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO NonCompliances(ID, Year, Quantity, OpeningDate, User, Description, "
                + " ImmediateAction, Cost, Status, ClosureDate) VALUES("
                + maxID.ToString() + ", "
                + DateTime.UtcNow.Year.ToString() + ", "
                + "1, "
                + "'" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "', "
                + "'" + usr.ToString() + "', "
                + "'', "
                + "'', "
                + "0, "
                + "'O', "
                + "null"
                + ")";
            MySqlTransaction tr = conn.BeginTransaction();

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret[0]= maxID; ret[1] = DateTime.UtcNow.Year;
            }
            catch (Exception ex)
            {
                ret[0] = -1; ret[1] = -1;
                tr.Rollback();
                log = ex.Message + "<br />" + cmd.CommandText;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int NCID, int NCYear)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM NonCompliances WHERE ID = " + NCID.ToString() 
                + " AND Year = " + NCYear.ToString();
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                ret = false;
            }
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceTypes
    {
        public List<NonComplianceType> TypeList;

        public NonComplianceTypes()
        {
            this.TypeList = new List<NonComplianceType>();
        }

        public void loadTypeList()
        {
            this.TypeList = new List<NonComplianceType>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name, Description FROM NonCompliancesTypes ORDER BY Name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.TypeList.Add(new NonComplianceType(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean Add(String name, String desc)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) FROM NonCompliancesTypes";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO NonCompliancesTypes(ID, Name, Description) VALUES("
                + maxID.ToString()
                + ", '" + name + "', "
                + "'" + desc + "')";
 
            MySqlTransaction tr = conn.BeginTransaction();
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
            return ret;
        }

        public Boolean Delete(int typeID)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM NonCompliancesType_nc WHERE TypeID = " + typeID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            bool occurences = false;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                occurences = true;
            }
            rdr.Close();
            if(!occurences)
            { 
                cmd.CommandText = "DELETE FROM NonCompliancesTypes WHERE ID = " + typeID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
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
            
            }
            conn.Close();
            return ret;
        }

        public int findIDByName(String name)
        {
            int ret = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM NonCompliancesTypes WHERE Name = '" + name + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                ret = rdr.GetInt32(0);
            }
            rdr.Close();
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceType
    {
        public String log;

        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliancesTypes SET Name = '"+value+"' WHERE ID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Name = value;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliancesTypes SET Description = '" + value + "' WHERE ID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public NonComplianceType(int typeID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name, Description FROM NonCompliancesTypes WHERE ID = " + typeID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Name = rdr.GetString(1);
                this._Description = rdr.GetString(2);
            }
            else
            {
                this._ID = -1;
                this._Name = "";
                this._Description = "";
            }
            conn.Close();
        }
    }

    public class NonComplianceCauses
    {
        public List<NonComplianceCause> CausesList;

        public NonComplianceCauses()
        {
            this.CausesList = new List<NonComplianceCause>();
        }

        public void loadCausesList()
        {
            this.CausesList = new List<NonComplianceCause>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name, Description FROM NonCompliancesCause ORDER BY Name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.CausesList.Add(new NonComplianceCause(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean Add(String name, String desc)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) FROM NonCompliancesCause";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO NonCompliancesCause(ID, Name, Description) VALUES("
                + maxID.ToString()
                + ", '" + name + "', "
                + "'" + desc + "')";
            MySqlTransaction tr = conn.BeginTransaction();
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
            conn.Close();
            return ret;
        }

        public Boolean Delete(int typeID)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM NonCompliancesCause_nc WHERE CauseID = " + typeID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            bool occurences = false;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                occurences = true;
            }
            rdr.Close();
            if (!occurences)
            {

                cmd.CommandText = "DELETE FROM NonCompliancesCause WHERE ID = " + typeID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
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
            }
            conn.Close();
            return ret;
        }

        public int findIDByName(String name)
        {
            int ret = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM NonCompliancesCause WHERE Name = '" + name + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                ret = rdr.GetInt32(0);
            }
            rdr.Close();
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceCause
    {
        public String log;

        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliancesCause SET Name = '" + value + "' WHERE ID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Name = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE NonCompliancesCause SET Description = '" + value + "' WHERE ID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public NonComplianceCause(int typeID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name, Description FROM NonCompliancesCause WHERE ID = " + typeID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Name = rdr.GetString(1);
                this._Description = rdr.GetString(2);
            }
            else
            {
                this._ID = -1;
                this._Name = "";
                this._Description = "";
            }
            conn.Close();
        }
    }

    public class NonComplianceProduct
    {
        public String log;

        private int _NonComplianceID;
            public int NonComplianceID { get { return this._NonComplianceID; } }
        private int _NonComplianceYear;
        public int NonComplianceYear { get { return this._NonComplianceYear; } }
        private int _ProductID;
        public int ProductID { get { return this._ProductID; } }
        private int _ProductYear;
        public int ProductYear { get { return this._ProductYear; } }
        private String _processName;
        public String processName { get { return this._processName; } }
        private String _varianteName;
        public String varianteName { get { return this._varianteName; } }
        private String _SerialNumber;
        public String SerialNumber { get { return this._SerialNumber; } }
        private int _Quantity;
        public int Quantity { get { return this._Quantity; } }
        private String _CustomerID;
        public String CustomerID { get { return this._CustomerID; } }
        private String _CustomerName;
        public String CustomerName { get { return this._CustomerName; } }
        /*'C' = Customer claim
         * 'P' = Internal production
         */
        private char _Source;
        public char Source { get { return this._Source; }
            set {
                if(this.ProductID!=-1 && this.ProductYear > 1970 && this.NonComplianceID !=-1 &&this.NonComplianceYear>1970)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE noncompliances_products SET source = '" + value.ToString() + "' WHERE "
                    + " NonComplianceID = " + this.NonComplianceID.ToString() + " AND NonComplianceYear = " + this.NonComplianceYear.ToString()
                    + " AND ProductID = " + this.ProductID.ToString() + " AND ProductYear = " + this.ProductYear.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message + " " + cmd.CommandText;
                        tr.Rollback();
                    }
                conn.Close();
                }
            }
        }
        private int _WarningID;
        public int WarningID { get { return this._WarningID; }
        }
        private int _Workstation;
        public int Workstation {  get { return this._Workstation; }
        set
            {
                if (this.ProductID != -1 && this.ProductYear > 1970 && this.NonComplianceID != -1 && this.NonComplianceYear > 1970)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    if(value == -1)
                    {
                        cmd.CommandText = "UPDATE noncompliances_products SET WorkStation = null WHERE "
                            + " NonComplianceID = " + this.NonComplianceID.ToString() + " AND NonComplianceYear = " + this.NonComplianceYear.ToString()
                            + " AND ProductID = " + this.ProductID.ToString() + " AND ProductYear = " + this.ProductYear.ToString();
                    }
                    else {
                        cmd.CommandText = "UPDATE noncompliances_products SET WorkStation = " + value.ToString() + " WHERE "
                            + " NonComplianceID = " + this.NonComplianceID.ToString() + " AND NonComplianceYear = " + this.NonComplianceYear.ToString()
                            + " AND ProductID = " + this.ProductID.ToString() + " AND ProductYear = " + this.ProductYear.ToString();
                    }
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        if(value == -1)
                        {
                            this._WorkstationName = "";
                            this._Workstation = -1;
                        }
                        else
                        {
                            Postazione p = new Postazione(value);
                            this._Workstation = value;
                            this._WorkstationName = p.name;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }
        private String _WorkstationName;
        public String WorkstationName
        {
            get { return this._WorkstationName; }
        }

        private int _QuantityInvolved;
        public int QuantityInvolved { get { return this._QuantityInvolved; }
            set
            {
                Articolo art = new Articolo(this.ProductID, this.ProductYear);
                if (this.ProductID != -1 && this.ProductYear > 1970 && this.NonComplianceID != -1 && this.NonComplianceYear > 1970 && value <= art.QuantitaProdotta)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE noncompliances_products SET Quantity = '" + value.ToString() + "' WHERE "
                        + " NonComplianceID = " + this.NonComplianceID.ToString() + " AND NonComplianceYear = " + this.NonComplianceYear.ToString()
                        + " AND ProductID = " + this.ProductID.ToString() + " AND ProductYear = " + this.ProductYear.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message + " " + cmd.CommandText;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public NonComplianceProduct(int ncID, int ncYear, int prdID, int prdYear)
        {
            this._ProductID = -1;
            this._ProductYear = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT productionplan.id, productionplan.anno, processo.name, varianti.nomevariante, "
                                + "productionplan.matricola, productionplan.quantita, commesse.cliente, "
                                + "anagraficaclienti.ragsociale, noncompliances_products.source, noncompliances_products.warningID, "
                                + " noncompliances_products.Workstation, noncompliances_products.Quantity FROM "
                                + "noncompliances_products INNER JOIN productionplan "
                                + "ON(noncompliances_products.ProductID = productionplan.id AND noncompliances_products.ProductYear = productionplan.anno)"
                                + " INNER JOIN variantiprocessi ON(productionplan.processo = variantiprocessi.processo "
                                + " AND productionplan.revisione = variantiprocessi.revproc AND productionplan.variante = variantiprocessi.variante)"
                                + " INNER JOIN processo ON (processo.processID = variantiprocessi.processo"
                                + " AND processo.revisione = variantiprocessi.revProc) "
                                + " INNER JOIN varianti ON (variantiprocessi.variante = varianti.idvariante)"
                                + " INNER JOIN commesse ON(productionplan.commessa = commesse.idcommesse"
                                + " AND productionplan.annocommessa = commesse.anno)"
                                + " INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice) "
                                + " WHERE noncompliances_products.NonComplianceID = " + ncID.ToString()
                                + " AND noncompliances_products.NonComplianceYear = " + ncYear.ToString()
                                + " AND noncompliances_products.productID = " + prdID.ToString()
                                + " AND noncompliances_products.productYear = " + prdYear.ToString()
                                + " ORDER BY productionplan.anno, productionplan.id";

            log = cmd.CommandText;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._NonComplianceID = ncID;
                this._NonComplianceYear = ncYear;
                this._ProductID = prdID;
                this._ProductYear = prdYear;
                this._processName = rdr.GetString(2);
                this._varianteName = rdr.GetString(3);
                if(!rdr.IsDBNull(4))
                { 
                    this._SerialNumber = rdr .GetString(4);
                }
                this._Quantity = rdr.GetInt32(5);
                this._CustomerID = rdr.GetString(6);
                this._CustomerName = rdr.GetString(7);
                this._Source = rdr.GetChar(8);
                this._WarningID = !rdr.IsDBNull(9) ? rdr.GetInt32(9) : -1;
                this._Workstation = !rdr.IsDBNull(10) ? rdr.GetInt32(10) : -1;
                if (this.Workstation!=-1) { Postazione p = new Postazione(this.Workstation);this._WorkstationName = p.name; }
                this._QuantityInvolved = rdr.GetInt32(11);
    }
            else
            {
                this._ProductID = -1;
                this._ProductYear = -1;
                this._NonComplianceID = -1;
                this._NonComplianceYear = -1;
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class FreeWarnings
    {
        public String log;

        public List<FlatWarning> FreeWarningList;
        public FreeWarnings()
        {
            this.FreeWarningList = new List<FlatWarning>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT warningproduzione.id, warningproduzione.datachiamata, warningproduzione.task, "
+ " postazioni.idpostazioni, postazioni.name,"
+ " productionplan.id, productionplan.anno, processo.name, varianti.nomevariante, "
+ " productionplan.matricola, productionplan.quantita, commesse.cliente, anagraficaclienti.ragsociale, "
+ " warningproduzione.user, warningproduzione.motivo, warningproduzione.risoluzione "
+ " FROM noncompliances_products RIGHT JOIN warningproduzione ON"
+ " (noncompliances_products.warningID = warningproduzione.ID)"
+ " INNER JOIN tasksproduzione ON (warningproduzione.task = tasksproduzione.taskID)"
+ " INNER JOIN productionplan ON (productionplan.id = tasksproduzione.idArticolo AND"
+ " productionplan.anno = tasksproduzione.annoArticolo)"
+ " INNER JOIN variantiprocessi "
+ " ON (productionplan.processo = variantiprocessi.processo"
+ " AND productionplan.revisione = variantiprocessi.revproc"
+ " AND productionplan.variante = variantiprocessi.variante)"
+ " INNER JOIN processo ON (processo.processID = variantiprocessi.processo"
+ " AND processo.revisione = variantiprocessi.revProc)"
            + " INNER JOIN varianti ON (variantiprocessi.variante = varianti.idvariante)"
            + " INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse"
            + " AND productionplan.annocommessa = commesse.anno)"
            + " INNER JOIN anagraficaclienti ON (commesse.cliente = anagraficaclienti.codice)"
            + " INNER JOIN postazioni ON (postazioni.idpostazioni = tasksproduzione.postazione)"
            + " WHERE warningID IS NULL"
            + " ORDER BY warningproduzione.dataChiamata DESC";
            log = cmd.CommandText;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                FlatWarning curr = new FlatWarning();
                curr.WarningID = rdr.GetInt32(0);
                curr.WarningDate = rdr.GetDateTime(1);
                curr.TaskID = rdr.GetInt32(2);
                curr.Workstation = rdr.GetInt32(3);
                curr.WorkstationName = rdr.GetString(4);
                curr.ProductID = rdr.GetInt32(5);
                curr.ProductYear = rdr.GetInt32(6);
                curr.processName = rdr.GetString(7);
                curr.varianteName = rdr.GetString(8);
                if(!rdr.IsDBNull(9))
                { 
                    curr.SerialNumber = rdr.GetString(9);
                }
                curr.Quantity = rdr.GetInt32(10);
                curr.CustomerID = rdr.GetString(11);
                curr.CustomerName = rdr.GetString(12);
                if(!rdr.IsDBNull(13))
                { 
                    curr.User = rdr.GetString(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.Reason = rdr.GetString(14);
                }
                if(!rdr.IsDBNull(15))
                { 
                    curr.Resolution = rdr.GetString(15);
                }

                this.FreeWarningList.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class AnalysisNCCause
    {
        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int CauseID;
        public String CauseName;
        public String CauseDescription;
    }

    public struct AnalysisNCCategory
    {
        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int CategoryID;
        public String CategoryName;
        public String CategoryDescription;
    }

    public struct AnalysisNCProduct
    {
        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int ProductID;
        public int ProductYear;
        public int NCQuantity;
        public char Source;
        public int WarningID;
        public int Workstation;
        public int ProcessID;
        public int ProcessRev;
        public String ProcessName;
        public int VariantID;
        public String VariantName;

    }

    public class NCAnalysis
    {
        public List<AnalysisNCCause> ncCauses;
        public List<AnalysisNCCategory> ncCategory;
        public List<AnalysisNCProduct> ncProduct;
        public List<NonCompliancesAnalysisStruct> ncList;

        public NCAnalysis()
        {
            this.ncCauses = new List<AnalysisNCCause>();
            this.ncCategory = new List<AnalysisNCCategory>();
            this.ncProduct = new List<AnalysisNCProduct>();
            this.ncList = new List<NonCompliancesAnalysisStruct>();
        }

        public void loadNcCauses()
        {
            this.ncCauses = new List<AnalysisNCCause>();
            MySqlConnection conn = (new Dati.Dati().mycon());
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, noncompliances.User, "
                + "noncompliances.Description, noncompliances.ImmediateAction, noncompliances.Cost, "
                + "noncompliances.Status, noncompliances.ClosureDate, noncompliancescause.ID, noncompliancescause.Name, "
                + "noncompliancescause.Description FROM noncompliances INNER JOIN noncompliancescause_nc ON "
                + "(noncompliances.ID = noncompliancescause_nc.NCID AND noncompliances.Year = noncompliancescause_nc.NCYear) "
                + "INNER JOIN noncompliancescause ON (noncompliancescause.ID = noncompliancescause_nc.CauseID) "
                + " ORDER BY noncompliances.OpeningDate desc, noncompliancescause.ID ASC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                AnalysisNCCause curr = new AnalysisNCCause();
                curr.NCID = rdr.GetInt32(0);
                curr.NCYear = rdr.GetInt32(1);
                curr.OpeningDate = rdr.GetDateTime(2);
                curr.User = rdr.GetString(3);
                if (!rdr.IsDBNull(4)) { curr.NCDescription = rdr.GetString(4); }
                if (!rdr.IsDBNull(5)) { curr.ImmediateAction = rdr.GetString(5); }
                if (!rdr.IsDBNull(6)) { curr.Cost = rdr.GetDouble(6); }
                curr.Status = rdr.GetChar(7);
                if (!rdr.IsDBNull(8)) { curr.ClosureDate = rdr.GetDateTime(8); }
                curr.CauseID = rdr.GetInt32(9);
                curr.CauseName = rdr.GetString(10);
                if (!rdr.IsDBNull(11)) { curr.CauseDescription = rdr.GetString(11); }

                this.ncCauses.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }

        public void loadNcCategories()
        {
            this.ncCategory = new List<AnalysisNCCategory>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, noncompliances.User, "
                + "noncompliances.Description, noncompliances.ImmediateAction, noncompliances.Cost, "
                + " noncompliances.Status, noncompliances.ClosureDate, noncompliancestypes.ID, noncompliancestypes.Name, "
                + "noncompliancestypes.Description FROM noncompliances INNER JOIN noncompliancestype_nc ON "
                + "(noncompliances.ID = noncompliancestype_nc.NCID AND noncompliances.Year = noncompliancestype_nc.NCYear)"
                + " INNER JOIN noncompliancestypes ON (noncompliancestypes.ID = noncompliancestype_nc.TypeID)"
                + " ORDER BY noncompliances.OpeningDate desc, noncompliancestypes.ID ASC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                AnalysisNCCategory curr = new AnalysisNCCategory();
                curr.NCID = rdr.GetInt32(0);
                curr.NCYear = rdr.GetInt32(1);
                curr.OpeningDate = rdr.GetDateTime(2);
                curr.User = rdr.GetString(3);
                if (!rdr.IsDBNull(4)) { curr.NCDescription = rdr.GetString(4); }
                if (!rdr.IsDBNull(5)) { curr.ImmediateAction = rdr.GetString(5); }
                if (!rdr.IsDBNull(6)) { curr.Cost = rdr.GetDouble(6); }
                curr.Status = rdr.GetChar(7);
                if (!rdr.IsDBNull(8)) { curr.ClosureDate = rdr.GetDateTime(8); }
                curr.CategoryID = rdr.GetInt32(9);
                curr.CategoryName = rdr.GetString(10);
                if (!rdr.IsDBNull(11)) { curr.CategoryDescription = rdr.GetString(11); }

                this.ncCategory.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }

        public void loadNcProducts()
        {
            this.ncProduct = new List<AnalysisNCProduct>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, "
                + "noncompliances.User, noncompliances.Description, noncompliances.ImmediateAction, "
                + "noncompliances.Cost, noncompliances.Status, noncompliances.ClosureDate, "
                + "noncompliances_products.ProductID, noncompliances_products.ProductYear, "
                + "noncompliances_products.Quantity, noncompliances_products.Source, "
                + "noncompliances_products.WarningID, noncompliances_products.Workstation, "
                + "processo.ProcessID, processo.revisione, processo.Name, varianti.idvariante, varianti.nomevariante "
                + " FROM noncompliances INNER JOIN noncompliances_products "
                + "ON(noncompliances.ID = noncompliances_products.NonComplianceID "
                + "AND noncompliances.Year = noncompliances_products.NonComplianceYear) "
                + " INNER JOIN productionplan ON(noncompliances_products.ProductID = productionplan.id "
                + "AND noncompliances_products.ProductYear = productionplan.anno) "
                + "INNER JOIN variantiprocessi ON(productionplan.processo = variantiprocessi.processo "
                + " AND productionplan.revisione = variantiprocessi.revproc "
                + " AND productionplan.variante = variantiprocessi.variante) "
                + " INNER JOIN processo ON (processo.processID = variantiprocessi.processo "
                + " AND processo.revisione = variantiprocessi.revProc) INNER JOIN "
                + " varianti ON (variantiprocessi.variante = varianti.idvariante) "
                + " INNER JOIN commesse ON(productionplan.commessa = commesse.idcommesse "
                + " AND productionplan.annocommessa = commesse.anno) "
                + " INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice)";

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                AnalysisNCProduct curr = new AnalysisNCProduct();
                curr.NCID = rdr.GetInt32(0);
                curr.NCYear = rdr.GetInt32(1);
                curr.OpeningDate = rdr.GetDateTime(2);
                curr.User = rdr.GetString(3);
                if (!rdr.IsDBNull(4)) { curr.NCDescription = rdr.GetString(4); }
                if (!rdr.IsDBNull(5)) { curr.ImmediateAction = rdr.GetString(5); }
                curr.Cost = rdr.GetDouble(6);
                curr.Status = rdr.GetChar(7);
                if (!rdr.IsDBNull(8)) { curr.ClosureDate = rdr.GetDateTime(8); }
                curr.ProductID = rdr.GetInt32(9);
                curr.ProductYear = rdr.GetInt32(10);
                curr.NCQuantity = rdr.GetInt32(11);
                curr.Source = rdr.GetChar(12);
                if (!rdr.IsDBNull(13)) { curr.WarningID = rdr.GetInt32(13); }
                if (!rdr.IsDBNull(14)) { curr.Workstation = rdr.GetInt32(14); }
                curr.ProcessID = rdr.GetInt32(15);
                curr.ProcessRev = rdr.GetInt32(16);
                curr.ProcessName = rdr.GetString(17);
                curr.VariantID = rdr.GetInt32(18);
                curr.VariantName = rdr.GetString(19);

                this.ncProduct.Add(curr);
            }
        }

        public List<NonCompliancesAnalysisStruct> loadNonCompliances()
        {
            ncList = new List<NonCompliancesAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year, Quantity, OpeningDate, Status, ClosureDate, cost FROM noncompliances";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                NonCompliancesAnalysisStruct curr = new NonCompliancesAnalysisStruct();
                curr.ID = rdr.GetInt32(0);
                curr.Year = rdr.GetInt32(1);
                curr.Quantity = rdr.GetInt32(2);
                curr.OpeningDate = rdr.GetDateTime(3);
                curr.Status = rdr.GetChar(4);
                if(!rdr.IsDBNull(5))
                {
                    curr.ClosureDate = rdr.GetDateTime(5);
                }
                curr.Cost = rdr.GetDouble(6);
                this.ncList.Add(curr);
            }
            rdr.Close();
            conn.Close();
            return this.ncList;
        }
    }

    public struct NonCompliancesAnalysisStruct
    {
        public int ID;
        public int Year;
        public int Quantity;
        public DateTime OpeningDate;
        public char Status;
        public DateTime ClosureDate;
        public double Cost;
    }

    public class ImprovementActions
    {
        public String log;

        public List<ImprovementAction> ImprovementActionsList;
        public ImprovementActions()
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
        }

        public void loadImprovementActions()
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM ImprovementActions ORDER BY OpeningDate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.ImprovementActionsList.Add(new ImprovementAction(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadImprovementActions(char stat)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM ImprovementActions WHERE improvementactions.status = '" + stat 
                + "' ORDER BY OpeningDate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ImprovementActionsList.Add(new ImprovementAction(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadImprovementActions(User usr)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM ImprovementActions INNER JOIN improvementactions_team ON "
                +"(ImprovementActions.ID = improvementactions_team.ImprovementActionID AND "
                + "improvementactions.Year = improvementactions_team.ImprovementActionYear) "
                + " WHERE improvementactions_team.user = '" + usr.username + "'"
                + " ORDER BY OpeningDate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ImprovementActionsList.Add(new ImprovementAction(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadImprovementActions(User usr, Char status)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM ImprovementActions INNER JOIN improvementactions_team ON "
                + "(ImprovementActions.ID = improvementactions_team.ImprovementActionID AND "
                + "improvementactions.Year = improvementactions_team.ImprovementActionYear) "
                + " WHERE improvementactions_team.user = '" + usr.username + "' "
                + " AND improvementactions.status = '" + status + "'"
                + " ORDER BY OpeningDate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.ImprovementActionsList.Add(new ImprovementAction(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public int[] Add(User creator)
        {
            int[] ret = new int[2];
            ret[0] = -1;
            ret[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) FROM ImprovementActions WHERE Year = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int iActID = 0;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                iActID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO ImprovementActions(ID, Year, OpeningDate, CurrentSituation, ExpectedResults, "
                + " RootCauses, ClosureNotes, Status, EndDateExpected, EndDateReal, CreatedBy, ModifiedBy, ModifiedDate) VALUES("
                + iActID.ToString() + ", "
                + DateTime.UtcNow.Year.ToString() + ", "
                + "'" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "', "
                + "'', "
                + "'', "
                + "'', "
                + "'', "
                + "'O', "
                + "null, "
                + "null, "
                + "'" + creator.username + "', "
                + "'', "
                + "null"
                + ")";

            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret[0] = iActID;
                ret[1] = DateTime.UtcNow.Year;
            }
            catch(Exception ex)
            {
                log = ex.Message + " " + cmd.CommandText;
                tr.Rollback();
                ret[0] = -1;
                ret[1] = -1;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int id, int year)
        {
            Boolean ret = false;
            ImprovementAction curr = new ImprovementAction(id, year);
            if(curr.ID!=-1 && curr.Year!=-1)
            {
                curr.loadCorrectiveActions();
                for(int i = 0; i < curr.CorrectiveActions.Count; i++)
                {
                    curr.CorrectiveActions[i].loadTeamMembers();
                    for(int j = 0; j < curr.CorrectiveActions[i].TeamMembers.Count; j++)
                    {
                        curr.CorrectiveActions[i].MemberRemove(curr.CorrectiveActions[i].TeamMembers[j].User);
                    }

                    curr.CorrectiveActions[i].loadTasks();
                    for(int j = 0; j < curr.CorrectiveActions[i].Tasks.Count; j++)
                    {
                        curr.CorrectiveActions[i].TaskRemove(curr.CorrectiveActions[i].Tasks[j].TaskID);
                    }
                }
                curr.loadTeamMembers();
                for(int i =0; i < curr.TeamMembers.Count; i++)
                {
                    curr.MemberRemove(curr.TeamMembers[i].User);
                }

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM improvementactions WHERE ID = " + id.ToString() + " AND Year = " + year.ToString();
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                ret = false;
                tr.Rollback();
            }
            conn.Close();
            }
            return ret;
        }
    }

    public class ImprovementAction
    {
        public String log;

        private int _ID;
        public int ID { get { return this._ID; } }
        private int _Year;
        public int Year { get { return this._Year; } }
        private DateTime _OpeningDate;
        public DateTime OpeningDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._OpeningDate, fuso.tzFusoOrario);
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE ImprovementActions SET OpeningDate = '" + value.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._OpeningDate = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
        }

        private String _CreatedBy;
        public String CreatedBy
        {
            get { return this._CreatedBy; }
        }

        private String _CurrentSituation;
        public String CurrentSituation
        {
            get { return this._CurrentSituation; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET CurrentSituation = '" + value
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._CurrentSituation = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ExpectedResults;
        public String ExpectedResults
        {
            get { return this._ExpectedResults; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET ExpectedResults = '" + value
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ExpectedResults = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _RootCauses;
        public String RootCauses
        {
            get { return this._RootCauses; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET RootCauses = '" + value
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._RootCauses = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ClosureNotes;
        public String ClosureNotes
        {
            get { return this._ClosureNotes; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET ClosureNotes = '" + value
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ClosureNotes = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        /* O == Open
         * C == Closed
         */
        private Char _Status;
        public Char Status
        {
            get { return this._Status; }
            set
            {
                Boolean check = true;
                if(value == 'C')
                {
                    this.loadCorrectiveActions();
                    for(int i = 0; i < this.CorrectiveActions.Count; i++)
                    {
                        if(this.CorrectiveActions[i].Status != 'C')
                        {
                            check = false;
                        }
                    }
                }
                if(value == 'O' || (value == 'C' && check))
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET Status = '" + value
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Status = value;
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


        private DateTime _EndDateExpected;
        public DateTime EndDateExpected
        {
            get {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._EndDateExpected, fuso.tzFusoOrario); }
            set
            {
                FusoOrario fuso = new FusoOrario();
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET EndDateExpected = '" + curr.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EndDateExpected = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private DateTime _EndDateReal;
        public DateTime EndDateReal
        {
            get {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._EndDateReal, fuso.tzFusoOrario); }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET EndDateReal = '" + value.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EndDateReal = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ModifiedBy;
        public String ModifiedBy
        {
            get { return this._ModifiedBy; }
            set
            {
                User usr = new User(value);
                if(usr.username.Length > 0)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE ImprovementActions SET ModifiedBy = '" + value
                    + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._ModifiedBy = value;
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

        private DateTime _ModifiedDate;
        public DateTime ModifiedDate
        {
            get {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._ModifiedDate, fuso.tzFusoOrario); }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ImprovementActions SET ModifiedDate = '" + value.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.ID.ToString() + " AND Year = " + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ModifiedDate = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<ImprovementActionTeamMember> TeamMembers;

        public List<CorrectiveAction> CorrectiveActions;

        public ImprovementAction(int iActID, int iActYear)
        {
            this.TeamMembers = new List<ImprovementActionTeamMember>();
            this.CorrectiveActions = new List<CorrectiveAction>();

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year, OpeningDate, CurrentSituation, ExpectedResults, RootCauses, "
                + " ClosureNotes, Status, EndDateExpected, EndDateReal, CreatedBy, ModifiedBy, ModifiedDate "
                + " FROM ImprovementActions WHERE ID = " + iActID.ToString() + " AND Year = " + iActYear.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Year = rdr.GetInt32(1);
                if (!rdr.IsDBNull(2))
                {
                    this._OpeningDate = rdr.GetDateTime(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    this._CurrentSituation = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    this._ExpectedResults = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    this._RootCauses = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    this._ClosureNotes = rdr.GetString(6);
                }
                if(!rdr.IsDBNull(7))
                { 
                    this._Status = rdr.GetChar(7);
                }
                else
                {
                    this._Status = 'O';
                }
                if (!rdr.IsDBNull(8))
                {
                    this._EndDateExpected = rdr.GetDateTime(8);
                }
                else
                {
                    this._EndDateExpected = new DateTime(1970, 1, 1);
                }

                if (!rdr.IsDBNull(9))
                {
                    this._EndDateReal = rdr.GetDateTime(9);
                }
                else
                {
                    this._EndDateReal = new DateTime(1970, 1, 1);
                }
                if(!rdr.IsDBNull(10))
                {
                    this._CreatedBy = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    this._ModifiedBy = rdr.GetString(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    this._ModifiedDate = rdr.GetDateTime(12);
                }
            }
            else
            {
                this._ID = -1;
                this._Year = -1;
            }
            rdr.Close();
                conn.Close();
        }

        public void loadTeamMembers()
        {
            this.TeamMembers = new List<ImprovementActionTeamMember>();

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT user, role FROM improvementactions_team WHERE ImprovementActionID = " + this.ID.ToString()
                + " AND ImprovementActionYear = " + this.Year.ToString() + " ORDER BY role, user";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.TeamMembers.Add(new ImprovementActionTeamMember(this.ID, this.Year, rdr.GetString(0), rdr.GetChar(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean MemberAdd(String username, Char role)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO ImprovementActions_Team(ImprovementActionID, ImprovementActionYear, user, role) "
                + " VALUES("+this.ID.ToString() +", " + this.Year.ToString() + ", '"
                + username +"', '" + role + "')";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + cmd.CommandText;
                }
            conn.Close();
            }
            return ret;
        }

        public Boolean MemberRemove(String username)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
             cmd.CommandText = "DELETE FROM improvementactions_team WHERE ImprovementActionID = "
                + this.ID.ToString() + " AND ImprovementActionYear = " + this.Year.ToString()
                + " AND user = '" + username + "'";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                ret = false;
                log = ex.Message + " " + cmd.CommandText;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public void loadCorrectiveActions()
        {
            this.CorrectiveActions = new List<CorrectiveAction>();
            if(this.ID!=-1 && this.Year > 1970)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM correctiveactions WHERE ImprovementActionID = " + this.ID.ToString()
                + " AND ImprovementActionYear = " + this.Year.ToString()
                + " ORDER BY correctiveactions.LateStart";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.CorrectiveActions.Add(new CorrectiveAction(this.ID, this.Year, rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
            }
        }

        public int CorrectiveActionAdd()
        {
            int ret = -1;
            if(this.ID!=-1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(ID) FROM correctiveactions WHERE ImprovementActionID = " + this.ID.ToString()
                    + " AND ImprovementActionYear = " + this.Year.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                int caID = 0;
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    caID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();

                cmd.CommandText = "INSERT INTO correctiveactions(ID, ImprovementActionID, ImprovementActionYear, "
                    + " Description, LeadTimeExpected, EarlyStart, LateStart, EarlyFinish, LateFinish, Status) VALUES("
                    + caID.ToString() + ", "
                    + this.ID.ToString() + ", "
                    + this.Year.ToString() + ", "
                    + "'', "
                    + "0.0, "
                    + "null, "
                    + "null, "
                    + "null, "
                    + "null, "
                    + "'O'"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    if(this.Status == 'C')
                    {
                        this.Status = 'O';
                    }
                    ret = caID;
                }
                catch(Exception ex)
                {
                    ret = -1;
                    this.log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CorrectiveActionRemove(int CorrectiveActionID)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year > 1970)
            {
                CorrectiveAction curr = new CorrectiveAction(this.ID, this.Year, CorrectiveActionID);
                curr.loadTasks();
                for(int i = 0; i < curr.Tasks.Count; i++)
                {
                    // Remove tasks
                }

                curr.loadTeamMembers();
                for(int i = 0; i < curr.TeamMembers.Count; i++)
                {
                    curr.MemberRemove(curr.TeamMembers[i].User);
                }

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM correctiveactions WHERE ID = " + CorrectiveActionID.ToString()
                    + " AND ImprovementActionID = " + this.ID.ToString() + " AND ImprovementActionYear = "
                    + this.Year.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    this.log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                }
                conn.Close();
            }
            return ret;
        }
    }

    public class ImprovementActionTeamMember
    {
        public String log;

        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }

        /* M == Manager --> can create, edit, delete improvement action's team members and tasks
         * T == Team Member --> tasks can be assigned to him, and he can execute tasks
         */
        private Char _Role;
        public Char Role
        {
            get
            {
                return this._Role;
            }
        }

        public ImprovementActionTeamMember(int iActID, int iActYear, String iActUser)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ImprovementActionID, ImprovementActionYear, user, role FROM "
                + " ImprovementActions_Team WHERE ImprovementActionID = " + iActID.ToString()
                + " AND ImprovementActionYear = " + iActYear.ToString()
                + " AND user = '" + iActUser + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ImprovementActionID = rdr.GetInt32(0);
                this._ImprovementActionYear = rdr.GetInt32(1);
                if(!rdr.IsDBNull(2))
                {
                    this._User = rdr.GetString(2);
                }
                if(!rdr.IsDBNull(3))
                {
                    this._Role = rdr.GetChar(3);
                }
            }
            rdr.Close();
            conn.Close();
        }

        public ImprovementActionTeamMember(int iActID, int iActYear, String iActUser, Char iActRole)
        {
            this._ImprovementActionID = iActID;
            this._ImprovementActionYear = iActYear;
            this._User = iActUser;
            this._Role = iActRole;
        }
    }

    public class CorrectiveAction
    {
        public String log;

        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET Description = '" + value
                + "' WHERE ID = " + this.CorrectiveActionID.ToString() 
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private Double _LeadTimeExpected;
        public Double LeadTimeExpected
        {
            get
            {
                return this._LeadTimeExpected;
            }
            set
            {
                if(value >=0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET LeadTimeExpected = " + value
                + " WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LeadTimeExpected = value;
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
        private DateTime _EarlyStart;
        public DateTime EarlyStart
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStart, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario();
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET EarlyStart = '" + curr.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyStart = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _LateStart;
        public DateTime LateStart
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStart, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario();
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET LateStart = '" + curr.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateStart = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _EarlyFinish;
        public DateTime EarlyFinish
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinish, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario();
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET EarlyFinish = '" + curr.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyFinish = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _LateFinish;
        public DateTime LateFinish
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinish, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario();
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET LateFinish = '" + curr.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateFinish = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        /* O == Opened
         * I = Running
         * C == Closed
         */
        private Char _Status;
        public Char Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if(Status == 'I' || Status == 'C' || Status == 'O')
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET Status = '" + value
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Status = value;
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

        private DateTime _EndDateReal;
        public DateTime EndDateReal
        {
            get { return this._EndDateReal; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CorrectiveActions SET EndDateReal = '" + value.ToString("yyyy-MM-dd HH:mm:ss")
                + "' WHERE ID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EndDateReal = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<CorrectiveActionTeamMember> TeamMembers;

        public List<CorrectiveActionTask> Tasks;

        public CorrectiveAction(int iActID, int iActYear, int CorrectiveActionID)
        {
            this.TeamMembers = new List<CorrectiveActionTeamMember>();
            this.Tasks = new List<CorrectiveActionTask>();

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, ImprovementActionID, ImprovementActionYear, Description, LeadTimeExpected, "
                + "EarlyStart, LateStart, EarlyFinish, LateFinish, Status FROM CorrectiveActions WHERE "
                + "ID = " + CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + iActID.ToString()
                + " AND ImprovementActionYear = " + iActYear.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._CorrectiveActionID = rdr.GetInt32(0);
                this._ImprovementActionID = rdr.GetInt32(1);
                this._ImprovementActionYear = rdr.GetInt32(2);
                if(!rdr.IsDBNull(3))
                {
                    this._Description = rdr.GetString(3);
                }
                if(!rdr.IsDBNull(4))
                {
                    this._LeadTimeExpected = rdr.GetDouble(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    this._EarlyStart = rdr.GetDateTime(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    this._LateStart = rdr.GetDateTime(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    this._EarlyFinish = rdr.GetDateTime(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    this._LateFinish = rdr.GetDateTime(8);
                }
                this._Status = rdr.GetChar(9);
            }
            else
            {
                this._CorrectiveActionID = -1;
                this._ImprovementActionID = -1;
                this._ImprovementActionYear = -1;
            }
            rdr.Close();
            conn.Close();
        }

        public void loadTeamMembers()
        {
            this.TeamMembers = new List<CorrectiveActionTeamMember>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT User, role "
                + " FROM correctiveactions_team WHERE CorrectiveActionID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString()
                + " ORDER BY role, user";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.TeamMembers.Add(new CorrectiveActionTeamMember(this.ImprovementActionID, this.ImprovementActionYear, this.CorrectiveActionID, rdr.GetString(0), rdr.GetChar(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean MemberAdd(String username, Char role)
        {
            Boolean ret = false;
            if (this.CorrectiveActionID != -1 && this.ImprovementActionID != -1 && this.ImprovementActionYear != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO CorrectiveActions_Team(CorrectiveActionID, ImprovementActionID, ImprovementActionYear, user, role) "
                    + " VALUES(" + this.CorrectiveActionID.ToString() + ", " + this.ImprovementActionID.ToString() 
                    + ", " + this.ImprovementActionYear.ToString() + ", '"
                    + username + "', '" + role + "')";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + cmd.CommandText;
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean MemberRemove(String username)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM correctiveactions_team WHERE CorrectiveActionID = " + this.CorrectiveActionID.ToString()
                + " AND ImprovementActionID = " + this.ImprovementActionID.ToString() 
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString()
               + " AND user = '" + username + "'";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message + " " + cmd.CommandText;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public void loadTasks()
        {
            this.Tasks = new List<CorrectiveActionTask>();
            if(this.ImprovementActionYear!=-1 && this.ImprovementActionID != -1 && this.CorrectiveActionID!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TaskID, Description, User, Date FROM correctiveactions_tasks WHERE "
                + " ImprovementActionID = " + this.ImprovementActionID.ToString()
                + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString()
                + " AND CorrectiveActionID = " + this.CorrectiveActionID.ToString()
                + " ORDER BY Date";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Tasks.Add(new CorrectiveActionTask(this.ImprovementActionID,
                        this.ImprovementActionYear,
                        this.CorrectiveActionID,
                        rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean TaskAdd(String description, User usr, DateTime date)
        {
            Boolean ret = false;
            if (this.CorrectiveActionID!= -1 && this.ImprovementActionID!=-1 && this.ImprovementActionYear > 1970 &&
                usr != null && usr.username.Length > 0 && date!=null && date > new DateTime(1970, 1, 1))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(TaskID) FROM correctiveactions_tasks WHERE CorrectiveActionID = "
                    + this.CorrectiveActionID.ToString()
                    + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                    + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                int tskID = 0;
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    tskID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                cmd.CommandText = "INSERT INTO CorrectiveActions_Tasks(ImprovementActionID, ImprovementActionYear, "
                    + "CorrectiveActionID, TaskID, Description, User, Date) "
                    + " VALUES(" 
                    + this.ImprovementActionID.ToString() + ", " 
                    + this.ImprovementActionYear.ToString() + ", " 
                    + this.CorrectiveActionID.ToString() + ", "
                    + tskID.ToString() + ", "
                    + "'" + description + "', "
                    + "'" + usr.username + "', "
                    + "'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + cmd.CommandText;
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean TaskRemove(int idTask)
        {
            Boolean ret = false;
            if(this.CorrectiveActionID!=-1 && this.ImprovementActionID!=-1 && this.ImprovementActionYear>1970)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM correctiveactions_tasks WHERE TaskID = " + idTask.ToString()
                    + " AND CorrectiveActionID = " + this.CorrectiveActionID.ToString()
                    + " AND ImprovementActionID = " + this.ImprovementActionID.ToString()
                    + " AND ImprovementActionYear = " + this.ImprovementActionYear.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    this.log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }
    }

    public class CorrectiveActionTeamMember
    {
        public String log;

        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }

        /* E == Executioner --> can create, edit, delete tasks
         * H == Helper --> can help the Executioner, read-only access to information.
         */
        private Char _Role;
        public Char Role
        {
            get
            {
                return this._Role;
            }
        }

        public CorrectiveActionTeamMember(int iActID, int iActYear, int cActID, String iActUser)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ImprovementActionID, ImprovementActionYear, CorrectiveActionID, user, role FROM "
                + " ImprovementActions_Team WHERE ImprovementActionID = " + iActID.ToString()
                + " AND ImprovementActionYear = " + iActYear.ToString()
                + " AND CorrectiveActionID = " + cActID.ToString()
                + " AND user = '" + iActUser + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ImprovementActionID = rdr.GetInt32(0);
                this._ImprovementActionYear = rdr.GetInt32(1);
                this._CorrectiveActionID = rdr.GetInt32(2);
                if (!rdr.IsDBNull(3))
                {
                    this._User = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    this._Role = rdr.GetChar(4);
                }
            }
            rdr.Close();
            conn.Close();
        }

        public CorrectiveActionTeamMember(int iActID, int iActYear, int cActID, String iActUser, Char iActRole)
        {
            this._ImprovementActionID = iActID;
            this._ImprovementActionYear = iActYear;
            this._CorrectiveActionID = cActID;
            this._User = iActUser;
            this._Role = iActRole;
        }
    }

    public class CorrectiveActionTask
    {
        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
            }
        }
        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._Date, fuso.tzFusoOrario);
            }
        }

        public CorrectiveActionTask(int iActID, int iActYear, int cActID, int tskID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Description, User, Date FROM correctiveactions_tasks WHERE "
                + " ImprovementActionID = " + iActID.ToString()
                + " AND ImprovementActionYear = " + iActYear.ToString()
                + " AND CorrectiveActionID = " + cActID.ToString()
                + " AND TaskID = " + tskID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ImprovementActionID = iActID;
                this._ImprovementActionYear = iActYear;
                this._CorrectiveActionID = cActID;
                this._TaskID = tskID;
                if(!rdr.IsDBNull(0))
                {
                    this._Description = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    this._User = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    this._Date = rdr.GetDateTime(2);
                }
            }
            else
            {
                this._ImprovementActionID = -1;
                this._ImprovementActionYear = -1;
                this._CorrectiveActionID = -1;
                this._TaskID = -1;
            }
            conn.Close();
        }

    }

    public class ImprovementActionsEvents
    {
        public List<ImprovementAction> LateImprovementActions;
        public ImprovementActionsEvents()
        {
            this.LateImprovementActions = new List<ImprovementAction>();
        }

        public void loadLateImprovementActions()
        {
            FusoOrario fuso = new FusoOrario();
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year FROM improvementactions WHERE status <> 'C' AND "
                + " CONVERT_TZ(EndDateExpected, '+00:00', '"+tzOffset+"') < '" +
                DateTime.UtcNow.ToString("yyyy-MM-dd") + "' ORDER BY EndDateExpected";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.LateImprovementActions.Add(new ImprovementAction(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        
    }

    public class CorrectiveActionsEvents
    {
        public List<CorrectiveAction> NotFinishedCorrectiveActions;
        public List<CorrectiveAction> NotStartedCorrectiveActions;
        public CorrectiveActionsEvents()
        {
            this.NotStartedCorrectiveActions = new List<CorrectiveAction>();
            this.NotFinishedCorrectiveActions = new List<CorrectiveAction>();
        }

        public void loadNotStartedCorrectiveActions()
        {
            FusoOrario fuso = new FusoOrario();
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;

            this.NotStartedCorrectiveActions = new List<CorrectiveAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, ImprovementActionID, ImprovementActionYear FROM correctiveactions "
                + " WHERE status = 'O' AND CONVERT_TZ(EarlyStart, '+00:00', '"+ tzOffset
                + "') < '" +
                DateTime.UtcNow.ToString("yyyy-MM-dd") + "' AND '" 
                + DateTime.UtcNow.ToString("yyyy-MM-dd") 
                + "' <= CONVERT_TZ(LateFinish, '+00:00', '" + tzOffset + "') ORDER BY LateStart";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.NotStartedCorrectiveActions.Add(new CorrectiveAction(rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadNotFinishedCorrectiveActions()
        {
            FusoOrario fuso = new FusoOrario();
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;

            this.NotFinishedCorrectiveActions = new List<CorrectiveAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, ImprovementActionID, ImprovementActionYear FROM correctiveactions "
                + " WHERE status <> 'C' AND CONVERT_TZ(LateFinish, '+00:00', '" + tzOffset+ "') < '" +
                DateTime.UtcNow.ToString("yyyy-MM-dd") + "' ORDER BY LateFinish";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.NotFinishedCorrectiveActions.Add(new CorrectiveAction(rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    public struct ImprovementActionAnalysisStruct
    {
        public int ID;
        public int Year;
        public DateTime OpeningDate;
        public char Status;
        public DateTime EndDateExpected;
        public DateTime EndDateReal;
    }

    public class ImprovementActionAnalysis
    {
        public List<ImprovementActionAnalysisStruct> IAList;

        public ImprovementActionAnalysis()
        {
            this.IAList = new List<ImprovementActionAnalysisStruct>();
        }

        public void loadIAList()
        {
            this.IAList = new List<ImprovementActionAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Year, OpeningDate, Status, EndDateExpected, EndDateReal From improvementactions";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                ImprovementActionAnalysisStruct curr = new ImprovementActionAnalysisStruct();
                curr.ID = rdr.GetInt32(0);
                curr.Year = rdr.GetInt32(1);
                curr.OpeningDate = rdr.GetDateTime(2);
                curr.Status = rdr.GetChar(3);
                if(!rdr.IsDBNull(4))
                { 
                curr.EndDateExpected = rdr.GetDateTime(4);
                }
                if(!rdr.IsDBNull(5))
                { 
                curr.EndDateReal = rdr.GetDateTime(5);
                }
                this.IAList.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }
    }
}