using Azure.Messaging.ServiceBus;
using Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class GeneralReceiver
    {
        public async Task Receiver(string SubscriptionName)
        {
            string ServiceBusConnectionString = Connection.ServiceBusConnectionString;
            string TopicName = Connection.TopicName;
            // Create the topic client and the subscriptions
            await using var topicClient = new ServiceBusClient(ServiceBusConnectionString);
            var receiver = topicClient.CreateReceiver(TopicName, SubscriptionName);

            // Start receiving messages from both subscribers
            _ = ReceiveMessagesAsync(receiver);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            // Close the connections
            await receiver.CloseAsync();
        }
        public async Task ReceiveMessagesAsync(ServiceBusReceiver receiver)
        {
            while (true)
            {
                // Receive a batch of messages
                IEnumerable<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(maxMessages: 10);

                foreach (ServiceBusReceivedMessage message in messages)
                {
                    Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");
                    await receiver.CompleteMessageAsync(message);
                }
            }
        }
    }
}
