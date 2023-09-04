using MassTransit;

using Northwind.WelcomeEmailService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<WelcomeEmailConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");

                cfg.ReceiveEndpoint("welcome-email-queue", e =>
                {
                    e.ConfigureConsumer<WelcomeEmailConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();