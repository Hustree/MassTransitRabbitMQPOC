using MassTransit;
using MassTransitRabbitMQPOC.Configurations;
using MassTransitRabbitMQPOC.Consumers;
using MassTransitRabbitMQPOC.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HealthChecks.RabbitMQ;

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
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<RabbitMqOptions>(hostContext.Configuration.GetSection("RabbitMQ"));

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                            var host = rabbitMqOptions.Host ?? "localhost";
                            var username = rabbitMqOptions.Username ?? "guest";
                            var password = rabbitMqOptions.Password ?? "guest";

                            cfg.Host(host, "/", h =>
                            {
                                h.Username(username);
                                h.Password(password);
                            });

                            cfg.ReceiveEndpoint("message-queue", e =>
                            {
                                e.ConfigureConsumer<MessageConsumer>(context);
                            });
                        });
                    });

                    services.AddLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                    });

                    var rabbitMqHost = hostContext.Configuration["RabbitMQ:Host"] ?? "localhost";
                    var rabbitMqUsername = hostContext.Configuration["RabbitMQ:Username"] ?? "guest";
                    var rabbitMqPassword = hostContext.Configuration["RabbitMQ:Password"] ?? "guest";

                    var rabbitMqConnectionString = $"amqp://{rabbitMqUsername}:{rabbitMqPassword}@{rabbitMqHost}";

                    services.AddHealthChecks()
                        .AddRabbitMQ(rabbitConnectionString: rabbitMqConnectionString);
                });
    }
}
