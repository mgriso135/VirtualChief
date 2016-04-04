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

        public static string GetConnectionString()
        {
            string connStr = String.Format("server={0};user id={1}; password={2};database=master; pooling=false", "127.0.0.1", "matteo", "pippo");

            return connStr;
        }

        public static MySql.Data.MySqlClient.MySqlConnection mycon()
        {
            return new MySqlConnection(GetConnectionString());
        }
    }
}