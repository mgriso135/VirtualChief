using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dati;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using WebApplication1.kpi;

namespace WebApplication1
{
    public class macroProcessi
    {
        public processo[] elenco;
        private int _numeroProc;
        public int numeroProc
        {
            get { return _numeroProc; }
        }

        public macroProcessi()
        {
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
                string strSQL = "SELECT COUNT(*) FROM processo WHERE processoPadre = -1";
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                this._numeroProc = mysqlReader.GetInt32(0);
                elenco = new processo[this._numeroProc];
                strSQL = "SELECT processID FROM processo WHERE processoPadre = -1";
                mysqlReader.Close();
                MySqlCommand cmdRead = new MySqlCommand(strSQL, conn);
                mysqlReader = cmdRead.ExecuteReader();
                int i = 0;
                while (i < this._numeroProc && mysqlReader.Read())
                {
                    elenco[i] = new processo(mysqlReader.GetInt32(0));
                    i++;
                }
                mysqlReader.Close();
            conn.Close();            
        }
        
        public bool Add(String nome, String Descr, bool isVSM)
        {
            bool res = false;
            if(nome.Length > 0 && Descr.Length > 0)
            {                
                string strSQL = "SELECT MAX(processID) from processo";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd1.ExecuteReader();
                int maxCod;
                rdr.Read();
                if (!rdr.IsDBNull(0))
                {
                    maxCod = rdr.GetInt32(0) + 1;
                }
                else
                {
                    maxCod = 0;
                }
                strSQL = "INSERT INTO processo(processID, Name, Description, ProcessoPadre, isVSM, posx, posy) " +
                    "VALUES(" + maxCod + ", '" + nome + "', '" + Descr + "', -1, " + isVSM.ToString() + ", 0, 0)";
                MySqlCommand cmd2 = new MySqlCommand(strSQL, conn);
                rdr.Close();
                cmd2.ExecuteNonQuery();
                res = true;
                conn.Close();
            }
            else
            {
                res = false;
            }
            return res;
        }

        public int deleteMacroProcess(int macroProcID)
        {
            int res = 0;
            int found = -1;
            // Check if it is a macroprocess
            for (int i = 0; i < numeroProc; i++)
            {
                if (elenco[i].processID == macroProcID)
                {
                    found = i;
                }
            }
            if (found != -1)
            {
                // Check if it has subprocesses
                if (elenco[found].numSubProcessi == 0)
                {
                    string strSQL = "DELETE FROM processo WHERE processID = " + macroProcID.ToString();
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    res = 1;
                }
                else
                {
                    res = 2;
                }
            }
            return res;
        }
    }
    public class processo
    {
        private int _processID;
        private String _processName;
        private String _processDescription;
        private int[] _processiSucc;
        public int[] processiSucc
        {
            get { return _processiSucc; }
        }

        private int _numProcessiSucc;
        public int numProcessiSucc
        {
            get { return this._numProcessiSucc; }
        }

        private relazione[] _relazioneSucc;
        public relazione[] relazioneSucc
        {
            get { return this._relazioneSucc; }
        }

        private int[] _processiPrec;
        public int[] processiPrec
        {
            get { return _processiPrec; }
        }

        private int _numProcessiPrec;
        public int numProcessiPrec
        {
            get { return this._numProcessiPrec; }
        }

        private relazione[] _relazionePrec;
        public relazione[] relazionePrec
        {
            get { return this._relazionePrec; }
        }

        public processo[] subProcessi;
        private int _numSubProcessi;
        public int numSubProcessi
        {
            get { return this._numSubProcessi; }
        }
        
        private int _processoPadre;
        public int processoPadre { get { return _processoPadre; } }

        public int processID { get { return _processID; } }
        public String processName
        {
            get { return _processName; }
            set
            {
                string strSQL = "UPDATE processo SET Name = '" + value + "' WHERE processID = " + _processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                _processName = value;
                conn.Close();
            }
        }
        public String processDescription {
            get { return _processDescription; }
            set
            {
                string strSQL = "UPDATE processo SET Description = '" + value + "' WHERE processID = " + _processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                _processDescription = value;
                conn.Close();
            }
        }

        private int _posX;
        public int posX
        {
            get { return this._posX; }
            set
            {
                string strSQL = "UPDATE processo SET posx = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                this._posX = value;
                conn.Close();
            }
        }

        private int _posY;
        public int posY
        {
            get { return this._posY; }
            set
            {
                string strSQL = "UPDATE processo SET posy = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                this._posY = value;
                conn.Close();
            }
        }

