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

namespace SIAV_GetDotFile
{
    class Program
    {
        static void Main(string[] args)
        {
            String token = getToken().Result;
            Console.WriteLine("Token: " + token.ToString());
            Console.Read();

            // Access the url
           /* url: "https://tstdemocpm.siav.net:9443/FileSystemManager/getDotPerformanceFromCustomerAndPid?customer=kaizenkey&PID=Serramenti%20PVC",
                    type: "GET",
                    beforeSend: function(xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + JSON.parse(accesstoken).access_token);
            },*/

            Console.Read();
        }

        public static async Task<string> getToken()
        {
            // Add the certificate
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            //if (cert != null)
            //{
                handler.ClientCertificates.Add(cert);
            //}

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
                var jsontoken = JsonConvert.DeserializeObject<AuthenticationStruct>(content);
                return jsontoken.access_token;
            };


        }

        public static async Task<string> getDotData()
        {
            // Add the certificate
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            //if (cert != null)
            //{
            handler.ClientCertificates.Add(cert);
            //}

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
                var jsontoken = JsonConvert.DeserializeObject<AuthenticationStruct>(content);
                return jsontoken.access_token;
            };


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
            public string roles;
            public int expires_in;
            public string access_token;
            public string refresh_token;
            public string token_type;
        }
    }
}
