using Azure.Messaging.ServiceBus;
using Publisher;
using Subscriber;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber2
{
    class Program
    {
 
        const string SubscriptionName = "Subscriber2";

        public static void Main(string[] args)
        {
            GeneralReceiver generalReceiver = new GeneralReceiver();
            _ = generalReceiver.Receiver(SubscriptionName);
        }
    }
}
