using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Net.Mail;


namespace ThirdPartySalesOrders_Finestra3000
{
    class Program
    {
        public static void Main(string[] args)
        {
            /* checks variable
             * 1 if everything is fine
             * 2 if some products are not known by Virtual Chief
             * 3 if Customer does not exists in Virtual Chief and it was not possible to create a new one
             * 4 if it was not possible to create a new Sales order in Virtual Chief
             */
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string sGetFolder = ConfigurationManager.AppSettings["getfolder"];
            string sArchiveFolder = ConfigurationManager.AppSettings["archivefolder"];

            Console.WriteLine(sBaseUrl + "\n" + sGetFolder + "\n" + sArchiveFolder);


            DirectoryInfo d = new DirectoryInfo(sGetFolder);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*"); //Getting Xml files
            string str = "";

            Console.WriteLine("\nListing files...\n");
            List<String> newfiles = new List<string>();
            foreach (FileInfo file in Files)
            {
                newfiles.Add(file.Name);
                Console.WriteLine(file.Name);
            }

            if (newfiles.Count == 0) { Console.WriteLine("No file found\n"); }


            for (int i = 0; i < newfiles.Count; i++)
            {
                int checks = 1;

                List<Productorder> lstProducts = new List<Productorder>();
                int SalesOrderID = -1;
                int SalesOrderYear = -1;

                XmlDocument doc = new XmlDocument();
                Console.WriteLine(sGetFolder + '\\' + newfiles[i]);
                doc.Load(sGetFolder + "\\" + newfiles[i]);
                String externalOrderId = "";
                XmlNode node = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/OrdineNum");
                externalOrderId = node.InnerText;

                String customer = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/AnagrafeNumScheda").InnerText;
                String notes = "";
                String customerName = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/RagioneSociale").InnerText;
                String vatNumber = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/PartitaIVA").InnerText;
                String codFiscale = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/CodiceFiscale").InnerText;
                String address = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Indirizzo").InnerText;
                String city = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Citta").InnerText;
                String province = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Provincia").InnerText;
                String zipcode = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/CAP").InnerText;
                String country = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Nazione").InnerText;
                String phone = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Cellulare").InnerText;
                String email = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/Email").InnerText;

                String OriginalOrderNumber = doc.DocumentElement.SelectSingleNode("/Ordine/ORDScheda/OrdineNum").InnerText;

                // get a list of all <Contact> nodes
                XmlNodeList products = doc.SelectNodes("/Ordine/ORDPosizione");

                // iterate over the <Contact> nodes
                String NotFoundProducts = "";
                foreach (XmlNode prod in products)
                {
                    String codArt = prod.SelectSingleNode("CodArt").InnerText.ToString().Trim();
                    String sQuantity = prod.SelectSingleNode("Quantita").InnerText;
                    Double Quantity = 0;
                    try
                    {
                        Quantity = Double.Parse(sQuantity);
                    }
                    catch
                    {
                        Quantity = 0;
                    }

                    Productorder curr = new Productorder();
                    curr.ProductID = codArt;
                    curr.Quantity = Quantity;
                    curr.DeliveryDate = DateTime.UtcNow.AddDays(14);
                    curr.EndProductionDate = DateTime.UtcNow.AddDays(14);
                    lstProducts.Add(curr);

                    // Checks if current product is known!
                    String resCheck = CheckIfProductExists(curr.ProductID).Result;
                    Console.Write("Check if product " +curr.ProductID+ " exists... ");
                    if (resCheck == "1")
                    {
                        Console.Write("Ok\n");
                    }
                    else
                    {
                        checks = 2;
                        NotFoundProducts += curr.ProductID.ToString() + "<br />";
                        Console.Write("Product not found\n");
                    }
                }

                if (checks == 2)
                {
                    Console.WriteLine("Sending product not found error via e-mail...");
                    NotifyError("Problema critico: i seguenti prodotti non sono presenti in Virtual Chief:<br />" + NotFoundProducts 
                        + "<br /><br />Per questo motivo l'importazione dell'ordine "+ OriginalOrderNumber + " per cliente " + customerName + " è stata sospesa.<br/><br />"
                        +"Per proseguire, è necessario creare i prodotti elencati in Virtual Chief");
                }

                if (checks == 1)
                {
                    // Consume a Web API
                    String qry = "?customer=" + customer
                        + "&notes=" + notes
                        + "&externalID=" + externalOrderId
                        + "&customerName=" + customerName
                        + "&vatNumber=" + vatNumber
                        + "&codFiscale=" + codFiscale
                        + "&address=" + address
                        + "&city=" + city
                        + "&province=" + province
                        + "&zipcode=" + zipcode
                        + "&country=" + country
                        + "&phone=" + phone
                        + "&email=" + email
                        ;
                    String resCreateSalesOrder = GetRemoteAddSalesOrderHeader(qry).Result;

                    SalesOrderID = -2;
                    try
                    {
                        SalesOrderID = Int32.Parse(resCreateSalesOrder);
                        SalesOrderYear = DateTime.UtcNow.Year;
                    }
                    catch
                    {
                        SalesOrderID = -2;
                        SalesOrderYear = -1;
                    }

                    if (SalesOrderID >= 0)
                    {
                        checks = 1;
                    }
                    else
                    {
                        switch (SalesOrderID)
                        {
                            case -2: checks = 3; break;
                            case -3: checks = 4; break;
                            default: break;
                        }
                    }

                    Console.Write("Sales Order creation... ");

                    if (checks != 1)
                    {
                        Console.Write("Error\n");
                        String strError = "Problema critico: non è stato possibile importare in Virtual Chief l'ordine " + OriginalOrderNumber + " per cliente " + customer + " ";
                        if (checks == 3)
                        {
                            strError += "perché non è stato possible creare il cliente in Virtual Chief.<br /><br />"
                                + "Correggere i dati del cliente, oppure crearlo manualmente in Virtual Chief.";
                        }
                        else if (checks == 4)
                        {
                            strError += "perché è avvenuto un errore durante la creazione dell'ordine di vendita in Virtual Chief.<br /><br/>";
                        }
                        strError += "Se non si incontrano errori nei dati di input, contattare l'amministratore del sistema.<br /><br />Il team di Virtual Chief";

                        NotifyError(strError);
                    }
                    else
                    {
                        Console.Write("Ok\n");
                    }
                }


                    // if the sales order has been created fine, it's time to add products!
                    if (checks == 1)
                    {
                        if (SalesOrderID != -1 && SalesOrderYear != -1)
                        {
                        String strErrors = "";
                            for (int j = 0; j < lstProducts.Count; j++)
                            {
                                HttpClient client2 = new HttpClient();
                                client2.DefaultRequestHeaders.Add("X-API-KEY", "zk6eNbKHmlq=M1xMS");
                            String qryAddProd = "?OrderID=" + SalesOrderID.ToString()
                                + "&OrderYear=" + SalesOrderYear.ToString()
                                + "&ProductID=" + lstProducts[j].ProductID
                                + "&Quantity=" + lstProducts[j].Quantity.ToString()
                                + "&DeliveryDate=" + lstProducts[j].DeliveryDate.ToString("yyyy-MM-dd")
                                + "&EndProductionDate=" + lstProducts[j].EndProductionDate.ToString("yyyy-MM-dd")
                                ;

                            String resAddProductToOrder = AddProductToSalesOrderAndPlan(qryAddProd).Result;
                            Console.Write("Adding Product " + lstProducts[j].ProductID + " to order... ");
                            switch(resAddProductToOrder)
                            {
                                case "1":
                                    Console.WriteLine("Ok");
                                    break;
                                case "2": checks = 10; Console.WriteLine("Error: user not authorized");
                                    strErrors += "Il prodotto" + lstProducts[j].ProductID + " non è stato aggiunto all'ordine perché l'utente non ha il permesso necessario<br/>";
                                    break;
                                case "3": checks = 10; Console.WriteLine("Error: product not found"); 
                                    strErrors += "Il prodotto" + lstProducts[j].ProductID + " non è stato aggiunto all'ordine perché non esiste in Virtual Chief<br/>";
                                    break;
                                case "4": checks = 10; Console.WriteLine("Error: invalid delivery date. Product was added but not planned");
                                    strErrors += "Non è stato possibile pianificare in produzione il prodotto" + lstProducts[j].ProductID + " perché la data di fine produzione non è valida. E' necessario pianificare il prodotto manualmente in Virtual Chief<br/>";
                                    break;
                                case "5": checks = 10; Console.WriteLine("Error: sales order not found.");
                                    strErrors += "Il prodotto " + lstProducts[j].ProductID + " non è stato aggiungo all'ordine, perché non è stato possibile trovare l'ordine di vendita<br/>";
                                    break;
                                case "6": checks = 10; Console.WriteLine("Error: an error occured while adding the product to the sales order");
                                    strErrors += "Il prodotto " + lstProducts[j].ProductID + " non è stato aggiungo all'ordine a causa di un errore generico. Contattare l'amministratore del sistema.<br/>";
                                    break;
                                default:break;
                            }
                        }
                            if(checks!=1)
                        { 
                        NotifyError("Durante l'importazione dell'ordine " + OriginalOrderNumber + " in Virtual Chief sono avvenuti i seguenti errori:<br />" 
                            + strErrors + "<br />Il team di Virtual Chief");
                        }
                    }
                        else
                    {
                        NotifyError("Attenzione: non è stato possibile aggiungere prodotti all'ordine di vendita perché l'ordine di vendita non esiste.");
                        checks = 7;
                    }
                    }


                // Ultimo atto, se tutto è andato bene, sposto il file xml in un'altra directory!
                if(checks==10 || checks == 1)
                {
                    string sourceFile = @sGetFolder + '\\' + newfiles[i];
                    string destinationFile = @sArchiveFolder + '\\' + newfiles[i];
                    try
                    {
                        File.Move(sourceFile, destinationFile);
                        Console.WriteLine("Sales order file moved to archive\nSales order import ended successfully.\n");
                    }
                    catch (IOException iox)
                    {
                        Console.WriteLine(iox.Message);
                        NotifyError("Attenzione: non è stato possibile spostare il file " + newfiles[i] + "nell'archivio, anche se l'importazione dello stesso è avvenuta con successo.<br/>"
                            + "<br />Contattare l'amministratore del sistema.<br /><br />Il team di Virtual Chief");
                    }
                }

            }
            Console.ReadLine();
            }

