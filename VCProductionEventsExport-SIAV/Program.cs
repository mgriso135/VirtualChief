using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace VCProductionEventsExport_SIAV
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            String res = ExportEvents("").Result;

            Console.WriteLine(res);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 

            
        }

        public static async Task<string> ExportEvents(String QueryString)
        {
            DateTime start1 = DateTime.UtcNow.AddDays(-50);
            DateTime start2 = new DateTime(start1.Year, start1.Month, start1.Day, 0, 0, 0);
            DateTime end1 = start1.AddDays(1);
            QueryString = "?start=" + start2.ToString("yyyy-MM-dd") + "&end=" + end1.ToString("yyyy-MM-dd");

            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/EventsExport/ExportFinishedTasksEvents";

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "TEST");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}{QueryString}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
