using MassTransit;
using MassTransitRabbitMQPOC.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MassTransitRabbitMQPOC.Publishers
{
    public class Publisher
    {
        public static async Task Publish(IHost host)
        {
            var bus = host.Services.GetRequiredService<IBusControl>();
            var logger = host.Services.GetRequiredService<ILogger<Publisher>>();

            try
            {
                while (true)
                {
                    Console.Write("Enter message: ");
                    var text = Console.ReadLine();

                    if (string.IsNullOrEmpty(text)) break;

                    await bus.Publish(new Message { Text = text });
                    logger.LogInformation($"Message published: {text}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while publishing the message");
            }
        }
    }
}
