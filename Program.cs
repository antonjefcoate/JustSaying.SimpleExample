using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustSaying.Messaging.MessageHandling;
using JustSaying.Models;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = JustSaying.CreateMeABus.InRegion("eu-west-1")
                .WithSnsMessagePublisher<SomeMessage>()
                .WithSqsTopicSubscriber()
                .IntoQueue("AntonsQueue")
                .WithMessageHandler(new HandleSomeMessage());
            
            bus.StartListening();

            Console.WriteLine("What would you like to say?");
            bus.Publish(new SomeMessage(Console.ReadLine()));

            Console.ReadKey();
        }

        class SomeMessage : Message
        {
            public SomeMessage(string info)
            {
                Info = info;
            }

            public string Info { get; private set; }
        }

        class HandleSomeMessage : IHandler<SomeMessage>
        {
            public bool Handle(SomeMessage message)
            {
                Console.WriteLine("You wrote: '{0}'", message.Info);
                return true;
            }
        }
    }
}
