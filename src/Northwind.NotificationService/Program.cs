using MassTransit;

using Northwind.NotificationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserNotificationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");

                cfg.ReceiveEndpoint("user-notification-queue", e =>
                {
                    e.ConfigureConsumer<UserNotificationConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();