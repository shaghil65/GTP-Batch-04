using Azure.Messaging.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string ServiceBusConnectionString = Connection.ServiceBusConnectionString;
            string TopicName = Connection.TopicName;

            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            var sender = client.CreateSender(TopicName);

            while (true)
            {
                Console.WriteLine("Enter message details:");

                var message = new Message();

                // Take user input for message properties
                message.Id = Guid.NewGuid();

                Console.Write("Select Category: 1 for ProcessVideo 2 for ProcessImage & 3 for ProcessTimeSeriesData: ");
                message.Category = (MessageType)int.Parse(Console.ReadLine());

                Console.Write("Payload Message: ");
                message.Payload = Console.ReadLine();

                message.CreatedTime = DateTime.Now;
                message.SenderId = Guid.NewGuid();


                // Create the message using user input
                var smessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message.Payload));
                smessage.MessageId = message.Id.ToString();
                smessage.ApplicationProperties.Add("Category", message.Category.ToString()); // Convert enum to string
                smessage.ApplicationProperties.Add("CreatedTime", message.CreatedTime);
                smessage.ApplicationProperties.Add("SenderId", message.SenderId);

                // Publish the message
                await sender.SendMessageAsync(smessage);
                Console.WriteLine("Published message");

                using (var dbContext = new ApplicationDbContext())
                {
                    dbContext.Messages.Add(message);
                    dbContext.SaveChanges();
                }

                Console.WriteLine("Press Enter to publish another message or type 'exit'");
                var userInput = Console.ReadLine();
                if (userInput == "exit")
                {
                    await sender.CloseAsync();
                    break;
                }
            }
        }
    }
}
