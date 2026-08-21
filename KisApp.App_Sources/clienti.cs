using System.Collections.Generic;
using MySqlConnector;

namespace KisApp.App_Sources
{
    public class clienti
    {
        protected string Tenant;

        public clienti(string tenant)
        {
            Tenant = tenant;
        }

        public List<Cliente> GetAllClients()
        {
            var result = new List<Cliente>();

            using (var conn = new MySqlConnection("Server=localhost;Database=virtualchief;User=root;"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM anagraficaclienti WHERE tenant = @tenant";
                cmd.Parameters.AddWithValue("@tenant", Tenant);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Cliente
                    {
                        CodiceCliente = reader["codice"]?.ToString(),
                        RagioneSociale = reader["ragsociale"]?.ToString(),
                        PartitaIva = reader["partitaiva"]?.ToString(),
                        CodiceFiscale = reader["codicefiscale"]?.ToString()
                    });
                }
            }

            return result;
        }
    }

    public class Cliente
    {
        public string CodiceCliente { get; set; }
        public string RagioneSociale { get; set; }
        public string PartitaIva { get; set; }
        public string CodiceFiscale { get; set; }
        public string Tenant { get; set; }
    }
}