        public Kpi[] KPIs;
        private int _numKPIs;
        public int numKPIs
        {
            get { return _numKPIs; }
        }

        private bool _isVSM;
        public bool isVSM
        {
            get { return this._isVSM; }
            set
            {
                string strSQL = "UPDATE processo SET isVSM = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                this._isVSM = value;
                conn.Close();
            }
        }

        private User[] _processOwners;
        public User[] processOwners
        {
            get { return this._processOwners; }
        }

        private int _numProcessOwners;
        public int numProcessOwners
        {
            get { return this._numProcessOwners; }
        }

        public String err;

        public processo()
        {
            this._processID = -1;
            this._processName = "NULL";
            this._processDescription = "NULL";
            this.subProcessi = null;
            this._numSubProcessi = 0;
            this._processoPadre = -1;
            this.KPIs = null;
            this._isVSM = false;
            this._numKPIs = 0;
            this._numProcessiPrec = 0;
            this._numProcessiSucc = 0;
            this._posX = 0;
            this._posY = 0;
        }

        public processo(int procID)
        {
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;
            this._numSubProcessi = 0;

            // Carico le informazioni di base del processo.
            string strSQL = "SELECT processo.processID, processo.Name, processo.Description, ProcessoPadre, isVSM, posx, posy FROM processo WHERE processID = " + procID.ToString();
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            bool found = true;
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._processID = procID;
                this._processName = mysqlReader.GetString(1);
                this._processDescription = mysqlReader.GetString(2);
                this._processoPadre = mysqlReader.GetInt32(3);
                if (mysqlReader.GetBoolean(4) == true)
                {
                    this._isVSM = true;
                }
                else
                {
                    this._isVSM = false;
                }
                this._posX = mysqlReader.GetInt32(5);
                this._posY = mysqlReader.GetInt32(6);
            }
            else
            {
                found = false;
            }
            mysqlReader.Close();

            if (found == true)
            {
                // Carico i processi figli
                //this.loadFigli();

                //this.loadPrecedenti();
                //this.loadSuccessivi();

                // Carico i KPIs del processo
                //this.loadKPIs();

                // Carico i process owners
                //this.loadProcessOwners();
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
                this.subProcessi = null;
                this._numSubProcessi = 0;
            }
             conn.Close();
        }

