using MassTransit;

using Northwind.UserRegistrationNotifier;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserRegistrationNotificationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");

                cfg.ReceiveEndpoint("user-registration-notification-queue", e =>
                {
                    e.ConfigureConsumer<UserRegistrationNotificationConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();