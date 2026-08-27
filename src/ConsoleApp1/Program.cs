using Lando;  // Assuming this is your RFID library
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Xml.Linq;

// socket
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LandoTestConsoleApp
{
    class Program
    {

        static TcpListener server;
        static List<TcpClient> clients = new List<TcpClient>();


        static void Main(string[] args)
        {

            // Instantiate the Lando card reader
            Cardreader cardReader = new Cardreader();
            cardReader.CardConnected += CardReader_CardConnected;


            // List all detected smart card readers
            //ListAllConnectedDevices();
            ListAllPNPDeviceIDs(cardReader);

            server = new TcpListener(IPAddress.Any, 9000);
            server.Start();

            Console.WriteLine("TCP Server started on port 9000");

            server.BeginAcceptTcpClient(OnClientConnected, null);

            // Start watching for cards
            cardReader.StartWatch();
            Console.WriteLine("\nWatching for NFC cards...");
            Console.ReadLine();

            // Stop and dispose
            cardReader.StopWatch();
            cardReader.Dispose();
        }

        // Make sure this method matches the CardConnected event signature from Lando.dll
        private static void CardReader_CardConnected(object sender, CardreaderEventArgs e)
        {
            try
            {
                // Access the card reader instance from the event args
                var reader = sender as Cardreader;
                if (reader != null)
                {
                    // Optionally trigger the buzzer on the reader
                    reader.SetBuzzerOutputForCardDetection(e.Card, false);
                }

                // Clean card ID
                string cardId = e.Card.Id.Replace("-", "").ToLower();

                // Reader info
                string readerId = e.ReaderId;               // SDK-provided reader ID
                string readerName = e.CardreaderName;      // SDK-provided reader name

                // Print to console
                Console.WriteLine($"Card detected: {cardId} on Reader ID: {readerId} with Name: {readerName}");

                string message = $"{readerId}:{cardId}";
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");

                foreach (var client in clients.ToList())
                {
                    if (client.Connected)
                    {
                        client.GetStream().Write(data, 0, data.Length);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CardConnected event: " + ex.Message);
            }
        }
        static void ListAllPNPDeviceIDs(Cardreader _cardReader)
        {
            Console.WriteLine(new string(' ', 40));
            Console.WriteLine(new string(' ', 40));
            Console.WriteLine("=== All connected Card Readers ===");
            Dictionary<string, string> readerMapping = _cardReader.GetHardWareIDs();

            foreach (KeyValuePair<string, string> readerMap in readerMapping)
            {
                Console.WriteLine($"Name: {readerMap.Key}");
                Console.WriteLine($"PNPDeviceID: {readerMap.Value}");
                Console.WriteLine(new string('-', 40));
            }
        }


        static void OnClientConnected(IAsyncResult ar)
        {
            TcpClient client = server.EndAcceptTcpClient(ar);
            clients.Add(client);

            Console.WriteLine("Unity client connected");

            server.BeginAcceptTcpClient(OnClientConnected, null);
        }
    }
}
