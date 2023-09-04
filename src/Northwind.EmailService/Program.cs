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
                        x.AddConsumer<UserValidatedConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq://rabbitmq");

                            cfg.ReceiveEndpoint("welcome-email-queue", e =>
                            {
                                e.ConfigureConsumer<UserValidatedConsumer>(context);
                            });
                        });
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}