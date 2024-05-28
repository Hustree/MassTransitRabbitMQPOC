using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace MassTransitRabbitMQPOC
{
    public class Publisher
    {
        public static async Task Publish(IHost host)
        {
            var bus = host.Services.GetRequiredService<IBusControl>();

            while (true)
            {
                Console.Write("Enter message: ");
                var text = Console.ReadLine();

                if (string.IsNullOrEmpty(text)) break;

                await bus.Publish(new Message { Text = text });
            }
        }
    }
}
