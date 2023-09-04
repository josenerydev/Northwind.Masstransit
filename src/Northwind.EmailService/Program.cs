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
                        x.AddConsumer<WelcomeEmailConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq://rabbitmq");

                            cfg.ReceiveEndpoint("user-registered-queue", e =>
                            {
                                e.ConfigureConsumer<UserRegisteredConsumer>(context);
                            });

                            cfg.ReceiveEndpoint("welcome-email-queue", e =>
                            {
                                e.ConfigureConsumer<WelcomeEmailConsumer>(context);
                            });
                        });
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}