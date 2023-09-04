using MassTransit;

namespace Northwind.EmailService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<UserRegisteredConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq://rabbitmq");

                            cfg.ReceiveEndpoint("welcome-email-queue", e =>
                            {
                                e.ConfigureConsumer<UserRegisteredConsumer>(context);
                            });
                        });
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}