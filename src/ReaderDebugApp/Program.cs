using System;
using System.Management;
using Lando;  // Assuming this is your RFID library

namespace LandoTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // List all detected smart card readers
            ListAllSmartCardReaders();

            // Instantiate the Lando card reader
            Cardreader cardReader = new Cardreader();

            // Start watching for cards
            cardReader.StartWatch();
            Console.WriteLine("\nWatching for NFC cards... Press Enter to stop.");
            Console.ReadLine();

            // Stop and dispose
            cardReader.StopWatch();
            cardReader.Dispose();
        }

        static void ListAllSmartCardReaders()
        {
            try
            {
                Console.WriteLine("=== Detected Smart Card Readers ===");

                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SmartCardReader"))
                {
                    foreach (var device in searcher.Get())
                    {
                        string name = device["Name"]?.ToString() ?? "Unknown";
                        string deviceId = device["DeviceID"]?.ToString() ?? "N/A";
                        string pnpId = device["PNPDeviceID"]?.ToString() ?? "N/A";
                        string status = device["Status"]?.ToString() ?? "N/A";

                        Console.WriteLine($"Name: {name}");
                        Console.WriteLine($"DeviceID: {deviceId}");
                        Console.WriteLine($"PNPDeviceID: {pnpId}");
                        Console.WriteLine($"Status: {status}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while listing smart card readers: " + ex.Message);
            }
        }

        // Optional: If you still want a specific match by name
        static string GetReaderDeviceId(string readerName)
        {
            try
            {
                string deviceId = null;
                string query = $"SELECT * FROM Win32_SmartCardReader WHERE Name LIKE '%{readerName}%'";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (var device in searcher.Get())
                    {
                        deviceId = device["PNPDeviceID"]?.ToString();
                        if (!string.IsNullOrEmpty(deviceId))
                            break;
                    }
                }

                return deviceId;
            }
            catch
            {
                return null;
            }
        }
    }
}
