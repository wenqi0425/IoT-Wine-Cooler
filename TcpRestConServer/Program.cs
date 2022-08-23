using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary;

namespace TcpRestConServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cooler Concurrent TCP Server:");

            TcpListener listener = new TcpListener(IPAddress.Any, 26000);
            listener.Start();
            Console.WriteLine("Server listening");                     

            int counter = 1;
            while (counter <= 5)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClientAsync(client));
            }

            listener.Stop();
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            // get data from Tcp Client
            NetworkStream ns = client.GetStream();

            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            // read one line of the ns, byteArrey --> message
            string message = reader.ReadLine();

            string sendData = "";

            Console.WriteLine("TCP Server receive data from TCP Client: " + message);

            // Json --> data model
            Cooler receivedData = JsonSerializer.Deserialize<Cooler>(message);

            int storage = receivedData.Storage;
            int capacity = receivedData.Capacity;

            if (storage < 20)
            {
                sendData = "Storage is too Small";
                Console.WriteLine("TCP Server send data to TCP Client: " + sendData);
            }

            if (storage == capacity)
            {
                sendData += " / Wine cooler is full";
                Console.WriteLine("TCP Server send data to TCP Client: " + sendData);
            }

            //sending message to TCP Clients via sockets.
            writer.WriteLine(sendData);

            writer.Flush();

            client.Close();
        }
    }
}
