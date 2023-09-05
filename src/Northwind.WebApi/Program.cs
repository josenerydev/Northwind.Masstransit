using MassTransit;

using Northwind.Contracts;
using Northwind.UserRegistrationService;

var builder = WebApplication.CreateBuilder(args);

// Configuração padrão e outros serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.Host("rabbitmq://rabbitmq");
        cfg.Host("rabbitmq://localhost");
    });
});

//RegisterWithEndpointConvention
EndpointConvention.Map<RegisterUser>(new Uri("rabbitmq://localhost/register-user-queue"));

// RegisterWithNameFormatter
//var formatter = DefaultEndpointNameFormatter.Instance;
//string queueName = formatter.Consumer<RegisterUserConsumer>();
//EndpointConvention.Map<RegisterUser>(new Uri($"queue:{queueName}"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();