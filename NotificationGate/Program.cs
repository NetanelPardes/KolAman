using System;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json;
using Confluent.Kafka;
using NotificationGate.service;
using Microsoft.Extensions.Configuration;


namespace NotificationGate
{
    class Program
    {
        private static kafkaProduser _service = null!;
        private static string _alertsTopic = "";
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connection = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

            string alertsTopic = configuration["Kafka:Topics:Alerts"] ?? "alerts-events";

            var service = new kafkaProduser(connection);

            await service.EnsureTopicEistsAsync(alertsTopic);

            _service = service;
            _alertsTopic = alertsTopic;
            using var watcher = new FileSystemWatcher(@"C:\Users\sendi\Desktop\alert-simulator\alerts");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;

            watcher.Filter = "*.ready";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            
        }

        private static async void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            string ready = e.FullPath.Replace(".ready",".json");
            System.Console.WriteLine(ready);
            if(!File.Exists(ready))
            {
                Console.WriteLine("no");
                return;
            }
            Console.WriteLine("yes");
            string folder = Path.GetDirectoryName(e.FullPath)!;
            string jsonPath = Path.Combine(folder, "alert.json");
            System.Console.WriteLine(jsonPath);
            string content = File.ReadAllText(jsonPath);
            await _service.SendAsync(_alertsTopic,content);
        }
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
        private static async void OnCreated(object sender, FileSystemEventArgs e)
        {
            string ready = e.FullPath.Replace(".ready",".json");
            System.Console.WriteLine(ready);
            if(!File.Exists(ready))
            {
                Console.WriteLine("no");
                return;
            }
            Console.WriteLine("yes");
            string folder = Path.GetDirectoryName(e.FullPath)!;
            string jsonPath = Path.Combine(folder, "alert.json");
            Console.WriteLine(jsonPath);
            string content = File.ReadAllText(jsonPath);
            await _service.SendAsync(_alertsTopic,content);

        }
    }
}


