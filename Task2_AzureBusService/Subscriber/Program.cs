using Azure.Messaging.ServiceBus;
using Publisher;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {

        const string SubscriptionName = "Subscriber1";

        public static void Main(string[] args)
        {
            GeneralReceiver generalReceiver = new GeneralReceiver();
            _ = generalReceiver.Receiver(SubscriptionName);
        }
    }
}
