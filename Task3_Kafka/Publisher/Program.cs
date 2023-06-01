using Confluent.Kafka;
using Newtonsoft.Json;
using Publisher;
using System;
using System.Text;
using System.Threading.Channels;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            string topic = "my_topic";
            while (true)
            {
                var message = new Message();
                message.Id = Guid.NewGuid();

                Console.Write("Select Category: 0 for ProcessVideo, 1 for ProcessImage & 2 for ProcessTimeSeriesData: ");
                var categoryInput = Console.ReadLine();
                if (!int.TryParse(categoryInput, out int category))
                {
                    Console.WriteLine("Invalid choice. Please enter a valid category number.");
                    continue;
                }

                message.Category = (MessageType)category;

                if (message.Category == MessageType.ProcessVideo)
                {
                    VideoPayload payload = new VideoPayload();
                    payload.id = Guid.NewGuid();
                    Console.Write("Enter File Name: ");
                    payload.filename = Console.ReadLine();
                    Console.Write("Enter File Type: ");
                    payload.type = Console.ReadLine();

                    message.Payload = JsonConvert.SerializeObject(payload);
                }
                else if (message.Category == MessageType.ProcessImage)
                {
                    ImagePayload payload = new ImagePayload();
                    payload.id = Guid.NewGuid();
                    Console.Write("Enter File Name: ");
                    payload.filename = Console.ReadLine();
                    Console.Write("Enter Codec: ");
                    payload.codec = Console.ReadLine();

                    message.Payload = JsonConvert.SerializeObject(payload);
                }
                else if (message.Category == MessageType.ProcessTimeSeriesData)
                {
                    TimeData payload = new TimeData();
                    payload.id = Guid.NewGuid();
                    Console.Write("Enter File Name: ");
                    payload.filename = Console.ReadLine();
                    Console.Write("Enter Timestamp: ");
                    payload.timestamp = Console.ReadLine();

                    message.Payload = JsonConvert.SerializeObject(payload);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid category number.");
                    continue;
                }

                message.CreatedTime = DateTime.Now;
                message.SenderId = Guid.NewGuid();



                // Add message to the database
                using (var dbContext = new ApplicationDbContext())
                {
                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();
                }

                var deliveryReport = producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = message.Payload
                }) ;

                Console.WriteLine("Press Enter to publish another message or type 'exit'");
                var userInput = Console.ReadLine();
                if (userInput == "exit")
                {
                    break;
                }
            }
        }
    }
}
