using MassTransit;

using Northwind.UserRegistrationHandlerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserRegistrationEmailSender>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");

                cfg.ReceiveEndpoint("user-registration-email-sender-queue", e =>
                {
                    e.ConfigureConsumer<UserRegistrationEmailSender>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();