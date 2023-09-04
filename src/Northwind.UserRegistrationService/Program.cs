using MassTransit;

using Northwind.UserRegistrationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Configuração do MassTransit dentro do serviço de injeção de dependência.
        services.AddMassTransit(x =>
        {
            // Adiciona o consumidor RegisterUserConsumer ao MassTransit.
            x.AddConsumer<RegisterUserConsumer>();

            // Configura o uso do RabbitMQ como o broker de mensagens.
            x.UsingRabbitMq((context, cfg) =>
            {
                // Define o endereço do host RabbitMQ.
                cfg.Host("rabbitmq://rabbitmq");

                // Configura um endpoint de recepção para ouvir mensagens destinadas à fila "register-user-queue".
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