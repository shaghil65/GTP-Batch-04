using Confluent.Kafka;
using System;
using System.Threading.Tasks;
namespace Subscriber
{
    public class GeneralReceiver
    {
        public void Reciever()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consumer-" + Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var topic = "my_topic";
                consumer.Subscribe(topic);

                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var message = consumeResult.Message.Value;

                    // Process the message
                    Console.WriteLine("Received Payload Message: " + message);
                }
            }
        }
    }
}
