using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace MassTransitRabbitMQPOC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Length > 0 && args[0] == "publish")
            {
                await Publisher.Publish(host);
            }
            else
            {
                await host.RunAsync();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => { });

                            cfg.ReceiveEndpoint("message-queue", e =>
                            {
                                e.ConfigureConsumer<MessageConsumer>(context);
                            });
                        });
                    });

                    //services.AddMassTransitHostedService();
                });
    }
}
