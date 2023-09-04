using MassTransit;

using Northwind.UserRegistrationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RegisterUserConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");

                cfg.ReceiveEndpoint("register-user-queue", e =>
                {
                    e.ConfigureConsumer<RegisterUserConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();