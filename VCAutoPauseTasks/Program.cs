using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;

namespace VCAutoPauseTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AutoPauseTasks...\n");
            var ret = AutoPauseTasks();
            Console.WriteLine(ret.Result);
            Console.ReadLine();
        }

        public static async Task<string> AutoPauseTasks()
        {
            //DateTime start1 = DateTime.UtcNow.AddDays(-2);
            DateTime start2 = DateTime.UtcNow.AddDays(-2);
            //DateTime end1 = start1.AddDays(1);
            DateTime end1 = DateTime.UtcNow.AddDays(-1);

            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/AutoPauseTasks/AutoPauseTasks";

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "PlO9qMULFJOPKJ6D");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
