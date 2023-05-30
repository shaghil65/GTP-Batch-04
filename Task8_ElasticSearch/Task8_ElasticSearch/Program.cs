using System;
using System.Diagnostics.Metrics;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Nest;

namespace LogDataGenerator
{
    class Program
    {
        static int counter = 0;
        static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = "mydata-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true
                })
                .CreateLogger();
            SearchLogs("This is Random Text4");
            //while (true)
            //{
            //    Guid id = Guid.NewGuid();
            //    string randomText = GetRandomText();

            //    Log.Information("Log ID: " + id + " - Random Text:" + randomText);

            //    Thread.Sleep(5000);
            //}
        }
        private static string GetRandomText()
        {
            return "This is Random Text" + counter++;
        }
        private static void SearchLogs(string searchTerm)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
            var client = new ElasticClient(settings);

            var response = client.Search<string>(s => s
                .Index("mydata-*")
                .Query(q => q
                    .MatchPhrase(m => m
                        .Field("message")
                        .Query(searchTerm)
                    )
                )
            );

            if (response.IsValid)
            {
                Console.WriteLine("Search Results:");
                foreach (var hit in response.Hits)
                {
                    Console.WriteLine($"Log ID: {hit.Id}, Message: {hit.Source}");
                }
            }
            else
            {
                Console.WriteLine("Search failed");
            }
        }

    }
}
