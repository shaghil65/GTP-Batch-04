using Nest;
using System;
using System.Diagnostics.Metrics;

namespace LogDataConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("logdata");

            var client = new ElasticClient(settings);
            if (!client.Indices.Exists("logdata").Exists)
            {
                client.Indices.Create("logdata");
            }
            while (true)
            {
                var log = GenerateLogData();
                IndexResponse response = client.IndexDocument(log);

                if (response.IsValid)
                {
                    Console.WriteLine($"Indexed log with GUID: {log.Id}");
                }
                else
                {
                    Console.WriteLine($"Failed to index log with GUID: {log.Id}");
                }
                Thread.Sleep(4000);
            }

            //PerformSearch(client);
        }

        static void PerformSearch(IElasticClient client)
        {
            while (true)
            {
                Console.WriteLine("Enter search query or enter to exit:");
                var query = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(query))
                {
                    break;
                }

                var searchResponse = SearchLogs(client, query);

                Console.WriteLine($"Search results for '{query}':");
                foreach (var hit in searchResponse.Hits)
                {
                    Console.WriteLine($"- {hit.Source.Message}");
                }
            }
        }

        static ISearchResponse<LogData> SearchLogs(IElasticClient client, string query)
        {
            return client.Search<LogData>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Message)
                        .Query(query)
                    )
                )
            );
        }

        static LogData GenerateLogData()
        {
            return new LogData
            {
                Id = Guid.NewGuid().ToString(),
                Message = GenerateRandomText()
            };
        }

        public static int counter = 0; 
        static string GenerateRandomText()
        {
            counter++;
            return "Random Text " + counter;
        }
    }

    public class LogData
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
