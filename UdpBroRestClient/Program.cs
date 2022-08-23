using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Threading;
using ClassLibrary;
using System.Reflection.Emit;

namespace UdpBroRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wine Cooler UDP Client start...");
            //string message = Console.ReadLine();

            /// create socket for UDP communication  
            UdpClient socket = new UdpClient();

            /// Re-set the attribute of the Socket instance. Enable broadcast and create endpoint for broadcast. 
            socket.EnableBroadcast = true;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, 26000);  // 255.255.255.255 

            int coolerId = 1;
            string url = "http://localhost:20915/Cooler";

            /// while loop controller:
            int counter = 1;
            while (counter <= 50)
            {
                /// Create: windData data model. It should be in the while loop for renew data model
                CoolerGenerator _generator = new CoolerGenerator();
                Cooler cooler = new Cooler()
                {
                    Location = _generator.CreateLocation(),
                    Capacity = _generator.CreateCapacity(),
                    Storage = _generator.CreateStorage(),
                    Temp = _generator.CreateTemp()
                };

                //SendJsonString(cooler, socket, endpoint);
                //SendMessage(cooler, socket, endpoint);
                // SendSpeed(cooler, socket, endpoint);

                PostToRest(cooler, url);

                /// Get From Rest 
                //GetFromRest(coolerId, url);
                //coolerId++;

                // GetFromRestById(coolerId, url);

                // SendDirection(cooler, socket, endpoint); 

                counter++;
                Thread.Sleep(5000);
            }

            socket.Close();

            static void SendJsonString(Cooler cooler, UdpClient socket, IPEndPoint endpoint)
            {
                /// Data Model --> Json String
                string jsonString = JsonSerializer.Serialize(cooler);

                /// string --> byte[]: for UDP communication. UDP can only send and receive byte[]
                byte[] byteData = Encoding.UTF8.GetBytes(jsonString);
                socket.Send(byteData, byteData.Length, endpoint);
                /// the string is in the Json style: {"Id":0,"Speed":5,"Direction":"N"}, default id = 0
                Console.WriteLine("UDP broadcast transmitter send " + jsonString);
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

            static void GetFromRest(int coolerId, string url)
            {
                HttpClient httpClient = new HttpClient();

                // Get data from REST
                var httpResponse = httpClient.GetAsync(url + "/" + coolerId).Result;
                var jsonString = httpResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine("UDP broadcast receives from REST:" + jsonString);
            }

            static void GetFromRestById(int coolerId, string url)
            {
                /// consumer REST need the HTTP Client
                HttpClient httpClient = new HttpClient();

                Console.WriteLine("Please type in the id:");

                /// string --> int
                coolerId = Int32.Parse(Console.ReadLine());

                // Get data from REST
                var httpResonsponse = httpClient.GetAsync(url + coolerId).Result;
                var jsonString = httpResonsponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine("UDP broadcast receives from REST:" + jsonString);
            }

            static void SendMessage(Cooler cooler, UdpClient socket, IPEndPoint endpoint)
            {
                //string message = "Direction:" + windData.Direction + Environment.NewLine + "Speed:" + windData.Speed;
                //string message = windData.Direction + " " + windData.Speed;

                string message = cooler.Id + "," + cooler.Location + "," + cooler.Capacity + "," + cooler.Storage + "," + cooler.Temp;

                byte[] byteData = Encoding.UTF8.GetBytes(message);
                socket.Send(byteData, byteData.Length, endpoint);
                Console.WriteLine($"UDP broadcast transmitter send:{Environment.NewLine}{message}");
            }

            /*
            static void SendDirection(Cooler cooler, UdpClient socket, IPEndPoint endpoint)
            {
                string firstMessage = "Please typing the direction you want: ";

                byte[] byteData = Encoding.UTF8.GetBytes(firstMessage);
                socket.Send(byteData, byteData.Length, endpoint);


                IPEndPoint from = null;
                byte[] byteReceived = socket.Receive(ref from);
                string request = Encoding.UTF8.GetString(byteReceived);
                Console.WriteLine("Client received: " + request + " from UDP Server");

                //List<WindData> list = new List<WindData>();
                //foreach (var wind in list)
                //{
                //    if (windData.Direction == request)
                //    {
                //        string message = "Direction:" + windData.Direction;
                //        byte[] byteData = Encoding.UTF8.GetBytes(message);
                //        socket.Send(byteData, byteData.Length, endpoint);
                //        //socket.Send(byteData, byteData.Length, "255.255.255.255", 26000);
                //        Console.WriteLine($"UDP Client send: {message} to Server");
                //    }

                //    Console.WriteLine("Request can not find.");
                //}              
            }
            */
            /*
            static void SendSpeed(Cooler cooler, UdpClient socket, IPEndPoint endpoint)
            {
                string message = "Speed:" + cooler.Speed + " m/s";
                byte[] byteData = Encoding.UTF8.GetBytes(message);
                socket.Send(byteData, byteData.Length, endpoint);
                Console.WriteLine($"UDP broadcast transmitter send: {message} to {endpoint.Address}");
            }
            */
        }
    }
}
