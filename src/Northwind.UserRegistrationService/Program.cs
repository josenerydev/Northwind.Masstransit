using MassTransit;

using Northwind.UserRegistrationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Configura��o do MassTransit dentro do servi�o de inje��o de depend�ncia.
        services.AddMassTransit(x =>
        {
            // Adiciona o consumidor RegisterUserConsumer ao MassTransit.
            x.AddConsumer<RegisterUserConsumer>();

            // Configura o uso do RabbitMQ como o broker de mensagens.
            x.UsingRabbitMq((context, cfg) =>
            {
                // Define o endere�o do host RabbitMQ.
                cfg.Host("rabbitmq://rabbitmq");

                // Configura um endpoint de recep��o para ouvir mensagens destinadas � fila "register-user-queue".
                cfg.ReceiveEndpoint("register-user-queue", e =>
                {
                    // Vincula o consumidor RegisterUserConsumer a este endpoint.
                    e.ConfigureConsumer<RegisterUserConsumer>(context);
                });
            });
        });
    })
    .Build();

// Inicia o host.
await host.RunAsync();