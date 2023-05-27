using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

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

                // Take user input for message properties
                message.Id = Guid.NewGuid();

                Console.Write("Select Category: 1 for ProcessVideo 2 for ProcessImage & 3 for ProcessTimeSeriesData: ");
                message.Category = (MessageType)int.Parse(Console.ReadLine());

                Console.Write("Payload Message: ");
                message.Payload = Console.ReadLine();

                message.CreatedTime = DateTime.Now;
                message.SenderId = Guid.NewGuid();

                // Add message to the database
                using (var dbContext = new ApplicationDbContext())
                {
                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();
                }

                // Convert message to JSON
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
