using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace WebApplication1
{
    public class produzione
    {
        private int _mainProcess;
        public int mainProcess
        {
            get { return this._mainProcess; }
        }

        private prodotto[] _queueBatches;
        public prodotto[] queueBatches
        {
            get
            {
                return this._queueBatches;
            }
        }
        private int _numQueueBatches;
        public int numQueueBatches
        {
            get { return this._numQueueBatches; }
        }
        public produzione()
        {
            _mainProcess = -1;
            _queueBatches = null;
        }

        public produzione(int mainProcID)
        {
            this._mainProcess = mainProcID;
            String strSQL = "SELECT COUNT(*) FROM productionPlan WHERE processo = " + mainProcID.ToString() + " ORDER BY cadenzaIniziale";
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._numQueueBatches = rdr.GetInt32(0);
            }
            else
            {
                this._numQueueBatches = 0;
            }
            rdr.Close();
            this._queueBatches = new prodotto[this._numQueueBatches];

            strSQL = "SELECT matricola FROM productionPlan WHERE processo = " + mainProcID.ToString() + " ORDER BY cadenzaIniziale";
            cmd = new MySqlCommand(strSQL, conn);
            rdr = cmd.ExecuteReader();
            int i = 0;
            while (rdr.Read() && i < this.numQueueBatches)
            {
                this._queueBatches[i] = new prodotto(rdr.GetString(0));
                i++;
            }
            rdr.Close();

            // Ora ricerco lo status della cadenza e carico la data dell'ultimo evento registrato
            strSQL = "SELECT evento, ora FROM eventicadenza WHERE processo = " + mainProcID.ToString() + " ORDER BY ora";
            cmd = new MySqlCommand(strSQL, conn);
            rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._statusCadenza = rdr.GetChar(0);
                this._lastEventCadenza = rdr.GetDateTime(1);
            }
            else
            {
                this._statusCadenza = 'N';
            }
            rdr.Close();

            conn.Close();
        }
    
        /* Returns:
         * 0 if some error
         * 1 if product correctly added
         */
        public bool addProduct(String matr)
        {
            bool rt = false;
            if(mainProcess!= -1)
            {
                // Aggiungo il prodotto al batch!
                int primaCadenzaDisponibile = 1;
                if (this.numQueueBatches > 0)
                {
                    primaCadenzaDisponibile = this.queueBatches[this.numQueueBatches - 1].cadenzaIniziale + 1;
                }
                String strSQL = "INSERT INTO ProductionPlan(processo, matricola, cadenzainiziale, status) VALUES(" + this.mainProcess.ToString() + ", '" + matr +
                    "', " + primaCadenzaDisponibile.ToString() + ", 'N')";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();

                // Ora casino: copio i sub task del processo originale nei task del piano di produzione cambiandone il codice e tenendo le relazioni originali
                processo current = new processo(this.mainProcess);
                if (current.processID != -1 && current.checkConsistencyPERT() == 1)
                {
                    current.loadKPIs();
                    current.loadFigli();
                    for (int i = 0; i < current.numSubProcessi; i++)
                    {
                        current.subProcessi[i].loadPrecedenti();
                        current.subProcessi[i].loadSuccessivi();
                        current.subProcessi[i].loadKPIs();
                        
                    }
                    current.calculateCriticalPath();
                
                    // Genero dei task ad hoc per la matricola macchina copiandoli dal processo modello
                    for (int i = 0; i < current.numSubProcessi; i++)
                    {
                        // Copio il task
                        String strTask = "SELECT MAX(taskID) FROM tasksproduzione";
                        MySqlCommand cmdTask = new MySqlCommand(strTask, conn);
                        MySqlDataReader rdrTask = cmdTask.ExecuteReader();
                        int max = 0;
                        if (rdrTask.Read() && !rdrTask.IsDBNull(0))
                        {
                            max = rdrTask.GetInt32(0) + 1;
                        }
                        rdrTask.Close();

                        // Calcolo la cadenza prevista per il task
                        
                        double lateStart = current.subProcessi[i].lateStartTime;
                        double floatCadenzaCurrent = primaCadenzaDisponibile + ((lateStart + current.subProcessi[i].getKPIBaseValByName("Tempo ciclo")) / current.getKPIBaseValByName("Cadenza"));
                        int cadenzaCurrent = Convert.ToInt32(floatCadenzaCurrent);
                        current.subProcessi[i].loadProcessOwners();
                        strTask = "INSERT INTO tasksproduzione(taskID, matricola, name, description, padre, cadenza, origTask, processOwner) VALUES (" +
                            max.ToString() + ", '" + matr + "', '" + current.subProcessi[i].processName + "', '" + current.subProcessi[i].processDescription 
                            + "', " + current.subProcessi[i].processoPadre + ", " + cadenzaCurrent + ", " + current.subProcessi[i].processID.ToString() 
                            + ", '" + current.subProcessi[i].processOwners[0].username + "')";
                        cmdTask = new MySqlCommand(strTask, conn);
                        cmdTask.ExecuteNonQuery();                        
                    }

                    // Ed ora copio le relazioni tra i task, partendo dal modello iniziale

                    // Step 1: creo una query dove siano coinvolti i task appena creati
                    String strTaskRel = "SELECT precedenti.taskID, successivi.taskID, precedenzeprocessi.relazione FROM tasksproduzione AS precedenti " +
                        " INNER JOIN precedenzeprocessi ON (precedenzeprocessi.prec = precedenti.origTask) INNER JOIN tasksproduzione AS successivi " +
                        "ON (successivi.origTask = precedenzeprocessi.succ) WHERE precedenti.matricola = '" + matr + "' AND successivi.matricola = '" + matr + "'";
                    MySqlCommand cmdTaskR = new MySqlCommand(strTaskRel, conn);
                    MySqlDataReader rdrRelTasks = cmdTaskR.ExecuteReader();
                    while (rdrRelTasks.Read())
                    {
                        int prec = rdrRelTasks.GetInt32(0);
                        int succ = rdrRelTasks.GetInt32(1);
                        int rel = rdrRelTasks.GetInt32(2);
                        String addRel = "INSERT INTO prectasksproduzione(prec, succ, relazione) VALUES(" + prec.ToString() + ", " + succ.ToString() +
                         ", " + rel.ToString() + ")";
                        MySqlConnection conn2 = Dati.Dati.mycon();
                        conn2.Open();
                        MySqlCommand cmdAdd = new MySqlCommand(addRel, conn2);
                        cmdAdd.ExecuteNonQuery();
                        conn2.Close();
                    }
                    rdrRelTasks.Close();
                }
                conn.Close();
                rt = true;
            }
            return rt;
        }

        /* STATUS CADENZE:
         * N se mai iniziata
         * I cadenza iniziata "I"
         * P cadenza in pausa "P"
         * F cadenza terminata "F"
         */
        public bool startCadenza()
        {
            bool rt = false;
            if(this.mainProcess != -1)
            {
                String strSQL = "SELECT evento, cadenza FROM eventicadenza WHERE processo = " + this.mainProcess.ToString() + " ORDER BY ora DESC";
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int cadenza = -1;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    if(rdr.GetChar(0) != 'I')
                    {
                        cadenza = rdr.GetInt32(1);
                    }
                }
                else
                {
                    cadenza = 1;
                }
                rdr.Close();
                if (cadenza != -1)
                {
                    strSQL = "INSERT INTO eventicadenza(processo, cadenza, evento, ora) VALUES (" + this.mainProcess.ToString() + ", " + cadenza +
                    ", 'I', '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "')";
                    cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            return rt;
        }

        public bool stopCadenza()
        {
            return false;
        }

        private char _statusCadenza;
        public char statusCadenza
        {
            get { return _statusCadenza; }
        }

        private DateTime _lastEventCadenza;
        public DateTime lastEventCadenza
        {
            get { return _lastEventCadenza; }
        }
    }

    public class prodotto
    {
        private String _matricola;
        public String matricola
        {
            get { return this._matricola; }
        }
        public processo tasks;
        private int _cadenzaIniziale;
        public int cadenzaIniziale
        {
            get { return this._cadenzaIniziale; }
        }

        private char _status;
        public char status
        {
            get { return _status; }
        }
        public prodotto()
        {
            tasks = null;
            this._matricola = "";
            this._cadenzaIniziale = -1;
        }

        public prodotto(String prodID)
        {
            String strSQL = "SELECT processo, matricola, cadenzaIniziale, status FROM productionPlan WHERE matricola LIKE '" + prodID + "'";
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._matricola = rdr.GetString(1);
                this.tasks = new processo(rdr.GetInt32(0));
                this._cadenzaIniziale = rdr.GetInt32(2);
                this._status = rdr.GetChar(3);
            }
            else
            {
                this._matricola = "";
                tasks = null;
                this._cadenzaIniziale = -1;
            }

            conn.Close();
        }
    }
}