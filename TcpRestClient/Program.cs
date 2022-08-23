using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace TcpRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cooler Tcp client: ");

            HttpClient httpClient = new HttpClient();  // consumer REST

            string url = "http://localhost:20915/Cooler";
            int i = 1;

            // Json --> data model
            // WindData receivedData = JsonSerializer.Deserialize<WindData>(message);

            /// while loop controller:
            int counter = 1;
            while (counter <= 5)
            {
                // Get data from REST 
                var httpResonsponse = httpClient.GetAsync(url + "/" + i).Result;
                var dataString = httpResonsponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine("TCP Client receives from REST:" + dataString);

                // TCP is point to point, has no broadcast
                TcpClient client = new TcpClient("localhost", 26000); //The socket

                NetworkStream ns = client.GetStream();
                StreamReader reader = new StreamReader(ns); // Make the ns available for reading
                StreamWriter writer = new StreamWriter(ns); //Write something the ns

                writer.WriteLine(dataString);
                Console.WriteLine(String.Format("TCP Client send {0} to TCP Server ", dataString));

                writer.Flush();

                string response = reader.ReadLine();
                Console.WriteLine(String.Format("TCP Client receives {0} from TCP Server ", response));

                i++;
                client.Close();
            }
        }

        
        /*
        static void SendMessage(HttpClient httpClient, Cooler cooler, string url)
        {
            /// Data Model --> Json String
            string jsonString = JsonSerializer.Serialize(cooler);

            // media type is Json
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            httpClient.PostAsync(url, content);
            Console.WriteLine("TCP client send to REST: " + jsonString);
        }

        static async Task<string> GetFromRestAsync(HttpClient httpClient, string url, int i)
        {
            string message = null;
            string dataString = null;
            // Get data from REST 
            var httpResonsponse = httpClient.GetAsync(url + "/" + i).Result;
            dataString = httpResonsponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine("TCP Client receives from REST:" + dataString);
            //message = await httpResonsponse.Content.ReadAsStringAsync();
            return dataString;
            //return message;
        }

        static void PostToRest(Cooler cooler, string url)
        {
            HttpClient httpClient = new HttpClient();

            /// Data Model --> Json String
            string jsonString = JsonSerializer.Serialize(cooler);

            // media type is Json
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            httpClient.PostAsync(url, content);
            Console.WriteLine("UDP broadcast transmitter send to REST: " + jsonString);
        }
        */
    }
}
