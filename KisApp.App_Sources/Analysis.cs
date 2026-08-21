using System.Collections.Generic;
using MySqlConnector;

namespace KisApp.App_Sources
{
    public class Analysis
    {
        protected string Tenant;

        public Analysis(string tenant)
        {
            Tenant = tenant;
        }

        public List<ProductionAnalysisStruct> loadProductionAnalysis()
        {
            var result = new List<ProductionAnalysisStruct>();

            using (var conn = new MySqlConnection("Server=localhost;Database=virtualchief;User=root;"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM productionanalysis WHERE tenant = @tenant";
                cmd.Parameters.AddWithValue("@tenant", Tenant);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new ProductionAnalysisStruct
                    {
                        CustomerName = reader["customername"]?.ToString(),
                        ProductName = reader["productname"]?.ToString(),
                        Quantity = reader.IsDBNull(reader.GetOrdinal("quantity")) ? 0 : Convert.ToDecimal(reader["quantity"]),
                        UnitPrice = reader.IsDBNull(reader.GetOrdinal("unitprice")) ? 0 : Convert.ToDecimal(reader["unitprice"]),
                        TotalPrice = reader.IsDBNull(reader.GetOrdinal("totalprice")) ? 0 : Convert.ToDecimal(reader["totalprice"])
                    });
                }
            }

            return result;
        }
    }

    public struct ProductionAnalysisStruct
    {
        public string CustomerName;
        public string ProductName;
        public decimal Quantity;
        public decimal UnitPrice;
        public decimal TotalPrice;
    }
}