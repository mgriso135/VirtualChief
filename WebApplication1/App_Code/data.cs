using System;
using System.IO;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;


/// <summary>
/// Descrizione di riepilogo per Class1
/// </summary>

namespace Dati
{
    public class Dati
    {
        public Dati()
        {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
        }

        public string GetConnectionString()
        {
            string connStr = String.Format(System.Configuration.ConfigurationManager.ConnectionStrings["masterDB"].ConnectionString);
            //string connStr = String.Format("server={0};user id={1}; password={2};database=sitecal; pooling=false", "127.0.0.1", "SITECAL", "sitecalKIS");
            return connStr;
        }

        public MySql.Data.MySqlClient.MySqlConnection mycon()
        {
            return new MySqlConnection(GetConnectionString());
        }
    }
}