using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VCAlarmsEvents
{
    class Program
    {
        static async Task MainAsync(string[] args)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3358/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method  
                HttpResponseMessage response = await client.GetAsync("DelaysAlarm/Main");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Delays alarms correctly sent");
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }

        }
    }
}
