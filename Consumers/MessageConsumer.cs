using MassTransit;
using MassTransitRabbitMQPOC.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MassTransitRabbitMQPOC.Consumers
{
    public class MessageConsumer : IConsumer<Message>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"Received: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
