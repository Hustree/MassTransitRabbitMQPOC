using MassTransit;
using System;
using System.Threading.Tasks;

namespace MassTransitRabbitMQPOC
{
    public class MessageConsumer : IConsumer<Message>
    {
        public Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine($"Received: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
