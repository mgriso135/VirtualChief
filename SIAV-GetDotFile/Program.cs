using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

namespace SIAV_GetDotFile
{
    class Program
    {
        static void Main(string[] args)
        {
            String token = getToken().Result;
            Console.WriteLine("Token: " + token.ToString());

            if(token.Length>0)
            {
                var basepath = ConfigurationManager.AppSettings["basepath"];
                Console.WriteLine(ConfigurationManager.AppSettings["products"]);
                var arrPids = (ConfigurationManager.AppSettings["products"]).ToString().Split(';');
                foreach(var m in arrPids)
                { 
                    var dotFile = getDotData(m.ToString(), token).Result;
                    Console.Write(dotFile.ToString());
                    File.WriteAllText(basepath + m.ToString() + ".dot", dotFile.ToString());

                    var frequencyGraph = getFrequencyGraph(m.ToString(), token).Result;
                    Console.Write(frequencyGraph.ToString());
                    File.WriteAllText(basepath + m.ToString() + "_FREQUENCY.dot", frequencyGraph.ToString());
                }
            }
            else
            {
                Console.WriteLine("Token not found");
            }

        }

        public static async Task<string> getToken()
        {
            // Add the certificate
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            if (cert != null)
            {
                handler.ClientCertificates.Add(cert);
            

            HttpClient client = new HttpClient(handler);
            Uri u = new Uri("https://tstdemocpm.siav.net:9443/login");
            var payload = "{\"username\": \"kaizenkey\",\"password\": \"kk\"}";
            var c = new StringContent(payload, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("username", "kaizenkey");
            client.DefaultRequestHeaders.Add("password", "kk");
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            using (var result = await client.PostAsync(u, c))
            {
                string content = await result.Content.ReadAsStringAsync();
                Console.Write("Content: " + content);
                var jsontoken = JsonConvert.DeserializeObject<AuthenticationStruct>(content.ToString());
                Console.WriteLine("Token: " + jsontoken.access_token);
                return jsontoken.access_token;
            };

            }
            else
            {
                return "";
            }

        }
        
        public static async Task<string> getDotData(String processID, String token)
        {
            // Add the certificate
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            if (cert != null)
            {
                Console.WriteLine("Go Get Performance Chart!");
                handler.ClientCertificates.Add(cert);

                HttpClient client = new HttpClient(handler);
                var payload = "{\"username\": \"kaizenkey\",\"password\": \"kk\"}";
                var c = new StringContent(payload, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("username", "kaizenkey");
                client.DefaultRequestHeaders.Add("password", "kk");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                // Gets normal graph
                String u = "https://tstdemocpm.siav.net:9443/FileSystemManager/getDotPerformanceFromCustomerAndPid?customer=kaizenkey&PID=" + processID.ToString()
                    +"&statisticFunction=MEAN";
                Console.WriteLine(u.ToString());
                using (var result = await client.GetAsync(u))
                {
                    Console.WriteLine("IsSuccessStatusCode: " + result.StatusCode + " " + result.IsSuccessStatusCode);
                    string content = await result.Content.ReadAsStringAsync();
                    Console.WriteLine("Content in getFrequencyGraph: " + content);
                    return content;
                };
            }

            return "";
        }

        public static async Task<string> getFrequencyGraph(String processID, String token)
        {
            // Add the certificate
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            if (cert != null)
            {
                Console.WriteLine("Go Get Frequency Chart!");
                handler.ClientCertificates.Add(cert);

                HttpClient client = new HttpClient(handler);
                var payload = "{\"username\": \"kaizenkey\",\"password\": \"kk\"}";
                var c = new StringContent(payload, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.DefaultRequestHeaders.Add("username", "kaizenkey");
                client.DefaultRequestHeaders.Add("password", "kk");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                // Gets normal graph
                String u = "https://tstdemocpm.siav.net:9443/FileSystemManager/getDotFrequenceFromCustomerAndPid?customer=kaizenkey&PID=" + processID.ToString();
                Console.WriteLine(u.ToString());
                using (var result = await client.GetAsync(u))
                {
                    Console.WriteLine("IsSuccessStatusCode: " + result.StatusCode + " " + result.IsSuccessStatusCode);
                    string content = await result.Content.ReadAsStringAsync();
                    Console.WriteLine("Content in getDotData: " + content);
                    return content;
                };
            }

            return "";
        }

        protected static X509Certificate2 GetMyCert()
        {
            string certThumbprint = ConfigurationManager.AppSettings["thumbprint"];

            Console.WriteLine("certThumbPrint: " + certThumbprint);
            X509Certificate2 cert = null;

            // Load the certificate
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = store.Certificates.Find
            (
                X509FindType.FindByThumbprint,
                certThumbprint,
                false    // Including invalid certificates
            );
            if (certCollection.Count > 0)
            {
                cert = certCollection[0];
                Console.WriteLine("cert: " + cert.ToString());
            }
            store.Close();

            return cert;
        }

        public struct AuthenticationStruct
        {
            public string username;
            public string[] roles;
            public int expires_in;
            public string access_token;
            public string refresh_token;
            public string token_type;
        }
    }
}
