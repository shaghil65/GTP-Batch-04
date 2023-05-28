using System;
using System.Diagnostics.Metrics;
using Serilog;
using Serilog.Sinks.Elasticsearch;


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
                    IndexFormat = "logdata-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true
                })
                .CreateLogger();
            while (true)
            {
                Guid id = Guid.NewGuid();
                string randomText = GetRandomText();

                Log.Information("Log ID: {LogId} - Random Text: {RandomText}", id, randomText);

                Thread.Sleep(5000); 
            }
        }
        private static string GetRandomText()
        {
            return "This is Random Text" + counter++;
        }
    }
}