        public static async Task<string> CheckIfProductExists(string ExternalID)
        {
            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/ThirdPartySalesOrders/CheckIfProductExists?ExternalID=";
             _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "dhqUTLJsLCtq047F");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}{ExternalID}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }


        public static async Task<string> GetRemoteAddSalesOrderHeader(string QueryString)
        {

            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/ThirdPartySalesOrders/GetRemoteAddSalesOrderHeader";

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "dhqUTLJsLCtq047F");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}{QueryString}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

        public static async Task<string> AddProductToSalesOrderAndPlan(string QueryString)
        {

            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/ThirdPartySalesOrders/AddProductToSalesOrderAndPlan";

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "dhqUTLJsLCtq047F");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}{QueryString}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

        public static void NotifyError(String text)
        {
            string sCustomer = ConfigurationManager.AppSettings["installationname"];
            string sDestinations = ConfigurationManager.AppSettings["ErrorsManagers"];
            string smtphost = ConfigurationManager.AppSettings["smtphost"];
            string ssmtpport = ConfigurationManager.AppSettings["smtpport"];
            int smtpport = 587;
            try
            {
                smtpport = Int32.Parse(ssmtpport);
            }
            catch { smtpport = 587; }
            string smtpUsername = ConfigurationManager.AppSettings["smtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];

            System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
            mMessage.From = new MailAddress("matteo.griso@virtualchief.net", "Scheduler@VirtualChief");
            List<String> emails = sDestinations.Split(';').ToList();
            for(int i = 0; i < emails.Count; i++)
            { 
                mMessage.To.Add(emails[i]);
            }

            mMessage.Subject = "[Virtual Chief][" + sCustomer + "] Error importing 3rd Party Sales Orders";
            mMessage.IsBodyHtml = true;

            mMessage.Body = "<html><body><div>" + text + "</div>"
                + "</body></html>";

            SmtpClient smtpcli = new SmtpClient();
            smtpcli.Host = smtphost;
            smtpcli.Port = smtpport;
            smtpcli.EnableSsl = true;
            smtpcli.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);


            smtpcli.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpcli.EnableSsl = true;
            smtpcli.Send(mMessage);
        }


        public struct Productorder
        {
            //public int ordID;
            //public int ordYear;
            public String ProductID;
            public Double Quantity;
            public DateTime DeliveryDate;
            public DateTime EndProductionDate;
        }


        
    }
}
