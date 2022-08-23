using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ClassLibrary;

namespace UdpBroRestServer
{
    class Program
    {
        static async Task /*void*/ Main(string[] args)
        {
            Console.WriteLine("Wine Cooler UDP Server start...");

            /// create socket for UDP communication
            UdpClient socket = new UdpClient();

            // UDP broadcast server only need to bind the same port with UDP broadcast Client
            //socket.Client.Bind(new IPEndPoint(IPAddress.Any, 26000));
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 26000);
            socket.Client.Bind(endpoint);

            Cooler cooler = new Cooler();
            int coolerId = 1;

            string url = "http://localhost:20915/Cooler";

            /// GetFromRest          

            int i = 1;
            while (i <= 5)
            {
                GetFromRest(coolerId, url);
                i++;
                coolerId++;
                Thread.Sleep(5000);
            }

            // await Task.Run(() => GetFromRest(coolerId));

            // GetFromRestById(windId,url);

            int counter = 1;
            while (counter <= 5)
            {
                IPEndPoint from = null;

                // socket receive byteData from client
                byte[] byteData = socket.Receive(ref from);

                // byteData --> String
                string recieved = Encoding.UTF8.GetString(byteData);
                Console.WriteLine("Server received: " + Environment.NewLine + recieved /*+ "from " +  from.Address*/);

                //string direction = Console.ReadLine(); ;
                //RequestDirection(direction, socket, endpoint);
                //await Task.Run(() => RequestDirection(direction, socket, endpoint));

                //JsonToDataModel(cooler, recieved);
                // SplitMessage(cooler, recieved);

                // PostToRestByJson(recieved, url);

                Thread.Sleep(5000);
            }

            socket.Close();


            // Json doesn't need to be split, just need to convert to data model
            static void JsonToDataModel(Cooler cooler, string recieved)
            {
                // Json String --> Data Model  
                cooler = JsonSerializer.Deserialize<Cooler>(recieved);
                Console.WriteLine("Location: " + cooler.Location + Environment.NewLine +
                                  "Capacity: " + cooler.Capacity + Environment.NewLine +
                                  "Storage: " + cooler.Storage + Environment.NewLine +
                                  "Temp: " + cooler.Temp);
            }

            static void SplitMessage(Cooler cooler, string recieved)
            {
                // Json String --> Data Model  
                // split (" ") works
                var splittedStrings = recieved.Split(",");


                int id = Convert.ToInt32(splittedStrings[0]);
                string location = splittedStrings[1];
                int capacity = Convert.ToInt32(splittedStrings[2]);
                int storage = Convert.ToInt32(splittedStrings[3]);
                int temp = Convert.ToInt32(splittedStrings[4]);

                // Data Model
                cooler = new Cooler()
                {
                    Location = location,
                    Capacity = capacity,
                    Storage = storage,
                    Temp = temp
                };

                Console.WriteLine($"Id: {id} - Speed: {cooler.Location} - Direction: {cooler.Capacity} - Storage: {cooler.Storage} - Temp: {cooler.Temp}");

                /*
                 var splittedStrings = recieved.Split(" ");
                 foreach (var s in splittedStrings)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("Splitted string is:" + splittedStrings);
                Console.WriteLine("Splitted string length:" + splittedStrings.Length);                

                string returnData = "";

                if (splittedStrings[0].Equals("Joule"))
                {
                    double value = double.Parse(splittedStrings[1]);
                    returnData = "Calorie:" + _converter.ToCalorie(value);
                }

                if (splittedStrings[0].Equals("Calorie"))
                {
                    double value = double.Parse(splittedStrings[1]);
                    returnData = "Joule:" + _converter.ToCalorie(value);
                }

                // send Joule 63 on the SocketTest, not "Joule 63"
                Console.WriteLine("Server return:" + returnData);
                socket.Send(Encoding.UTF8.GetBytes(returnData), returnData.Length, "127.0.0.1", 26000);
                */
            }


            static void PostToRestByJson(string recieved, string url)  // if received is in Json style
            {
                //string url = "http://localhost:20915/Cooler";
                /// consumer REST need the HTTP Client
                HttpClient httpClient = new HttpClient();

                // media type is Json
                HttpContent content = new StringContent(recieved, Encoding.UTF8, "application/json");
                httpClient.PostAsync(url, content);
                Console.WriteLine("UDP broadcast transmitter send to REST: " + recieved);
            }

            static void GetFromRest(int coolerId, string url)
            {
                //string url = "http://localhost:20915/Cooler";
                HttpClient httpClient = new HttpClient();

                // Get data from REST
                var httpResponse = httpClient.GetAsync(url + "/" + coolerId).Result;
                var jsonString = httpResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine("UDP Server receives from REST:" + jsonString);
            }

            static void GetFromRestById(int coolerId, string url)
            {
                //string url = "http://localhost:20915/Cooler";
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

            static void RequestDirection(string direction, UdpClient socket, IPEndPoint endpoint)
            {
                byte[] byteData = Encoding.UTF8.GetBytes(direction);
                socket.Send(byteData, byteData.Length, endpoint);
                /// the string is in the Json style: {"Id":0,"Speed":5,"Direction":"N"}, default id = 0
                Console.WriteLine("UDP Server send Requst " + direction);
            }
        }
    }
}
