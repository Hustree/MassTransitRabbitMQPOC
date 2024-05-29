using MassTransit;
using MassTransit.Testing;
using MassTransitRabbitMQPOC.Consumers;
using MassTransitRabbitMQPOC.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace MassTransitRabbitMQPOC.Tests
{
    public class MessageConsumerTests
    {
        [Fact]
        public async Task Should_Consume_Message()
        {
            var provider = new ServiceCollection()
                .AddLogging()
                .AddMassTransitInMemoryTestHarness(cfg =>
                {
                    cfg.AddConsumer<MessageConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<InMemoryTestHarness>();
            await harness.Start();

            try
            {
                var bus = provider.GetRequiredService<IBus>();
                await bus.Publish(new Message { Text = "Hello, World!" });

                Assert.True(await harness.Consumed.Any<Message>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
