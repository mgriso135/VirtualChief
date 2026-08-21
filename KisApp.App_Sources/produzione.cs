using System.Collections.Generic;
using MySqlConnector;

namespace KisApp.App_Sources
{
    public class produzione
    {
        protected string Tenant;

        public produzione(string tenant)
        {
            Tenant = tenant;
        }

        public List<Articolo> GetAllArticoli()
        {
            var result = new List<Articolo>();

            using (var conn = new MySqlConnection("Server=localhost;Database=virtualchief;User=root;"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM articoli WHERE tenant = @tenant";
                cmd.Parameters.AddWithValue("@tenant", Tenant);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Articolo
                    {
                        Matricola = reader["matricola"]?.ToString(),
                        Descrizione = reader["descrizione"]?.ToString(),
                        Status = char.Parse(reader["status"]?.ToString() ?? " ")
                    });
                }
            }

            return result;
        }
    }

    public class Articolo
    {
        public string Matricola { get; set; }
        public string Descrizione { get; set; }
        public char Status { get; set; }
        public string Tenant { get; set; }
    }
}