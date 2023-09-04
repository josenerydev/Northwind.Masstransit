using MassTransit;

using Northwind.Contracts;

namespace Northwind.EmailService
{
    public class UserValidatedConsumer : IConsumer<UserValidated>
    {
        public async Task Consume(ConsumeContext<UserValidated> context)
        {
            Console.WriteLine($"Recebendo mensagem UserValidated para o usuário: {context.Message.UserId} com email: {context.Message.Email}");

            // ... Processamento da validação ...
            Console.WriteLine("Processando validação...");

            await context.Send<SendWelcomeEmail>(new Uri("queue:welcome-email-queue"), new
            {
                UserId = context.Message.UserId,
                Email = context.Message.Email
            });

            Console.WriteLine($"Mensagem SendWelcomeEmail enviada para o usuário: {context.Message.UserId}");
        }
    }
}