        public bool loadFigli()
        {
            bool rt;
            if (this.processID != -1)
            {
                String strSQL = "SELECT COUNT(*) FROM processo WHERE processoPadre = " + this._processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                this._numSubProcessi = mysqlReader.GetInt32(0);
                mysqlReader.Close();
                this.subProcessi = new processo[this._numSubProcessi];
                strSQL = "SELECT ProcessID FROM Processo " +
                         "WHERE processo.processoPadre = " + this._processID;
                int i = 0;
                cmd = new MySqlCommand(strSQL, conn);
                mysqlReader = cmd.ExecuteReader();
                while (i < this.numSubProcessi && mysqlReader.Read())
                {
                    this.subProcessi[i] = new processo(mysqlReader.GetInt32(0));
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                   /* this.subProcessi[i]._earlyFinishTime = 0.0;
                    this.subProcessi[i]._earlyStartTime = 0.0;
                    this.subProcessi[i]._lateStartTime = 0.0;
                    this.subProcessi[i]._lateFinishTime = 0.0;*/
                    i++;
                }
                mysqlReader.Close();
                conn.Close();
                sortSons();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        // Ordina l'array subProcessi. Funziona nel caso di VSM o SIPOC
        public void sortSons()
        {
            // Utile in caso di Value - Stream. Ricercare il primo e accodare gli altri
            if (isVSM)
            {
                int firstIndex = -1;
                for (int i = 0; i < this._numSubProcessi; i++)
                {
                    if (this.subProcessi[i].numProcessiPrec == 0)
                    {
                        // Questo è il primo.
                        firstIndex = i;
                    }
                }
                // scambio processo di indice 0 con quelli di indice firstIndex.
                if (firstIndex != -1)
                {
                    processo swapProc = this.subProcessi[firstIndex];
                    this.subProcessi[firstIndex] = this.subProcessi[0];
                    this.subProcessi[0] = swapProc;
                    for (int i = 1; i < this.numSubProcessi; i++)
                    {
                        // Trovo dove si trova il successivo dell'i-1 e lo salvo su found
                        int found = -1;
                        for (int j = i; j < this.numSubProcessi; j++)
                        {
                            if (this.subProcessi[i - 1].numProcessiSucc > 0)
                            {
                                if (this.subProcessi[i - 1].processiSucc[0] == this.subProcessi[j].processID)
                                {
                                    found = j;
                                }
                            }

                        }
                        if (found != -1)
                        {
                            swapProc = this.subProcessi[i];
                            this.subProcessi[i] = this.subProcessi[found];
                            this.subProcessi[found] = swapProc;
                        }
                    }
                }
            }
        }

        public bool loadKPIs()
        {
            bool rt;
            if (this.processID != -1)
            {
                String strSQL = "SELECT COUNT(id) FROM kpi_description WHERE idprocesso = " + this.processID.ToString() + " AND attivo = 1";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                this._numKPIs = mysqlReader.GetInt32(0);
                this.KPIs = new Kpi[this.numKPIs];
                mysqlReader.Close();
                strSQL = "SELECT id FROM kpi_description WHERE idprocesso = " + this.processID.ToString() + " AND attivo = 1";
                cmd = new MySqlCommand(strSQL, conn);
                mysqlReader = cmd.ExecuteReader();
                int c = 0;
                while (c < this.numKPIs && mysqlReader.Read())
                {
                    this.KPIs[c] = new Kpi(mysqlReader.GetInt32(0));
                    c++;
                }
                mysqlReader.Close();
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public double getKPIBaseValByName(String kpiName)
        {
            double baseValKPI = 0;
            for (int i = 0; i < this.numKPIs; i++)
            {
                if(kpiName == this.KPIs[i].name)
                {
                    baseValKPI = this.KPIs[i].baseVal;
                }
            }
            return baseValKPI;
        }

        // Carica l'array dei processi precedenti
        public bool loadPrecedenti()
        {
            bool res = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                // Ricerco il numero di processi precedenti
                String strSQL = "SELECT COUNT(precedenzeprocessi.prec) FROM processo INNER JOIN precedenzeprocessi ON (processo.processID = precedenzeprocessi.succ) " +
                    " INNER JOIN relazioniprocessi ON (relazioniprocessi.relazioneID = precedenzeprocessi.relazione) WHERE " +
                    "processo.processID = " + this.processID.ToString();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                if (!mysqlReader.IsDBNull(0))
                {
                    this._numProcessiPrec = mysqlReader.GetInt32(0);
                }
                else
                {
                    this._numProcessiPrec = 0;
                }
                this._processiPrec = new int[this._numProcessiPrec];
                this._relazionePrec = new relazione[this._numProcessiPrec];

                mysqlReader.Close();


                // Carico i processi precedenti
                strSQL = "SELECT precedenzeprocessi.prec, relazioniprocessi.relazioneID FROM processo INNER JOIN precedenzeprocessi ON (processo.processID = precedenzeprocessi.succ) " +
                    " INNER JOIN relazioniprocessi ON (relazioniprocessi.relazioneID = precedenzeprocessi.relazione) WHERE " +
                    "processo.processID = " + this.processID.ToString();
                cmd = new MySqlCommand(strSQL, conn);
                mysqlReader = cmd.ExecuteReader();


                for (int i = 0; i < this._numProcessiPrec && mysqlReader.Read(); i++)
                {
                    this._processiPrec[i] = mysqlReader.GetInt32(0);
                    this._relazionePrec[i] = new relazione(mysqlReader.GetInt32(1));
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        // Carica l'array dei processi successivi
        public bool loadSuccessivi()
        {
            bool res = false;
            if (this._processID != -1)
            {
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                // Ricerco il numero di processi successivi
                String strSQL = "SELECT COUNT(precedenzeprocessi.succ) FROM processo INNER JOIN precedenzeprocessi ON (processo.processID = precedenzeprocessi.prec) " +
                    " INNER JOIN relazioniprocessi ON (relazioniprocessi.relazioneID = precedenzeprocessi.relazione) WHERE " +
                    "processo.processID = " + this.processID.ToString();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                if (!mysqlReader.IsDBNull(0))
                {
                    this._numProcessiSucc = mysqlReader.GetInt32(0);
                }
                else
                {
                    this._numProcessiSucc = 0;
                }
                this._processiSucc = new int[this._numProcessiSucc];
                this._relazioneSucc = new relazione[this._numProcessiSucc];

                mysqlReader.Close();

                // Carico i processi successivi
                strSQL = "SELECT precedenzeprocessi.succ, relazioniprocessi.relazioneID FROM processo INNER JOIN precedenzeprocessi ON (processo.processID = precedenzeprocessi.prec) " +
                    " INNER JOIN relazioniprocessi ON (relazioniprocessi.relazioneID = precedenzeprocessi.relazione) WHERE " +
                    "processo.processID = " + this.processID.ToString();
                cmd = new MySqlCommand(strSQL, conn);
                mysqlReader = cmd.ExecuteReader();


                for (int i = 0; i < this._numProcessiSucc && mysqlReader.Read(); i++)
                {
                    this._processiSucc[i] = mysqlReader.GetInt32(0);
                    this._relazioneSucc[i] = new relazione(mysqlReader.GetInt32(1));
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        // Crea un pocesso di default
        // TO-DO: estendere la funzione per VSM/SIPOC
        public int createDefaultSubProcess()
        {
            
            int res = -1;
            if (this.processID != -1)
            {
                if (this.isVSM)
                {
                    int procID = 0;
                    MySqlConnection mycon = Dati.Dati.mycon();
                    mycon.Open();

                    // Trovo l'ultimo processo
                    string strSELECT = "SELECT MAX(ProcessID) FROM processo";
                    MySqlCommand mysqlCmd = new MySqlCommand(strSELECT, mycon);
                    MySqlDataReader mysqlReader = mysqlCmd.ExecuteReader();
                    mysqlReader.Read();
                    if (!mysqlReader.IsDBNull(0))
                    {
                        procID = mysqlReader.GetInt32(0) + 1;
                    }
                    else
                    {
                        procID = 0;
                    }

                    mysqlReader.Close();

                    string strINSERT = "INSERT INTO processo(ProcessID, Name, Description, ProcessoPadre, isVSM, posx, posy) " +
                        "VALUES(\"" + procID + "\", \"New Default Process\", \"New Default Process Notes\", " + this.processID + ", 0, 0, 0)";

                    MySqlCommand cmd = new MySqlCommand(strINSERT, mycon);
                    cmd.ExecuteNonQuery();

                    // Crea un collegamento con l'ultimo subprocesso, se già presenti
                    this.loadFigli();
                    if (this.numSubProcessi > 1)
                    {
                        int indLast = -1;
                        for (int i = 0; i < this.numSubProcessi; i++)
                        {
                            this.subProcessi[i].loadSuccessivi();
                            if (this.subProcessi[i].numProcessiSucc == 0)
                            {
                                indLast = i;
                            }
                        }
                        if (indLast != -1)
                        {
                            processo corrente = new processo(procID);
                            this.subProcessi[indLast].addProcessoSuccessivo(corrente);
                        }
                        
                        
                    }
                    
                    // Crea i KPI di default!
                    Kpi def = new Kpi();
                    def.add("Cadenza", "Cadenza della stream", new processo(procID), 0);

                    res = procID;
                }
                else
                {
                    // Aggiungi processo di default se trattasi di PERT!

                    // Aggiungo un processo "orfano", poi bisognerà linkarlo!

                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    String strSQL = "SELECT MAX(processID) FROM processo";
                    MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd1.ExecuteReader();
                    rdr.Read();
                    int newProcID = 0;
                    if (!rdr.IsDBNull(0))
                    {
                        newProcID = rdr.GetInt32(0) + 1;
                    }
                    rdr.Close();
                    strSQL = "INSERT INTO processo(ProcessID, Name, Description, ProcessoPadre, isVSM, posx, posy) " +
                        "VALUES(\"" + newProcID.ToString() + "\", \"New Default Process\", \"New Default Process Notes\", " + this.processID + ", 0, 100, 100)";
                    cmd1 = new MySqlCommand(strSQL, conn);
                    cmd1.ExecuteNonQuery();
                    conn.Close();
                    res = newProcID;

                    // Crea i KPI di default!
                    Kpi def = new Kpi();
                    def.add("Tempo ciclo", "Tempo ciclo del processo", new processo(newProcID), 0);
                }
            }

            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if process deleted
         * 2 if process not deleted because of sub-processes or because of kpis
         * 3 if process is not deleted because of multi-precedent processes
         */
        // To-do: cancellare i PERMESSI associati al processo
       public int delete()
       {
           int res = 1;
           if (this.processID != -1)
           {
               if (this.numSubProcessi == 0 && this.numKPIs == 0)
               {
                   if (this.numProcessiPrec == 0)
                   {
                       // E' il primo processo. Cancello il precedente dei successivi ed elimino il processo
                       MySqlConnection conn = Dati.Dati.mycon();
                       String strSQL = "DELETE FROM precedenzeprocessi WHERE prec = " + this.processID.ToString();
                       conn.Open();
                       MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();
                       strSQL = "DELETE FROM processo WHERE processID = " + this.processID.ToString();
                       cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();
                       conn.Close();
                   }
                   else if (this.numProcessiPrec == 1 && this.numProcessiSucc == 1)
                   {
                       // E' un processo in mezzo allo stream.

                       // Collego precedente e successivo
                       string strSQL = "INSERT INTO precedenzeprocessi(prec, succ, relazione) VALUES(" + this.processiPrec[0].ToString() + "," + this.processiSucc[0].ToString() + ", 0)";
                       MySqlConnection conn = Dati.Dati.mycon();
                       conn.Open();
                       MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();

                       // Elimino i legami dei processi precedente e successivo con quello da cancellare
                       strSQL = "DELETE FROM precedenzeprocessi WHERE prec = " + this.processID.ToString() + " OR succ = " + this.processID.ToString();
                       cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();

                       // Elimino il processo.
                       strSQL = "DELETE FROM processo WHERE processID = " + this.processID.ToString();
                       cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();
                       conn.Close();
                   }
                   else if (this.numProcessiSucc == 0)
                   {
                       // E' l'ultimo processo.
                       MySqlConnection conn = Dati.Dati.mycon();
                       conn.Open();
                       String strSQL = "DELETE FROM precedenzeprocessi WHERE succ = " + this.processID.ToString();
                       MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();

                       // Elimino il processo.
                       strSQL = "DELETE FROM processo WHERE processID = " + this.processID.ToString();
                       cmd = new MySqlCommand(strSQL, conn);
                       cmd.ExecuteNonQuery();
                       conn.Close();
                   }
                   else
                   {
                       res = 3;
                   }
               }
               else
               {
                   res = 2;
               }

           }
           else
           {
               res = 0;
           }
           return res;
       }

        /* Returns:
         * 0 if this is not loaded or if this has no precedent processes
         * 1 if relation has been correctly changed
         */
        public bool changeRelationPrec(processo precedente, relazione rel)
        {
            bool res = false;
            if (this.processID != -1 && this.numProcessiPrec >= 1)
            {
                string strSQL = "UPDATE precedenzeprocessi SET relazione = " + rel.relationID + " WHERE succ = " + this.processID.ToString() + " AND prec = " + precedente.processID.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                cmd1.ExecuteNonQuery();
                res = true;
            }
            return res;
        }

        public bool addKPI(String nomeKPI, String descrKPI)
        {
            Kpi tmp = new Kpi();
            return tmp.add(nomeKPI, descrKPI, new processo(this.processID), 0);            
        }

        /* Returns:
         * 1 if next process correctly added
         * 0 if error
         */
        public bool addProcessoSuccessivo(processo next)
        {
            bool res = false;
            if(this.processID != -1)
            {
                String strSQL = "INSERT INTO precedenzeprocessi(prec, succ, relazione) VALUES(" + this.processID.ToString() + ", " + next.processID.ToString() + ", 0)";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                loadSuccessivi();
                res = true;
            }
            return res;
        }

        /* Returns:
         * 1 if precedent process correctly added
         * 0 if error
         */
        public bool addProcessoPrecedente(processo preced)
        {
            bool res = false;
            if (this.processID != -1)
            {
                String strSQL = "INSERT INTO precedenzeprocessi(prec, succ, relazione) VALUES(" + preced.processID.ToString() + ", " + this.processID.ToString() + ", 0)";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                res = true;
                loadPrecedenti();
            }
            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if link between processes is deleted successfully
         * 2 if "next" process remains orphan (nothing will be deleted)
         */
        public int deleteProcessoSuccessivo(processo next)
        {
            int res = 0;
            if (this.processID != -1)
            {
                // Look if next è un processo successivo rispetto a this.
                bool found = false;
                this.loadSuccessivi();
                this.loadPrecedenti();
                next.loadPrecedenti();
                next.loadSuccessivi();
                for (int i = 0; i < this.numProcessiSucc; i++)
                {
                    if (this.processiSucc[i] == next.processID)
                    {
                        found = true;
                    }
                }

                if (found == true)
                {
                    if (next.numProcessiSucc > 0 || next.numProcessiPrec > 0)
                    {
                        // Se il processo successivo non rimane orfano cancello il legame con this.
                        MySqlConnection conn = Dati.Dati.mycon();
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE prec = " + this.processID.ToString() + " AND succ = " + next.processID.ToString();
                        MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        res = 1;
                        loadSuccessivi();
                    }
                    else
                    {
                        // Se il processo successivo rimane orfano non cancello nulla e lo segnalo
                        res = 2;
                    }
                }
                else
                {
                    res = 0;
                }
            }
            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if link between processes is deleted successfully
         * 2 if "next" process remains orphan (nothing will be deleted)
         */
        public int deleteProcessoPrecedente(processo preced)
        {
            int res = 0;
            if (this.processID != -1)
            {
                this.loadPrecedenti();
                this.loadSuccessivi();
                preced.loadPrecedenti();
                preced.loadSuccessivi();
                // Look if next è un processo successivo rispetto a this.
                bool found = false;
                for (int i = 0; i < this.numProcessiPrec; i++)
                {
                    if (this.processiPrec[i] == preced.processID)
                    {
                        found = true;
                    }
                }

                if (found == true)
                {
                    if (preced.numProcessiPrec > 0 || preced.numProcessiSucc > 0)
                    {
                        // Se il processo successivo non rimane orfano cancello il legame con this.
                        MySqlConnection conn = Dati.Dati.mycon();
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE succ = " + this.processID.ToString() + " AND prec = " + preced.processID.ToString();
                        MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        res = 1;
                        loadPrecedenti();
                    }
                    else
                    {
                        // Se il processo successivo rimane orfano non cancello nulla e lo segnalo
                        res = 2;
                    }
                }
                else
                {
                    res = 0;
                }
            }
            return res;
        }

        public bool isPrecedente(processo curr)
        {
            bool found = false;
            this.loadPrecedenti();
            for (int i = 0; i < this.numProcessiPrec && found == false; i++)
            {
                if (this.processiPrec[i] == curr.processID)
                {
                    found = true;
                }
                else
                {
                    processo prec = new processo(this.processiPrec[i]);
                    found = prec.isPrecedente(curr);
                }
            }
            return found;
            
        }

        public bool isSuccessivo(processo curr)
        {
            bool found = false;
            this.loadSuccessivi();
            for (int i = 0; i < this.numProcessiSucc && found == false; i++)
            {
                if (this.processiSucc[i] == curr.processID)
                {
                    found = true;
                }
                else
                {
                    processo succ = new processo(this.processiSucc[i]);
                    found = succ.isSuccessivo(curr);
                }
            }
            return found;

        }

        public bool loadProcessOwners()
        {
            bool rt;
            if (this.processID != -1)
            {
                this._numProcessOwners = 0;
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();

                // trovo il numero di processowners
                String strSQL = "SELECT COUNT(users.userID) FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = " + this.processID.ToString();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                this._numProcessOwners = rdr.GetInt32(0);
                this._processOwners = new User[this._numProcessOwners];
                rdr.Close();

                // creo l'istanza degli utenti
                strSQL = "SELECT processo.ProcessID, users.userID FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = " + this.processID.ToString();
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                for (int i = 0; i < this.numProcessOwners && rdr.Read(); i++)
                {
                    this._processOwners[i] = new User(rdr.GetString(1));
                }
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool deleteProcessOwner(User currProcOwner)
        {
            bool rt = true;
            if (this.processID != -1)
            {
                String strSQL = "DELETE FROM processOwners WHERE process = " + this.processID.ToString() + " AND user = '" + currProcOwner.username + "'";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool addProcessOwner(User newProcOwner)
        {
            bool rt = true;
            if (this.processID != -1)
            {
                // Controllo se l'utente da aggiungere è già presente o meno
                this.loadProcessOwners();
                for (int i = 0; i < this.numProcessOwners; i++)
                {
                    if (newProcOwner.username == this.processOwners[i].username)
                    {
                        rt = false;
                    }
                }

                // Se l'utente da aggiungere non è già presente lo aggiungo
                if (rt == true)
                {
                    String strSQL = "INSERT INTO processOwners(process, user) VALUES(" + this.processID + ", '" + newProcOwner.username + "')";
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this._processOwners = null;
                    this._numProcessOwners = 0;
                    this.loadProcessOwners();
                }
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        /*Returns:
         * 0 if generic error
         * 1 if all ok
         * 2 if some tasks have not following tasks or precedences.
         */
        public int checkConsistency()
        {
            int rt = 1;
            if (this.processID != -1)
            {
                // Controllo che tutti i processi figli del task abbiano almeno un precedente oppure un successivo
                this.loadFigli();
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                    if (this.subProcessi[i].numProcessiPrec == 0 && this.subProcessi[i].numProcessiSucc == 0)
                    {
                        rt = 2;
                    }
                }
            }
            else
            {
                rt = 0;
            }
            return rt;
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is fine
         * 2 if there is some task without link between other tasks
         * 3 if diagram type is NOT Pert
         * 4 if this task is missing Kpi called "Cadenza"
         * 5 if some subtask is missing Kpi called "Tempo ciclo"
         * 6 if some subtask is missing process owner
         */
        public int checkConsistencyPERT()
        {
            int rt;
            rt = this.checkConsistency();
            if (this.processID != -1 && rt == 1)
            {
                // Controllo che il task sia effettivamente un PERT
                if (this.isVSM == true)
                {
                    rt = 3;
                }
                // Controllo che tutti i subtasks abbiano un KPI chiamato "Tempo ciclo"
                if (rt == 1)
                {
                    this.loadFigli();
                    for (int i = 0; i < this.numSubProcessi && rt == 1; i++)
                    {
                        this.subProcessi[i].loadKPIs();
                        if (this.subProcessi[i].getKPIBaseValByName("Tempo ciclo") == 0.0)
                        {
                            //return subProcessi[i].processID;
                            rt = 5;
                        }
                    }
                }

                // Controllo che il padre abbia un KPI chiamato cadenza

                this.loadKPIs();
                if (rt == 1 && this.getKPIBaseValByName("Cadenza") == 0.0)
                {
                    rt = 4;
                }

                // Controllo che tutti i subtasks abbiano almeno un process owner
                if (rt == 1)
                {
                    for (int i = 0; i < this.numSubProcessi; i++)
                    {
                        this.subProcessi[i].loadProcessOwners();
                        if (this.subProcessi[i].numProcessOwners == 0)
                        {
                            rt = 6;
                        }
                    }
                }
            }
            return rt;
        }

        private double _earlyStartTime;
        public double earlyStartTime
        {
            get { return this._earlyStartTime; }
        }

        private double _earlyFinishTime;
        public double earlyFinishTime
        {
            get { return this._earlyFinishTime; }
        }

        private double _lateStartTime;
        public double lateStartTime
        {
            get
            {
                return this._lateStartTime;
            }
        }

        private double _lateFinishTime;
        public double lateFinishTime
        {
            get { return this._lateFinishTime; }
        }

        public List<processo> CriticalPath;

        public void calculateCriticalPath()
        {
            if (this.checkConsistencyPERT() == 1)
            {
                // Carico i processi figli e i loro legami di precedente / successivo
                this.loadFigli();
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                }

                // Ricerco i task capostipite e calcolo il loro earlyStartTime ed earlyFinishTime
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    if (this.subProcessi[i].numProcessiPrec == 0)
                    {
                        this.subProcessi[i].loadKPIs();
                        this.subProcessi[i]._earlyStartTime = 0.0;
                        this.subProcessi[i]._earlyFinishTime = this.subProcessi[i].getKPIBaseValByName("Tempo ciclo");
                        // Ora calcolo earlyStartTime e earlyFinishTime per i loro successivi, fino alla fine!!!
                        this.calculateEarlyTimesforSucc(this.subProcessi[i].processID, this.subProcessi[i]._earlyFinishTime);
                    }
                }

                // Ricerco i task finali e calcolo il loro lateStartTime e lateFinishTime
                // Per farlo devo trovare il task finale con il massimo earlyFinishTime e impostare lateFinishTime = max(earllyFinishTime)
                
                double maxEarlyFinishTime = 0.0;
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    if (this.subProcessi[i].numProcessiSucc == 0)
                    {
                        if (this.subProcessi[i].earlyFinishTime > maxEarlyFinishTime)
                        {
                            maxEarlyFinishTime = this.subProcessi[i].earlyFinishTime;
                        }
                    }
                }

                // Inoltre inizializzo tutti quanti i task a maxEarlyFinishTime + 1
                for(int i = 0; i < this.numSubProcessi; i++)
                {
                    this.subProcessi[i]._lateFinishTime = maxEarlyFinishTime + 1;
                }

                // Ora calcolo il lateFinishTime e il lateStartTime per i processi finali!
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    if (this.subProcessi[i].numProcessiSucc == 0)
                    {
                        this.subProcessi[i]._lateFinishTime = maxEarlyFinishTime;
                        this.subProcessi[i]._lateStartTime = this.subProcessi[i]._lateFinishTime - this.subProcessi[i].getKPIBaseValByName("Tempo ciclo");
                        calculateLateTimesforPrec(this.subProcessi[i].processID, this.subProcessi[i].lateStartTime);
                    }
                }

                // Ora posso raccogliere le attività critiche
                // Condizione: dove data minima inizio == data massima inizio && data minima fine == data massima fine
                int numProcCritical = 0;
                CriticalPath = new List<processo>();
                for (int i = 0; i < this.numSubProcessi; i++)
                {
                    if (this.subProcessi[i].earlyStartTime == this.subProcessi[i].lateStartTime && this.subProcessi[i].earlyFinishTime == this.subProcessi[i].lateFinishTime)
                    {
                        CriticalPath.Add(this.subProcessi[i]);
                        numProcCritical++;
                    }
                }

                // Ordino l'array per "LateStartTime"
                for (int j = 0; j < CriticalPath.Count - 1; j++)
                {
                    processo temp;
                    int pos_min = j;
                    for (int i = j + 1; i < CriticalPath.Count; i++)
                    {
                        if (CriticalPath[pos_min].lateStartTime > CriticalPath[i].lateStartTime)
                        {
                            pos_min = i;
                        }
                    }
                    if (pos_min != j)
                    {
                        temp = CriticalPath[j];
                        CriticalPath[j] = CriticalPath[pos_min];
                        CriticalPath[pos_min] = temp;
                    }
                }
            }
        }

        private void calculateEarlyTimesforSucc(int prcId, double finish)
        {
            int procIndex = -1;
            for (int i = 0; i < this.numSubProcessi; i++)
            {
                if (this.subProcessi[i].processID == prcId)
                {
                    procIndex = i;
                }
            }

            this.subProcessi[procIndex].loadSuccessivi();
            for (int i = 0; i < this.subProcessi[procIndex].numProcessiSucc; i++)
            {
                // Cerco l'indice cazzo
                int index = -1;
                for(int j = 0; j < this.numSubProcessi; j++)
                {
                    if (this.subProcessi[procIndex].processiSucc[i] == this.subProcessi[j].processID)
                    {
                        index = j;
                    }
                }

                // Ora andiamo a calcolare earlyStart e earlyFinish
                this.subProcessi[index].loadKPIs();
                if (this.subProcessi[index]._earlyStartTime < finish)
                {
                    this.subProcessi[index]._earlyStartTime = finish;
                }
                if (this.subProcessi[index]._earlyFinishTime < finish + this.subProcessi[index].getKPIBaseValByName("Tempo ciclo"))
                {
                    this.subProcessi[index]._earlyFinishTime = finish + this.subProcessi[index].getKPIBaseValByName("Tempo ciclo");
                }
                calculateEarlyTimesforSucc(this.subProcessi[index].processID, this.subProcessi[index]._earlyFinishTime);

            }
        }

        private void calculateLateTimesforPrec(int prcId, double start)
        {
            int procIndex = -1;
            for (int i = 0; i < this.numSubProcessi; i++)
            {
                if (this.subProcessi[i].processID == prcId)
                {
                    procIndex = i;
                }
            }

            this.subProcessi[procIndex].loadPrecedenti();
            for (int i = 0; i < this.subProcessi[procIndex].numProcessiPrec; i++)
            {
                // Trovo l'indice del precedente
                int index = -1;
                for(int j = 0; j < this.numSubProcessi; j++)
                {
                    if (this.subProcessi[j].processID == this.subProcessi[procIndex].processiPrec[i])
                    {
                        index = j;
                    }
                }
                if (index != -1)
                {
                    this.subProcessi[index].loadKPIs();
                    if (this.subProcessi[index]._lateFinishTime > start)
                    {
                        this.subProcessi[index]._lateFinishTime = start;
                    }
                    this.subProcessi[index]._lateStartTime = this.subProcessi[index]._lateFinishTime - this.subProcessi[index].getKPIBaseValByName("Tempo ciclo");
                    calculateLateTimesforPrec(this.subProcessi[index].processID, this.subProcessi[index]._lateStartTime);
                }
                
            }
        }
    }
}