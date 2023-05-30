using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Publisher
{
    static class Program
    {
        static void Main(string[] args)
        {
            Connection cs = new Connection();
            var channel = cs.Connect();

            // Declare a fanout exchange
            channel.ExchangeDeclare("demo-exchange", ExchangeType.Fanout, durable: true);

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

                using (var dbContext = new ApplicationDbContext())
                {
                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();
                }

                var messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                // Publish the message to the fanout exchange
                channel.BasicPublish("demo-exchange", "", null, body);

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
