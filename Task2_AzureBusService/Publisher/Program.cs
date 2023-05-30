using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
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



                //var message = new Message();

                //// Take user input for message properties
                //message.Id = Guid.NewGuid();

                //Console.Write("Select Category: 1 for ProcessVideo 2 for ProcessImage & 3 for ProcessTimeSeriesData: ");
                //message.Category = (MessageType)int.Parse(Console.ReadLine());

                //Console.Write("Payload Message: ");
                //message.Payload = Console.ReadLine();

                //message.CreatedTime = DateTime.Now;
                //message.SenderId = Guid.NewGuid();


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
