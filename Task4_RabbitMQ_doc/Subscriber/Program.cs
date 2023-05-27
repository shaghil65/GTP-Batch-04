using Publisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Subsscriber
{
    static class Program
    {
        static void Main(string[] args)
        {
            Connection cs = new Connection();
            var channel = cs.Connect();

            // Declare a fanout exchange
            channel.ExchangeDeclare("demo-exchange", ExchangeType.Fanout, durable: true);

            // Declare a unique queue for each consumer instance
            var queueName = channel.QueueDeclare().QueueName;

            // Bind the queue to the fanout exchange
            channel.QueueBind(queueName, "demo-exchange", "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                // Deserialize the JSON string into an object
                var message = JsonSerializer.Deserialize<Message>(messageJson);
                Console.WriteLine($"Id: {message.Id}");
                Console.WriteLine($"Category: {message.Category}");
                Console.WriteLine($"Payload: {message.Payload}");
                Console.WriteLine($"CreatedTime: {message.CreatedTime}");
                Console.WriteLine($"SenderId: {message.SenderId}");

            };

            // Start consuming from the unique queue
            channel.BasicConsume(queueName, true, consumer);
            Console.ReadLine();
        }
    }
}
